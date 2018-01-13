using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignMe3.Controllers
{ 
    public class DocumentController:Controller
    {
        //private static Libraries.GemBox PDFTool = new Libraries.GemBox();
        private static Libraries.TextSharp PDFTool = new Libraries.TextSharp();
        private int userid = 1;
        public string username = "mellison";
        public IActionResult Index()
        {
            using (var DataContext = new DocumentEntities())
            {
                var docs = (from x in DataContext.Documents
                           select new
                           {
                               Name = x.DocumentName,
                               ID = x.Id
                           }).ToList();

                return View(docs);

            }
        }

        public IActionResult Details(int id)
        {
            using (var DataContext = new DocumentEntities())
            {
                DocumentActivity.RecordActivity(DocumentActivityOptions.Viewed, id, userid);

                var doc = DataContext.Documents.First(x => x.Id == id);
                var vm = new ViewModels.SignFileViewModel
                {
                    DocumentID = doc.Id,
                    Filename = doc.DocumentName,
                    Base64 = doc.SignedBased64 ?? doc.Base64
                };

                return View(vm);
            }
             
        }

        public IActionResult ActivityHistory(int id)
        {
            using (var DataContext = new DocumentEntities())
            {
                var db = (from x in DataContext.ActivityHistories//.Where(x => x.DocumentID == id);
                          where x.DocumentID == id
                          select new
                          {
                              Status = x.DocumentStatu.Name,
                              InsertDate = x.InsertDate,
                              UserID = x.UserID
                          }).ToList();


                var result = from x in db
                             select new ViewModels.ActivityHistoryViewModel
                             {
                                 Status = x.Status,
                                 InsertDate = x.InsertDate.ToString("g"),
                                 UserName = username,
                                 UserID = x.UserID
                             };

                return Json(result);
            }

                
             
        }

        [HttpPost, HttpGet]
        public IActionResult SignImage(int id, int pageNumber, float x , float y )
        {
            using (var DataContext = new DocumentEntities())
            {
                DocumentActivity.RecordActivity(DocumentActivityOptions.Signed, id, userid);

                var db = new DocumentEntities();

                var file = Convert.FromBase64String(db.Documents.Find(id).Base64).ToArray();
                var userSignature = DataContext.UserSignatures.FirstOrDefault(sig => sig.UserName == username).SignatureBase64;
                var userSignatureBytes = Convert.FromBase64String(userSignature.Replace("image/png;base64,", ""));

                var signedFile = PDFTool.SignFile(file,id, pageNumber, userSignatureBytes, x, y);

                var doc = db.Documents.First(xx => xx.Id == id);
                doc.SignedBased64 = signedFile;
                db.SaveChanges();

                return Content(signedFile);
            }
             
        }

        
    }
}
