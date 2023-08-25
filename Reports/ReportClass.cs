using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoltaTimeSheet.Reports
{
    public class billable
    {
        public string ProjectNumber { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string SegmentName { get; set; }
        public string MonthYear { get; set; }
        public string EmployeeName { get; set; }
        public decimal Hours { get; set; }
    }

    public class nonBillable
    {
        public string ProjectNumber { get; set; }
        public string CustomerName { get; set; }
        public string SegmentName { get; set; }
        public string ProjectName { get; set; }
        public string MonthYear { get; set; }
        public string EmployeeName { get; set; }
        public decimal Hours { get; set; }
    }

    public class officeSupport
    {
        public string ProjectNumber { get; set; }
        public string CustomerName { get; set; }
        public string SegmentName { get; set; }
        public string ProjectName { get; set; }
        public string MonthYear { get; set; }
        public string EmployeeName { get; set; }
        public decimal Hours { get; set; }
    }

    public class TotalHours
    {
        public string EmployeeName { get; set; }
        public decimal Hours { get; set; }
    }
}