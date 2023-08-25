using RoltaTimeSheet.Cache;
using RoltaTimeSheet.DatabaseController;
using RoltaTimeSheet.Models;
using RoltaTimeSheet.WebAPI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace RoltaTimeSheet.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Login(HomeModel homeModel)
        {
            ApplicationCache cache = ApplicationCache.Instance;

            if (homeModel != null && !string.IsNullOrEmpty(homeModel.Email) && !string.IsNullOrEmpty(homeModel.Password))//notemptycheck
            {
                TimesheetAPIController timesheetAPI = new TimesheetAPIController(); 

                employeeData response = timesheetAPI.Login(homeModel);//call webAPI (connects DB)

                if (response.IsValid) //check role, redirect based on role
                {
                    Session["Email"]= response.Email; 
                    Session["Role"] = response.Role;
                    Session["FirstName"] = response.FirstName;
                    Session["LastName"] = response.LastName;
                    Session["EmployeeID"] = response.EmployeeID;

                    string sessionEmail = Session["Email"] as string;
                    cache.Add("Email", sessionEmail);

                   string sessionEmployeeID = Session["EmployeeID"] as string;
                    cache.Add("EmployeeID", sessionEmployeeID);



                    if (response.Role.ToLower() == "employee")
                    {
                        
                        return RedirectToAction("EmployeeDashboard", "Employee");
                    }
                    else if (response.Role.ToLower() == "admin")
                    {
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid login credentials";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Email and password cannot be empty";
            }

            return View("Index");
        }

       
        public ActionResult Logout()
        {
            Session.Clear(); // clears session data
            return RedirectToAction("Index", "Home");
        }

        
    }
}

