﻿using System;
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
        //private static Libraries.GemBox PDFTool = new Libraries.GemBox();
        private static Libraries.TextSharp PDFTool = new Libraries.TextSharp();

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


        [HttpPost]
        public IActionResult UploadFile(IFormFile file, double x = 1, double y = 1)
        {

            string base64 = PDFTool.ConvertFile(file);
            //string base64 = Libraries.GemBox.ConvertFile(file);

            var db = new DocumentEntities();
            var newDoc = new Document()
            {
                DocumentName = file.FileName,
                Base64 = base64
            };
            db.Documents.Add(newDoc);
            db.SaveChanges();


            DocumentActivity.RecordActivity(DocumentActivityOptions.Created, newDoc.Id, userid: 1);
            return Json(newDoc.Id);

            //return Content(Convert.ToBase64String(fileContents));

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

        [HttpPost, HttpGet]
        public IActionResult SignImage(int documentID, float x = 1, float y = 1)
        {
            var userid = 1;
            DocumentActivity.RecordActivity(DocumentActivityOptions.Signed, documentID, userid);

            var db = new DocumentEntities();

            var signedFile = PDFTool.SignFile(db.Documents.Find(documentID).Base64, x, y);

            db.Documents.Find(documentID).SignedBased64 = signedFile;
            db.SaveChanges();

            return Content(signedFile);
        }


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

        public IActionResult MySignature()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
