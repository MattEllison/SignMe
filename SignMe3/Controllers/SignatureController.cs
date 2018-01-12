using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccess;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignMe3.Controllers
{
    public class SignatureController : Controller
    {
        private static DocumentEntities DataContext = new DocumentEntities();
        private static string user = "mellison";
        // GET: /<controller>/
        public IActionResult Index()
        {
            var vm = (from x in DataContext.UserSignatures
                      where x.UserName == user
                      select new
                      {
                          Signature = x.SignatureBase64
                      }).FirstOrDefault();
            return View(vm);
        }
        public IActionResult Update(string signature)
        {
            var usersig = DataContext.UserSignatures.FirstOrDefault(x => x.UserName == user);
            if (usersig == null)
            {
                DataContext.UserSignatures.Add(new UserSignature
                {
                    SignatureBase64 = signature,
                    UserName = user
                });

            }
            else
            {
                usersig.SignatureBase64 = signature;
            }


            DataContext.SaveChanges();

            return Json(signature);
        }

        public IActionResult Delete()
        {
            var db = DataContext.UserSignatures.FirstOrDefault(x => x.UserName == user);
            DataContext.UserSignatures.Remove(db);
            var result = DataContext.SaveChanges();

            return Json(result);

        }
    }
}
