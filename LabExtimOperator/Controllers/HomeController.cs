using System.Web.Mvc;

namespace LabExtimOperator.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize]
        public ActionResult Index()
        {
            
            return View();    
        }
        
    
    }
}

public enum HeaderViewRenderMode { Full, Menu, Title }