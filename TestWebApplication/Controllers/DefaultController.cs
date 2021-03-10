namespace TestWebApplication.Controllers
{
    using System.Configuration;
    using System.Web.Mvc;

    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            this.ViewBag.WebServiceRoot = GetWebServiceRoot();
            return this.View();
        }

        private string GetWebServiceRoot()
        {
            var webServiceRoot = ConfigurationManager.AppSettings.Get("WebServiceRoot");
            if (string.IsNullOrWhiteSpace(webServiceRoot))
            {
                throw new ConfigurationErrorsException("Set WebServiceRoot in web.config");
            }

            if (webServiceRoot.EndsWith("/"))
            {
                return webServiceRoot;
            }

            return webServiceRoot + "/";
        }
    }
}