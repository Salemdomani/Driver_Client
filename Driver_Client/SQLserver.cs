using System;
using System.Collections.Generic;
using System.Linq;

namespace Driver_Client
{
    class SQLserver
    {

        public static List<TODO> FindJobs()
        {
            SQLdb db = new SQLdb();
            return db.TODOs.Where(x => x.VMS == ShouldBeService.VmNum && x.state=="waiting").ToList();
        }

        public static void ThisJobIsDone(TODO todo)
        {
            SQLdb db = new SQLdb();
            db.TODOs.Single(x => x.Id == todo.Id).state = "done";
            db.SubmitChanges();
        }

        public static List<int> GetProfiles()
        {
            try
            {
                SQLdb db = new SQLdb();
                return (from p in db.Accounts
                        where p.Status == "Fine" && p.VMS == ShouldBeService.VmNum
                        orderby p.Profile
                        select p.Profile.Value)
                        .ToList();
            }
            catch (Exception){ return null; }
        }

        public static void ReportBlocked(int profile)
        {
            SQLdb db = new SQLdb();
            db.Accounts.Single(x=>x.VMS == ShouldBeService.VmNum && x.Profile == profile).Status = "Blocked";
            db.SubmitChanges();
        }

        public static void ReportVmIs(string state)
        {
            SQLdb db = new SQLdb();
            db.Vms.Single(x => x.Id == ShouldBeService.VmNum).Status = state;
            db.SubmitChanges();
        }
    }
}
