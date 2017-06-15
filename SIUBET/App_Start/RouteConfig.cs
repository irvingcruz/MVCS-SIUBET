﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIUBET
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Movimiento",
                "movimiento/precrear/{ets}",
                new { controller = "Movimiento", action = "PreCrear", ets = "" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Expedientes", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
