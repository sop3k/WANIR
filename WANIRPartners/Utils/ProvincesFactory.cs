using System;
using System.Collections.Generic;

namespace WANIRPartners.Utils
{
    class ProvincesFactory
    {
        private static string dataFile = "province.dat";
        public static Dictionary<String, List<String>> Initialize()
        {
            return LoadFromFile(dataFile);
        }

        private static Dictionary<String, List<String>> LoadFromFile(string filename)
        {
            Dictionary<String, List<String> > provinces = new Dictionary<string, List<string>>();
            
            string province = "";
            foreach (string line in System.IO.File.ReadLines(filename))
            {
                if (line.StartsWith(" "))
                {
                    string district = line.Trim();
                    provinces[province].Add(district);
                }
                else if(line.Trim().Length > 0)
                {
                    province = line;
                    provinces.Add(province, new List<string> { Const.NOT_SET });
                }
            }

            provinces.Add(Const.NOT_SET, new List<string> { Const.NOT_SET });
            return provinces;
        }
    }
}
