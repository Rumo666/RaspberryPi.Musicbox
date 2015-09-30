using System.Web.Http;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Jukebox.Device.Web
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            // web api
            config.MapHttpAttributeRoutes();

            // static files
            appBuilder.UseFileServer(new FileServerOptions
            {
                FileSystem = new EmbeddedResourceFileSystem(typeof(Startup).Assembly, "Jukebox.Device.Web")
            });

            config.EnsureInitialized();

            appBuilder.UseWebApi(config);

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        } 
    }
}
