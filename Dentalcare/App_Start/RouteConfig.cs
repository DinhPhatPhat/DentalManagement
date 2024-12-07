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
                name: "Account",
                url: "tai-khoan",
                defaults: new { controller = "Account", action = "Login" },
                namespaces: new[] { "Dentalcare.Controllers" }
            );

            routes.MapRoute(
                name: "Dentist",
                url: "nha-si",
                defaults: new { controller = "Dentist", action = "Index" },
                namespaces: new[] { "Dentalcare.Controllers" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Default", action = "Contact" },
                namespaces: new[] { "Dentalcare.Controllers" }
            );

            routes.MapRoute(
                name: "Blog",
                url: "tin-tuc",
                defaults: new { controller = "Blog", action = "Index" },
                namespaces: new[] { "Dentalcare.Controllers" }
            );
            routes.MapRoute(
                name: "BlogDetail",
                url: "tin-tuc/{meta}",
                defaults: new { controller= "Blog", action = "Details", meta = UrlParameter.Optional },
                namespaces: new[] { "Dentalcare.Controllers" }
            );
            routes.MapRoute(
                name: "ServiceCategory",
                url: "dich-vu",
                defaults: new { controller = "ServiceCategory", action = "Index", meta = UrlParameter.Optional },
                namespaces: new[] { "Dentalcare.Controllers" }
            );
            routes.MapRoute(
                name: "ServiceCategoryDetails",
                url: "dich-vu/{meta}",
                defaults: new { controller = "ServiceCategory", action = "Details", meta = UrlParameter.Optional },
                namespaces: new[] { "Dentalcare.Controllers" }
            );
            routes.MapRoute(
                name: "Comments",
                url: "binh-luan",
                defaults: new { controller = "Comment", action = "Index" },
                namespaces: new[] { "Dentalcare.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login" },
                namespaces: new[] { "Dentalcare.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Home", id = UrlParameter.Optional },
                namespaces: new[] { "Dentalcare.Controllers" }
            );
            routes.MapRoute(
                name: "MenuRoute",  
                url: "{meta}",      
                defaults: new { controller = "Default", action = "PageByMeta" },
                namespaces: new[] { "Dentalcare.Controllers" }
            );




        }
    }
}
