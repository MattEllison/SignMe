using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignMe3.ViewModels
{
    public class SignFileViewModel
    {
        public string Filename { get; set; }
        public int UserId { get; set; }
        
        public IEnumerable<ActivityHistoryViewModel> ActivityHistory { get; set; }
        public string Base64 { get;  set; }
        public int DocumentID { get; internal set; }
    }

    public class ActivityHistoryViewModel
    {
        public string Status { get; set; }
        public DateTime InsertDate { get; set; }
        public int UserID { get; set; }
    }
}
