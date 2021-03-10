namespace TestWebService.Controllers.Extensions
{
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Results;

    public static class ApiControllerExtensions
    {
        public static StatusCodeResult NoContent(this ApiController apiController)
        {
            return new StatusCodeResult(HttpStatusCode.NoContent, apiController);
        }
    }
}