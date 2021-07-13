using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SdaiaSurvey.Model.Options;

namespace SdaiaSurvey.Middlewares
{
    public class ReverseProxyMiddleware
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly RequestDelegate nextMiddleware;
        private readonly IOptions<ReverseProxyOptions> reverseProxyConfig;
        private List<string> controllers;

        public ReverseProxyMiddleware(
            RequestDelegate nextMiddleware,
            IHttpClientFactory clientFactory,
            IOptions<ReverseProxyOptions> reverseProxyConfig)
        {
            this.nextMiddleware = nextMiddleware;
            this.clientFactory = clientFactory;
            this.reverseProxyConfig = reverseProxyConfig;

            Assembly asm = Assembly.GetExecutingAssembly();
            controllers = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .Select(x => x.Name).ToList();
        }

        public async Task Invoke(HttpContext context)
        {
            var client = clientFactory.CreateClient();

            var redirect = BuildTargetUri(context.Request, out var targetUri);

            if (targetUri != null && redirect)
            {
                var targetRequestMessage = CreateTargetMessage(context, targetUri);

                // Add bearer token authorization header to the request
                var accessToken = await context.GetTokenAsync("access_token");
                targetRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var responseMessage = await client.SendAsync(targetRequestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted))
                {
                    context.Response.StatusCode = (int)responseMessage.StatusCode;

                    if (accessToken != null && responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await context.SignOutAsync("Cookies");
                        await context.SignOutAsync("oidc");
                    }

                    CopyFromTargetResponseHeaders(context, responseMessage);

                    await ProcessResponseContent(context, responseMessage);
                }

                return;
            }

            await nextMiddleware(context);
        }

        private async Task ProcessResponseContent(HttpContext context, HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsByteArrayAsync();
            if (content != null && content.Length > 0)
            {
                if (IsContentOfType(responseMessage, "text/html") || IsContentOfType(responseMessage, "text/javascript"))
                {
                    var stringContent = Encoding.UTF8.GetString(content);
                    // Use this statement to edit the response string
                    //var newContent = stringContent.Replace("https://www.google.com", "/google")
                    //    .Replace("https://www.gstatic.com", "/googlestatic")
                    //    .Replace("https://docs.google.com/forms", "/googleforms");
                    await context.Response.WriteAsync(stringContent, Encoding.UTF8);
                }
                else
                {
                    await context.Response.Body.WriteAsync(content);
                }
            }
        }

        private bool IsContentOfType(HttpResponseMessage responseMessage, string type)
        {
            var result = false;

            if (responseMessage.Content?.Headers?.ContentType != null)
            {
                result = responseMessage.Content.Headers.ContentType.MediaType == type;
            }

            return result;
        }

        private HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
        {
            var requestMessage = new HttpRequestMessage();
            CopyFromOriginalRequestContentAndHeaders(context, requestMessage);
            // Use this line if you want to add additional query strings
            //targetUri = new Uri(QueryHelpers.AddQueryString(targetUri.OriginalString, new Dictionary<string, string>() { { "entry.1884265043", "John Doe" } }));

            requestMessage.RequestUri = targetUri;
            requestMessage.Headers.Host = targetUri.Host;
            requestMessage.Method = GetMethod(context.Request.Method);

            return requestMessage;
        }

        private void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
        {
            var requestMethod = context.Request.Method;

            if (!HttpMethods.IsGet(requestMethod) &&
                !HttpMethods.IsHead(requestMethod) &&
                !HttpMethods.IsDelete(requestMethod) &&
                !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(context.Request.Body);
                requestMessage.Content = streamContent;
            }

            foreach (var header in context.Request.Headers)
            {
                if (header.Key.ToLower() == "cookie")
                    continue;
                    
                if (HttpMethods.IsGet(requestMethod) ||
                    HttpMethods.IsHead(requestMethod) ||
                    HttpMethods.IsDelete(requestMethod) ||
                    HttpMethods.IsTrace(requestMethod))
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
                requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }

        private void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
        {
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            //set each one of these headers
            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            context.Response.Headers.Remove("transfer-encoding");
        }

        private static HttpMethod GetMethod(string method)
        {
            if (HttpMethods.IsDelete(method)) return HttpMethod.Delete;
            if (HttpMethods.IsGet(method)) return HttpMethod.Get;
            if (HttpMethods.IsHead(method)) return HttpMethod.Head;
            if (HttpMethods.IsOptions(method)) return HttpMethod.Options;
            if (HttpMethods.IsPost(method)) return HttpMethod.Post;
            if (HttpMethods.IsPut(method)) return HttpMethod.Put;
            if (HttpMethods.IsTrace(method)) return HttpMethod.Trace;
            return new HttpMethod(method);
        }

        private bool BuildTargetUri(HttpRequest request, out Uri uri)
        {
            uri = null;
            string apiUrl = reverseProxyConfig.Value.BaseUrl;

            foreach (var controller in controllers)
            {
                if (request.Path
                        .ToString()
                        .ToLower()
                        .StartsWith("/api/"
                            + controller
                                .Replace("Controller", "")
                                .ToLower()))
                {
                    return false;
                }
            }

            if (request.Path.StartsWithSegments("/api/file", out _))
            {
                return false;
            }

            if (!request.Path.StartsWithSegments("/api", out _))
            {
                return false;
            }

            uri = new Uri(Path.Join(apiUrl, request.Path + request.QueryString));
            return true;
        }
    }
}
