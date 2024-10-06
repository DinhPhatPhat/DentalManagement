using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dentalcare
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "About",
                url: "About",
                defaults: new { controller = "Default", action = "About", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Service",
                url: "Service",
                defaults: new { controller = "Default", action = "Service", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Doctors",
                url: "Doctors",
                defaults: new { controller = "Default", action = "Doctors", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Blog",
                url: "Blog",
                defaults: new { controller = "Default", action = "Blog", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Contact",
                url: "Contact",
                defaults: new { controller = "Default", action = "Contact", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Default", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Register",
                url: "Register",
                defaults: new { controller = "Default", action = "Register", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Home", id = UrlParameter.Optional }
            );

        }
    }
}
