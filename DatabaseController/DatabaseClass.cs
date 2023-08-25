using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace RoltaTimeSheet.DatabaseController
{
    public class DatabaseClass
    {
    }
    public class employeeData
    {
        public bool IsValid { get; internal set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeID { get; set; }
        public DBProjects billableProjects { get; set; }
        public NonBillableProjects NonBillableProjects { get; set; }
        public OfficeSupport officeSupport { get; set; }
        
    }
    public class ViewTimesheet
    {
        public string FullName { get; set; }
        public int EmployeeId { get; set; }
    }

    public class DBProjects
    {
        public IList<Projects> M_Projects { get; set; }
        public IList<Customers> M_Customers { get; set; }
        public IList<Segments> M_Segments { get; set; }
    }

    public class NonBillableProjects
    {
        public List<Projects> M_Projects { get; set; }
    }

    public class OfficeSupport
    {
        public List<Projects> M_Projects { get; set; }
    }
    public class Projects
    {
        public string projectName { get; set; }
        public int projectNumber { get; set; }
        public string projectType { get; set; }
    }


    public class Customers
    {
        public string customerName { get; set; }
        public int customerID { get; set; }
    }

    public class Segments
    {
        public string segmentName { get; set; }
        public int segmentID { get; set; }
    }
}


public class ProjectInfo
{
    public string CustomerName { get; set; }
    public int CustomerID { get; set; }
    public string SegmentName { get; set; }
    public int SegmentID { get; set; }
}

public class BillableData
{
    public string projectNumber { get; set; }      
    public string customerid { get; set; }
    public string segmentid { get; set; }
    public string monthYear { get; set; }
    public int hours { get; set; }
}

public class NonBillableData
{
    public string projectNumber { get; set; }
    public string monthYear { get; set; }
    public int hours { get; set; }
}

public class OfficeSupportData
{
    public string projectNumber { get; set; }
    public string monthYear { get; set; }
    public int hours { get; set; }
}

public class mainData
{
    public string refNo { get; set; }
    public List<BillableData> billableData { get; set; }
    public List<NonBillableData> nonBillableData { get; set; }
    public List<OfficeSupportData> officeSupportData { get; set; }
}