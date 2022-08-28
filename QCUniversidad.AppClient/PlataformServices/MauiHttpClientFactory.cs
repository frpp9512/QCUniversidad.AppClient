using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.PlataformServices
{
    public class MauiHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateHttpClient()
        {
            var client = new HttpClient(new HttpsClientHandlerService().GetPlatformMessageHandler());
            return client;
        }
    }
}
