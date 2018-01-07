using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public enum DocumentActivityOptions
    {
        Created,
        Viewed,
        Signed
    }

    public static class DocumentActivity
    {
        public static void RecordActivity(DocumentActivityOptions activity, int documentID, int userid)
        {
            var db = new DocumentEntities();
            var newActivity = new ActivityHistory()
            {
                UserID = userid,
                DocumentID = documentID

            };
            
            switch (activity)
            {
                case DocumentActivityOptions.Created:
                    newActivity.StatusID = db.DocumentStatus.First(x => x.Code == DocumentActivityOptions.Created.ToString().ToLower()).Id;
                    break;

                case DocumentActivityOptions.Viewed:
                    newActivity.StatusID = db.DocumentStatus.First(x => x.Code == DocumentActivityOptions.Viewed.ToString().ToLower()).Id;
                    break;

                case DocumentActivityOptions.Signed:
                    newActivity.StatusID = db.DocumentStatus.First(x => x.Code == DocumentActivityOptions.Signed.ToString().ToLower()).Id;
                    break;

            }

            db.ActivityHistories.Add(newActivity);
            db.SaveChanges();


        }
    }
}
