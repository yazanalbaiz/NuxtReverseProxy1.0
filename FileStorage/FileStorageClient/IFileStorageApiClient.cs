using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sdaia.FileStorage.Model;

namespace Sdaia.FileStorage.FileStorageClient
{
    public interface IFileStorageApiClient
    {
        Task<IActionResult> UploadFile(PostFile file, int systemId);
        Task<IActionResult> GetFile(Guid id);
    }
}
