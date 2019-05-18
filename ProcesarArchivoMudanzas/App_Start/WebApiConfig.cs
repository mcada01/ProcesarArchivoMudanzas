using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace ProcesarArchivoMudanzas
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors(new EnableCorsAttribute("http://localhost:4200", "*", "*"));
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            //var formatter = GlobalConfiguration.Configuration.Formatters.Where(f => f is System.Net.Http.Formatting.JsonMediaTypeFormatter).FirstOrDefault();
            //if (!formatter.SupportedMediaTypes.Any(mt => mt.MediaType == "text/plain"))
            //    formatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

        }
    }
}
