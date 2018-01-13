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
            doc.Add(new Paragraph("Image"));

            Image gif = Image.GetInstance(pathToResources + @"\matt signature.png");

            doc.Add(gif);

            return Convert.ToBase64String(stream.ToArray());
        }
        public string SignFile(byte[] fileToSign, int pageNumber, byte[] userSignature, float x, float y)
        {
            //byte[] file = Convert.FromBase64String(base64);
            var stream = new MemoryStream(fileToSign);

            PdfReader pdfReader = new PdfReader(stream);
            //var sig = new MemoryStream(System.IO.File.ReadAllBytes(Path.Combine(pathToResources, "Matt Signature.png")));
            var newFile = new MemoryStream();
            PdfStamper pdfStamper = new PdfStamper(pdfReader, newFile);

            //Image image = Image.GetInstance(Path.Combine(pathToResources, "Matt Signature.png"));
            Image image = Image.GetInstance(userSignature.ToArray());
            image.ScaleAbsolute(150f, 75f);
            PdfContentByte content = pdfStamper.GetOverContent(pageNumber);
            //image.SetAbsolutePosition(pdfReader.GetPageSize(1).Width * x, pdfReader.GetPageSize(1).Height * y);
            //image.SetAbsolutePosition(content.co, 0f);
            image.SetAbsolutePosition(PageSize.LETTER.Width * (x * .4f), PageSize.LETTER.Height * (1-y));
            //image.SetAbsolutePosition(PageSize.LETTER.Width - image.ScaledWidth, 0);

            content.AddImage(image);

            pdfStamper.Close();

            return Convert.ToBase64String(newFile.ToArray());
        }
    }
}
