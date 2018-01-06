using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GemBox.Document;
using System.IO;
using GemBox.Document.Drawing;

namespace SignMe3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(double h = 1, double v =1 )
        {

            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;

            DocumentModel document = DocumentModel.Load(@"c:\users\matth\documents\visual studio 2017\Projects\SignMe3\SignMe3\wwwroot\resources\file.pdf");

            var section = new Section(document);
            document.Sections.Add(section);

            Paragraph paragraph = new Paragraph(document);
            section.Blocks.Add(paragraph);


            var pathToResources = @"c:\users\matth\documents\visual studio 2017\Projects\SignMe3\SignMe3\wwwroot\images\";


            var shapeSize = new Size(3, 1, LengthUnit.Centimeter);


            var picture = new Picture(document, new MemoryStream(System.IO.File.ReadAllBytes(Path.Combine(pathToResources, "pic.jpg"))), PictureFormat.Png, new FloatingLayout(
    new HorizontalPosition(h * .9,LengthUnit.Pixel,HorizontalPositionAnchor.LeftMargin),
    new VerticalPosition(v * .9, LengthUnit.Pixel,VerticalPositionAnchor.TopMargin),
    shapeSize)
            { WrappingStyle = TextWrappingStyle.InFrontOfText }, ShapeType.Rectangle);
            picture.Outline.Width = 5;
            picture.Outline.Fill.SetSolid(Color.Blue);


    //        var picture2 = new Picture(document, new MemoryStream(System.IO.File.ReadAllBytes(Path.Combine(pathToResources, "pic.jpg"))), PictureFormat.Png, new FloatingLayout(
    //new HorizontalPosition(HorizontalPositionType.Center, HorizontalPositionAnchor.Margin),
    //new VerticalPosition(VerticalPositionType.Center, VerticalPositionAnchor.Margin),
    //shapeSize)
    //        { WrappingStyle = TextWrappingStyle.InFrontOfText }, ShapeType.Oval);
    //        picture.Outline.Width = 5;
    //        picture.Outline.Fill.SetSolid(Color.Blue);

            // Fill is visible because picture contains transparent pixels.
            picture.Fill.SetSolid(Color.Orange);
            paragraph.Inlines.Add(picture);


            //document.Content.Start.InsertRange(new Paragraph(document, "Second paragraph.").Content);
            document.Content.Start.InsertRange(paragraph.Content);
            




            document.Save(pathToResources + @"modified.pdf");

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
