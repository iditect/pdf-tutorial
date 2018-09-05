using iDiTect.Pdf.Editing;
using iDiTect.Pdf.Editing.Flow;
using iDiTect.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace iDiTect.Pdf.Demo
{
    public static class ImageHelper
    {
        public static void ConvertPDF2Image()
        {

            //Copy "x86" and "x64" folders from download package to your .NET project Bin folder.
            PdfConverter document = new PdfConverter("sample.pdf");
            //Default is 72, the higher DPI, the bigger size out image will be
            document.DPI = 96;
            //The value need to be 1-100. If set to 100, the converted image will take the
            //original quality with less time and memory. If set to 1, the converted image 
            //will be compressed to minimum size with more time and memory.
            //document.CompressedRatio = 80;

            for (int i = 0; i < document.PageCount; i++)
            {
                //The converted image will keep the original size of PDF page
                System.Drawing.Image pageImage = document.PageToImage(i);
                //To specific the converted image size by width and height
                //Image pageImage = document.PageToImage(i, 100, 150);
                //You can save this Image object to jpeg, tiff and png format to local file.
                //Or you can make it in memory to other use.
                pageImage.Save(i.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        public static void ConvertPDF2MultipageTiff()
        {

            //Copy "x86" and "x64" folders from download package to your .NET project Bin folder.
            PdfConverter document = new PdfConverter("sample.pdf");
            //Default is 72, the higher DPI, the bigger size out image will be
            document.DPI = 96;
            //Save pdf to multiple pages tiff to local file
            document.DocumentToMultiPageTiff("sample.tif");
            //Or save the multiple pages tiff in memory to other use
            //Image multipageTif = document.DocumentToMultiPageTiff();
        }

        public static void ConvertPdfToImgParallel(String[] Inputfiles)
        {

            //Copy "x86" and "x64" folders from download package to your .NET project Bin folder.
            Parallel.ForEach(Inputfiles, (currentFile) =>
            {
                PdfConverter document = new PdfConverter(currentFile);
                //Default is 72, the higher DPI, the bigger size out image will be
                document.DPI = 96;

                for (int i = 0; i < document.PageCount; i++)
                {
                    //The converted image will keep the original size of PDF page
                    System.Drawing.Image pageImage = document.PageToImage(i);
                    //To specific the converted image size by width and height
                    //Image pageImage = document.PageToImage(i, 100, 150);                  

                    Console.WriteLine(currentFile.ToString());

                    // Save converted image to png format
                    pageImage.Save(currentFile.Replace("INPUT", "OUTPUT") + i + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            });
        }

        public static void AddImage()
        {
            PdfFile pdfFile = new PdfFile();
            PdfDocument document;

            using (FileStream fs = File.OpenRead("sample.pdf"))
            {
                //Read pdf document from stream
                document = pdfFile.Import(fs);
            }
            //Get first page of pdf
            PdfPage page = document.Pages[0];
            //Create page level builder
            PageContentBuilder builder = new PageContentBuilder(page);

            //Add image using builder's DrawImage directly
            using (Stream imgStream = File.OpenRead("sample.jpg"))
            {
                //Set image position
                builder.Position.Translate(100, 100);

                //insert image as original size
                builder.DrawImage(imgStream);
                //insert image with customized size
                //builder.DrawImage(imgStream, new Size(80, 80));
            }

            //Add image using Block object
            using (Stream imgStream = File.OpenRead("sample2.jpg"))
            {
                //Set image position
                builder.Position.Translate(100, 400);

                Block block = new Block();
                //insert image as original size
                //block.InsertImage(imgStream);
                //insert image with customized size
                block.InsertImage(imgStream, new Size(100, 100));

                builder.DrawBlock(block);
            }

            using (FileStream fs = File.OpenWrite("InsertImage.pdf"))
            {
                pdfFile.Export(document, fs);
            }
        }

        public static void AddImageInLine()
        {           
            PdfDocument document = new PdfDocument();

            //Create document level builder
            using (PdfDocumentBuilder builder = new PdfDocumentBuilder(document))
            {
                builder.InsertParagraph();
                //You can insert same image to different position in page, or insert different images to respective position
                using (Stream sampleImage = File.OpenRead("sample.jpg"))
                {
                    builder.InsertImageInline(sampleImage, new Size(40, 40));
                    builder.InsertText(", second one:");
                    builder.InsertImageInline(sampleImage, new Size(100, 100));
                    builder.InsertText(" and third one:");
                    builder.InsertImageInline(sampleImage, new Size(100, 60));
                    builder.InsertText(" the end.");
                }
            }
                
            using (FileStream fs = File.OpenWrite("InsertImageInLine.pdf"))
            {
                PdfFile pdfFile = new PdfFile();
                pdfFile.Export(document, fs);
            }
        }

        public static void AddImageAsPage()
        {            
            PdfDocument document = new PdfDocument();
                       
            using (Stream imgStream = File.OpenRead("sample.jpg"))
            {
                iDiTect.Pdf.Resources.ImageSource image = new iDiTect.Pdf.Resources.ImageSource(imgStream);

                //Create a new page with image's size
                PdfPage page = new PdfPage();
                page.Size = new Size(image.Width, image.Height);
                PageContentBuilder builder = new PageContentBuilder(page);

                //draw image to page at position (0,0)
                builder.DrawImage(image);

                document.Pages.Add(page);
            }           

            using (FileStream fs = File.OpenWrite("ConvertImageToPdf.pdf"))
            {
                PdfFile pdfFile = new PdfFile();
                pdfFile.Export(document, fs);
            }
        }

        public static void AddImageWatermark()
        {
            PdfFile pdfFile = new PdfFile();
            PdfDocument document;

            using (FileStream fs = File.OpenRead("sample.pdf"))
            {
                //Read pdf document from stream
                document = pdfFile.Import(fs);
            }
            //Get first page of pdf
            PdfPage page = document.Pages[0];
            PageContentBuilder builder = new PageContentBuilder(page);

            //Set watermark image position
            builder.Position.Translate(100, 100);
            using (Stream stream = File.OpenRead("watermark.png"))
            {
                //Insert watermark image as original size
                builder.DrawImage(stream);
                //Insert watermark image in customized size
                //builder.DrawImage(stream, new Size(80, 80));
            }

            using (FileStream fs = File.OpenWrite("ImageWatermark.pdf"))
            {
                pdfFile.Export(document, fs);
            }
        }
    }
}
