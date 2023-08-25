using RoltaTimeSheet.DatabaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoltaTimeSheet.Models
{
    public class employeeModel
    {
        public bool IsValid { get; internal set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string month { get; set; }
        public int hours { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeID { get; set; }



        public DBProjects billableProjects { get; set; }
        public NonBillableProjects NonBillableProjects { get; set; }
        public OfficeSupport officeSupport { get; set; }
        
       

    }

    public class DBProjects
    {
        public List<Projects> M_Projects { get; set; }
        public List<Customers> M_Customers { get; set; }
        public List<Segments> M_Segments { get; set; }
        public List<Projects> billableProjectsNumbers { get; set; }
    }

    public class ProjectDetails
    {
        public int projectId { get; set; }
        public string projectName { get; set; }
        public string customerName { get; set; }
        public string segmentName { get; set; }
    }
    public class NonBillableProjects
    {
        public List<Projects> M_Projects { get; set; }
    }

    public class OfficeSupport
    {
        public List<Projects> M_Projects { get; set; }
    }
}


