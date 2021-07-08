using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sdaia.FileStorage.Model
{
    internal class PostFileResult : IActionResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public PostFileResult()
        {

        }

        public PostFileResult(Guid id, string name, long size, string contentType)
        {
            Id = id;
            Name = name;
            Size = size;
            ContentType = contentType;
        }

        public override bool Equals(object obj)
        {
            return obj is PostFileResult other &&
                   EqualityComparer<object>.Default.Equals(Id, other.Id) &&
                   EqualityComparer<object>.Default.Equals(Name, other.Name) &&
                   EqualityComparer<object>.Default.Equals(Size, other.Size) &&
                   EqualityComparer<object>.Default.Equals(ContentType, other.ContentType) &&
                   EqualityComparer<object>.Default.Equals(Message, other.Message) &&
                   EqualityComparer<object>.Default.Equals(StatusCode, other.StatusCode);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Size, ContentType, Message, StatusCode);
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(this)
            {
                StatusCode = (int)StatusCode == 0 ? (int)HttpStatusCode.Created : (int)StatusCode
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
