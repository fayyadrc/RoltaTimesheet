using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoltaTimeSheet.Models
{
    public class ViewTimesheetModel
    {
        public string FullName { get; set; }
        public string EmployeeID { get; set; }

        public List<ViewTimesheetModel> Timesheets { get; set; }
        public List<BillableDataModel> BillableData { get; set; }
        public List<NonBillableDataModel> NonBillableData { get; set; }
        public List<OfficeSupportDataModel> OfficeSupportData { get; set; }
    }


    public class BillableDataModel
    {
        public string RefNo { get; set; }
        public int ProjectNumber { get; set; }
        public string ProjectName { get; set; }
        public string Customer { get; set; }
        public string Segment { get; set; }
        public string MonthYear { get; set; }
        public int Hours { get; set; }
        public string FormattedMonthYear { get; internal set; }
    }
    public class NonBillableDataModel
    {
        public string RefNo { get; set; }
        public string ProjectName { get; set; }
        public string MonthYear { get; set; }
        public int Hours { get; set; }
        public string FormattedMonthYear { get; internal set; }
    }
    public class OfficeSupportDataModel
    {
        public string RefNo { get; set; }
        public string ProjectName { get; set; }
        public string MonthYear { get; set; }
        public int Hours { get; set; }
        public string FormattedMonthYear { get; internal set; }
    }

    

}