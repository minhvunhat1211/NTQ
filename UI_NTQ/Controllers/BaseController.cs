using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UI_NTQ.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var Role = context.HttpContext.Session.GetString("Role");
            
            ViewBag.Role = Role;
            
            base.OnActionExecuting(context);
        }
    }
}
