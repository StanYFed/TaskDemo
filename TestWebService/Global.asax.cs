using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TestWebService.Tests")]

namespace TestWebService
{
    using System.Web;
    using System.Web.Http;

    // TODO: Authorization?
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DIConfig.RegisterDependencies(GlobalConfiguration.Configuration);
        }
    }
}
