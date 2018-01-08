using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GemBox.Document;
using System.IO;
using GemBox.Document.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using DataAccess;

namespace SignMe3.Controllers
{

    public class HomeController : Controller
    {
        private static DocumentEntities DataContext = new DocumentEntities();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignedFile(int id)
        {
            var doc = DataContext.Documents.Find(id);
            var vm = new ViewModels.SignFileViewModel
            {
                DocumentID = doc.Id,
                Filename = doc.DocumentName,
                Base64 = doc.SignedBased64 ?? doc.Base64,
                ActivityHistory = doc.ActivityHistories.Select(x => new
                ViewModels.ActivityHistoryViewModel
                {
                    Status = x.DocumentStatu.Name,
                    InsertDate = x.InsertDate.ToString("g"),
                    UserID = x.UserID
                })
            };

            //var history = db.ActivityHistories.Where(x => x.UserID == 1);
            return View(vm);
        }

        private Dictionary<string, LoadOptions> DocTypes = new Dictionary<string, LoadOptions>()
        {
            {".docx",LoadOptions.DocxDefault},
            {".doc",LoadOptions.DocDefault},
            {".pdf",LoadOptions.PdfDefault}
        };

        [HttpPost]
        public IActionResult UploadFile(IFormFile file, double x = 1, double y = 1)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;

            DocumentModel document;// = DocumentModel.Load(fileContents, LoadOptions.TxtDefault);

            byte[] fileContents;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                //fileContents = memoryStream.ToArray();
                document = DocumentModel.Load(memoryStream, DocTypes[Path.GetExtension(file.FileName)]);
                var options = SaveOptions.PdfDefault;

                document.Save(memoryStream, options);
                fileContents = memoryStream.ToArray();
            }

            //byte[] fileContents2;

            //// Save document to DOCX format in byte array.
            //using (var memoryStream = new MemoryStream())
            //{
            //    fileContents2 = memoryStream.ToArray();
            //}
            //return Convert.ToBase64String(fileContents);


            var db = new DocumentEntities();
            var newDoc = new Document()
            {
                DocumentName = file.FileName,
                Base64 = Convert.ToBase64String(fileContents)
            };
            db.Documents.Add(newDoc);
            db.SaveChanges();


            DocumentActivity.RecordActivity(DocumentActivityOptions.Created, newDoc.Id, userid: 1);
            return Json(newDoc.Id);

            //return Content(Convert.ToBase64String(fileContents));

        }

        [HttpPost, HttpGet]
        public IActionResult UpdateImage(int documentID, double x = 1, double y = 1)
        {
            var userid = 1;
            DocumentActivity.RecordActivity(DocumentActivityOptions.Signed, documentID, userid);

            var db = new DocumentEntities();

            var signedFile = SignMe.SignFile(db.Documents.Find(documentID).Base64, x, y);

            db.Documents.Find(documentID).SignedBased64 = signedFile;
            db.SaveChanges();

            return Content(signedFile);
        }

        //[HttpPost, HttpGet]
        //public IActionResult UpdateImage(string base64, double x = 1, double y = 1)
        //{
        //    byte[] file = Convert.FromBase64String(base64);
        //    Stream stream = new MemoryStream(file);

        //    ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        //    ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;


        //    DocumentModel document = DocumentModel.Load(stream, LoadOptions.PdfDefault);

        //    //            DocumentModel document = DocumentModel.Load(@"c:\users\matth\documents\visual studio 2017\Projects\SignMe3\SignMe3\wwwroot\resources\file.pdf");

        //    var section = new Section(document);
        //    document.Sections.Add(section);

        //    Paragraph paragraph = new Paragraph(document);
        //    section.Blocks.Add(paragraph);


        //    var pathToResources = @"c:\users\matth\documents\visual studio 2017\Projects\SignMe3\SignMe3\wwwroot\images\";


        //    var shapeSize = new Size(3, 1, LengthUnit.Centimeter);


        //    var picture = new Picture(document, new MemoryStream(System.IO.File.ReadAllBytes(Path.Combine(pathToResources, "Matt Signature.png"))), PictureFormat.Png,
        //        new FloatingLayout(
        //        new HorizontalPosition(x * .9, LengthUnit.Pixel, HorizontalPositionAnchor.LeftMargin),
        //        new VerticalPosition(y * .9, LengthUnit.Pixel, VerticalPositionAnchor.TopMargin), shapeSize)
        //        { WrappingStyle = TextWrappingStyle.InFrontOfText }, ShapeType.Rectangle);

        //    picture.Outline.Width = 1;
        //    picture.Outline.Fill.SetSolid(Color.Blue);


        //    //        var picture2 = new Picture(document, new MemoryStream(System.IO.File.ReadAllBytes(Path.Combine(pathToResources, "pic.jpg"))), PictureFormat.Png, new FloatingLayout(
        //    //new HorizontalPosition(HorizontalPositionType.Center, HorizontalPositionAnchor.Margin),
        //    //new VerticalPosition(VerticalPositionType.Center, VerticalPositionAnchor.Margin),
        //    //shapeSize)
        //    //        { WrappingStyle = TextWrappingStyle.InFrontOfText }, ShapeType.Oval);
        //    //        picture.Outline.Width = 5;
        //    //        picture.Outline.Fill.SetSolid(Color.Blue);

        //    // Fill is visible because picture contains transparent pixels.
        //    picture.Fill.SetSolid(Color.Orange);
        //    paragraph.Inlines.Add(picture);


        //    //document.Content.Start.InsertRange(new Paragraph(document, "Second paragraph.").Content);
        //    document.Content.Start.InsertRange(paragraph.Content);





        //    byte[] fileContents;
        //    var options = SaveOptions.PdfDefault;

        //    // Save document to DOCX format in byte array.
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        document.Save(memoryStream, options);
        //        fileContents = memoryStream.ToArray();
        //    }
        //    return Content(Convert.ToBase64String(fileContents));

        //    //using (var memoryStream = new MemoryStream())
        //    //{
        //    //    var file2 = response.Body;
        //    //    file2.CopyTo(memoryStream);
        //    //    //return View("FileUploaded", new { File = "data:document/pdf;base64," + Convert.ToBase64String(memoryStream.ToArray()) });
        //    //    return Content(Convert.ToBase64String(memoryStream.ToArray()));
        //    //    //return new FileStreamResult(memoryStream,"");
        //    //}

        //    //return Json(document);
        //}


        public IActionResult Documents()
        {
            var docs = from x in DataContext.Documents
                       select new
                       {
                           Name = x.DocumentName,
                           ID = x.Id
                       };

            return View(docs);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
