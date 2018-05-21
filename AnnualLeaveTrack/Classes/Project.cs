using AnnualLeaveTrack.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeaveTrack
{
    public class Project
    {
        public string Name;
        public int Size;
        public Members Members;

        Utils util = new Utils();

        public Project GetProjLeave(string project)
        {
            //Read json file contents
            String alTxt = util.ReadJsonFile("annual-leave.json");
            //Deserialize JSON into C# Obj
            Members allMembers = JsonConvert.DeserializeObject<Members>(alTxt);

            //Initialise empty User array for all project members belonging to given project name
            List<Employee> projMembers = new List<Employee>();
            
            //Loop round members find members belonging to specific project
            for (int i = 0; i < allMembers.Employees.Count; i++)
            {
                if (allMembers.Employees[i].Project == project)
                {
                    //Create User obj & add to projMembers object
                    Employee projMember = new Employee();
                    projMember = allMembers.Employees[i];
                    projMembers.Add(projMember);
                }
            }

            //Create new Project obj to return, assign relevant proj members & properties to obj
            Project proj = new Project();
            Members members = new Members() { Employees = projMembers };
            proj.Members = members;
            proj.Name = project;
            proj.Size = members.Employees.Count;

            return proj;
        }

        //Gets names of projects from projects.json
        public List<String> GetProjectNames(string businessUnit)
        {
            //Initialise projects String List
            List<String> projects = new List<string>();

            //Read json file contents
            String projTxt = util.ReadJsonFile("projects.json");
            //Deserialize JSON into C# Obj
            Projects projObj = JsonConvert.DeserializeObject<Projects>(projTxt);

            //projects = projObj.projects;            
            foreach (BusinessUnits proj in projObj.projects)
            {
               if (proj.BusinessUnit == businessUnit)
                {
                    foreach (String p in proj.Projects)
                    {
                        projects.Add(p);
                    }
                }
            }            

            return projects;
        }

        //Gets names of Business Units from projects.json
        public List<String> GetBUNames()
        {
            //Initialise projects String List
            List<String> bus = new List<string>();

            //Read json file contents
            String projTxt = util.ReadJsonFile("projects.json");
            //Deserialize JSON into C# Obj
            Projects projObj = JsonConvert.DeserializeObject<Projects>(projTxt);

            foreach (BusinessUnits bu in projObj.projects)
            {
                bus.Add(bu.BusinessUnit);
            }

            //projects = projObj.projects;

            return bus;
        }
    }
    
    public class Projects
    {
        public List<BusinessUnits> projects;
    }    
    public class BusinessUnits
    {
        public String BusinessUnit;
        public List<string> Projects;
    }
}