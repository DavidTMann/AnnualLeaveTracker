using AnnualLeaveTrack.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeaveTrack
{
    public class AnnualLeave
    {
        public string Dates;
        public double Days;
        public List<string> HalfDay;

        Utils util = new Utils();

        //Obj's for working out busy periods
        private List<ProjectBusyPeriodsHelper> projDateConflicts; //This holds the actual list of conflict objects
        private MembersBusyPeriodsHelper busyMemberDates;

        private double givenAnnualLeave = 25; //25 days given annual leave 

        public Employee GetMembersLeave(string userid)
        {
            Employee emp = new Employee();
            //Read json file contents
            String alTxt = util.ReadJsonFile("annual-leave.json");
            //Deserialize JSON into C# Obj
            Members allMembers = JsonConvert.DeserializeObject<Members>(alTxt);

            //Loop round members find members belonging to specific project
            for (int i = 0; i < allMembers.Employees.Count; i++)
            {
                if (allMembers.Employees[i].UserID.ToLower() == userid.ToLower())
                {
                    emp = allMembers.Employees[i];
                    break;
                }
            }

            return emp;
        }

        public double CalculateDaysBooked(Employee emp)
        {
            double daysBooked = 0;

            //Loop round emp.Leave and add to daysBooked
            foreach (var l in emp.Leave)
            {
                daysBooked += l.Days;
            }

            return daysBooked;
        }

        public double CalculateAnnLeaveLeft(Employee emp)
        {
            double daysBooked = CalculateDaysBooked(emp);
            //Take away daysBooked from given annual leave
            return givenAnnualLeave - daysBooked;
        }

        //This function takes two dates and returns all dates between, also removes weekend dates
        public List<DateTime> GetDatesBetweenTwoDates(DateTime d0, DateTime d1)
        {
            List<DateTime> dates = new List<DateTime>();
            List<DateTime> datesNoWeekends = new List<DateTime>();

            for (var dt = d0; dt <= d1; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            //Loop round dates, rid of weekends
            for (int i = 0; i < dates.Count; i++)
            {
                if (dates[i].DayOfWeek.ToString() == "Saturday" || dates[i].DayOfWeek.ToString() == "Sunday")
                {
                    //Don't add
                } else
                {
                    datesNoWeekends.Add(dates[i]);
                }
            }

            return datesNoWeekends;
        }

        //This function takes two given dates containing a half day, removes weekend dates & returns last day
        //to be rendered on front end as half day.
        //This function is no longer used, due to code amendments relating to Defect ID 1: Half days shown as last
        public DateTime GetLastHalfDay(DateTime d0, DateTime d1)
        {
            List<DateTime> dates = new List<DateTime>();
            List<DateTime> datesNoWeekends = new List<DateTime>();

            for (var dt = d0; dt <= d1; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            //Loop round dates, rid of weekends
            for (int i = 0; i < dates.Count; i++)
            {
                if (dates[i].DayOfWeek.ToString() == "Saturday" || dates[i].DayOfWeek.ToString() == "Sunday")
                {
                    //Don't add
                }
                else
                {
                    datesNoWeekends.Add(dates[i]);
                }
            }

            //Return last day as half day
            return datesNoWeekends.Max();
        }

        //This function takes emp.Leave as parameter and loops round each item to present friendly string of dates
        public string GetFriendlyStringOfDates(Employee emp)
        {
            string friendlyString = "";

            friendlyString += "<br/>";

            foreach (var l in emp.Leave)
            {
                friendlyString += l.Dates;
                friendlyString += " Days: " + l.Days;
                friendlyString += "<br/>";
            }
            friendlyString += "<br/>";

            return friendlyString;
        }
        
        //Identifies periods where members are on leave at same time
        public List<ProjectBusyPeriodsHelper> IdentifyBusyLeavePeriods(Project project)
        {
            List<DateTime> conflictingDates = new List<DateTime>(); //LIST OF ALL CONFLICTING DATES

            MemberBusyPeriodsHelper memberDates;
            busyMemberDates = new MembersBusyPeriodsHelper();
            projDateConflicts = new List<ProjectBusyPeriodsHelper>();
            List<DateTime> temp = new List<DateTime>();

            List<DateTime> listOfAllDates = new List<DateTime>();

            int totalMembers = project.Members.Employees.Count; //total members in project

            foreach (var emp in project.Members.Employees)
            {
                if (emp.Leave.Length > 0)
                {
                    memberDates = new MemberBusyPeriodsHelper();

                    foreach (var leave in emp.Leave)
                    {
                        if (leave.Dates.Contains('-'))
                        {
                            String[] datesArr = leave.Dates.Split('-');
                            DateTime d0 = Convert.ToDateTime(datesArr[0]);
                            DateTime d1 = Convert.ToDateTime(datesArr[1]);
                            temp = GetDatesBetweenTwoDates(d0, d1);
                        }
                        foreach (var d in temp)
                        {
                            memberDates.Dates.Add(d);
                            listOfAllDates.Add(d);
                        }
                    }

                    memberDates.Name = emp.FirstName + " " + emp.LastName;
                    busyMemberDates.members.Add(memberDates);
                }
            }

            //Inspect list of all dates for duplicates
            conflictingDates = listOfAllDates.GroupBy(s => s)
                .SelectMany(grp => grp.Skip(1)).Distinct().ToList();

            //Right now I can get a list of all conflicting dates..
            //Loop through members and see if any of the conflicting dates is contained in members dates

            //If busyMemberDates.members[i].Dates.Contains(date) then add 
            for (int i = 0; i < busyMemberDates.members.Count; i++)
            {
                foreach (DateTime d in conflictingDates)
                {
                    if (busyMemberDates.members[i].Dates.Contains(d))
                    {
                        //Set conflict to true
                        busyMemberDates.members[i].Conflict = true;
                        //Add to conflicting dates
                        busyMemberDates.members[i].ConflictingDates.Add(d);
                    }
                }
            }

            for (int i = 0; i < busyMemberDates.members.Count; i++)
            {
                if (busyMemberDates.members[i].Conflict)
                {
                    ProjectBusyPeriodsHelper conflictObj = new ProjectBusyPeriodsHelper();

                    conflictObj.ConflictingDates = busyMemberDates.members[i].ConflictingDates;
                    conflictObj.Name = busyMemberDates.members[i].Name;

                    projDateConflicts.Add(conflictObj);
                }
            }

            if (projDateConflicts.Count >= 1)
            {
                return projDateConflicts;
            }
            else
            {
                return null;
            }
        }
  }

    //HELPER CLASSES TO DECIPHER BUSY PERIODS
    class MembersBusyPeriodsHelper
    {
        public List<MemberBusyPeriodsHelper> members = new List<MemberBusyPeriodsHelper>();
    }

    class MemberBusyPeriodsHelper
    {
        public string Name = "";
        public List<DateTime> Dates = new List<DateTime>();
        public List<DateTime> ConflictingDates = new List<DateTime>();
        public bool Conflict = false;

    }

    public class ProjectBusyPeriodsHelper
    {
        public string Name = "";
        public List<DateTime> ConflictingDates = new List<DateTime>();

    }
}