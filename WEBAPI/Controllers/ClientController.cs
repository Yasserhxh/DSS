using ClientConsommationMensuelle;
using ClientListeRèglements;
using Customer_details;
using Domain.Models.ApiModels;
using Domain.Models.Client;
using GetListClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ServiceModel;
using WEBAPI.Helpers;

namespace WEBAPI.Controllers
{
    [Route("[controller]")]
    public class ClientController : Controller
    {
        private readonly SAPEndpoints _options;

        public ClientController(IOptions<SAPEndpoints> options)
        {
                _options = options.Value;
        }

        //-------------------------------- Client Détails -------------------------------------

        [HttpPost]
        [Route("GetListSAP")]
        public async Task<IActionResult> GetListSAPAsync([FromBody] SapSearchVMFindClient searchVM)
        {
            try
            {
                string endpointurl = _options.ClientDétails!;
                BasicHttpBinding binding = new BasicHttpBinding();

                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                EndpointAddress endpoint = new(endpointurl);
                var wsclient = new zBAPI_CUSTOMER_GETLISTClient(binding, endpoint);

                wsclient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
                wsclient.ClientCredentials.UserName.Password = "azerty2023++";

                var request = new BAPI_CUSTOMER_GETLIST
                {
                    MAXROWS = 100,
                    IDRANGE = new[]
                    {
                        new BAPICUSTOMER_IDRANGE()
                        {
                            SIGN = Constants.I,
                            OPTION = Constants.I,
                            LOW =  searchVM.ClientSapLow.ToString(),
                            HIGH = searchVM.ClientSapHigh.ToString()
                        }
                    },
                    ADDRESSDATA = new[]
                    {
                        new BAPICUSTOMER_ADDRESSDATA()
                        {

                        }
                    }
                };

                wsclient.Open();
                var response = await wsclient.BAPI_CUSTOMER_GETLISTAsync(request);
                var clientsFromSap = response.BAPI_CUSTOMER_GETLISTResponse.ADDRESSDATA;

                return Ok(clientsFromSap);
            }
            catch (Exception ex)
            {
                string message = "Caught an exception: " + ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\nInner exception: " + ex.InnerException.Message;
                }
                return BadRequest(message);
            }
        }

        //-------------------------------- Client Détails Crédit -----------------------------

