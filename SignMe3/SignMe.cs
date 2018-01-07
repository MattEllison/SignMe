using GemBox.Document;
using GemBox.Document.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignMe3
{
    public static class SignMe
    {
        public static string SignFile(string base64, double x, double y )
        {
            byte[] file = Convert.FromBase64String(base64);
            Stream stream = new MemoryStream(file);

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;


            DocumentModel document = DocumentModel.Load(stream, LoadOptions.PdfDefault);

            //            DocumentModel document = DocumentModel.Load(@"c:\users\matth\documents\visual studio 2017\Projects\SignMe3\SignMe3\wwwroot\resources\file.pdf");

            var section = new Section(document);
            document.Sections.Add(section);

            Paragraph paragraph = new Paragraph(document);
            section.Blocks.Add(paragraph);


            var pathToResources = @"c:\users\matth\documents\visual studio 2017\Projects\SignMe3\SignMe3\wwwroot\images\";


            var shapeSize = new Size(3, 1, LengthUnit.Centimeter);


            var picture = new Picture(document, new MemoryStream(System.IO.File.ReadAllBytes(Path.Combine(pathToResources, "Matt Signature.png"))), PictureFormat.Png,
                new FloatingLayout(
                new HorizontalPosition(x * .9, LengthUnit.Pixel, HorizontalPositionAnchor.LeftMargin),
                new VerticalPosition(y * .9, LengthUnit.Pixel, VerticalPositionAnchor.TopMargin), shapeSize)
                { WrappingStyle = TextWrappingStyle.InFrontOfText }, ShapeType.Rectangle);

            picture.Outline.Width = 1;
            picture.Outline.Fill.SetSolid(Color.Blue);

            // Fill is visible because picture contains transparent pixels.
            picture.Fill.SetSolid(Color.Orange);
            paragraph.Inlines.Add(picture);

            document.Content.Start.InsertRange(paragraph.Content);

            byte[] fileContents;
            var options = SaveOptions.PdfDefault;

            // Save document to DOCX format in byte array.
            using (var memoryStream = new MemoryStream())
            {
                document.Save(memoryStream, options);
                fileContents = memoryStream.ToArray();
            }
            return Convert.ToBase64String(fileContents);

        }
    }
}
