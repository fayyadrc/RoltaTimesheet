using RoltaTimeSheet.DatabaseController;
using RoltaTimeSheet.Models;
using RoltaTimeSheet.WebAPI;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RoltaTimeSheet.Controllers
{
    public class EmployeeController : Controller
    {

        public ActionResult EmployeeDashboard()
        {

            if (Session["Email"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            DatabaseLogic databaseLogic = new DatabaseLogic();
            employeeModel model = new employeeModel();
            TimesheetAPIController timesheet = new TimesheetAPIController();
            employeeData empData = new employeeData();
            empData = timesheet.GetmasterConfigData();
            model.billableProjects = FromEmpDataProjects(empData.billableProjects);
            model.NonBillableProjects = FromEmpDataProjects(empData.NonBillableProjects);
            model.officeSupport = FromEmpDataProjects(empData.officeSupport);

            ViewBag.Message = "This is your Employee Dashboard";


            return View("EmployeeDashboard", model);
        }

        public Models.DBProjects FromEmpDataProjects(DatabaseController.DBProjects empProjects)
        {

            Models.DBProjects dbProjects = new Models.DBProjects();
            dbProjects.M_Projects = (List<Projects>)empProjects.M_Projects;
            dbProjects.M_Customers = (List<Customers>)empProjects.M_Customers;
            dbProjects.M_Segments = (List<Segments>)empProjects.M_Segments;
            dbProjects.billableProjectsNumbers = (List<Projects>)empProjects.M_Projects;


            return dbProjects;
        }
        public Models.NonBillableProjects FromEmpDataProjects(DatabaseController.NonBillableProjects empProjects)
        {

            Models.NonBillableProjects dbProjects = new Models.NonBillableProjects();
            dbProjects.M_Projects = empProjects.M_Projects;

            return dbProjects;
        }
        public Models.OfficeSupport FromEmpDataProjects(DatabaseController.OfficeSupport empProjects)
        {

            Models.OfficeSupport dbProjects = new Models.OfficeSupport();
            dbProjects.M_Projects = empProjects.M_Projects;

            return dbProjects;
        }



    }
}
