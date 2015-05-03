using System;
using System.Collections.Generic;

using MSWord = Microsoft.Office.Interop.Word;

namespace WANIRPartners.Utils.Doc
{
	class DocumentPrinter
	{
		static public void Print(string path, Object fillerObject)
		{
			Word word = new Word(false);
			try
			{
				using (WordDoc doc = word.Open(path))
				{
					foreach (Placeholder holder in doc.FindAllPlaceholders())
					{
						try
						{
							holder.Replace(fillerObject);
							holder.Dispose();
						}
						catch (KeyNotFoundException)
						{
						}
					}
                    doc.Print();
                    //doc.SaveAsPdf(string.Format(pdfDirectory));
                }
            }
            finally
            {
                word.Dispose();
            }
        }
    }
}
