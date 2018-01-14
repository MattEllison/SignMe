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
        public IActionResult Index()
        {
            using(DocumentEntities DataContext = new DocumentEntities())
            {

                ViewBag.Count = DataContext.Documents.Count();
                if (!DataContext.UserSignatures.Any(x=>x.UserName == "mellison"))
                {
                    return RedirectToAction("Index", "Signature", null);
                    
                }
                return View();
            }
            
        }







        //[HttpPost]
        //public IActionResult UploadFile(IFormFile file, double x = 1, double y = 1)
        //{


        //    ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        //    ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;

        //    DocumentModel document;// = DocumentModel.Load(fileContents, LoadOptions.TxtDefault);

        //    byte[] fileContents;
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        file.CopyTo(memoryStream);
        //        //fileContents = memoryStream.ToArray();
        //        document = DocumentModel.Load(memoryStream, DocTypes[Path.GetExtension(file.FileName)]);
        //        var options = SaveOptions.PdfDefault;

        //        document.Save(memoryStream, options);
        //        fileContents = memoryStream.ToArray();
        //    }

        //    //byte[] fileContents2;

        //    //// Save document to DOCX format in byte array.
        //    //using (var memoryStream = new MemoryStream())
        //    //{
        //    //    fileContents2 = memoryStream.ToArray();
        //    //}
        //    //return Convert.ToBase64String(fileContents);


        //    var db = new DocumentEntities();
        //    var newDoc = new Document()
        //    {
        //        DocumentName = file.FileName,
        //        Base64 = Convert.ToBase64String(fileContents)
        //    };
        //    db.Documents.Add(newDoc);
        //    db.SaveChanges();


        //    DocumentActivity.RecordActivity(DocumentActivityOptions.Created, newDoc.Id, userid: 1);
        //    return Json(newDoc.Id);

        //    //return Content(Convert.ToBase64String(fileContents));

        //}

        [HttpPost]
        public IActionResult UploadFile(IFormFile file, double x = 1, double y = 1)
        {
            var stream = new MemoryStream();
            file.CopyTo(stream);


            //string base64 = PDFTool.ConvertFile(file);
            //string base64 = Libraries.GemBox.ConvertFile(file);

            var db = new DocumentEntities();
            var newDoc = new Document()
            {
                DocumentName = file.FileName,
                Base64 = Convert.ToBase64String(stream.ToArray())
            };
            db.Documents.Add(newDoc);
            db.SaveChanges();


            DocumentActivity.RecordActivity(DocumentActivityOptions.Created, newDoc.Id, userid: 1);
            return Json(newDoc.Id);


        }




        public IActionResult Error()
        {
            return View();
        }
    }
}
