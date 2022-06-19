using System.Web.Mvc;
using System.Web.Routing;

namespace CodeNerdFilm
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Account",
                url: "dang-nhap/{action}/{id}",
                defaults: new { controller = "NguoiDung", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
