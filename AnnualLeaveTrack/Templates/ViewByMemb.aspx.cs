using AnnualLeaveTrack.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AnnualLeaveTrack.Templates
{
    public partial class ViewByMemb : System.Web.UI.Page
    {
        AnnualLeave al = new AnnualLeave();
        Employee emp = new Employee();
        Utils util = new Utils();

        List<DateTime> list = new List<DateTime>();
        List<DateTime> halfDays = new List<DateTime>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Permissions.ValidateDomain(Request.LogonUserIdentity.Name))
            {
                //Validated by domain
                if (!IsPostBack)
                {
                    userCalendar.Visible = false;
                    userGrid.Visible = false;
                }
            }
            else
            {
                Response.Redirect("ValidationFail.aspx");
            }
            
        }

        protected void userSearchBtn_Click(object sender, EventArgs e)
        {
            GetFilterDates();

            if (emp.Leave != null)
            {
                if (emp.Leave.Length > 0)
                {
                    //Get earliest date and set visible month
                    DateTime minDate = list.Min();
                    userCalendar.VisibleDate = minDate;
                }
            }           

        }

        protected void userCalendar_DayRender(object sender, DayRenderEventArgs e)
        {
            //Loop round leave date values from list variable
            //Render calendar cell if date matches with relevant name
            if (list != null)
            {
                foreach (var d in list)
                {
                    if (e.Day.Date == d.Date)
                    {
                        Literal lb = util.ReturnLineBreak();
                        e.Cell.Controls.Add(lb);

                        Label b = util.ReturnLabel(emp.FirstName + " " + emp.LastName);
                        e.Cell.Controls.Add(b);

                        Literal lb1 = util.ReturnLineBreak();
                        e.Cell.Controls.Add(lb1);

                        //Label b1 = util.returnLabelHalfDay("James T.");
                        //e.Cell.Controls.Add(b1);
                    }
                }
                //Render half days
                foreach (var d in halfDays)
                {
                    if (e.Day.Date == d.Date)
                    {
                        Literal lb = util.ReturnLineBreak();
                        e.Cell.Controls.Add(lb);

                        Label b = util.ReturnLabelHalfDay(emp.FirstName + " " + emp.LastName);
                        e.Cell.Controls.Add(b);

                        Literal lb1 = util.ReturnLineBreak();
                        e.Cell.Controls.Add(lb1);
                    }
                }
            }            
        }

        protected void userCalendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            GetFilterDates();
        }

        //Gets dates to input into user calendar
        //Additionally calls method to set gridview values
        private void GetFilterDates()
        {
            //Ensure search box contains value
            if (userSearchTxtBox.Text.Trim() != "")
            {
                //Grab user obj from JSON file
                emp = al.GetMembersLeave(userSearchTxtBox.Text.Trim());
            }
            else
            {
                //Show message no details for given userid
                string message = "No userid in search box.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + message + "');", true);
                return;
            }
            if (emp.Leave != null)
            {
                if (emp.Leave.Length > 0)
                {
                    //Make calendar visible
                    userCalendar.Visible = true;
                    userGrid.Visible = true;

                    //Assign row values to data grid
                    SetUserGrid();

                    list = new List<DateTime>();
                    halfDays = new List<DateTime>();
                    List<DateTime> temp = new List<DateTime>();
                    //Split dates by - and store into list
                    foreach (var l in emp.Leave)
                    {
                        if (l.Dates.Contains("-"))
                        {
                            String[] datesArr = l.Dates.Split('-');
                            DateTime d0 = Convert.ToDateTime(datesArr[0]);
                            DateTime d1 = Convert.ToDateTime(datesArr[1]);

                            temp = al.GetDatesBetweenTwoDates(d0, d1);

                            //If l.HalfDays exists
                            if (l.HalfDay != null)
                            {
                                for (int i = 0; i < l.HalfDay.Count; i++)
                                {
                                    //Add half day date to halfDays
                                    halfDays.Add(Convert.ToDateTime(l.HalfDay[i]));
                                }
                            }

                            //Code needs to loop round each emp.Leave object, so make equal to tempList and loop to add to main list
                            foreach (var d in temp)
                            {
                                list.Add(d);
                            }
                        }
                    }

                    //Remove halfDay dates from list to avoid duplication
                    list = list.Except(halfDays).ToList();
                } else
                {
                    userCalendar.Visible = false;
                    userGrid.Visible = false;
                    //Show message no details for given userid
                    string message = "No leave found for given userid.";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + message + "');", true);
                    return;
                }
            }            
            else
            {
                userCalendar.Visible = false;
                userGrid.Visible = false;
                //Show message no details for given userid
                string message = "No leave found for given userid.";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + message + "');", true);
                return; 
            }
        }

        private void SetUserGrid()
        {
            //Create DataTable obj to be used as data source by gridview
            //Create DataRow to add
            DataTable dt = new DataTable();
            dt.Columns.Add("Full name");
            dt.Columns.Add("Leave dates");
            dt.Columns.Add("Days booked");
            dt.Columns.Add("Days left");

            DataRow dr = dt.NewRow();
            //Fill each column in row
            dr[0] = emp.FirstName + " " + emp.LastName;
            dr[1] = al.GetFriendlyStringOfDates(emp); //HtmlEncode set to false on this column
            dr[2] = al.CalculateDaysBooked(emp);
            dr[3] = al.CalculateAnnLeaveLeft(emp);
            
            dt.Rows.Add(dr);

            userGrid.DataSource = dt;            
            userGrid.DataBind();

            //Fix width of columns
            userGrid.HeaderRow.Cells[0].Attributes["Width"] = "100px";
            userGrid.HeaderRow.Cells[1].Attributes["Width"] = "300px";
            userGrid.HeaderRow.Cells[2].Attributes["Width"] = "100px";
            userGrid.HeaderRow.Cells[3].Attributes["Width"] = "100px";

        }
    }
}