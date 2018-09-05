using iDiTect.Pdf.Actions;
using iDiTect.Pdf.Editing;
using iDiTect.Pdf.IO;
using iDiTect.Pdf.Licensing;
using iDiTect.Pdf.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace iDiTect.Pdf.Demo
{
    class Program
    {
        static void Main(string[] args)
        {           
            //This license registration line need to be at very beginning of our other code
            LicenseManager.SetKey("CKTCM-SP43J-72JFD-L979M-2KQMH-8WEEZ");


            //CreateLoadHelper.CreateNewPdfDocument();
            //TextHelper.AddText();
            ImageHelper.ConvertPDF2Image();
            //SecurityHelper.Encrypt();
            //FormFieldsHelper.CreateFormField();
            //ShapeHelper.AddShape();
            //LinkHelper.AddLinkInsideDocument();
            //TableHelper.AddTable2();
            //PageHelper.SplitPage();
            //SignatureHelper.AddTextSignature2PDF();
        }

       
    }
}
