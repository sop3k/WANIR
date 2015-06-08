using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

using NHibernate;
using Excel;
using WANIRPartners.Models;

namespace WANIRPartners.Utils
{
    class PartnersExcelImporter
    {
        static public int Import(string path, ISession session, bool isFirstRowAsColumnNames)
        {
            var file = new FileInfo(path);
            using (var stream = new FileStream(path, FileMode.Open))
            {
                IExcelDataReader reader = null;
                if (file.Extension == ".xls")
                {
                   reader = ExcelReaderFactory.CreateBinaryReader(stream, Excel.ReadOption.Loose);
                }
                else if (file.Extension == ".xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }

                if (reader == null)
                    return 0;

                using(var tx = session.BeginTransaction())
                {
                    while (reader.Read())
                    {
                        Partner partner = new Partner
                        {
                            Name = reader.GetString(0),
                            Type = reader.GetString(1),
                            Province = reader.GetString(2),
                            District = reader.GetString(3),
                            Gmina = reader.GetString(4),
                            Address = reader.GetString(5),
                            Phone = reader.GetString(6),
                            Email = reader.GetString(7),
                            ContactPerson = reader.GetString(8),
                            Position = reader.GetString(9),
                            Department = reader.GetString(10),
                            ContactAddress = reader.GetString(11),
                            ContactPhone = reader.GetString(12),
                            ContactEmail = reader.GetString(13),
                            Region = reader.GetString(14),
                            SP = reader.GetInt32(15),
                            G = reader.GetInt32(16),
                            P = reader.GetInt32(17),
                            LO = reader.GetInt32(18),
                            ZSZTECH = reader.GetInt32(19),
                            Other = reader.GetString(20),
                            ProjectWritingAndRealization = Const.ConvertToBool(reader.GetString(21)) ?? false,
                            ProjectWriting = Const.ConvertToBool(reader.GetString(22)) ?? false,
                            ProjectMeeting = Const.ConvertToBool(reader.GetString(23)) ?? false,
                            ProjectRealization = Const.ConvertToBool(reader.GetString(24)) ?? false,
                            Comment = reader.GetString(25),
                            Projects = reader.GetString(26),
                            Aggrements = reader.GetString(27),
                            AcquiredBy = reader.GetString(28),
                            ServicedBy = reader.GetString(29),
                            Cooperation = !string.IsNullOrEmpty(reader.GetString(30))

                        };

                        session.Save(partner);

                    }

                    tx.Commit();
                    reader.Close();

                    return 0;
                }
            }
        }
    }
}
