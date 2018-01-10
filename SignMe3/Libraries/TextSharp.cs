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
    public static class TextSharp
    {
        public static string ConvertFile(IFormFile file)
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
            var pathToResources = @"c:\users\matth\documents\visual studio 2017\Projects\SignMe3\SignMe3\wwwroot\images\";
            Image gif = Image.GetInstance(pathToResources + @"\matt signature.png");

            doc.Add(gif);

            return Convert.ToBase64String(stream.ToArray());
        }
        public static string SignFile(IFormFile file)
        {
            //string pdfpath = Server.MapPath("PDFs");
            //byte[] file = Convert.FromBase64String(base64);
            var stream = new MemoryStream();
            file.CopyTo(stream);
            Document doc = new Document();




            //PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfpath + "/Graphics.pdf", FileMode.Create));
            PdfWriter writer = PdfWriter.GetInstance(doc, stream);
            writer.Open();
            doc.Open();

            //PdfContentByte cb = writer.DirectContent;
            doc.Add(new Paragraph("GIF"));
            var pathToResources = @"c:\users\matth\documents\visual studio 2017\Projects\SignMe3\SignMe3\wwwroot\images\";
            Image gif = Image.GetInstance(pathToResources + @"\matt signature.png");

            doc.Add(gif);

            return Convert.ToBase64String(stream.ToArray());

        }
    }
}
