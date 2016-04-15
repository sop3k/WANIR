using System;
using System.Collections.Generic;
using System.Linq;
using MSWord = Microsoft.Office.Interop.Word;

namespace WANIRPartners.Utils.Doc
{
    class Word : IDisposable
    {
        MSWord.Application WordApp;
        List<WordDoc> Documents = new List<WordDoc>();

        public delegate void DocCloseHandler(MSWord.Document doc);
        public event DocCloseHandler OnDocClose;

        public Word(bool visible)
        {
            WordApp = new MSWord.Application();
            WordApp.Visible = visible;
            if ( visible )
                WordApp.Activate();

            WordApp.DocumentBeforeClose += new MSWord.ApplicationEvents4_DocumentBeforeCloseEventHandler(WordApp_DocumentBeforeClose);
        }

        void WordApp_DocumentBeforeClose(MSWord.Document doc, ref bool cancel)
        {
            if(OnDocClose != null)
                OnDocClose(doc);
        }

        public WordDoc Open( String path )
        {
            MSWord.Documents docs = WordApp.Documents;
            MSWord.Document doc;
            bool opened = false;

            try
            {
                Object filename = path;
                Object readOnly = false;

                doc = docs.Open(ref filename, ref Common.Missing, ref readOnly, ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing);

                opened = true;

                Documents.Add( new WordDoc( doc, path ) );
                return Documents.Last();
            }
            catch ( Exception )
            {
                if(opened)
                    docs.Close(ref Common.Missing, ref Common.Missing, ref Common.Missing);
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                foreach (WordDoc doc in Documents)
                {
                    doc.Dispose();
                }
            }
            catch (Exception e) { }

            WordApp.Quit(ref Common.Missing, ref Common.Missing, ref Common.Missing);
        }
    }

    class WordDoc : IDisposable
    {
        private MSWord.Document document;
        public String FullPath;

        public WordDoc( MSWord.Document doc, string path )
        {
            FullPath = path;
            document = doc;
            document.Activate();
        }

        public IEnumerable<Placeholder> FindAllPlaceholders()
        {
            return Utils.EnumeratorJoin<Placeholder>.Join(FindAllBlockPlaceholders(),
                FindAllFieldPlaceholders(), FindAllSinglePlaceholders());
        }

        public IEnumerable<Placeholder> FindMatchedPlaceholders(String regex, Type type)
        {
            String Regex = String.Format(@"(&{0}&)", regex);
            MSWord.Range WholeDoc = document.Content;

            WholeDoc.Find.ClearFormatting();

            //WholeDoc.Find.Forward = true;
            WholeDoc.Find.MatchWildcards = true;
            WholeDoc.Find.Wrap = MSWord.WdFindWrap.wdFindStop;

            WholeDoc.Find.Text = Regex;

            WholeDoc.Find.Execute(ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                   ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                   ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing);

            while (WholeDoc.Find.Found)
            {
                Object Start = WholeDoc.Start;
                Object End = WholeDoc.End;

                MSWord.Range FoundRange = document.Range(ref Start, ref End);

                WholeDoc.Find.Execute(ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                       ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                       ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing);

                var m = type.GetConstructor(new Type[]{typeof(MSWord.Range)});
                yield return (Placeholder)m.Invoke(new Object[]{FoundRange});
            }
        }

        public IEnumerable<Placeholder> FindAllBlockPlaceholders()
        {
            return FindMatchedPlaceholders(@"[0-9]@", typeof(BlockPlaceholder));
        }

        public IEnumerable<Placeholder> FindAllSinglePlaceholders()
        {
            return FindMatchedPlaceholders(@"[a-zA-Z_]@", typeof(Placeholder));
        }
            
        public IEnumerable<Placeholder> FindAllFieldPlaceholders()
        {
            foreach (MSWord.Field field in  document.Fields)
            {
                MSWord.Range rngFieldCode = field.Code;
                String fieldText = rngFieldCode.Text;

                if (fieldText.StartsWith(" MERGEFIELD"))
                {
                    Int32 endMerge = fieldText.IndexOf("\\");
                    String fieldName;

                    if (endMerge > 0)
                    {
                        Int32 fieldNameLength = fieldText.Length - endMerge;
                        fieldName = fieldText.Substring(11, endMerge - 11);
                        fieldName = fieldName.Trim();
                    }
                    else
                    {
                        fieldName = fieldText.Remove(0, " MERGEFIELD".Length);
                        fieldName = fieldName.Trim().Trim('\\').Trim('\"');
                    }

                    yield return new FieldPlaceholder(field, fieldName);
                } 
            }
        }

        public void Save()
        {
            document.Save();
        }

        public void SaveAsPdf(object filename)
        {
            document.ExportAsFixedFormat((string)filename, MSWord.WdExportFormat.wdExportFormatPDF);
        }

        public void Print()
        {
            Document.PrintOut(ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                  ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                  ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                  ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                  ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                  ref Common.Missing, ref Common.Missing, ref Common.Missing);
        }

        public void Dispose()
        {
            try
            {
                document.Close(MSWord.WdSaveOptions.wdDoNotSaveChanges, ref Common.Missing, ref Common.Missing);
            }
            catch (Exception e)
            {
                //best effort
            }
        }

        public String Filename
        {
            get { return System.IO.Path.GetFileName(document.FullName); }
        }

        public String Path
        {
            get { return document.FullName; }
        }

        public MSWord.Document Document
        {
            get { return document; }
        }
    }

