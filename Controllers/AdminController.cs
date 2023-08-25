using Microsoft.Reporting.WebForms;
using RoltaTimeSheet.DatabaseController;
using RoltaTimeSheet.Models;
using RoltaTimeSheet.Reports;
using RoltaTimeSheet.WebAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoltaTimeSheet.Controllers
{
    public class AdminController : Controller
    {

        [HttpGet]
        public ActionResult AdminDashboard(string id)
        {
            ViewTimesheetModel model = new ViewTimesheetModel();
            TimesheetAPIController timesheet = new TimesheetAPIController();
            model=timesheet.GetBillableDataByEmployeeID(id);
            if (model == null)
            {
                model = new ViewTimesheetModel();
            }
            model.EmployeeID = id;
            return View("AdminDashboard",model);
        }

        public ActionResult LogTimesheet()
        {
            
            return View("Employee/EmployeeDashboard");
        }
        public ActionResult ViewTimesheet()
        {


            return PartialView("ViewTimesheet");
        }

        public ActionResult AddUser()
        {

           return PartialView("AddUser");
        }

        public ActionResult AddProject()
        {
            
            return View("AddProject");
        }

        public DataTable GetBillableDataset()
        {
            List<billable> billableDataset = new List<billable>();
            DataTable billableDataTable = new DataTable();


            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT
                b.projectNumber,
                p.projectName,
                c.customerName,
                s.segmentName,
                b.monthYear,
                (SELECT CONCAT(Fname, ' ', Lname) FROM Employee WHERE employee_id = b.EmployeeID) ename,
                b.hours
            FROM
                billableData AS b
            INNER JOIN
                M_Projects AS p ON b.projectNumber = p.projectId
            INNER JOIN
                M_Customer AS c ON b.customerid = c.customerID
            INNER JOIN
                M_Segments AS s ON b.segmentid = s.segmentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            billable item = new billable
                            {
                                ProjectNumber = reader["projectNumber"].ToString(),
                                ProjectName = reader["projectName"].ToString(),
                                CustomerName = reader["customerName"].ToString(),
                                SegmentName = reader["segmentName"].ToString(),
                                MonthYear = reader["monthYear"].ToString(),
                                EmployeeName = reader["ename"].ToString(),
                                Hours = Convert.ToDecimal(reader["hours"])
                            };
                            billableDataset.Add(item);
                        }
                        if (true)
                        {

                            // Adding columns to the DataTable
                            billableDataTable.Columns.Add("projectNumber", typeof(string));
                            billableDataTable.Columns.Add("projectName", typeof(string));
                            billableDataTable.Columns.Add("customerName", typeof(string));
                            billableDataTable.Columns.Add("segmentName", typeof(string));
                            billableDataTable.Columns.Add("monthYear", typeof(string));
                            billableDataTable.Columns.Add("ename", typeof(string));
                            billableDataTable.Columns.Add("hours", typeof(decimal));

                            // Populating the DataTable from the IList<billable>
                            foreach (var item in billableDataset)
                            {
                                billableDataTable.Rows.Add(
                                    item.ProjectNumber,
                                    item.ProjectName,
                                    item.CustomerName,
                                    item.SegmentName,
                                    item.MonthYear,
                                    item.EmployeeName,
                                    item.Hours
                                );
                            }
                        }
                    }
                }
            }

            return billableDataTable;
        }

        public DataTable GetNonBillableDataset()
        {
            List<nonBillable> nonBillableDataset = new List<nonBillable>();
            DataTable NonbillableDataTable = new DataTable();

            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
        SELECT
            0 AS projectNumber,
            NULL AS customerName,
            NULL AS segmentName,
            p.projectName,
            nb.monthYear,
            (SELECT CONCAT(Fname, ' ', Lname) FROM Employee WHERE employee_id = nb.EmployeeID) ename,
            nb.hours
        FROM
            nonBillableData AS nb
        INNER JOIN
            M_Projects AS p ON nb.projectNumber = p.projectId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nonBillable item = new nonBillable
                            {
                                ProjectNumber = reader["projectNumber"].ToString(),
                                CustomerName = reader["customerName"]?.ToString(), // Handle null value
                                SegmentName = reader["segmentName"]?.ToString(),   // Handle null value
                                ProjectName = reader["projectName"].ToString(),
                                MonthYear = reader["monthYear"].ToString(),
                                EmployeeName = reader["ename"].ToString(),
                                Hours = Convert.ToDecimal(reader["hours"])
                            };
                            nonBillableDataset.Add(item);
                        }

                        // Adding columns to the DataTable
                        NonbillableDataTable.Columns.Add("projectNumber", typeof(string));
                        NonbillableDataTable.Columns.Add("customerName", typeof(string));
                        NonbillableDataTable.Columns.Add("segmentName", typeof(string));
                        NonbillableDataTable.Columns.Add("projectName", typeof(string));
                        NonbillableDataTable.Columns.Add("monthYear", typeof(string));
                        NonbillableDataTable.Columns.Add("ename", typeof(string));
                        NonbillableDataTable.Columns.Add("hours", typeof(decimal));

                        // Populating the DataTable from the List<nonBillable>
                        foreach (var item in nonBillableDataset)
                        {
                            NonbillableDataTable.Rows.Add(
                                item.ProjectNumber,
                                item.CustomerName,
                                item.SegmentName,
                                item.ProjectName,
                                item.MonthYear,
                                item.EmployeeName,
                                item.Hours
                            );
                        }
                    }
                }
            }

            return NonbillableDataTable;
        }

        public DataTable GetOfficeSupportDataset()
        {
            List<officeSupport> officeSupportDataset = new List<officeSupport>();
            DataTable officeSupportDataTable = new DataTable();

            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
        SELECT
            0 AS projectNumber,
            NULL AS customerName,
            NULL AS segmentName,
            p.projectName,
            os.monthYear,
            (SELECT CONCAT(Fname, ' ', Lname) FROM Employee WHERE employee_id = os.EmployeeID) ename,
            os.hours AS [Hours]
        FROM
            officeSupportData AS os
        INNER JOIN
            M_Projects AS p ON os.projectNumber = p.projectId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            officeSupport item = new officeSupport
                            {
                                ProjectNumber = reader["projectNumber"].ToString(),
                                CustomerName = reader["customerName"]?.ToString(),
                                SegmentName = reader["segmentName"]?.ToString(),
                                ProjectName = reader["projectName"].ToString(),
                                MonthYear = reader["monthYear"].ToString(),
                                EmployeeName = reader["ename"].ToString(),
                                Hours = Convert.ToDecimal(reader["Hours"])
                            };
                            officeSupportDataset.Add(item);
                        }

                        // Adding columns to the DataTable
                        officeSupportDataTable.Columns.Add("projectNumber", typeof(string));
                        officeSupportDataTable.Columns.Add("customerName", typeof(string));
                        officeSupportDataTable.Columns.Add("segmentName", typeof(string));
                        officeSupportDataTable.Columns.Add("projectName", typeof(string));
                        officeSupportDataTable.Columns.Add("monthYear", typeof(string));
                        officeSupportDataTable.Columns.Add("ename", typeof(string));
                        officeSupportDataTable.Columns.Add("Hours", typeof(decimal));

                        // Populating the DataTable from the List<officeSupport>
                        foreach (var item in officeSupportDataset)
                        {
                            officeSupportDataTable.Rows.Add(
                                item.ProjectNumber,
                                item.CustomerName,
                                item.SegmentName,
                                item.ProjectName,
                                item.MonthYear,
                                item.EmployeeName,
                                item.Hours
                            );
                        }
                    }
                }
            }

            return officeSupportDataTable;
        }

        public DataTable GetTotalHoursData()
        {
            List<TotalHours> totalHoursData = new List<TotalHours>();
            DataTable totalHoursDataTable = new DataTable();

            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
        SELECT
            ename,
            SUM(hours) AS totalHours
        FROM (
            SELECT
                (SELECT CONCAT(Fname, ' ', Lname) FROM Employee WHERE employee_id = b.EmployeeID) ename,
                b.hours
            FROM billableData AS b
            UNION ALL
            SELECT
                (SELECT CONCAT(Fname, ' ', Lname) FROM Employee WHERE employee_id = nb.EmployeeID) ename,
                nb.hours
            FROM nonBillableData AS nb
            UNION ALL
            SELECT
                (SELECT CONCAT(Fname, ' ', Lname) FROM Employee WHERE employee_id = os.EmployeeID) ename,
                os.hours
            FROM officeSupportData AS os
        ) AS combinedData
        GROUP BY ename
        ORDER BY ename";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TotalHours item = new TotalHours
                            {
                                EmployeeName = reader["ename"].ToString(),
                                Hours = Convert.ToInt32(reader["totalHours"])
                            };
                            totalHoursData.Add(item);
                        }

                        // Adding columns to the DataTable
                        totalHoursDataTable.Columns.Add("ename", typeof(string));
                        totalHoursDataTable.Columns.Add("totalHours", typeof(int));

                        // Populating the DataTable from the List<TotalHours>
                        foreach (var item in totalHoursData)
                        {
                            totalHoursDataTable.Rows.Add(
                                item.EmployeeName,
                                item.Hours
                            );
                        }
                    }
                }
            }

            return totalHoursDataTable;
        }


        public ActionResult ExportToExcel()
        {
            var reportViewer = new ReportViewer();

            string rdlFile = Server.MapPath("~/Reports/TimesheetReportv4.rdl");
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = rdlFile; // Set the report path

            // Retrieve data for each dataset
            DataTable billableData = GetBillableDataset();
            DataTable nonBillableData = GetNonBillableDataset();
            DataTable officeSupportData = GetOfficeSupportDataset();
            DataTable totalHoursData = GetTotalHoursData();

            // Create report data sources for each dataset
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("billable", billableData));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("nonBillable", nonBillableData));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("officeSupport", officeSupportData));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("TotalHours", totalHoursData));

            byte[] excelBytes = reportViewer.LocalReport.Render("Excel", null, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings);

            return File(excelBytes, mimeType, "RoltaTimesheet.xls");
        }


        [HttpGet]
    public ActionResult Logout()
        {
            
            Session.Clear();

            return RedirectToAction("Login", "Home"); 
        }


    }

}