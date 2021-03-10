namespace TestWebApplication.Controllers
{
    using System.Web.Mvc;

    public class UsersController : Controller
    {
        // GET: List
        public ActionResult List()
        {
            return PartialView();
        }

        // GET: Editor
        public ActionResult Editor()
        {
            return PartialView();
        }
    }
}