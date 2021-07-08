using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SdaiaSurvey.Security.Abstractions;

namespace SdaiaSurvey.Security.Services
{
    public class AllowAnonymousLinksEvaluator
    {
        private readonly IAllowAnonymousLinksProvider linksProvider;

        public AllowAnonymousLinksEvaluator(IAllowAnonymousLinksProvider linksProvider)
        {
            this.linksProvider = linksProvider;
        }

        public async Task<bool> Evaluate(string path)
        {
            if (path == null)
                throw new ArgumentNullException();

            var allowAnonymousLinks = await linksProvider.GetLinks();

            if (!allowAnonymousLinks?.Any() == null)
                return false;

            if (path.Contains("?"))
                path = path.Substring(0, path.IndexOf("?"));

            path = path.Trim('/');

            var result = allowAnonymousLinks.Any(x => Regex.IsMatch(path, x, RegexOptions.IgnoreCase));
            return false;
        }
    }
}
