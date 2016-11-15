namespace Security1.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        /// <summary>
        /// Works in firefox but not in chrome
        /// </summary>
        /// <param name="xss"></param>
        /// <returns></returns>
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Index(string xss)
        {
            return View("index", new Models.Home() {
                 HomeText = xss
            });
        }
    }
}
