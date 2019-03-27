﻿namespace LinksMediaCorp
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
           name: "DefaultChallenges",
           url: "{controller}/{action}/{id}",
           defaults: new { controller = "Reporting", action = "Challenges", id = UrlParameter.Optional }
       );     

        }
    }
}