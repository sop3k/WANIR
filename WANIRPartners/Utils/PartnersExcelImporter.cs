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
                    //Skip headers
                    reader.Read();

                    while (reader.Read())
                    {
                        Partner partner = new Partner
                        {
                            Name = TypeUtils.GetStringFromReader(reader, 0),
                            Type = TypeUtils.GetStringFromReader(reader, 1),
                            Province = TypeUtils.GetStringFromReader(reader, 2),
                            District = TypeUtils.GetStringFromReader(reader, 3),
                            Gmina = TypeUtils.GetStringFromReader(reader, 4),
                            Address = TypeUtils.GetStringFromReader(reader, 5),
                            Phone = TypeUtils.GetStringFromReader(reader, 6),
                            Email = TypeUtils.GetStringFromReader(reader, 7),
                            ContactPerson = TypeUtils.GetStringFromReader(reader, 8),
                            Position = TypeUtils.GetStringFromReader(reader, 9),
                            Department = TypeUtils.GetStringFromReader(reader, 10),
                            ContactAddress = TypeUtils.GetStringFromReader(reader, 11),
                            ContactPhone = TypeUtils.GetStringFromReader(reader, 12),
                            ContactEmail = TypeUtils.GetStringFromReader(reader, 13),
                            Region = TypeUtils.GetStringFromReader(reader, 14),
                            SP = TypeUtils.GetIntFromReader(reader, 15),
                            G = TypeUtils.GetIntFromReader(reader, 16),
                            P = TypeUtils.GetIntFromReader(reader, 17),
                            LO = TypeUtils.GetIntFromReader(reader, 18),
                            ZSZTECH = TypeUtils.GetIntFromReader(reader, 19),
                            Other = TypeUtils.GetStringFromReader(reader, 20),
                            Cooperation = Const.ConvertToBool(TypeUtils.GetStringFromReader(reader, 21)) ?? false,
                            ProjectWritingAndRealization = Const.ConvertToBool(TypeUtils.GetStringFromReader(reader, 22)) ?? false,
                            ProjectWritingAndRealizationDetails = TypeUtils.GetStringFromReader(reader, 23),
                            ProjectWriting = Const.ConvertToBool(TypeUtils.GetStringFromReader(reader, 24)) ?? false,
                            ProjectWritingDetails = TypeUtils.GetStringFromReader(reader, 25),
                            ProjectMeeting = Const.ConvertToBool(TypeUtils.GetStringFromReader(reader, 26)) ?? false,
                            ProjectMeetingDetails = TypeUtils.GetStringFromReader(reader, 27),
                            ProjectRealization = Const.ConvertToBool(TypeUtils.GetStringFromReader(reader, 28)) ?? false,
                            ProjectRealizationDetails = TypeUtils.GetStringFromReader(reader, 29),
                            Comment = TypeUtils.GetStringFromReader(reader, 30),
                            AcquiredBy = TypeUtils.GetStringFromReader(reader, 31),
                            ServicedBy = TypeUtils.GetStringFromReader(reader, 32)
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
