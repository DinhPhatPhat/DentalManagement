using System.Web.Mvc;

namespace Dentalcare.Areas.dentist
{
    public class dentistAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "dentist";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "dentist_default",
                "dentist/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}