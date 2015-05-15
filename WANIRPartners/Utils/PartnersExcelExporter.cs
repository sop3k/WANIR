using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

using WANIRPartners.Models;

namespace WANIRPartners.Utils
{
    public class SchemaElem<T>
    {
        private String _Header;
        private String _MemberGetter;
        Func<T, int, Object> Getter = null;

        public SchemaElem(String header, String getter)
        {
            _Header = header;
            _MemberGetter = getter;
        }

        public SchemaElem(String header, Func<T, int, Object> getter)
        {
            _Header = header;
            Getter = getter;
        }

        public Object GetValue<U>(Object obj, int index)
        {
            if (!String.IsNullOrEmpty(_MemberGetter))
            {
                return TypeUtils.GetValueFromAnonymousType<U>(obj, _MemberGetter);
            }
            else if (Getter != null)
            {
                return Getter.Invoke((T)obj, index);
            }
            return null;
        }

        public String HeaderName
        {
            get { return _Header; }
        }
    }

    class CSVSchema<T>
    {
        private ProviderXLSRules<T> rules;

        public static SchemaElem<T> FreeSpace = new SchemaElem<T>(String.Empty, String.Empty);

        public CSVSchema(String file)
        {
            rules = new ProviderXLSRules<T>(file);
        }

        public String[] GetHeaders()
        {
            return rules.GetNames();
        }

        public Object GetValue(String name, SchemaProvider<T> hit, int index)
        {
            return rules.GetValue(name, hit, index);
        }

        public String GetTitle(SchemaProvider<T> hit)
        {
            return rules.GetTitle(hit);
        }

        public String GetFooter(SchemaProvider<T> hit)
        {
            return rules.GetFooter(hit);
        }

        public String GetFilename(SchemaProvider<T> hit)
        {
            return rules.GetFilename(hit);
        }

        public String GetFormat(String name)
        {
            return rules.GetFormat(name);
        }

        public String GetFormatAndReplaceWithId(String Name)
        {
            return rules.GetFormatCodeAndReplaceWithID(Name);
        }

        public String GetType(String Name)
        {
            return rules.GetType(Name);
        }

        public bool GetAutoFilter()
        {
            return rules.GetAutoFilter();
        }

        public FileFormat GetOutputFormat()
        {
            return rules.GetOutputFormat();
        }

        public String GetDelimiter()
        {
            return rules.GetDelimiter();
        }
    }

    class ExcelExporter<T>
    {
        protected StreamWriter writer;
        protected String Path;

        public ExcelExporter(String path)
        {
            Path = System.IO.Path.ChangeExtension(path, "xls");
            writer = new StreamWriter(Path, false, Encoding.GetEncoding("utf-8"));
        }

        public void Export(List<T> toExport, Project project, string schema_name)
        {
            CSVSchema<T> schema = LoadSchema(schema_name);
            String SheetName = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetFileName(Path));

