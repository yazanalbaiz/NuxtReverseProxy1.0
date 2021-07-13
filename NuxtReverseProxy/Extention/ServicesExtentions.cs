using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SdaiaISdaiaSurveyndex.Model.Options;
using SdaiaSurvey.HealthChecks;
using SdaiaSurvey.Model.Options;
using SdaiaSurvey.Model.Options.SecurityHeaders;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace SdaiaSurvey.Extention
{
    static class ServicesExtentions
    {
        public static void ConfigureSingleSignOn(this IServiceCollection services, IConfiguration Configuration)
        {
            var httpClientOptions = new HttpClientOptions();
            Configuration.GetSection("httpClientOptions").Bind(httpClientOptions);
            var singleSignOnOptions = new SingleSignOnOptions();
            Configuration.GetSection("ssoOptions").Bind(singleSignOnOptions);

            // Authentication Section: add authintecation method based on client id
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddAuthentication(options =>
            {
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = singleSignOnOptions.Authority;
                options.ClientId = singleSignOnOptions.ClientId;
                options.ClientSecret = singleSignOnOptions.ClientSecret;
                options.ResponseType = singleSignOnOptions.ResponseType;
                options.SaveTokens = singleSignOnOptions.SaveTokens;
                options.AccessDeniedPath = singleSignOnOptions.AccessDeniedPath;

                options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents()
                {
                    
                    OnRemoteFailure = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");

                        return Task.FromResult(0);
                    }
                };

                if (httpClientOptions.DisableSslVerification)
                {
                    options.RequireHttpsMetadata = false;
                    HttpClientHandler handler = new HttpClientHandler();
                    handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                    options.BackchannelHttpHandler = handler;
                }

                foreach (var scope in singleSignOnOptions.ScopesArray)
                {
                    options.Scope.Add(scope);
                }
            });

            // services.AddTransient<IAllowAnonymousLinksProvider, ConfigurationAllowAnonymousLinksProvider>();
            // services.AddTransient<AllowAnonymousLinksEvaluator>();
            // services.AddTransient<IAuthorizationService, AnonymousLinksAwareAuthorizationService>();
        }

        public static void ConfigureAppConfigurations(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<SingleSignOnOptions>(Configuration.GetSection("ssoOptions"));
            services.Configure<HealthCheckOptions>(Configuration.GetSection("healthCheckStatus"));
            services.Configure<SecurityContentPolicyOptions>(Configuration.GetSection("securityContentPolicyOptions"));
            services.AddSecurityHeaders(options =>
            {

            });
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration Configuration)
        {
            List<string> cors = new List<string>();
            Configuration.GetSection("corsOrigins").Bind(cors);

            // Add cors
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(cors.ToArray());
                    });
            });
        }

        public static void ConfigureAppServices(this IServiceCollection services, IConfiguration Configuration)
        {
            var httpClientOptions = new HttpClientOptions();
            Configuration.GetSection("httpClientOptions").Bind(httpClientOptions);
            services.AddControllers();
            // connect vue app - middleware  
            services.AddSpaStaticFiles(options => options.RootPath = "dist");
            services.Configure<ReverseProxyOptions>(Configuration.GetSection("reverseProxy"));

            if (httpClientOptions.DisableSslVerification)
            {
                services.AddHttpClient(Microsoft.Extensions.Options.Options.DefaultName).ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new HttpClientHandler();
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) => { return true; };
                    return handler;
                });
            }
            else
            {
                services.AddHttpClient();
            }

            services.AddHttpContextAccessor();
            services.AddAntiforgery();
            //services.AddSwaggerGen();
            services.AddHealthChecks()
                .AddCheck<AvailabilityHealthCheck>("availability",
                tags: new[] { "availability" });
        }

        public static void AddI18N(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(
            opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("ar-SA"),
                    new CultureInfo("en-US"),
                };

                opts.DefaultRequestCulture = new RequestCulture("ar-SA");
                // Formatting numbers, dates, etc.
                opts.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                opts.SupportedUICultures = supportedCultures;
            });
        }

        public static void UseI18N(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
        }

        public static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));

            return context.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }

        public static IServiceCollection AddSecurityHeaders(this IServiceCollection services, Action<SecurityHeadersOptions> setupAction)
        {
            return services
                 .ConfigureOptions<SecurityHeadersOptionsSetup>()
                 .Configure(setupAction);
        }

        public static IServiceCollection ConfigureDataProtection(this IServiceCollection services, IConfiguration configuration)
        {
            DataProtectionKeyOptions options = new DataProtectionKeyOptions();
            configuration.GetSection("DataProtection").Bind(options);
            if (options.Enable)
            {
                using (var x509Store = new X509Store(StoreLocation.LocalMachine))
                {
                    x509Store.Open(OpenFlags.ReadOnly);
                    var cert = x509Store.Certificates.Find(X509FindType.FindByThumbprint, options.Thumbprint, false);

                    if (options.UseRedis)
                    {
                        services.AddDataProtection(config =>
                        {
                            config.ApplicationDiscriminator = options.ApplicationName;
                        })
                        .PersistKeysToStackExchangeRedis(
                            ConnectionMultiplexer.Connect(options.RedisConnectionString),
                            $"DataProtection-Keys-{options.ApplicationName}")
                        .SetApplicationName(options.ApplicationName)
                        .ProtectKeysWithCertificate(cert[0]);

                    }
                    else
                    {
                        services.AddDataProtection(config =>
                        {
                            config.ApplicationDiscriminator = options.ApplicationName;
                        })
                        .PersistKeysToFileSystem(new System.IO.DirectoryInfo(options.KeyFilePath))
                        .SetApplicationName(options.ApplicationName)
                        .ProtectKeysWithCertificate(cert[0]);
                    }
                    x509Store.Close();
                }
            }

            return services;
        }
    }

}
