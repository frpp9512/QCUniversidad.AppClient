using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Services.Platform
{
    public interface IApiCallerHttpClientFactory
    {
        Task<HttpClient> CreateApiCallerHttpClientAsync();
    }
}
