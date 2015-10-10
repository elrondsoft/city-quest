using System.Web.Mvc;

namespace CityQuest.Web.Controllers
{
    public class HomeController : CityQuestControllerBase
    {
        public ActionResult Index()
        { 
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
	}
}