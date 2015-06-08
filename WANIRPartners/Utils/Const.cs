using System;
using System.Collections.Generic;
using System.Linq;


namespace WANIRPartners.Utils
{
    class Const
    {
        public static string SEND_CAPTION = "Wyślij";
        public static string SEND_ALL_CAPTION = "Wyślij do wszystkich";
        public static string PRINT_CAPTION = "Drukuj";
        public static string EXPORT_CAPTION = "Exportuj";
        public static string IMPORT_CAPTION = "Importuj";
        public const string YES_CAPTION = "Tak";
        public const string NO_CAPTION = "Nie";
        public const string EXPORT_PROJECT_CAPTION = "Exportuj projekt";
        public const string ADD_PARTNER_CAPTION = "Dodaj partnera";

        public const string ADD_PROJECT_CAPTION = "Dodaj projekt";
        public const string DELETE_PROJECT_CAPTION = "Usuń projekt";

        public const string CALL_CAPTION = "Zadzwoń";
        public const string REMOVE_PROJECT = "Usuwanie projektu";
        public const string SAVE_CAPTION = "Zapisz";
        public const string CANCEL_CAPTION = "Anuluj";
        public const string ADD_CAPTION = "Dodaj";
        public const string DELETE_CAPTION = "Usuń";
        public const string EDIT_CAPTION = "Edytuj";

        public const string PARTNER_CREATE_CAPTION = "Tworzenie nowego partnera";
        public const string PROJECT_CREATE_CAPTION = "Tworzenie nowego projektu";

        public const string PROJECTS_CAPTION = "PROJEKTY";
        public const string PARTNERS_CAPTION = "PARTNERZY";
        public const string MAILING_PROJECTS_CAPTION = "MAILING";
        public const string SETTINGS_CAPTION = "USTAWIENIA";

        public const string OUTLOOK_OPEN = "Otwórz outlooka";

        public const string PARTNERS_SCHEMA = "Partners.schema";
        public const string PROJECTS_SCHEMA = "Projects.schema";

        public const string NOT_SET = "Brak";

        public static Dictionary<String, List<String>> Provinces
        {
            get { return Boostrapper.Provinces; }
        }

        public static List<string> Types
        {
            get
            {
                return new List<string>
                {
                    Const.NOT_SET,
                    "GMINA",
                    "POWIAT",
                    "OPS",
                    "PRZEDSZKOLE",
                    "SZKOŁA PODSTAWOWA",
                    "SZKOŁA GIMNAZJALNA",
                    "SZKOŁA ZAWODOWA/TECHNIKUM",
                    "SZKOŁA PONAD GIMNAZJALNA",
                    "INNY"
                };
            }
        }

        public static List<string> Regions
        {
            get
            {
                return new List<string>
                {
                    Const.NOT_SET,
                    "Wiejski",
                    "Miejski",
                    "Wiejsko-Miejski"
                };
            }
        }

        public static List<int> Years
        {
            get { return Enumerable.Range(2000, 20).ToList(); }
        }

        public static List<bool> Cooperation
        {
            get { return new List<bool> { true, false }; }
        }

        public static string Convert(bool? x)
        {
            if (x.HasValue)
                return x.Value ? YES_CAPTION : NO_CAPTION;
            else
                return NOT_SET;
        }

        public static bool? ConvertToBool(string x)
        {
            switch(x)
            {
                case Const.YES_CAPTION:
                    return true;
                case Const.NO_CAPTION:
                    return false;
                default:
                    return null;
            }
        }


    }
}
