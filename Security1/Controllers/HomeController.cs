namespace Security1.Controllers
{
    using System.Web;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        /// <summary>
        /// XSS example. (Works in firefox but not in chrome)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Index(string username)
        {
            return View("index", new Models.Home() {
                 HomeText = username
            });
            //username = HttpUtility.HtmlEncode(username);
        }
    }
}
