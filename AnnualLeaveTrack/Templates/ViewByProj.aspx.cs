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
    public partial class ViewByProj : System.Web.UI.Page
    {
        static Project proj = new Project();
        AnnualLeave al = new AnnualLeave();
        Utils util = new Utils();

        List<DateTime> list = new List<DateTime>();

        List<ProjCalendarHelper> projDates;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Permissions.ValidateDomain(Request.LogonUserIdentity.Name))
            {
                //Validated by domain
                if (!IsPostBack)
                {
                    //Hide Proj list until BU is selected
                    projDropDown.Visible = false;
                    selectProjBtn.Visible = false;

                    buDropDown.Visible = true;
                    selectBuBtn.Visible = true;

                    //Assign BU list to drop down data source
                    buDropDown.DataSource = proj.GetBUNames();
                    buDropDown.DataBind();
                    if (Session["BUValue"] != null)
                    {
                        buDropDown.SelectedValue = Session["BUValue"].ToString();
                    }

                    projGrid.Visible = false;
                    projCalendar.Visible = false;
                    conflictGrid.Visible = false;
                }

                getConflictBtn.Visible = false;
            }
            else
            {
                Response.Redirect("ValidationFail.aspx");
            }
        }

        protected void projCalendar_DayRender(object sender, DayRenderEventArgs e)
        {
            //Loop through projDates object, check each emp and render fulldays/halfdays appropriately
            //projDates comes from ProjCalendarHelper class
            if (list != null && projDates != null)
            {
                foreach (var emp in projDates)
                {
                    foreach (var f in emp.FullDays)
                    {
                        if (e.Day.Date == f.Date)
                        {
                            Literal lb = util.ReturnLineBreak();
                            e.Cell.Controls.Add(lb);

                            Label b = util.ReturnLabel(emp.Name);
                            e.Cell.Controls.Add(b);
                        }
                    }
                    foreach (var h in emp.HalfDays)
                    {
                        if (e.Day.Date == h.Date)
                        {
                            Literal lb = util.ReturnLineBreak();
                            e.Cell.Controls.Add(lb);

                            Label b = util.ReturnLabelHalfDay(emp.Name);
                            e.Cell.Controls.Add(b);
                        }
                    }
                }
            }
        }

        protected void projCalendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            GetProjectFilterDates();
        }

        protected void selectProjBtn_Click(object sender, EventArgs e)
        {
            GetProjectFilterDates();

            if (proj.Size > 0)
            {
                //Get earliest date and set visible month
                DateTime minDate = list.Min();
                projCalendar.VisibleDate = minDate;
            }
        }

        private void SetProjGrid()
        {
            //Create DataTable obj to be used as data source by gridview
            //Create DataRow to add
            DataTable dt = new DataTable();
            dt.Columns.Add("Full name");
            dt.Columns.Add("Leave dates");
            dt.Columns.Add("Days booked");
            dt.Columns.Add("Days left");

            foreach (var emp in proj.Members.Employees)
            {
                DataRow dr = dt.NewRow();
                //Fill each column in row
                dr[0] = emp.FirstName + " " + emp.LastName;
                dr[1] = al.GetFriendlyStringOfDates(emp); //HtmlEncode set to false on this column
                dr[2] = al.CalculateDaysBooked(emp);
                dr[3] = al.CalculateAnnLeaveLeft(emp);

                dt.Rows.Add(dr);
            }

            projGrid.DataSource = dt;
            projGrid.DataBind();

            //Fix width of columns
            projGrid.HeaderRow.Cells[0].Attributes["Width"] = "100px";
            projGrid.HeaderRow.Cells[1].Attributes["Width"] = "300px";
            projGrid.HeaderRow.Cells[2].Attributes["Width"] = "100px";
            projGrid.HeaderRow.Cells[3].Attributes["Width"] = "100px";
        }

        protected void getConflictBtn_Click(object sender, EventArgs e)
        {
            //Assign conflicting date values to grid if they exist
            conflictGrid.Visible = true;
            SetConflictGrid();

            GetProjectFilterDates();

            if (proj.Size > 0)
            {
                //Get earliest date and set visible month
                DateTime minDate = list.Min();
                projCalendar.VisibleDate = minDate;
            }

        }

        private void SetConflictGrid()
        {
            //Create DataTable obj to be used as data source by gridview
            //Create DataRow to add
            DataTable dt = new DataTable();
            dt.Columns.Add("Members");
            dt.Columns.Add("Conflicting Dates");

            //Get all Project Conflicts and set to data row values via loop
            List<ProjectBusyPeriodsHelper> projConflicts = al.IdentifyBusyLeavePeriods(proj);

            if (projConflicts != null)
            {
                foreach (var conflict in projConflicts)
                {
                    DataRow dr = dt.NewRow();
                    //Fill each column in row
                    dr[0] = conflict.Name;
                    foreach (var date in conflict.ConflictingDates)
                    {
                        dr[1] += date.ToShortDateString() + "<br/>"; //HtmlEncode set to false on this column
                    }
                    dt.Rows.Add(dr);
                }

            } else
            {
                DataRow dr = dt.NewRow();
                dr[0] = "No conflicts found in project";
                dr[1] = "N/A"; 
                dt.Rows.Add(dr);
            }

            conflictGrid.DataSource = dt;
            conflictGrid.DataBind();

            //Fix width of columns
            conflictGrid.HeaderRow.Cells[0].Attributes["Width"] = "100px";
            conflictGrid.HeaderRow.Cells[1].Attributes["Width"] = "300px";
        }

        private void GetProjectFilterDates()
        {       
            proj = proj.GetProjLeave(projDropDown.SelectedValue);
            

            if (proj != null)
            {
                if (proj.Size > 0)
                {
                    //Int counter to check whether project could have leave conflicts
                    int memWithLeave = 0;

                    //Make calendar visible
                    projCalendar.Visible = true;
                    projGrid.Visible = true;

                    List<DateTime> temp = new List<DateTime>();

                    //Assign row values to data grid
                    SetProjGrid();

                    //Each new empDates obj will be added to this list
                    projDates = new List<ProjCalendarHelper>();

                    //Split dates by - and store into list
                    foreach (var emp in proj.Members.Employees)
                    {
                        //Create new empDates obj and assign fullDays,halfDays
                        ProjCalendarHelper empDates = new ProjCalendarHelper();
                        empDates.Name = emp.FirstName + " " + emp.LastName;

                        if (emp.Leave != null)
                        {
                            if (emp.Leave.Count() > 0)
                            {
                                memWithLeave++;
                            }

                            foreach (var d in emp.Leave)
                            {
                                if (d.Dates.Contains('-'))
                                {
                                    String[] datesArr = d.Dates.Split('-');
                                    DateTime d0 = Convert.ToDateTime(datesArr[0]);
                                    DateTime d1 = Convert.ToDateTime(datesArr[1]);

                                    temp = al.GetDatesBetweenTwoDates(d0, d1);

                                    if (d.HalfDay != null)
                                    {
                                        for (int i = 0; i < d.HalfDay.Count; i++)
                                        {
                                            //Add converted datetime to halfdays 
                                            empDates.HalfDays.Add(Convert.ToDateTime(d.HalfDay[i]));
                                        }
                                    }
                                }
                                //Code needs to loop round each emp.Leave object, so make equal to tempList and loop to add to main list
                                foreach (var t in temp)
                                {
                                    empDates.FullDays.Add(t);
                                    list.Add(t); //this is used to set earliest visible date in calendar
                                }
                            }
                            //Remove halfDay dates from fullDays to avoid duplication
                            empDates.FullDays = empDates.FullDays.Except(empDates.HalfDays).ToList();
                            projDates.Add(empDates);
                        }
                    }

                    //Ensure more than one member has leave !null
                    //If more than one member in proj give option to show conflits
                    if (memWithLeave > 1)
                    {
                        getConflictBtn.Visible = true;
                    }
                }
                else
                {
                    //Make calendar invisible
                    projCalendar.Visible = false;
                    projGrid.Visible = false;
                    //Show msg no members in this proj
                    string message = "There are no members in this project";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + message + "');", true);
                    return;
                }
                
            }
        }

        protected void projDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            conflictGrid.Visible = false;
        }

        protected void buDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            conflictGrid.Visible = false;
            getConflictBtn.Visible = false;
            projCalendar.Visible = false;
            projGrid.Visible = false;
            projDropDown.Visible = false;
            selectProjBtn.Visible = false;
        }

        protected void selectBuBtn_Click(object sender, EventArgs e)
        {
            projDropDown.Visible = true;
            selectProjBtn.Visible = true;
            //Store selected value in Session otherwise its lost in postback
            Session["BUValue"] = buDropDown.SelectedValue;

            //Assign project list to drop down data source
            projDropDown.DataSource = proj.GetProjectNames(Session["BUValue"].ToString());
            projDropDown.DataBind();
                       
        }
    }
    //Helper class to assign properties best formatted for Calendar Control to render
    class ProjCalendarHelper
    {
        public string Name = "";
        public List<DateTime> FullDays = new List<DateTime>();
        public List<DateTime> HalfDays = new List<DateTime>();
    }
}