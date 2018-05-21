using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeaveTrack.Classes
{
    public class Employee : User
    {
        public string StaffNum;
        public string MobileNum;
        public string Project;
        public AnnualLeave[] Leave;
        
    }
}