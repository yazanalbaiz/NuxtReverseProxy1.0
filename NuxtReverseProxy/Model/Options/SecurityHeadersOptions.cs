namespace SdaiaSurvey.Model.Options.SecurityHeaders
{
    public class SecurityHeadersOptions
    {
        public FrameOptionsSecurityHeader FrameOptions { get; set; }
        public XssProtectionSecurityHeader XssProtection { get; set; }
        public StrictTransportSecuritySecurityHeader StrictTransportSecurity { get; set; }
        public ContentTypeOptionsSecurityHeader ContentTypeOptions { get; set; }
        public ContentPolicySecurityHeader ContentPolicyOptions { get; set; }
        public XContentPolicySecurityHeader XContentPolicyOptions { get; set; }
        public ReferrerPolicySecurityHeader ReferrerPolicy { get; set; }
        public PoweredBySecurityHeader PoweredBy { get; set; }
        public ServerSecurityHeader Server { get; set; }
    }

    public abstract class SecurityHeaderBase
    {
        public abstract string Header { get; }
        public string Value { get; protected set; }
    }

    public class XssProtectionSecurityHeader : SecurityHeaderBase
    {
        public override string Header => "X-XSS-Protection";

        public void Enable() => Value = "1";
        public void Disable() => Value = "0";
        public void Block() => Value = "1; mode=block";
        public void Report(string reportUrl) => Value = $"1; report={reportUrl}";
    }

    public class FrameOptionsSecurityHeader : SecurityHeaderBase
    {
        public override string Header => "X-Frame-Options";

        public void Deny() => Value = "DENY";
        public void AddFrameOptionsSameOrigin() => Value = "SAMEORIGIN";
        public void AddFrameOptionsAllowFromUri(string uri) => Value = $"ALLOW-FROM {uri}";
    }

    public class StrictTransportSecuritySecurityHeader : SecurityHeaderBase
    {
        public override string Header => "Strict-Transport-Security";

        public void MaxAge(int maxAge = 31536000) => Value = $"max-age={maxAge}";
        public void MaxAgeIncludeSubDomains(int maxAge = 31536000) => Value = $"max-age={maxAge}; includeSubDomains";
        public void NoCache() => Value = "max-age=0";
    }

    public class ContentTypeOptionsSecurityHeader : SecurityHeaderBase
    {
        public override string Header => "X-Content-Type-Options";

        public void NoSniff() => Value = "nosniff";
    }

    public class ReferrerPolicySecurityHeader : SecurityHeaderBase
    {
        public override string Header => "Referrer-Policy";

        public void NoReferrer() => Value = "no-referrer";
        public void NoReferrerWhenDowngrade() => Value = "no-referrer-when-downgrade";
        public void Origin() => Value = "origin";
        public void OriginWhenCrossOrigin() => Value = "origin-when-cross-origin";
        public void SameOrigin() => Value = "same-origin";
        public void StrictOrigin() => Value = "strict-origin";
        public void StrictOriginWhenCrossOrigin() => Value = "strict-origin-when-cross-origin";
        public void UnsafeUrl() => Value = "unsafe-url";
    }

    public class PoweredBySecurityHeader : SecurityHeaderBase
    {
        public override string Header => "X-Powered-By";
    }

    public class ServerSecurityHeader : SecurityHeaderBase
    {
        public override string Header => "Server";
    }

    public class ContentPolicySecurityHeader : SecurityHeaderBase
    {
        public override string Header => "Content-Security-Policy";
        public void SetContentPolicy(string policy) => Value = policy;
    }

    public class XContentPolicySecurityHeader : SecurityHeaderBase
    {
        public override string Header => "X-Content-Security-Policy";
        public void SetContentPolicy(string policy) => Value = policy;
    }
}
