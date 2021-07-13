
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SdaiaSurvey.Model.Options.SecurityHeaders;

namespace SdaiaSurvey.Middlewares
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate nextMiddleware;
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;
        public SecurityHeadersMiddleware(RequestDelegate nextMiddleware, IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.nextMiddleware = nextMiddleware;
            this.environment = environment;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IOptions<SecurityHeadersOptions> optionsAcccessor)
        {
            context.Response.OnStarting(
                async state =>
                {
                    try
                    {
                        var response = context.Response;
                        if (configuration.GetValue<bool>("activateSecurityHeaders"))
                        {
                            await ProcessHeaders(response, optionsAcccessor.Value);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }, context);
            await nextMiddleware(context);
        }
        private static Action<HttpResponse, SecurityHeaderBase> TryAddHeader = (response, header) =>
        {
            if (header != null && !string.IsNullOrEmpty(header.Value))
                response.Headers[header.Header] = header.Value;
        };

        private static Action<HttpResponse, SecurityHeaderBase> TryRemoveHeader = (response, header) =>
        {
            if (response.Headers.ContainsKey(header.Header))
                response.Headers.Remove(header.Header);
        };

        public static async Task ProcessHeaders(HttpResponse response, SecurityHeadersOptions options)
        {
            TryAddHeader(response, options.FrameOptions);
            TryAddHeader(response, options.XssProtection);
            TryAddHeader(response, options.StrictTransportSecurity);
            TryAddHeader(response, options.ContentTypeOptions);
            TryAddHeader(response, options.ReferrerPolicy);
            TryAddHeader(response, options.ContentPolicyOptions);
            TryAddHeader(response, options.XContentPolicyOptions);
            TryRemoveHeader(response, options.PoweredBy);
            TryRemoveHeader(response, options.Server);
            await ClearCacheHeaders(response);
        }

        private static Task ClearCacheHeaders(HttpResponse resposne)
        {
            var headers = resposne.Headers;
            headers[HeaderNames.CacheControl] = "no-cache";
            headers[HeaderNames.Pragma] = "no-cache";
            headers[HeaderNames.Expires] = "-1";
            headers.Remove(HeaderNames.ETag);
            return Task.CompletedTask;
        }
    }
}