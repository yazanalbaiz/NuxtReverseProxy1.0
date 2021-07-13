using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdaiaSurvey.Security.Abstractions
{
    public interface IAllowAnonymousLinksProvider
    {
        Task<IEnumerable<string>> GetLinks();
    }
}
