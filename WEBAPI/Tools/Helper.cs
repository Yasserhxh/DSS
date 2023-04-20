using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using Microsoft.Extensions.Configuration;

namespace WEBAPI.Tools
{
    public static class Helper
    {
        public static BasicHttpBinding GetBinding ()
        {

            var binding = new BasicHttpBinding()
            {
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                TransferMode = TransferMode.Buffered,
                Security =
                {

                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport = new HttpTransportSecurity()
                    {
                        ClientCredentialType = HttpClientCredentialType.Basic,
                     }
                }
            };
            return binding;
        }
        
        public static EndpointAddress GetEndpoint(IConfiguration configuration,string suffix)
        {
            return new EndpointAddress($"{configuration["Sap:Url"]}/{suffix}");
        }
    }
}