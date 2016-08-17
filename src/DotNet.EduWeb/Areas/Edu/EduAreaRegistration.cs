using System.Web.Mvc;

namespace DotNet.EduWeb.Areas.Edu
{
    public class EduAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Edu";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Edu_default",
                "Edu/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}