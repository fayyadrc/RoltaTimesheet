using Newtonsoft.Json.Linq;
using RoltaTimeSheet.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;



namespace RoltaTimeSheet.DatabaseController
{
    public class DatabaseLogic
    {
        private readonly string connectionString;

        public DatabaseLogic()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        }



        public employeeData ValidateLogin(HomeModel homeModel)
        {
            string role = string.Empty;
            employeeData empData = new employeeData();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    DataTable data = new DataTable();

                    string query = @"SELECT e.email AS Email, e.password AS Password, r.Role, e.Fname, e.Lname, employee_id
                                     FROM Employee e
                 INNER JOIN Role r ON e.roleID = r.roleID
                 WHERE e.email = @Email AND e.password = @Password";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", homeModel.Email);
                        command.Parameters.AddWithValue("@Password", homeModel.Password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                empData.Email = reader["Email"].ToString();
                                empData.Password = reader["Password"].ToString();
                                empData.Role = reader["Role"].ToString();
                                empData.FirstName = reader["Fname"].ToString();
                                empData.LastName = reader["Lname"].ToString();
                                empData.EmployeeID = reader["employee_id"].ToString();
                                empData.IsValid = true;
                            }
                            else

                            {
                                empData.IsValid = false;
                            }
                        }
                    }

                }
                catch (Exception)
                {

                }
            }

            return empData;
        }

        public employeeData GetMasterData()
        {
            DataSet dataSet = new DataSet();
            employeeData data = new employeeData();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();


                    SqlCommand command = new SqlCommand("MasterData", connection);

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        dataAdapter.Fill(dataSet);
                    }
                    connection.Close();
                    if (dataSet != null && dataSet.Tables.Count > 0)
                    {
                        if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                        {
                            DBProjects billableProjects = new DBProjects();
                            NonBillableProjects nonBillableProjects = new NonBillableProjects();
                            OfficeSupport officeSupport = new OfficeSupport();


                            List<Projects> projects = new List<Projects>();
                            foreach (DataRow project in dataSet.Tables[0].Rows)
                            {
                                Projects proj = new Projects();
                                proj.projectName = project["projectName"].ToString();
                                proj.projectNumber = Convert.ToInt32(project["projectId"]);
                                proj.projectType = project["projectType"].ToString();
                                projects.Add(proj);
                            }
                            if (projects.Count > 0)
                            {
                                billableProjects.M_Projects = projects.Where(i => i.projectType == "Billable").ToList();
                                nonBillableProjects.M_Projects = projects.Where(i => i.projectType == "Non-billable").ToList(); ;
                                officeSupport.M_Projects = projects.Where(i => i.projectType == "Office Support").ToList(); ;
                            }


                            List<Customers> customers = new List<Customers>();
                            foreach (DataRow customer in dataSet.Tables[1].Rows)
                            {
                                Customers cust = new Customers();
                                cust.customerName = customer["customerName"].ToString();
                                cust.customerID = Convert.ToInt32(customer["customerID"]);
                                customers.Add(cust);
                            }
                            if (customers.Count > 0)
                            {
                                billableProjects.M_Customers = customers;
                            }


                            List<Segments> segments = new List<Segments>();
                            foreach (DataRow segment in dataSet.Tables[2].Rows)
                            {
                                Segments seg = new Segments();
                                seg.segmentName = segment["segmentName"].ToString();
                                seg.segmentID = Convert.ToInt32(segment["segmentID"]);
                                segments.Add(seg);
                            }
                            if (segments.Count > 0)
                            {
                                billableProjects.M_Segments = segments;
                            }

                            data.officeSupport = officeSupport;
                            data.billableProjects = billableProjects;
                            data.NonBillableProjects = nonBillableProjects;

                        }

                    }

                }
                catch (Exception)
                {

                }
            }


            return data;
        }

        public ProjectInfo GetProjectInfo(int projectNumber)
        {
            ProjectInfo projectInfo = new ProjectInfo();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetProjectInfo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@projectNumber", projectNumber);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            if (dataTable.Rows.Count > 0)
                            {
                                DataRow row = dataTable.Rows[0];
                                projectInfo.CustomerName = row["customerName"].ToString();
                                projectInfo.CustomerID = Convert.ToInt32(row["customerID"]);
                                projectInfo.SegmentName = row["segmentName"].ToString();
                                projectInfo.SegmentID = Convert.ToInt32(row["segmentID"]);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Handle the exception here
                }
            }

            return projectInfo;
        }


        public void InsertData(mainData MainData, string Email, string RefNo, string EmployeeID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                using (SqlCommand commandBillable = new SqlCommand("InsertBillableData", connection))
                {
                    try
                    {

                        commandBillable.CommandType = CommandType.StoredProcedure;
                        SqlParameter billableParam = commandBillable.Parameters.AddWithValue("@billableData", CreateBillableDataTable(MainData.billableData, Email, RefNo, EmployeeID));
                        billableParam.SqlDbType = SqlDbType.Structured;
                        billableParam.TypeName = "dbo.billableDataTableType";

                        commandBillable.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Billable: " + ex.Message);
                    }
                }


                using (SqlCommand commandNonBillable = new SqlCommand("InsertNonBillableData", connection))
                {
                    try
                    {
                        commandNonBillable.CommandType = CommandType.StoredProcedure;

                        SqlParameter nonBillableParam = commandNonBillable.Parameters.AddWithValue("@nonBillableData", CreateNonBillableDataTable(MainData.nonBillableData, RefNo, EmployeeID));
                        nonBillableParam.SqlDbType = SqlDbType.Structured;
                        nonBillableParam.TypeName = "dbo.nonBillableDataTableType";

                        commandNonBillable.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // Insert OfficeSupportData
                using (SqlCommand commandOfficeSupport = new SqlCommand("InsertOfficeSupportData", connection))
                {
                    try
                    {
                        commandOfficeSupport.CommandType = CommandType.StoredProcedure;

                        SqlParameter officeSupportParam = commandOfficeSupport.Parameters.AddWithValue("@officeSupportData", CreateOfficeSupportDataTable(MainData.officeSupportData, RefNo, EmployeeID));
                        officeSupportParam.SqlDbType = SqlDbType.Structured;
                        officeSupportParam.TypeName = "dbo.officeSupportDataTableType";

                        commandOfficeSupport.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                connection.Close();
            }
        }

        private DataTable CreateBillableDataTable(List<BillableData> billableData, string Email, string RefNo, string EmployeeID)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("projectNumber", typeof(string));
            dataTable.Columns.Add("customerid", typeof(string));
            dataTable.Columns.Add("segmentid", typeof(string));
            dataTable.Columns.Add("monthYear", typeof(DateTime));
            dataTable.Columns.Add("hours", typeof(int));
            dataTable.Columns.Add("RefNo", typeof(string));
            dataTable.Columns.Add("SubmittedBy", typeof(string));
            dataTable.Columns.Add("EmployeeID", typeof(string));

            foreach (var billableItem in billableData)
            {
                dataTable.Rows.Add(
                    billableItem.projectNumber,
                    billableItem.customerid,
                    billableItem.segmentid,
                    DateTime.Parse(billableItem.monthYear),
                    billableItem.hours,
                    RefNo,
                    Email,
                    EmployeeID
                );
            }

            return dataTable;
        }

        private DataTable CreateNonBillableDataTable(List<NonBillableData> nonBillableData, string RefNo, string EmployeeID)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("projectNumber", typeof(string));
            dataTable.Columns.Add("monthYear", typeof(DateTime));
            dataTable.Columns.Add("hours", typeof(int));
            dataTable.Columns.Add("RefNo", typeof(string));
            dataTable.Columns.Add("EmployeeID", typeof(string));

            foreach (var nonBillableItem in nonBillableData)
            {
                dataTable.Rows.Add(
                    nonBillableItem.projectNumber,
                    DateTime.Parse(nonBillableItem.monthYear),
                    nonBillableItem.hours,
                    RefNo,
                    EmployeeID
                );
            }

            return dataTable;
        }

        private DataTable CreateOfficeSupportDataTable(List<OfficeSupportData> officeSupportData, string RefNo, string EmployeeID)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("projectNumber", typeof(string));
            dataTable.Columns.Add("monthYear", typeof(DateTime));
            dataTable.Columns.Add("hours", typeof(int));
            dataTable.Columns.Add("RefNo", typeof(string));
            dataTable.Columns.Add("EmployeeID", typeof(string));

            foreach (var officeSupportItem in officeSupportData)
            {
                dataTable.Rows.Add(
                    officeSupportItem.projectNumber,
                    DateTime.Parse(officeSupportItem.monthYear),
                    officeSupportItem.hours,
                    RefNo,
                    EmployeeID
                );
            }

            return dataTable;

        }


        public List<ViewTimesheetModel> GetViewTimesheets()
        {
            List<ViewTimesheetModel> timesheets = new List<ViewTimesheetModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("GetEmployeeInfo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            ViewTimesheetModel timesheet = new ViewTimesheetModel();
                            timesheet.FullName = row["FullName"].ToString();
                            timesheet.EmployeeID = row["EmployeeID"].ToString();
                            timesheets.Add(timesheet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error: " + ex.Message);
                }


            }

            return timesheets;
        }

        public ViewTimesheetModel GetBillableDataByEmployeeID(string employeeID)
        {
            ViewTimesheetModel timesheetModel = new ViewTimesheetModel();
            timesheetModel.BillableData = new List<BillableDataModel>();
            timesheetModel.NonBillableData = new List<NonBillableDataModel>();
            timesheetModel.OfficeSupportData = new List<OfficeSupportDataModel>();
            mainData mainData = new mainData();

            try
            {
                Console.WriteLine("EmployeeID value: " + employeeID);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("BillableDataByEmployeeID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmployeeID", employeeID);
                        DataSet dataSet = new DataSet();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataSet);
                        }

                        if (dataSet.Tables.Count > 0)
                        {
                            DataTable billableTable = dataSet.Tables[0];
                            foreach (DataRow row in billableTable.Rows)
                            {
                                timesheetModel.BillableData.Add(new BillableDataModel
                                {
                                    RefNo = row["RefNo"] != DBNull.Value ? row["RefNo"].ToString() : string.Empty,
                                    ProjectNumber = Convert.ToInt32(row["projectNumber"]),
                                    ProjectName = row["projectName"] != DBNull.Value ? row["projectName"].ToString() : string.Empty,
                                    Customer = row["customerName"] != DBNull.Value ? row["customerName"].ToString() : string.Empty,
                                    Segment = row["segmentName"] != DBNull.Value ? row["segmentName"].ToString() : string.Empty,
                                    MonthYear = row["monthYear"] != DBNull.Value ? ((DateTime)row["monthYear"]).ToString("MM/yyyy") : string.Empty,
                                    Hours = row["hours"] != DBNull.Value ? Convert.ToInt32(row["hours"]) : 0
                                });
                            }
                        }

                        if (dataSet.Tables.Count > 0)
                        {
                            DataTable nonBillableTable = dataSet.Tables[1];
                            foreach (DataRow row in nonBillableTable.Rows)
                            {
                                timesheetModel.NonBillableData.Add(new NonBillableDataModel
                                {
                                    RefNo = row["RefNo"] != DBNull.Value ? row["RefNo"].ToString() : string.Empty,
                                    ProjectName = row["projectName"] != DBNull.Value ? row["projectName"].ToString() : string.Empty,
                                    //MonthYear = row["monthYear"] != DBNull.Value ? row["monthYear"].ToString() : string.Empty,
                                    MonthYear = row["monthYear"] != DBNull.Value ? ((DateTime)row["monthYear"]).ToString("MM/yyyy") : string.Empty,
                                    Hours = row["hours"] != DBNull.Value ? Convert.ToInt32(row["hours"]) : 0
                                });
                            }
                        }

                        if (dataSet.Tables.Count > 0)
                        {
                            DataTable officeSupportTable = dataSet.Tables[2];
                            foreach (DataRow row in officeSupportTable.Rows)
                            {
                                timesheetModel.OfficeSupportData.Add(new OfficeSupportDataModel
                                {
                                    RefNo = row["RefNo"] != DBNull.Value ? row["RefNo"].ToString() : string.Empty,
                                    ProjectName = row["projectName"] != DBNull.Value ? row["projectName"].ToString() : string.Empty,
                                    //MonthYear = row["monthYear"] != DBNull.Value ? row["monthYear"].ToString() : string.Empty,
                                    MonthYear = row["monthYear"] != DBNull.Value ? ((DateTime)row["monthYear"]).ToString("MM/yyyy") : string.Empty,
                                    Hours = row["Hours"] != DBNull.Value ? Convert.ToInt32(row["Hours"]) : 0
                                });
                            }
                        }

                    }
                }




               
            }
            catch (Exception ex)
            {
                // You can log the exception or perform any necessary error handling here
                Console.WriteLine("An error occurred: " + ex.Message);
                // Rethrow the exception if you want to propagate it further
                throw;
            }

            return timesheetModel;
        }

        







    }
}













































































