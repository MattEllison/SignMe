using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignMe3.Libraries
{
    public class TextSharp
    {
        public static string pathToResources = @"c:\users\matth\documents\visual studio 2017\Projects\SignMe3\SignMe3\wwwroot\images\";
        public string ConvertFile(IFormFile file)
        {
            //string pdfpath = Server.MapPath("PDFs");
            //byte[] file = Convert.FromBase64String(base64);
            var stream = new MemoryStream();
            file.CopyTo(stream);
            Document doc = new Document();

            PdfWriter writer = PdfWriter.GetInstance(doc, stream);
            writer.Open();
            doc.Open();

            //PdfContentByte cb = writer.DirectContent;
            //doc.Add(new Paragraph("Image"));

            //Image gif = Image.GetInstance(pathToResources + @"\matt signature.png");

            //doc.Add(gif);

            return Convert.ToBase64String(stream.ToArray());
        }
        public string SignFile(byte[] fileToSign, int documentID, int pageNumber, byte[] userSignature, float x, float y)
        {
            var stream = new MemoryStream(fileToSign);

            PdfReader pdfReader = new PdfReader(stream);
            var newFile = new MemoryStream();
            PdfStamper pdfStamper = new PdfStamper(pdfReader, newFile);

            //Image image = Image.GetInstance(Path.Combine(pathToResources, "Matt Signature.png"));
            Image image = Image.GetInstance(userSignature.ToArray());
            image.ScaleAbsolute(150f, 75f);
            PdfContentByte content = pdfStamper.GetOverContent(pageNumber);
            //image.SetAbsolutePosition(pdfReader.GetPageSize(1).Width * x, pdfReader.GetPageSize(1).Height * y);
            //image.SetAbsolutePosition(content.co, 0f);
            image.SetAbsolutePosition(PageSize.LETTER.Width * (x * .9f), PageSize.LETTER.Height * (1-y));
            //image.SetAbsolutePosition(PageSize.LETTER.Width - image.ScaledWidth, 0);

            content.AddImage(image);

            pdfStamper.Close();
            var stamp = $"http://signme/Documents/{documentID}";
            var final = AddWaterMark(newFile.ToArray(), $"Signed {DateTime.Now.ToString("d")} ");
            var final2 = ApplyVerificationStamp2(final.ToArray(), stamp);
            return Convert.ToBase64String(final2.ToArray());
        }

        private MemoryStream ApplyVerificationStamp(byte[] stream, string stamp)
        {
            var newFile = new MemoryStream();

            PdfReader reader = new PdfReader(stream);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            // open the writer
            PdfWriter writer = PdfWriter.GetInstance(document, newFile);
            document.Open();

            // the pdf content
            PdfContentByte cb = writer.DirectContent;

            // select the font properties
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 8);

            // write the text in the pdf content
            cb.BeginText();
            string text = stamp;
            // put the alignment and coordinates here
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text, 5, 5, 0);
            cb.EndText();


            // create the new page and add it to the pdf
            for (int i = 1; i < reader.NumberOfPages; i++)
            {
                PdfImportedPage page = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page, 0, 0);
            }


            // close the streams and voilá the file should be changed :)
            document.Close();
            newFile.Close();
            writer.Close();
            reader.Close();
            return newFile;
        }

        private MemoryStream AddWaterMark(byte[] file, string stamp)
        {
            var newFile = new MemoryStream();
            // Creating watermark on a separate layer
            // Creating iTextSharp.text.pdf.PdfReader object to read the Existing PDF Document
            PdfReader reader1 = new PdfReader(file);
            //using (FileStream fs = new FileStream(watermarkedFile, FileMode.Create, FileAccess.Write, FileShare.None))
            // Creating iTextSharp.text.pdf.PdfStamper object to write Data from iTextSharp.text.pdf.PdfReader object to FileStream object

            using (PdfStamper stamper = new PdfStamper(reader1, newFile))
            {
                // Getting total number of pages of the Existing Document
                int pageCount = reader1.NumberOfPages;

                // Create New Layer for Watermark
                PdfLayer layer = new PdfLayer("WatermarkLayer", stamper.Writer);
                // Loop through each Page
                for (int i = 1; i <= pageCount; i++)
                {
                    // Getting the Page Size
                    Rectangle rect = reader1.GetPageSize(i);

                    // Get the ContentByte object
                    PdfContentByte cb = stamper.GetUnderContent(i);

                    // Tell the cb that the next commands should be "bound" to this new layer
                    cb.BeginLayer(layer);
                    cb.SetFontAndSize(BaseFont.CreateFont(
                      BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 50);

                    PdfGState gState = new PdfGState();
                    gState.FillOpacity = 0.25f;
                    cb.SetGState(gState);

                    cb.SetColorFill(BaseColor.BLACK);
                    cb.BeginText();
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, stamp, rect.Width / 2, rect.Height / 2, 45f);
                    cb.EndText();

                    // Close the layer
                    cb.EndLayer();
                }
            }






            return newFile;
        }



        private MemoryStream ApplyVerificationStamp2(byte[] file, string stamp)
        {
            var newFile = new MemoryStream();
            // Creating watermark on a separate layer
            // Creating iTextSharp.text.pdf.PdfReader object to read the Existing PDF Document
            PdfReader reader1 = new PdfReader(file);
            //using (FileStream fs = new FileStream(watermarkedFile, FileMode.Create, FileAccess.Write, FileShare.None))
            // Creating iTextSharp.text.pdf.PdfStamper object to write Data from iTextSharp.text.pdf.PdfReader object to FileStream object
            
            using (PdfStamper stamper = new PdfStamper(reader1, newFile))
            {
                // Getting total number of pages of the Existing Document
                int pageCount = reader1.NumberOfPages;

                // Create New Layer for Watermark
                PdfLayer layer = new PdfLayer("WatermarkLayer", stamper.Writer);
                // Loop through each Page
                for (int i = 1; i <= pageCount; i++)
                {
                    // Getting the Page Size
                    Rectangle rect = reader1.GetPageSize(i);

                    // Get the ContentByte object
                    PdfContentByte cb = stamper.GetUnderContent(i);

                    // Tell the cb that the next commands should be "bound" to this new layer
                    cb.BeginLayer(layer);
                    cb.SetFontAndSize(BaseFont.CreateFont(
                      BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 15);

                    PdfGState gState = new PdfGState();
                    gState.FillOpacity = 0.25f;
                    cb.SetGState(gState);

                    cb.SetColorFill(BaseColor.BLACK);
                    cb.BeginText();
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, stamp, 5, 5, 0);
                    cb.EndText();

                    // Close the layer
                    cb.EndLayer();
                }
            }






            return newFile;
        }
    }
}
