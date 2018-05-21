using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnnualLeaveTrack;
using AnnualLeaveTrack.Classes;
using System.Collections.Generic;

namespace AnnualLeaveUnitTest
{
    [TestClass]
    public class ProjectUnitTests
    {
        //This unit test checks that project leave function works and returns relevant obj type of Project
        [TestMethod]
        public void GetProjLeaveTest()
        {
            Utils.testMode = true;

            Project proj = new Project();
            proj = proj.GetProjLeave("AM Dashboard");

            Assert.IsInstanceOfType(proj, typeof(Project));
        }

        //This unit test tests the get proj names function and ensures List<String> is returned
        //Pass in Government BU
        [TestMethod]
        public void GetProjNamesTest()
        {
            Project proj = new Project();
            List<String> projNames = proj.GetProjectNames("Government");

            Assert.IsInstanceOfType(projNames, typeof(List<String>));
        }

        //This unit test tests the get BU names function and ensures List<String> is returned
        [TestMethod]
        public void GetBUNamesTest()
        {
            Project proj = new Project();
            List<String> buNames = proj.GetBUNames();

            Assert.IsInstanceOfType(buNames, typeof(List<String>));
        }

    }
}
