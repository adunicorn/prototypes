using System;
using System.Web.Http;
using System.Web.Http.Filters;
using Owin;

namespace IssuingService
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            appBuilder.UseWebApi(config);

            config.Filters.Add( new CustomHeaderFilter());
        }
    }

    public class CustomHeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.Add("server", Environment.MachineName);
            actionExecutedContext.Response.Headers.Add("timestamp", $"{DateTime.Now:u}");
        }
    }
}