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
        public  string ConvertFile(IFormFile file)
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
        public  string SignFile(string base64, float x, float  y)
        {
            byte[] file = Convert.FromBase64String(base64);
            var stream = new MemoryStream(file);

            PdfReader pdfReader = new PdfReader(stream);
            var sig = new MemoryStream(System.IO.File.ReadAllBytes(Path.Combine(pathToResources, "Matt Signature.png")));
            var newFile = new MemoryStream();
            PdfStamper pdfStamper = new PdfStamper(pdfReader, newFile);

            Image image = Image.GetInstance(Path.Combine(pathToResources, "Matt Signature.png"));
            image.ScaleAbsolute(150f, 75f);
            PdfContentByte content = pdfStamper.GetOverContent(1);
            image.SetAbsolutePosition(pdfReader.GetPageSize(1).Width * .5f, pdfReader.GetPageSize(1).Height * .5f);
            image.SetAbsolutePosition(0f, 0f);
            image.SetAbsolutePosition(PageSize.LETTER.Width - image.ScaledWidth, PageSize.LETTER.Height - image.ScaledHeight);
            image.SetAbsolutePosition(PageSize.LETTER.Width - image.ScaledWidth, 0);

            content.AddImage(image);

            pdfStamper.Close();

            return Convert.ToBase64String(newFile.ToArray());
        }
    }
}
