using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SdaiaSurvey.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class CultureController : Controller
    {
        private readonly IOptions<RequestLocalizationOptions> LocalizationOptions;
        private readonly ILogger<CultureController> logger;
        private readonly IConfiguration configuration;
        public CultureController(IOptions<RequestLocalizationOptions> localizationOptions, ILogger<CultureController> logger, IConfiguration configuration)
        {
            LocalizationOptions = localizationOptions;
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpPost("{language}")]
        public void SetCulture(string language)
        {
            var culture = language switch
            {
                "en" => "en-US",
                "ar" => "ar-SA",
                _ => "ar-SA"
            };

            logger.LogInformation("CultureController, set language", culture);

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = false, Domain = configuration.GetValue<string>("Domain") }
            );
        }

        [HttpGet]
        public string GetCulture()
        {
            try
            {
                var languageString = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName]?.Split("|")?.FirstOrDefault()?.Split("=")?.FirstOrDefault(x => x != "c") ?? string.Empty;
                if (!string.IsNullOrEmpty(languageString))
                {
                    var culture = new CultureInfo(languageString);
                    logger.LogInformation("CultureController, get language", culture);
                    return culture.TwoLetterISOLanguageName;
                }
                return LocalizationOptions.Value.DefaultRequestCulture.Culture.TwoLetterISOLanguageName;
            }
            catch (Exception ex)
            {
                logger.LogError("Get language error", ex);
            }
            return LocalizationOptions.Value.DefaultRequestCulture.Culture.TwoLetterISOLanguageName;
        }
    }
}
