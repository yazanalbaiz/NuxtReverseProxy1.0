using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SdaiaSurvey.Model.Options;
using SdaiaSurvey.Model.Options.SecurityHeaders;

namespace SdaiaISdaiaSurveyndex.Model.Options
{

    public class SecurityHeadersOptionsSetup : IConfigureOptions<SecurityHeadersOptions>
    {
        private readonly IOptions<SecurityContentPolicyOptions> cspOptions;
        public SecurityHeadersOptionsSetup(IOptions<SecurityContentPolicyOptions> cspOptions)
        {
            this.cspOptions = cspOptions;
        }

        public void Configure(SecurityHeadersOptions options)
        {
            options.FrameOptions = new FrameOptionsSecurityHeader();
            options.XssProtection = new XssProtectionSecurityHeader();
            options.StrictTransportSecurity = new StrictTransportSecuritySecurityHeader();
            options.ContentTypeOptions = new ContentTypeOptionsSecurityHeader();
            options.ReferrerPolicy = new ReferrerPolicySecurityHeader();
            options.PoweredBy = new PoweredBySecurityHeader();
            options.Server = new ServerSecurityHeader();
            options.ContentPolicyOptions = new ContentPolicySecurityHeader();
            options.XContentPolicyOptions = new XContentPolicySecurityHeader();

            options.FrameOptions.Deny();
            options.XssProtection.Block();
            options.StrictTransportSecurity.MaxAge();
            options.ContentTypeOptions.NoSniff();
            options.ReferrerPolicy.NoReferrer();

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            var csp = "img-src 'self' data:;default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
            // add required style hashes
            csp += "style-src 'self'";

            if(cspOptions.Value.Styles != null && cspOptions.Value.Styles.Length > 0)
            {
                foreach (var source in cspOptions.Value.Styles)
                {
                   csp += $" '{source}'"; 
                }
            }
            csp += ";";

            // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
            csp += "upgrade-insecure-requests;";
            // also an example if you need client images to be displayed from twitter
            // csp += "img-src 'self' https://pbs.twimg.com;";

            options.ContentPolicyOptions.SetContentPolicy(csp);
            options.XContentPolicyOptions.SetContentPolicy(csp);
        }
    }
}