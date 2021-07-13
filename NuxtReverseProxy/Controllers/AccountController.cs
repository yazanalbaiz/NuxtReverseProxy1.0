using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SdaiaSurvey.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        [HttpGet("Login")]
        public async Task Login(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            Response.Redirect(returnUrl);
            await Response.CompleteAsync();
        }

        [HttpGet("Logout")]
        public async Task Logout(string returnUrl)
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        [HttpGet("UserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            return new JsonResult(HttpContext.User.Claims.Select(x => x.Value));
        }

        [AllowAnonymous]
        [HttpGet("User")]
        public IActionResult GetUser()
        {
            return new JsonResult(User.Identity.IsAuthenticated);
        }
    }
}
