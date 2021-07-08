using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sdaia.FileStorage.Model;
using BadRequestResult = Microsoft.AspNetCore.Mvc.BadRequestResult;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;

namespace Sdaia.FileStorage.FileStorageClient
{
    public class FileStorageApiClient : IFileStorageApiClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<FileStorageApiClient> logger;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IOptions<FileStorageOptions> options;
        public FileStorageApiClient(IHttpClientFactory httpClientFactory,
            ILogger<FileStorageApiClient> logger,
            IHttpContextAccessor contextAccessor,
            IOptions<FileStorageOptions> options)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            this.contextAccessor = contextAccessor;
            this.options = options;
        }

        public async Task<IActionResult> GetFile(Guid id)
        {
            try
            {
                FileModel file = new FileModel();
                var client = httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(options.Value.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await client.GetAsync($"/api/Files/{id}");
                if (response.IsSuccessStatusCode)
                {
                    file = JsonConvert.DeserializeObject<FileModel>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    IActionResult result = response.StatusCode switch
                    {
                        // might need later to include more cases and how to handle them
                        HttpStatusCode.BadRequest => new BadRequestResult(),
                        HttpStatusCode.NotFound => new NotFoundResult(),
                        HttpStatusCode.InternalServerError =>
                            new PostFileResult()
                            {
                                StatusCode = HttpStatusCode.InternalServerError,
                                Message = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync())
                            },
                        _ => new BadRequestResult(),
                    };

                    return result;
                }
                return new JsonResult(file);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw new Exception("Kindly contact support", ex);
            }
        }

        public async Task<IActionResult> UploadFile(PostFile file, int systemId)
        {
            try
            {
                double maxAllowedFileSizeInBytes = options.Value.FileMaxSizeInMB * 1048576;

                if (file.Data.Length > maxAllowedFileSizeInBytes)
                {
                    IActionResult result = new PostFileResult()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "حجم الملف أكبر من المسموح",
                    };

                    return result;
                }

                var client = httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(options.Value.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await client.PostAsJsonAsync($"/api/Files/{systemId}", file);
                if (response.IsSuccessStatusCode)
                {
                    var responseContentString = await response.Content.ReadAsStringAsync();
                    var resultContent = JsonConvert.DeserializeObject<PostFileResult>(responseContentString);
                    return resultContent;
                }
                else
                {
                    // add logic if not successful (log?)      
                    IActionResult result = response.StatusCode switch
                    {
                        // might need later to include more cases and how to handle them
                        HttpStatusCode.BadRequest =>
                            new PostFileResult()
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                Message = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync()) switch
                                {
                                    "Invalid system!" => "رمز النظام خاطىء",
                                    "Must provide file!" => "الملف المرفق لا يحتوي أي بيانات أو لم يتم توفير ملف",
                                    "Not acceptable file type!" => "نوع المرفق غير مسموح",
                                    _ => "الرجاء مراجعة الدعم"
                                },
                            },
                        HttpStatusCode.NotFound => new NotFoundResult(),
                        HttpStatusCode.InternalServerError =>
                            new PostFileResult()
                            {
                                StatusCode = HttpStatusCode.InternalServerError,
                                Message = await response.Content.ReadAsStringAsync()
                            },
                        _ => new BadRequestResult(),
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw new Exception("Kindly contact support", ex);
            }
        }
    }
}
