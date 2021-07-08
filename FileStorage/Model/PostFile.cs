using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdaia.FileStorage.Model
{
    public class PostFile
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }

        public string Base64Data
        {
            get
            {
                return Convert.ToBase64String(Data ?? new byte[0]);
            }
            set
            {
                Data = Convert.FromBase64String(value.Substring(value.IndexOf("base64,") + 7));
            }
        }
    }
}
