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
                name: "Blog",
                url: "tin-tuc",
                defaults: new { controller = "Blog", action = "Index" }
            );
            routes.MapRoute(
                name: "ServiceCategory",
                url: "dich-vu",
                defaults: new { controller = "ServiceCategory", action = "Index", meta = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ServiceCategoryDetails",
                url: "dich-vu/{meta}",
                defaults: new { controller = "ServiceCategory", action = "Details", meta = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Comments",
                url: "binh-luan",
                defaults: new { controller = "Comment", action = "Index" }
            );
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
