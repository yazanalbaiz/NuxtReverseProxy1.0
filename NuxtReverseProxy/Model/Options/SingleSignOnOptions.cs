using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SdaiaSurvey.Model.Options
{
    public class SingleSignOnOptions
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ResponseType { get; set; }
        public bool SaveTokens { get; set; }
        public string Scopes
        {
            get
            {
                return string.Join(" ", _scopes);
            }
            set 
            { 
                _scopes = value.Split(" "); 
            }
        }

        private string[] _scopes = new string[0];
        public string[] ScopesArray => _scopes;
        public PathString AccessDeniedPath { get; set; }
    }
}
