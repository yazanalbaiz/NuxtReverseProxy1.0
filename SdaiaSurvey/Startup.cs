using SdaiaSurvey.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sdaia.FileStorage;
using Sdaia.FileStorage.Model;
using SdaiaSurvey.Extention;
using VueCliMiddleware;
using Microsoft.Extensions.Options;
using SdaiaSurvey.Model.Options.SecurityHeaders;

namespace SdaiaSurvey
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            this.environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureAppServices(Configuration);
            services.AddFileStorage((options) =>
            {
                Configuration.GetSection("fileStorage").Bind(options);
                services.Configure<FileStorageOptions>(Configuration.GetSection("fileStorage"));
            });
            services.ConfigureCors(Configuration);
            services.ConfigureAppConfigurations(Configuration);
            services.ConfigureDataProtection(Configuration);
            services.ConfigureSingleSignOn(Configuration);
            services.AddI18N();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SecurityHeadersOptions> securityHeadersOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.None,
            });

            app.UseI18N();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            if (Configuration.GetValue<bool>("activateSecurityHeaders"))
            {
                app.UseSpaStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = async staticFileContext =>
                    {
                        await SecurityHeadersMiddleware.ProcessHeaders(
                            staticFileContext.Context.Response,
                            securityHeadersOptions.Value
                        );
                    }
                });
            }
            else
            {
                app.UseSpaStaticFiles();
            }
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseMiddleware<ReverseProxyMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapHealthChecks("/availability", new HealthCheckOptions()
                    {
                        Predicate = (check) => check.Tags.Contains("availability"),
                        AllowCachingResponses = false,
                        ResponseWriter = ServicesExtentions.WriteResponse
                    })
                    .WithMetadata(new AllowAnonymousAttribute());

                endpoints.MapControllers();
                if (env.IsDevelopment() && Configuration.GetValue<bool>("RunVueMiddleWare"))
                {
                    endpoints.MapToVueCliProxy(
                    "{*path}",
                    new SpaOptions { SourcePath = "ClientApp" },
                    npmScript: "dev",
                    regex: "Compiled successfully",
                    forceKill: true,
                    wsl: false // Set to true if you are using WSL on windows. For other operating systems it will be ignored
                    );
                }
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });
        }
    }
}
