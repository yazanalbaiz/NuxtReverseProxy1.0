using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using SdaiaSurvey.Model.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace SdaiaSurvey.HealthChecks
{
    public class AvailabilityHealthCheck : IHealthCheck
    {
        private readonly IOptions<HealthCheckOptions> options;

        public AvailabilityHealthCheck(IOptions<HealthCheckOptions> options)
        {
            this.options = options;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
            dataDictionary.Add("Availability", options.Value.Status);

            var result = options.Value.Status.ToLower() switch
            {
                "online" => HealthCheckResult.Healthy("Online", dataDictionary.ToReadOnlyDictionary()),
                "offline" => HealthCheckResult.Unhealthy("offline"),
                _ => HealthCheckResult.Healthy("Online")
            };

            return Task.FromResult(result);
        }
    }

    static class DictionaryExtention
    {
        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}
