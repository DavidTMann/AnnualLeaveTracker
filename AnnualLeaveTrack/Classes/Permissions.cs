using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeaveTrack.Classes
{
    public static class Permissions
    {
        internal static Boolean ValidateDomain(string userId)
        {
            //In real-world application, this function would:
            //Inspect domain and ensure it matches groupinfra
            /*if (userId.Split('\\')[0].ToUpper() != "GROUPINFRA")
            {
                return false;
            }
            else
            {
                return true;
            }
            */
            //COMMENT OUT for module leader testing

            return true;
        }
    }
}