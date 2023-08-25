using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RoltaTimeSheet
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "EmployeeDashboard",
                url: "Employee/Dashboard",
                defaults: new { controller = "Employee", action = "EmployeeDashboard" }
        );

            routes.MapRoute(
                name: "AdminDashboard",
                url: "Admin/Dashboard",
                defaults: new { controller = "Admin", action = "AdminDashboard" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
