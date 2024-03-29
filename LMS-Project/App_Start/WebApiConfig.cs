﻿using System.Net.Http.Headers;
using System.Web.Http;

namespace LMS_Project
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters
                  .JsonFormatter
                  .SupportedMediaTypes
                  .Add(new MediaTypeHeaderValue("text/html"));

            config.Formatters
                  .JsonFormatter
                  .SerializerSettings
                  .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}
