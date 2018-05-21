using AnnualLeaveTrack.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AnnualLeaveTrack.Templates
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Permissions.ValidateDomain(Request.LogonUserIdentity.Name))
            {
                //Validated by domain
            }
            else
            {
                Response.Redirect("ValidationFail.aspx");
            }            
        }
    }
}