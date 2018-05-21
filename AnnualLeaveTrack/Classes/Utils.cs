using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI.WebControls;

namespace AnnualLeaveTrack.Classes
{
    public class Utils
    {
        public static bool testMode = false;

        //This function reads contents from json file and returns contents
        public String ReadJsonFile(String fileName)
        {
            string path;

            if (!testMode)
            {
                path = Path.Combine(HttpContext.Current.Server.MapPath("~"), @"data\" + fileName);
            } else
            {
                path = (Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\data\" + fileName);
            }
                        
            string file = File.ReadAllText(path);

            return file;
        }

        public System.Web.UI.WebControls.Label ReturnLabel(String name)
        {
            System.Web.UI.WebControls.Label b = new System.Web.UI.WebControls.Label();
            b.Font.Size = 8;
            b.Font.Bold = true;
            b.ForeColor = System.Drawing.ColorTranslator.FromHtml("#5FE011");
            //Add name to text
            b.Text = name;

            return b;
        }

        public System.Web.UI.WebControls.Label ReturnLabelHalfDay(String name)
        {
            System.Web.UI.WebControls.Label b = new System.Web.UI.WebControls.Label();
            b.Font.Size = 8;
            b.Font.Bold = true;
            b.ForeColor = System.Drawing.ColorTranslator.FromHtml("#5FE011");
            //Add name to text w/ 0.5
            b.Text = name + " 0.5";

            return b;
        }

        public Literal ReturnLineBreak()
        {
            Literal lineBreak = new Literal();
            lineBreak.Text = "<br /><br />";

            return lineBreak;
        }
    }
}