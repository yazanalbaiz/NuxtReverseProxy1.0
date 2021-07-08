using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sdaia.FileStorage.FileStorageClient;
using Sdaia.FileStorage.Model;

namespace Sdaia.FileStorage.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IFileStorageApiClient client;

        public FileController(IFileStorageApiClient client)
        {
            this.client = client;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileAsync(Guid id)
        {
            return await client.GetFile(id);
        }

        [HttpPost("{systemId}")]
        public async Task<IActionResult> UploadFile([FromBody] PostFile file, [FromQuery] int systemId)
        {
            return await client.UploadFile(file, systemId);
        }
    }
}
