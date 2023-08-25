using RoltaTimeSheet.Models;
using System;
using System.Collections.Generic;
using RoltaTimeSheet.DatabaseController;
//using System.Web.Mvc;
using System.Web.Http;
using System.Web;
using System.Web.Caching;
using RoltaTimeSheet.Cache;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
//using System.Web.Mvc;



namespace RoltaTimeSheet.WebAPI
{
    public class TimesheetAPIController : ApiController
    {
        private ApplicationCache cache = ApplicationCache.Instance;

        [HttpPost]
        [Route("api/TimesheetAPI/Login")]
        public employeeData Login(HomeModel homeModel)
        {
            //call db, validate login,get role
            DatabaseLogic database = new DatabaseLogic();
            employeeData employee = new employeeData();
            employee = database.ValidateLogin(homeModel);
            return employee;
        }
        [HttpGet]
        [Route("GetmasterConfigData")]
        public employeeData GetmasterConfigData()
        {
            DatabaseLogic databaseLogic = new DatabaseLogic();
            employeeData data = new employeeData();
            data = databaseLogic.GetMasterData();

            return data;
        }

        [HttpGet]
        [Route("GetNonBillableProjects")]
        public List<Projects> GetNonBillableProjects()
        {
            DatabaseLogic databaseLogic = new DatabaseLogic();
            employeeData data = new employeeData();
            data = databaseLogic.GetMasterData();

            return data.NonBillableProjects.M_Projects;
        }


        [HttpGet]
        [Route("GetOfficeSupportProjects")]
        public List<Projects> GetOfficeSupportProjects()
        {
            DatabaseLogic databaseLogic = new DatabaseLogic();
            employeeData data = new employeeData();
            data = databaseLogic.GetMasterData();

            return data.officeSupport.M_Projects;
        }

        [HttpGet]
        [Route("api/TimesheetAPI/GetProjectInfoByProNo")]
        public IHttpActionResult GetProjectInfoByProNo(string projectNumber)
        {
            if (string.IsNullOrEmpty(projectNumber) || !int.TryParse(projectNumber, out _))
            {
                return BadRequest("Invalid project number.");
            }

            DatabaseLogic databaseLogic = new DatabaseLogic();
            ProjectInfo projectInfo = databaseLogic.GetProjectInfo(Convert.ToInt32(projectNumber));

            if (projectInfo != null)
            {
                ProjectInfo projectInfoDto = new ProjectInfo
                {
                    CustomerName = projectInfo.CustomerName,
                    CustomerID = projectInfo.CustomerID,
                    SegmentName = projectInfo.SegmentName,
                    SegmentID = projectInfo.SegmentID
                };

                return Ok(projectInfoDto);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/TimesheetAPI/InsertData")]
        public IHttpActionResult InsertData([FromBody] mainData MainData)
        {
            try
            {
                string Email = cache.Get("Email") as string;
                string EmployeeID = cache.Get("EmployeeID") as string;
                DateTime now = DateTime.Now;
                string uniqueNumber = now.ToString("yyMMddHHmmssfff");
                string RefNo = uniqueNumber;

                DatabaseLogic databaseLogic = new DatabaseLogic();
                databaseLogic.InsertData(MainData, Email, RefNo, EmployeeID);




                return Ok(new { success = true, message = "Data saved successfully" });

            }
            catch (Exception ex)
            {
                // Handle exception here, you might want to log the error
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/TimesheetAPI/GetViewTimesheets")]
        public IHttpActionResult GetViewTimesheets()
        {
            try
            {
                DatabaseLogic databaseLogic = new DatabaseLogic();


                List<ViewTimesheetModel> timesheets = databaseLogic.GetViewTimesheets();

                return Ok(timesheets);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/TimesheetAPI/GetBillableDataByEmployeeID")]
        public ViewTimesheetModel GetBillableDataByEmployeeID(string employeeID)
        {
            ViewTimesheetModel billableData = new ViewTimesheetModel();
            try
            {
               
                DatabaseLogic databaseLogic = new DatabaseLogic();
                billableData = databaseLogic.GetBillableDataByEmployeeID(employeeID);

               
            }
            catch (Exception ex)
            {
                
                
            }
            return billableData;
        }

        //[HttpGet]
        //[Route("api/TimesheetAPI/GetNonBillableDataByEmployeeID")]
        //public IHttpActionResult GetNonBillableDataByEmployeeID(string employeeID)
        //{
        //    try
        //    {
        //        DatabaseLogic databaseLogic = new DatabaseLogic();
        //        List<NonBillableDataModel> nonBillableData = databaseLogic.GetNonBillableDataByEmployeeID(employeeID);
        //        return Ok(nonBillableData);
        //    }
        //    catch (Exception ex)
        //    {
               
        //        return InternalServerError(ex);
        //    }
        //}

        //[HttpGet]
        //[Route("api/TimesheetAPI/GetOfficeSupportDataByEmployeeID")]
        //public IHttpActionResult GetOfficeSupportDataByEmployeeID(string employeeID)
        //{
        //    try
        //    {
        //        DatabaseLogic databaseLogic = new DatabaseLogic();
        //        List<OfficeSupportDataModel> officeSupportData = databaseLogic.GetOfficeSupportDataByEmployeeID(employeeID);
        //        return Ok(officeSupportData);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}


    }
}




