using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnnualLeaveTrack;
using AnnualLeaveTrack.Classes;
using System.Collections.Generic;

namespace AnnualLeaveUnitTest
{
    //Suite of unit tests for Annual Leave class
    [TestClass]
    public class AnnualLeaveUnitTests
    {
        //This unit test gets leave for 'mannd' and checks that employee object is returned
        [TestMethod]
        public void GetMembersLeaveTest()
        { 
            //Set Util to testMode so that json file can be read without referring to server dir
            Utils.testMode = true;

            AnnualLeave al = new AnnualLeave();
            Employee emp = new Employee();
            emp = al.GetMembersLeave("mannd");

           Assert.IsInstanceOfType(emp, typeof(Employee));
        }

        //This unit test gets leave for 'mannd' and checks that employee.Leave object exists
        [TestMethod]
        public void GetMembersLeaveDataPresentTest()
        {
            //Set Util to testMode so that json file can be read without referring to server dir
            Utils.testMode = true;

            AnnualLeave al = new AnnualLeave();
            Employee emp = new Employee();
            emp = al.GetMembersLeave("mannd");

            Assert.AreNotEqual(emp.Leave, null);
        }

        //This unit test gets leave for 'mannd' and checks the calculation for leave days booked
        [TestMethod]
        public void CalculateDaysBookedTest()
        {
            //Set Util to testMode so that json file can be read without referring to server dir
            Utils.testMode = true;

            AnnualLeave al = new AnnualLeave();
            Employee emp = new Employee();
            emp = al.GetMembersLeave("mannd");

            double days = al.CalculateDaysBooked(emp); 

            //Test data is currently set to 5.5 for mannd
            Assert.AreEqual(days, 9); 
        }

        //This unit test gets leave for 'mannd' and calculates annual leave left
        //Static 25 minus 5.5, we expect 19.5
        [TestMethod]
        public void CalculateAnnualLeaveLeftTest()
        {
            //Set Util to testMode so that json file can be read without referring to server dir
            Utils.testMode = true;

            AnnualLeave al = new AnnualLeave();
            Employee emp = new Employee();
            emp = al.GetMembersLeave("mannd");

            double days = al.CalculateAnnLeaveLeft(emp);

            //Test data is currently set to 5.5 for mannd
            Assert.AreEqual(days, 16);
        }

        //This unit test tests the function that gets all dates between two given dates
        //excluding weekends
        [TestMethod]
        public void GetDatesBetweenTwoDatesTest()
        {
            AnnualLeave al = new AnnualLeave();
            DateTime d0 = Convert.ToDateTime("08/05/2018");
            DateTime d1 = Convert.ToDateTime("14/05/2018");

            List<DateTime> expectedDates = new List<DateTime>();
            expectedDates.Add(Convert.ToDateTime("08/05/2018"));
            expectedDates.Add(Convert.ToDateTime("09/05/2018"));
            expectedDates.Add(Convert.ToDateTime("10/05/2018"));
            expectedDates.Add(Convert.ToDateTime("11/05/2018"));
            expectedDates.Add(Convert.ToDateTime("14/05/2018"));

            List<DateTime> genList = al.GetDatesBetweenTwoDates(d0, d1);
            
            //Ensure date lists are equal, excluding weekends.
            CollectionAssert.AreEqual(genList, expectedDates);
        }

        //This unit test tests the function that gets the last date between two given dates
        //excluding weekends, this is used when rendering half day.
        [TestMethod]
        public void GetLastDateBetweenTwoDatesTest()
        {
            AnnualLeave al = new AnnualLeave();
            DateTime d0 = Convert.ToDateTime("08/05/2018");
            DateTime d1 = Convert.ToDateTime("14/05/2018");

            DateTime expectedDate = new DateTime();
            expectedDate = Convert.ToDateTime("14/05/2018");

            DateTime genDate = al.GetLastHalfDay(d0, d1);

            //Ensure dates are equal
            Assert.AreEqual(genDate, expectedDate);
        }

        //This unit test tests the function that identifies busy leave periods
        //It checks whether over 25%+ of the project are off at same time, and returns the dates
        //where this occurs.
        //Uses 'AM Dashboard' project for test data
        [TestMethod]
        public void GetBusyDateConflictsTest()
        {
            Utils.testMode = true;
            AnnualLeave al = new AnnualLeave();
            Project proj = new Project();

            proj = proj.GetProjLeave("AM Dashboard");
            List<ProjectBusyPeriodsHelper> conflicts = al.IdentifyBusyLeavePeriods(proj);
            List<DateTime> busyDates = conflicts[0].ConflictingDates;

            List<DateTime> expectedDates = new List<DateTime>();
            expectedDates.Add(Convert.ToDateTime("04/04/2018"));
            expectedDates.Add(Convert.ToDateTime("05/04/2018"));
            expectedDates.Add(Convert.ToDateTime("06/04/2018"));

            //Ensure dates are equal
            CollectionAssert.AreEqual(busyDates, expectedDates);
        }

    }
}
