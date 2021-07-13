using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SdaiaSurvey.Security.Services
{
    public class AnonymousLinksAwareAuthorizationService : IAuthorizationService
    {
        private readonly DefaultAuthorizationService defaultAuthorizationService;
        private readonly AllowAnonymousLinksEvaluator linksEvaluator;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAuthorizationPolicyProvider policyProvider;

        public AnonymousLinksAwareAuthorizationService(
            AllowAnonymousLinksEvaluator linksEvaluator,
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationPolicyProvider policyProvider,
            IAuthorizationHandlerProvider handlers,
            ILogger<DefaultAuthorizationService> baseLogger,
            IAuthorizationHandlerContextFactory contextFactory,
            IAuthorizationEvaluator authorizationEvaluator,
            IOptions<AuthorizationOptions> options)
        {
            defaultAuthorizationService = new DefaultAuthorizationService(policyProvider, handlers, baseLogger, contextFactory, authorizationEvaluator, options);
            this.linksEvaluator = linksEvaluator;
            this.httpContextAccessor = httpContextAccessor;
            this.policyProvider = policyProvider;
        }

        public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            var httpRequestFeature = httpContextAccessor.HttpContext.Features.Get<IHttpRequestFeature>();

            if (!httpRequestFeature.RawTarget.StartsWith("/api"))
                return AuthorizationResult.Success();

            if (await linksEvaluator.Evaluate(httpRequestFeature.RawTarget))
                return AuthorizationResult.Success();

            return await defaultAuthorizationService.AuthorizeAsync(user, resource, requirements);
        }

        public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            if (policyName == null)
                throw new ArgumentNullException(nameof(policyName));

            var policy = await policyProvider.GetPolicyAsync(policyName) ?? throw new InvalidOperationException($"No policy found: {policyName}.");

            return await this.AuthorizeAsync(user, resource, policy);
        }
    }
}
