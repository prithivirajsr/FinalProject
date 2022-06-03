using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FinalProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Signup",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Signup", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Signin",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Signin", id = UrlParameter.Optional }
            );

            routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Member", action = "AllMembers", id = UrlParameter.Optional }
         );

            routes.MapRoute(
             name: "Event",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Event", action = "AllEvents", id = UrlParameter.Optional }
         );
        }
    }
}
