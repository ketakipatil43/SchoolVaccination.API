using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace UM.API.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        public string CallParams { get; set; }
        public BaseController()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CallParams = JsonConvert.SerializeObject(context.ActionArguments);
            if (!context.ActionArguments.TryGetValue("formData", out object formData) && !context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary());
            }
            base.OnActionExecuting(context);
        }
    }
}