            try
            {
                using (XmlWriter writer = XmlWriter.Create(this.writer))
                {
                    XMLHeader(writer);
                    WriteDocumentProperties(writer);
                    WriteDocumentStyles(writer, schema);
                    writer.WriteStartElement("Worksheet");
                    writer.WriteAttributeString("Name", "urn:schemas-microsoft-com:office:spreadsheet", "SheetName");
                    if (schema.GetAutoFilter())
                        WriteNamesClauseForHeader(writer, toExport.Count, schema.GetHeaders().GetLength(0));
                    writer.WriteStartElement("Table");

                    WriteColumnProperties(writer, schema);
                    WriteTitle(writer, project, schema, toExport);
                    WriteHeader(writer, schema);

                    int index = 0;
                    foreach (T partner in toExport)
                    {
                        index++;
                        writer.WriteStartElement("Row");
                        foreach (String name in schema.GetHeaders())
                        {
                            var value = schema.GetValue(name, new SchemaProvider<T>(partner, project), index);
                            WriteCell(writer, value, schema.GetType(name), schema.GetFormat(name), schema.GetAutoFilter());
                        }
                        writer.WriteEndElement();
                    }

                    WriteFooter(writer, project, schema, toExport);

                    writer.WriteEndElement();
                    if (schema.GetAutoFilter())
                        WriteAutoFilter(writer, toExport.Count, schema.GetHeaders().GetLength(0));
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            finally
            {
                writer.Close();
            }
        }

        private void WriteColumnProperties(XmlWriter writer, CSVSchema<T> schema)
        {
            foreach (String name in schema.GetHeaders())
            {
                writer.WriteStartElement("Column");
                writer.WriteAttributeString("ss", "StyleID", null, schema.GetFormat(name));
                writer.WriteEndElement();
            }
        }

        private void WriteNamesClauseForHeader(XmlWriter writer, int Rows, int Columns)
        {
            writer.WriteStartElement("Names");
            writer.WriteStartElement("NamedRange");
            writer.WriteAttributeString("Name", "urn:schemas-microsoft-com:office:spreadsheet", "_FilterDatabase");
            writer.WriteAttributeString("RefersTo", "urn:schemas-microsoft-com:office:spreadsheet", String.Format("R1C1:R{0}C{1}", Rows, Columns));
            writer.WriteAttributeString("Hidden", "urn:schemas-microsoft-com:office:spreadsheet", "1");
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        private void WriteAutoFilter(XmlWriter writer, int Rows, int Columns)
        {
            writer.WriteStartElement("AutoFilter", "urn:schemas-microsoft-com:office:excel");
            writer.WriteAttributeString("x", "Range", "urn:schemas-microsoft-com:office:excel", String.Format("R1C1:R{0}C{1}", Rows, Columns));
            //writer.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:excel");
            writer.WriteEndElement();
        }

        private void WriteCell(XmlWriter writer, Object value, String TypeName, String Format, bool autoFilter)
        {
            writer.WriteStartElement("Cell");

            if (!String.IsNullOrEmpty(Format))
            {
                writer.WriteAttributeString("ss", "StyleID", "urn:schemas-microsoft-com:office:spreadsheet", Format);
            }

            writer.WriteStartElement("Data");
            writer.WriteAttributeString("Type", "urn:schemas-microsoft-com:office:spreadsheet", TypeName);
            writer.WriteString(String.Format("{0}", value));
            writer.WriteEndElement();

            if (autoFilter)
            {
                writer.WriteStartElement("NamedCell");
                writer.WriteAttributeString("Name", "urn:schemas-microsoft-com:office:spreadsheet", "_FilterDatabase");
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private void WriteHeader(XmlWriter writer, CSVSchema<T> schema)
        {
            writer.WriteStartElement("Row");
            foreach (String name in schema.GetHeaders())
            {
                String Name = name;
                if (name.StartsWith("FreeSpace"))
                    Name = String.Empty;
                WriteCell(writer, Name, schema.GetType(name), schema.GetFormat(name), schema.GetAutoFilter());
            }
            writer.WriteEndElement();
        }

        private void WriteTitle(XmlWriter writer, Project project, CSVSchema<T> schema, List<T> toExport)
        {
            String title = schema.GetTitle(new SchemaProvider<T>(toExport.First(), project));
            if (String.IsNullOrEmpty(title))
                return;

            writer.WriteStartElement("Row");
            WriteCell(writer, title, schema.GetType(String.Empty), String.Empty, schema.GetAutoFilter());
            writer.WriteEndElement();
        }

        private void WriteFooter(XmlWriter writer, Project project, CSVSchema<T> schema, List<T> toExport)
        {
            String footer = schema.GetFooter(new SchemaProvider<T>(toExport.First(), project));
            if (String.IsNullOrEmpty(footer))
                return;

            writer.WriteStartElement("Row");
            WriteCell(writer, footer, String.Empty, String.Empty, schema.GetAutoFilter());
            writer.WriteEndElement();
        }

        private void WriteDocumentStyles(XmlWriter writer, CSVSchema<T> schema)
        {
            writer.WriteStartElement("Styles");

            foreach (String Name in schema.GetHeaders())
            {
                String Format = schema.GetFormatAndReplaceWithId(Name);
                String FormatID = schema.GetFormat(Name);
                String Type = schema.GetType(Name);

                if (String.IsNullOrEmpty(Format))
                    continue;

                writer.WriteStartElement("Style", "urn:schemas-microsoft-com:office:spreadsheet");
                writer.WriteAttributeString("ss", "ID", "urn:schemas-microsoft-com:office:spreadsheet", FormatID);

                writer.WriteStartElement("Alignment");
                writer.WriteAttributeString("ss", "Horizontal", "urn:schemas-microsoft-com:office:spreadsheet", "Center");
                writer.WriteAttributeString("ss", "Vertical", "urn:schemas-microsoft-com:office:spreadsheet", "Bottom");
                writer.WriteEndElement();

                writer.WriteStartElement("NumberFormat");
                writer.WriteAttributeString("ss", "Format", "urn:schemas-microsoft-com:office:spreadsheet", Format);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private void WriteDocumentProperties(XmlWriter writer)
        {
            writer.WriteStartElement("DocumentProperties", "urn:schemas-microsoft-com:office:office");
            writer.WriteElementString("Author", Environment.UserName);
            writer.WriteElementString("Created", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            writer.WriteElementString("Version", String.Format("{0}", 11.5606));
            writer.WriteEndElement();

            writer.WriteStartElement("ExcelWorkbook", "urn:schemas-microsoft-com:office:excel");
            writer.WriteElementString("ProtectStructure", "False");
            writer.WriteElementString("ProtectWindows", "False");
            writer.WriteEndElement();
        }

        private void XMLHeader(XmlWriter writer)
        {
            writer.WriteRaw("<?mso-application progid=\"Excel.Sheet\"?>");
            writer.WriteStartElement("Workbook", "urn:schemas-microsoft-com:office:spreadsheet");
            writer.WriteAttributeString("xmlns", "o", null, "urn:schemas-microsoft-com:office:office");
            writer.WriteAttributeString("xmlns", "x", null, "urn:schemas-microsoft-com:office:excel");
            writer.WriteAttributeString("xmlns", "ss", null, "urn:schemas-microsoft-com:office:spreadsheet");
            writer.WriteAttributeString("xmlns", "html", null, "http://www.w3.org/TR/REC-html40");
        }

        private CSVSchema<T> LoadSchema(String name)
        {
            String path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "schemas");
            return new CSVSchema<T>(System.IO.Path.Combine(path, System.IO.Path.ChangeExtension(name, "schema")));
        }
    }
}
