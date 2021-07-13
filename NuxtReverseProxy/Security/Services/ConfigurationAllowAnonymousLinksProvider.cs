using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SdaiaSurvey.Security.Abstractions;

namespace SdaiaSurvey.Security.Services
{
    public class ConfigurationAllowAnonymousLinksProvider : IAllowAnonymousLinksProvider
    {
        private readonly IConfiguration configuration;

        public ConfigurationAllowAnonymousLinksProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<IEnumerable<string>> GetLinks()
        {
            var section = configuration.GetSection("AllowAnonymousLinks");

            if (!section.Exists())
                return Task.FromResult(Enumerable.Empty<string>());

            return Task.FromResult(section.Get<IEnumerable<string>>());
        }
    }
}