        [HttpPost]
        [Route("GetClientDetails")]
        public async Task<IActionResult> GetClientDetails([FromBody] SapSearchVMGetStatus searchVM)
        {
            try
            {
                string endpointurl = _options.ClientDétailsCrédit!;
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                EndpointAddress endpoint = new(endpointurl);
                var serviceClient = new Z_bapi_customer_detailsClient(binding, endpoint);
                serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
                serviceClient.ClientCredentials.UserName.Password = "azerty2023++";
                var request = new Z_BAPI_CUSTOMER_CREDITDETAILS()
                {
                    CUSTOMER = searchVM.customerSap,
                    CREDIT_CONTROL_AREA = searchVM.creditControlArea,
                };
                serviceClient.Open();
                var response = await serviceClient.Z_BAPI_CUSTOMER_CREDITDETAILSAsync(request);
                var clientsFromSap = response.Z_BAPI_CUSTOMER_CREDITDETAILSResponse;
                return Ok(clientsFromSap);
            }
            catch (Exception ex)
            {
                string message = "Caught an exception: " + ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\nInner exception: " + ex.InnerException.Message;
                }
                return BadRequest(message);
            }
        }

        //-------------------------------- Client Consommation Mensuelle -----------------------------

        [HttpPost]
        [Route("GetClientConsommationMensuelle")]
        public async Task<IActionResult> GetClientConsommationMensuelle([FromBody] ClientConsommationMensuelleModel model)
        {
            try
            {
                string endpointurl = _options.ClientConsommationMensuelle!;
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
                binding.SendTimeout = TimeSpan.FromMinutes(3);
                binding.MaxReceivedMessageSize = 2147483647;

                EndpointAddress endpoint = new(endpointurl);
                var serviceClient = new ZSD_MCSI_S805_WSRClient(binding, endpoint);
                serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
                serviceClient.ClientCredentials.UserName.Password = "PH8YYzPZiUiTdn]jSWUYnYdJAAwnUmeJlFeAwEza";

                var request = new ZSD_MCSI_S805()
                {
                    T_CUST_SALES = new[]
                    {
                        new ZSTCUST_SALES()
                        {

                        }
                    },
                    ZKUNAG = new[]
                    {
                        new ZSTKUNAG()
                        {
                            SIGN = Constants.I,
                            OPTION = Constants.EQ,
                            LOW = model.customerNumber,
                            HIGH = ""
                        }
                    },
                    ZSPTAG = new[]
                    {
                        new ZSTSPTAG()
                        {
                            SIGN = Constants.I,
                            OPTION = Constants.BT,
                            LOW = model.startDate,
                            HIGH = model.endDate
                        }
                    }
                };

                serviceClient.Open();
                var response = await serviceClient.ZSD_MCSI_S805Async(request);
                var clientsFromSap = response.ZSD_MCSI_S805Response;
                var sales = clientsFromSap.T_CUST_SALES;
                return Ok(clientsFromSap);
            }
            catch (Exception ex)
            {
                string message = "Caught an exception: " + ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\nInner exception: " + ex.InnerException.Message;
                }
                return BadRequest(message);
            }
        }

        //-------------------------------- Client liste réglement -----------------------------

        [HttpPost]
        [Route("ClientListeRéglement")]
        public async Task<IActionResult> ClientListeRéglement([FromBody] ClientConsommationMensuelleModel model)
        {
            try
            {
                string endpointurl = _options.ClientListeRéglement!;
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
                binding.SendTimeout = TimeSpan.FromMinutes(3);
                binding.MaxReceivedMessageSize = 2147483647;

                EndpointAddress endpoint = new(endpointurl);
                var serviceClient = new zfi_cust_items_wsdlClient(binding, endpoint);
                serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
                serviceClient.ClientCredentials.UserName.Password = "azerty2023++";

                var request = new ZfiCustItems()
                {
                    ItBsad = new[]
                    {
                        new Ztybsad
                        {

                        }
                    },
                    ItBsid = new[]
                    {
                        new Ztybsid
                        {

                        }
                    },
                    ItBsid2 = new[]
                    {
                        new Ztybsid
                        {

                        }
                    },
                    Zblart = new[]
                    {
                        new Zsblart()
                        {
                            Sign = Constants.I,
                            Option = Constants.BT,
                            Low = Constants.D1,
                            High = Constants.D8
                        }
                    },
                    Zbudat = new[]
                    {
                        new Zsbudat()
                        {
                            Sign = Constants.I,
                            Option = Constants.BT,
                            Low = model.startDate,
                            High = model.endDate
                        }
                    },
                    Zbukrs = new[]
                    {
                        new Zsbukrs()
                        {
                            Sign = Constants.I,
                            Option = Constants.EQ,
                            Low = Constants._815,
                            High = ""
                        }
                    },
                    Zkeydate = "",
                    Zkunnr = new[]
                    {
                    new Zskunnr()
                        {
                            Sign = Constants.I,
                            Option = Constants.EQ,
                            Low = model.customerNumber,
                            High = ""
                        }
                    }
                };

                serviceClient.Open();
                var response = await serviceClient.ZfiCustItemsAsync(request);
                var clientsFromSap = response.ZfiCustItemsResponse;
                return Ok(clientsFromSap);
            }
            catch (Exception ex)
            {
                string message = "Caught an exception: " + ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\nInner exception: " + ex.InnerException.Message;
                }
                return BadRequest(message);
            }
        }

        //-------------------------------- Client facture échues -----------------------------

        [HttpPost]
        [Route("ClientFacturesÉchues")]
        public async Task<IActionResult> ClientFacturesÉchues([FromBody] ClientConsommationMensuelleModel model)
        {
            try
            {
                string endpointurl = _options.ClientFactureÉchues!;
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
                binding.SendTimeout = TimeSpan.FromMinutes(3);
                binding.MaxReceivedMessageSize = 2147483647;

                EndpointAddress endpoint = new(endpointurl);
                var serviceClient = new zfi_cust_items_wsdlClient(binding, endpoint);
                serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
                serviceClient.ClientCredentials.UserName.Password = "azerty2023++";

                var request = new ZfiCustItems()
                {
                    ItBsad = new[]
                    {
                        new Ztybsad
                        {

                        }
                    },
                    ItBsid = new[]
                    {
                        new Ztybsid
                        {

                        }
                    },
                    ItBsid2 = new[]
                    {
                        new Ztybsid
                        {

                        }
                    },
                    Zblart = new[]
                    {
                        new Zsblart()
                        {
                            Sign = Constants.I,
                            Option = Constants.BT,
                            Low = Constants.DO,
                            High = Constants.DU
                        }
                    },
                    Zbudat = new[]
                    {
                        new Zsbudat()
                        {
                            Sign = "",
                            Option = "",
                            Low = "",
                            High = ""
                        }
                    },
                    Zbukrs = new[]
                    {
                        new Zsbukrs()
                        {
                            Sign = Constants.I,
                            Option = Constants.EQ,
                            Low = Constants._815,
                            High = ""
                        }
                    },
                    Zkeydate = model.startDate,
                    Zkunnr = new[]
                    {
                        new Zskunnr()
                        {
                            Sign = Constants.I,
                            Option = Constants.EQ,
                            Low = model.customerNumber,
                            High = ""
                        }
                    }
                };

                serviceClient.Open();
                var response = await serviceClient.ZfiCustItemsAsync(request);
                var clientsFromSap = response.ZfiCustItemsResponse;
                return Ok(clientsFromSap);
            }
            catch (Exception ex)
            {
                string message = "Caught an exception: " + ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\nInner exception: " + ex.InnerException.Message;
                }
                return BadRequest(message);
            }
        }

        //-------------------------------- Client liste impayés physique -------------------

        [HttpPost]
        [Route("ClientListeImpayés")]
        public async Task<IActionResult> ClientListeImpayés([FromBody] ClientConsommationMensuelleModel model)
        {
            try
            {
                string endpointurl = _options.ClientListeImpayés!;
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
                binding.SendTimeout = TimeSpan.FromMinutes(3);
                binding.MaxReceivedMessageSize = 2147483647;

                EndpointAddress endpoint = new(endpointurl);
                var serviceClient = new zfi_cust_items_wsdlClient(binding, endpoint);
                serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
                serviceClient.ClientCredentials.UserName.Password = "azerty2023++";

                var request = new ZfiCustItems()
                {
                    ItBsad = new[]
                    {
                    new Ztybsad
                        {

                        }
                    },
                    ItBsid = new[]
                    {
                        new Ztybsid
                        {

                        }
                    },
                    ItBsid2 = new[]
                    {
                        new Ztybsid
                        {

                        }
                    },
                    Zblart = new[]
                    {
                        new Zsblart()
                        {
                            Sign = Constants.I,
                            Option = Constants.BT,
                            Low = Constants.DO,
                            High = Constants.DU
                        }
                    },
                    Zbudat = new[]
                    {
                        new Zsbudat()
                        {
                            Sign = "",
                            Option = "",
                            Low = "",
                            High = ""
                        }
                    },
                    Zbukrs = new[]
                    {
                        new Zsbukrs()
                        {
                            Sign = Constants.I,
                            Option = Constants.EQ,
                            Low = Constants._815,
                            High = ""
                        }
                    },
                    Zkeydate = model.startDate,
                    Zkunnr = new[]
                    {
                        new Zskunnr()
                        {
                            Sign = Constants.I,
                            Option = Constants.EQ,
                            Low = model.customerNumber,
                            High = ""
                        }
                    }
                };

                serviceClient.Open();
                var response = await serviceClient.ZfiCustItemsAsync(request);
                var clientsFromSap = response.ZfiCustItemsResponse;
                return Ok(clientsFromSap);
            }
            catch (Exception ex)
            {
                string message = "Caught an exception: " + ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\nInner exception: " + ex.InnerException.Message;
                }
                return BadRequest(message);
            }
        }

        //-------------------------------- Client liste facture -----------------------------

        [HttpPost]
        [Route("ClientListeFacture")]
        public async Task<IActionResult> ClientListeFacture([FromBody] ClientConsommationMensuelleModel model)
        {
            try
            {
                string endpointurl = _options.ClientListeFacture!;
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
                binding.SendTimeout = TimeSpan.FromMinutes(3);
                binding.MaxReceivedMessageSize = 2147483647;

                EndpointAddress endpoint = new(endpointurl);
                var serviceClient = new zfi_cust_items_wsdlClient(binding, endpoint);
                serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
                serviceClient.ClientCredentials.UserName.Password = "azerty2023++";

                var request = new ZfiCustItems()
                {
                    ItBsad = new[]
                        {
                        new Ztybsad
                        {

                        }
                    },
                    ItBsid = new[]
                        {
                        new Ztybsid
                        {

                        }
                    },
                    ItBsid2 = new[]
                    {
                        new Ztybsid
                        {

                        }
                    },
                    Zblart = new[]
                    {
                        new Zsblart()
                        {
                            Sign = Constants.I,
                            Option = Constants.EQ,
                            Low = Constants.RV,
                            High = ""
                        }
                    },
                    Zbudat = new[]
                    {
                        new Zsbudat()
                        {
                            Sign = Constants.I,
                            Option = Constants.BT,
                            Low = model.startDate,
                            High = model.endDate
                        }
                    },
                    Zbukrs = new[]
                    {
                        new Zsbukrs()
                        {
                            Sign = Constants.I,
                            Option = Constants.EQ,
                            Low = Constants._815,
                            High = ""
                        }
                    },
                    Zkeydate = "",
                    Zkunnr = new[]
                    {
                        new Zskunnr()
                        {
                            Sign = Constants.I,
                            Option = Constants.EQ,
                            Low = model.customerNumber,
                            High = ""
                        }
                    }
                };

                serviceClient.Open();
                var response = await serviceClient.ZfiCustItemsAsync(request);
                var clientsFromSap = response.ZfiCustItemsResponse;
                return Ok(clientsFromSap);
            }
            catch (Exception ex)
            {
                string message = "Caught an exception: " + ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\nInner exception: " + ex.InnerException.Message;
                }
                return BadRequest(message);
            }
        }


    }
}
