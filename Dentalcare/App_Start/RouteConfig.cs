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
                name: "Register", 
                url: "dang-ky", 
                defaults: new { controller = "Default", action = "Register" }
            );
            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "Default", action = "Login" }
            );
            routes.MapRoute(
                name: "MenuRoute",  // Tên route có thể tùy chỉnh
                url: "{meta}",      // Dùng 'meta' để thay thế cho URL
                defaults: new { controller = "Default", action = "PageByMeta" }  // Điều hướng về controller và action tương ứng
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Home", id = UrlParameter.Optional }
            );


        }
    }
}
