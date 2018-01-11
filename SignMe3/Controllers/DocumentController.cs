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
        private static DocumentEntities DataContext = new DocumentEntities();
        //private static Libraries.GemBox PDFTool = new Libraries.GemBox();
        private static Libraries.TextSharp PDFTool = new Libraries.TextSharp();
        private int userid = 1;
        public IActionResult Index()
        {
            var docs = from x in DataContext.Documents
                       select new
                       {
                           Name = x.DocumentName,
                           ID = x.Id
                       };

            return View(docs);
        }
        public IActionResult Details(int id)
        {
            
            DocumentActivity.RecordActivity(DocumentActivityOptions.Viewed, id, userid);

            var doc = DataContext.Documents.Find(id);
            var vm = new ViewModels.SignFileViewModel
            {
                DocumentID = doc.Id,
                Filename = doc.DocumentName,
                Base64 = doc.SignedBased64 ?? doc.Base64
            };

            //var history = db.ActivityHistories.Where(x => x.UserID == 1);
            return View(vm);
        }

        public IActionResult ActivityHistory(int id)
        {
            var db = (from x in DataContext.ActivityHistories//.Where(x => x.DocumentID == id);
                      where x.DocumentID == id
                      select x).ToList();
            var result = from x in db
                         select new ViewModels.ActivityHistoryViewModel
                         {
                             Status = x.DocumentStatu.Name,
                             InsertDate = x.InsertDate.ToString("g"),
                             UserID = x.UserID
                         };
            return Json(result); 
        }

        [HttpPost, HttpGet]
        public IActionResult SignImage(int id, float x = 1, float y = 1)
        {
            DocumentActivity.RecordActivity(DocumentActivityOptions.Signed, id, userid);

            var db = new DocumentEntities();

            var signedFile = PDFTool.SignFile(db.Documents.Find(id).Base64, x, y);

            db.Documents.Find(id).SignedBased64 = signedFile;
            db.SaveChanges();

            return Content(signedFile);
        }

        
    }
}
