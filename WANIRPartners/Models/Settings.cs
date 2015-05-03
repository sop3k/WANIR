using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WANIRPartners.Models
{
    static class Settings
    {
        public static string CallReportPrintTemplatePath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 
            "WzorzecKartyRozmowy.doc");

        public static string PartnerPrintTemplatePath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "WzorzecKartyPartnera.doc");
    }
}
