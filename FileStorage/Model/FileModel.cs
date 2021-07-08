using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdaia.FileStorage.Model
{
    public class FileModel
    {
        public Guid UniqueId { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
    }
}
