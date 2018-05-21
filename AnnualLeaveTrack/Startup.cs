using Microsoft.Owin;
using Owin;
using System.Collections.Generic;

[assembly: OwinStartupAttribute(typeof(AnnualLeaveTrack.Startup))]
namespace AnnualLeaveTrack
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            /*Project projObj = new Project();

            List<string> proj = new List<string>();
            proj = projObj.GetBUNames();
            */
            //string placeholder = "";
        }
    }
}
