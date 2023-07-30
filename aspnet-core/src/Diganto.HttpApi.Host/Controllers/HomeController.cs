using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Diganto.Controllers
{
    public class HomeController : AbpController
    {
        public ActionResult Index()
        {
            return Redirect("~/swagger");
        }
        [Route("google-response")]
        public async Task<ActionResult> GoogleResponse()
        {
            var google_csrf_name = "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";
            var cookie = Request.Cookies[google_csrf_name];
            if (cookie == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            var idtoken = Request.Form["credential"];
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idtoken).ConfigureAwait(false);
            TempData["name"] = payload.Name;
            TempData["emai"] = payload.Email;
            return Json(payload);
        }
    }
}