    class Placeholder : IDisposable
    {
        protected MSWord.Range Range;
        public String PlaceholderName
        {
            get;
            protected set;
        }

        public Placeholder(String name)
        {
            PlaceholderName = name;
        }

        public Placeholder( MSWord.Range range )
        {
            Range = range;
            PlaceholderName = Range.Text.Trim( '&' );
        }

        public String GetAspect(Object target)
        {
            object value = null;
            object aspect = PlaceholderNameMapping.GetAspect(PlaceholderName);

            if (aspect is String)
            {
                Munger munger = new Munger((String)aspect);
                value = munger.GetValue(target);
            }

            if (value == null)
                return String.Empty;

            return value.ToString();
        }

        public virtual void Replace( Object data )
        {
            String value = GetAspect(data);

            Range.Find.ClearFormatting();
            Range.Find.Replacement.ClearFormatting();

            Range.Find.Forward = true;
            Range.Find.MatchWildcards = true;
            Range.Find.Wrap = MSWord.WdFindWrap.wdFindContinue;

            Range.Find.Text = Range.Text;
            Range.Find.Replacement.Text = string.Format("{0}", value);

            Object ReplaceOne = MSWord.WdReplace.wdReplaceOne;
            Range.Find.Execute( ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing,
                                ref ReplaceOne, ref Common.Missing, ref Common.Missing, ref Common.Missing, ref Common.Missing );
        }

        public virtual void Dispose()
        {
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Range);
        }
    }

    class FieldPlaceholder : Placeholder
    {
        private MSWord.Field Field;

        public FieldPlaceholder(MSWord.Field field, String fieldName)
            : base(fieldName)   
        {
            Field = field;
        }

        public override void Replace(Object data)
        {
            Field.Select();
            String value = GetAspect(data);

            if (String.IsNullOrEmpty(value))
                Field.Delete();
            else
                Field.Application.Selection.TypeText(value);
        }

        public override void Dispose()
        {
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(Field);
        }
    }

    class BlockPlaceholder : Placeholder
    {
        public BlockPlaceholder(MSWord.Range range)
            : base(range)
        {}

        public override void Replace(object data)
        {
            String fname = String.Format("{0}.doc", PlaceholderName);
            String blocksDir = "blocks"; //Move to config !!!
            String path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), blocksDir);
            String srcPath = System.IO.Path.Combine(path, fname);

            using (Word word = new Word(false))
            {
                WordDoc doc = word.Open(srcPath);

                MSWord.Document d = doc.Document;
                MSWord.Range r = d.Content;

                r.Copy();
                Range.Paste();
            }
        }
    }

    class PlaceholderNameMapping
    {
        static Dictionary<string, object> map = new Dictionary<string, object>()
            {
                {"NAME",                    "Name"},
                {"TYPE",                    "Type"},
                {"DISTRICT",                "District"},
                {"PROVINCE",                "Province"},
                {"GMINA",                   "Gmina"},
                {"ADDRESS",                 "Address"},
                {"PHONE",                   "Phone"},
                {"EMAIL",                   "Email"},
                {"CONTACT_PERSON",          "ContactPerson"},
                {"POSITION",                "Position"},
                {"DEPARTMENT",              "Department"},
                {"CONTACT_ADDRESS",         "ContactAddress"},
                {"CONTACT_PHONE",           "ContactPhone"},
                {"CONTACT_EMAIL",           "ContactEmail"},
                {"REGION",                  "Region"},
                {"SP",                      "SP"},
                {"G",                       "G"},
                {"LO",                      "LO"},
                {"P",                       "P"},
                {"ZSZ_TECH",                "ZSZTECH"},
                {"OTHERS",                  "Others"},
                {"PROJECT_WRITING_AND_REALIZATION", "ProjectWritingAndRealizationStr"},
                {"PROJECTS",                        "Projects"},
                {"PROJECT_WRITING",                 "ProjectWritingStr"},
                {"PROJECT_MEETING",                 "ProjectMeetingStr"},
                {"PROJECT_MEETING_DETAILS",         "ProjectMeetingDetails"},
                {"PROJECT_REALIZATION",             "ProjectRealizationStr"},
                {"PROJECT_REALIZATION_DETAILS",     "ProjectRealizationDetails"},
                {"COMMENT",                         "Comment"},
                {"ACQUIRED_BY",                     "AcquiredBy"},
                {"SERVICED_BY",                     "ServicedBy"},
                
                //CallInfo
                {"FIRST_CALLE",                     "FirstCallee"},
                {"FIRST_INFO",                      "FirstInfo"},
                {"FIRST_DATE",                      "FirstDateStr"},
                {"OFFER",                           "OfferStr"},
                {"SECOND_CALLEE",                   "SecondCallee"},
                {"SECOND_INFO",                     "SecondInfo"},
                {"SECOND_DATE",                     "SecondDateStr"},
                {"UNDECIDED",                       "Undecided"},
                {"RESIGNATION_REASON",              "ResignationReason"},
                {"MEETING_PERSON",                  "MeetingPerson"},
                {"ACTIVE_PROJECT",                  "ActiveProjectStr"},
                {"MEETING_DATE",                    "MeetingDate"},
                {"PROJECT_WRITER",                  "ProjectWriter"},
                {"MEETING_INFO",                    "MeetingInfo"}
            };

        public static Object GetAspect(String name)
        {
            String upper = name.ToUpper();
            if (map.ContainsKey(upper))
                return map[upper];
            return name;
        }
    }
}