using System.Web.Mvc;

namespace Dentalcare.Areas.receptionist
{
    public class receptionistAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "receptionist";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "receptionist_default",
                "receptionist/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}