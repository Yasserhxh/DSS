using System.Diagnostics;
using System.Globalization;
using Domain.Authentication;
using Domain.Models;
using Domain.Models.ApiModels;
using Domain.Models.Commande;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using WEBAPI.Tools;
using GetListClients;
using System.ServiceModel;
using Domain.Entities;
using System.Net;
using NwRfcNet;
using NwRfcNet.Bapi;
using Microsoft.Extensions.Configuration;
using Service.Services;
using System.Data;
using ErrorOr;
using AccountGetStatutReference;
using GetClientPartnerFS;
using Create_Simulation_Commande;
using Customer_details;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Consomation_mensu;
using WEBAPI.Helpers;
using ClientListeRèglements;

namespace WEBAPI.Controllers;

[Route("[controller]")]

public class CommandeController : Controller
{
    private readonly ICommandeService _commandeService;
    private readonly IBlobService blobService;
    private readonly IAuthentificationService _authentificationService;
    private readonly UserManager<ApplicationUser> _userManager;
    private IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CommandeController(ICommandeService commandeService, IBlobService blobService,
        IAuthentificationService authentificationService, UserManager<ApplicationUser> userManager, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        _commandeService = commandeService;
        this.blobService = blobService;
        _authentificationService = authentificationService;
        _userManager = userManager;
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;

    }

    [HttpPost]
    [Route("CheckClient")]
    public async Task<IActionResult> CheckClient([FromBody] CommandeSearchVm clientInFos)
    {
        if (!string.IsNullOrEmpty(clientInFos.CodeSap))
        {
            String endpointurl = "http://ITCSAPWCT.grouphc.net:8000/sap/bc/srt/rfc/sap/zbapi_customer_getlist/150/zbapi_customer_getlist/zbapi_customer_getlist";
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
                        SIGN = "I",
                        OPTION = "BT",
                        LOW = clientInFos.CodeSap.ToString(),
                        HIGH = clientInFos.CodeSap.ToString()

                    }
                },
                ADDRESSDATA = new[] { new BAPICUSTOMER_ADDRESSDATA() { } }
            };
            wsclient.Open();
            var response = await wsclient.BAPI_CUSTOMER_GETLISTAsync(request);
            var clientsFromSap = response.BAPI_CUSTOMER_GETLISTResponse.ADDRESSDATA.FirstOrDefault();
            if(clientsFromSap != null)
            {
                var clientResult = new ClientModel()
                {
                    Adresse = clientsFromSap!.STREET,
                    RaisonSociale = clientsFromSap.NAME,
                    CodeClientSap = clientsFromSap.CUSTOMER,
                    Gsm = clientsFromSap.TEL1_NUMBR,
                    IdVille = Convert.ToInt32(clientsFromSap.REGION)
                };
                return Ok(clientResult);
            }
            return Ok(new ClientModel());
        }
        var result = _commandeService.FindFormulaireClient(clientInFos.IceClient, clientInFos.CnieClient, clientInFos.RsClient, clientInFos.IdClient);
        return Ok(result);
    }
    [HttpGet]
    [Route("CheckChantier/{chantierId}")]
    public IActionResult CheckChantier(int? chantierId)
    {
        var result = _commandeService.FindFormulaireChantier(chantierId);
        return Ok(result);
    }

    /*  [HttpPost]
      [Route("Convert")]
      public IActionResult ConvertFile([FromBody] string File)
      {

          var mystr = File.Replace("base64,",string.Empty);


          var testb = Convert.FromBase64String(mystr);

          System.IO.File.WriteAllBytes(@"c:\yourfile", Convert.FromBase64String(mystr));    

         // var file = Server.MapPath("~/Documents/"+File.name);
          //System.IO.File.WriteAllBytes(file, testb);
      }*/
    [HttpPost]
    [Route("ConvertFile")]

    public string ConvertFile([FromBody] IFormFile? file)
    {
        if (file == null) return "";
        var mimeType = file.ContentType;
        byte[] fileData;
        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            fileData = ms.ToArray();
        }

        var str = blobService.UploadFileToBlob(Guid.NewGuid() + "/" + file.FileName,
            "Beton Spécial", fileData, mimeType);
        return str;

    }

    [HttpPost]
    [Route("Create")]

    public async Task<IActionResult> Create([FromBody] CommandeViewModel commandeViewModel, IFormFile? file)
    {
        var redirect = await _commandeService.CreateCommande(commandeViewModel);
        return redirect.Any() ? Ok(redirect) : Problem();
    }
    [HttpPost]
    [Route("CreateProspect")]

    public async Task<bool> CreateProspect([FromBody] CommandeViewModel commandeViewModel)
    {
        var clientSap = commandeViewModel.CommandeV.CodeClientSap;
        var redirect = await _commandeService.CreateProspect(commandeViewModel);
        return redirect;
    }

    [HttpPost]
    [Route("CreateCommandeProspection")]
    public async Task<bool> CreateCommandeProspection([FromBody] CommandeViewModel commandeViewModel)
    {
        var redirect = await _commandeService.CreateCommandeProspection(commandeViewModel);
        return redirect;
    }

    [HttpGet]
    [Route("FindUserRoleByEmail/{email}")]
    public async Task<IActionResult> FindUserRoleByEmail(string email)
    {
        var res = await _authentificationService.FindUserRoleByEmail(email);
        return string.IsNullOrEmpty(email) ? Problem(statusCode: StatusCodes.Status409Conflict, title: "Utilisateur introuvable!") : Ok(res);
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel user)
    {
        user.Password = "123***Sss";
        return Ok(await _authentificationService.Register(user));
    }
    [HttpGet("ListeDesUtilisateurs")]
    public async Task<IActionResult> ListeDesUtilisateurs()
        => Ok(await _authentificationService.getListUsers());
    [HttpPost]
    [Route("UpdateUserRole")]
    public async Task<IActionResult> UpdateUserRole([FromBody] UserModel model)
    {
        var res = await _authentificationService.UpdateUserRole(model);
        return res.Success ? Ok(true) : Problem(statusCode: StatusCodes.Status409Conflict, title: res.Message);
    }
   /* [HttpGet("ListDesRoles")]
    public async Task<IActionResult> ListDesRoles()
        => Ok(await _authentificationService.GetRoles());
   */
    [HttpPost("ListeCommandes")]
    public async Task<IActionResult> ListeCommandes([FromBody] CommandeSearchVm vm)
    {
        var client = await _commandeService.GetClients(vm.IceClient, vm.CnieClient, vm.RsClient);
        if (!client.Any())
            return Ok(new List<CommandeApiModel>());
        vm.IdClients = client.Select(x => x.Client_Id).ToList();
        vm.CommandesAPI = vm.UserRole switch
        {
            "Commercial" => await _commandeService.GetCommandes(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch, vm.Email),
            "Prescripteur technique" => await _commandeService.GetCommandesPT(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            "DA BPE" => await _commandeService.GetCommandesDAPBE(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            "Responsable commercial" => await _commandeService.GetCommandesRC(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            "Chef de ventes" => await _commandeService.GetCommandesCV(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            "Responsable logistique" => await _commandeService.GetCommandesRL(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            _ => vm.CommandesAPI
        };
        return Ok(vm.CommandesAPI);
    }
    [HttpGet("DetailCommande")]
    public async Task<IActionResult> DetailCommande(int? commandeId, int isCommande)
    {
        if (commandeId is null) return Problem("Veuillez selectionner une commande");
        var details = await _commandeService.GetCommandesDetails(commandeId, isCommande);
        return !details.Any() ? Problem("Commande non trouvée") : Ok(details);

    }
    [HttpGet("getListProspects")]
    public async Task<IActionResult> getListProspects()
    {
        var prospects = await _commandeService.GetListProspects();
        return !prospects.Any() ? Problem("Prospects non trouvés") : Ok(prospects);

    }
    [HttpGet("StatusCommande/{commandeId}")]
    public async Task<IActionResult> StatusCommande(int? commandeId)
    {
        if (commandeId is null) return Problem("Veuillez selectionner une commande");
        var status = await _commandeService.GetCommandesStatuts(commandeId);

        return !status.Any() ? Problem("Commande non trouvée") : Ok(status);

    }
    [HttpGet("GetFormeJuridiques")]
    public async Task<IActionResult> GetFormesJuridique()
    {
        return Ok(await _commandeService.GetFormeJuridiques());
    }
    [HttpGet("GetTypeChantiers")]
    public async Task<IActionResult> GetTypeChantiers()
    {
        return Ok(await _commandeService.GetTypeChantiers());
    } [HttpGet("GetZones")]
    public async Task<IActionResult> GetZones()
    {
        return Ok(await _commandeService.GetZones());
    }
    [HttpGet("GetArticles")]
    [Route("GetArticles/{email}")]

    public async Task<IActionResult> GetArticles(string email)
    {
        var user = await _authentificationService.FindUserByEmail(email);
        return Ok(await _commandeService.GetArticles(user.VilleId));
    }
    [HttpGet("GetUserInfos")]
    [Route("GetUserInfos/{email}")]
    public async Task<IActionResult> GetUserInfos(string email)
    {
        var user = await _authentificationService.FindUserByEmail(email);
        if(user is not null)
            return Ok(new CommercialInfos(user.Id, user.VilleId, user.Email));
        return BadRequest();
    }
    [HttpGet("GetDelaiPaiements")]
    public async Task<IActionResult> GetDelaiPaiements()
    {
        return Ok(await _commandeService.GetDelaiPaiements());
    }
    [HttpGet("GetCentraleBetons")]
    public async Task<IActionResult> GetCentraleBetons()
    {
        return Ok(await _commandeService.GetCentraleBetons());
    }
    [HttpGet("GetTarifPompeRefs")]
    public async Task<IActionResult> GetTarifPompeRefs()
    {
        return Ok(await _commandeService.GetTarifPompeRefs());
    }
    [HttpGet("GetVilles")]
    public async Task<IActionResult> GetVilles() => Ok(await _commandeService.GetVilles());

    [HttpGet("GetPays")]
    public async Task<IActionResult> GetPays() => Ok(await _commandeService.GetPays());


    [HttpGet("GetTarifArticle/{id:int}")]
    public async Task<double> GetTarifArticle(int id)
    {
        return await _commandeService.GetTarifArticle(id);
    }
    [HttpGet("GetTarifZone/{id:int}")]
    public async Task<double> GetTarifZone(int id)
    {
        return await _commandeService.GetTarifZone(id);
    }
    [HttpGet("GetTarifPompe/{id:int}")]
    public async Task<double> GetTarifPompe(int id)
    {
        return await _commandeService.GetTarifPompe(id);
    }
    [HttpPost]
    [HttpPost("FixationPrixTransport")]
    public async Task<string> FixationPrixTransport([FromBody] CommandeModifApi commandeModifApi)
    {
        var success = await _commandeService.FixationPrixTransport(
            commandeModifApi.CommandeId, commandeModifApi.CommandeTarifTrans,
            commandeModifApi.CommandeTarifPomp, commandeModifApi.UserEmail);

        return success;
    }

    [HttpPost]
    [Route("PropositionPrix")]
    public async Task<bool> PropositionPrix([FromBody] CommandeModifApi commandeModifApi)
    {
        var res = await _commandeService.ProposerPrix(commandeModifApi.CommandeDetailId,
            commandeModifApi.CommandeTarifBeton, commandeModifApi.UserEmail, commandeModifApi.CommandeBetonArticleFile, commandeModifApi.CommandeCoutDeProdBeton,commandeModifApi.CommandeArticleName);
        return res;
    }
    [HttpPost]
    [Route("FixationPrixRC")]
    public async Task<bool> FixationPrixRC([FromBody] JsonBetonModifApi betonModifApi)
    {
        var res = await _commandeService.FixationPrixRC(betonModifApi.CommandeModifVenteApis, betonModifApi.Useremail, betonModifApi.IdCommande/*, betonModifApi.isBetonSpecial*/);
        return res;
    }
    [HttpGet("GetListValidation/{commandeId:int}")]
    public async Task<IActionResult> GetListValidation(int commandeId) =>
        Ok(await _commandeService.GetListValidation(commandeId));

    [HttpGet("GeneratePDF/{id:int}")]
    public async Task<string> GeneratePDf(int id)
    {
        try
        {
            var commande = await _commandeService.GetCommande(id);
            var user = await _authentificationService.GetUserByEmail(commande.CommercialId);
            var commandePdf = new CommandesPDFViewModel
            {
                commande = commande,
                user = user
            };
            Controller controller = this;

            var lFileResult = await ConvertHTmlToPdf.ConvertCurrentPageToPdf(controller, commandePdf, "devis1",
                "Devis" + commande.IdCommande, "Footer");

            var content = lFileResult as FileContentResult;
            var mimeType = content?.ContentType;
            var devis1 =  await blobService.UploadFileToBlobAsync(content!.FileDownloadName, Guid.NewGuid().ToString(), content.FileContents, mimeType!);            
            return devis1 ;        
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    [HttpPost]
    [Route("UpdateCommande")]
    public async Task<bool> UpdateCommande([FromBody] CommandeApiModel commandeApiModel)
      => await _commandeService.SetCommande(commandeApiModel.CommandeId);

    [HttpPost]
    [Route("GeneratePDfChantier")]
    public async Task<string> GeneratePDfChantier([FromBody] CommandeApiModel commandeApiModel)
    {
        try
        {
            var commande = await _commandeService.UpdateCommande(commandeApiModel) ;
            var validation = await _commandeService.GetListValidation(commande.IdCommande);
            var commandePdf = new CommandesPDFViewModel
            {
                commande = commande,
                validations = validation
            }; Controller controller = this;

            var lFileResult = await ConvertHTmlToPdf.ConvertCurrentPageToPdf(controller, commandePdf, "PdfChantier",
                "FicheChantier" + commande.IdCommande, "Footer");

            var content = lFileResult as FileContentResult;
            var mimeType = content?.ContentType;
            return await blobService.UploadFileToBlobAsync(content!.FileDownloadName, Guid.NewGuid().ToString(), content.FileContents, mimeType!);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    [HttpPost]
    [Route("SetCommande")]
    public async Task<bool> SetCommande([FromBody] CommandeApiModel commandeApiModel)
        => await _commandeService.SetCommande(commandeApiModel.CommandeId);

    [HttpPost]
    [Route("GetListCommandesValidee")]
    public async Task<IActionResult> GetListCommandesValidee([FromBody] CommandeSearchVm vm)
    {
        var client = await _commandeService.GetClients(vm.IceClient, vm.CnieClient, vm.RsClient);
        if (!client.Any())
            return Ok(new List<CommandeApiModel>());
        vm.IdClients = client.Select(x => x.Client_Id).ToList();
        vm.CommandesAPI =
            await _commandeService.GetCommandesValide(vm.IdClients, vm.DateCommande, vm.DateDebutSearch,
                vm.DateFinSearch);
        return Ok(vm.CommandesAPI);
    }
    [HttpPost]
    [Route("GetListSAP")]
    public async Task<IActionResult> GetListSAPAsync([FromBody] SapSearchVMFindClient searchVM)
    {
        //String endpointurl = "http://ITCSAPWCT.grouphc.net:8000/sap/bc/srt/rfc/sap/zbapi_customer_getlist/150/zbapi_customer_getlist/zbapi_customer_getlist";
        var endpointurl = "http://GRPSAFRANU103.grouphc.net:8020/sap/bc/srt/wsdl/flv_10002A111AD1/srvc_url/sap/bc/srt/rfc/sap/zbapi_material_getlist/150/zbapi_material_getlist/zbapi_material_getlist?sap-client=150";
        BasicHttpBinding binding = new BasicHttpBinding();

        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly; 
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        EndpointAddress endpoint = new(endpointurl);
        var wsclient = new zBAPI_CUSTOMER_GETLISTClient(binding,endpoint);

        //wsclient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        //wsclient.ClientCredentials.UserName.Password = "azerty2023++";
        wsclient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        wsclient.ClientCredentials.UserName.Password = "PH8YYzPZiUiTdn]jSWUYnYdJAAwnUmeJlFeAwEza";
        var request = new BAPI_CUSTOMER_GETLIST
        {
            MAXROWS = 100,
            IDRANGE = new[]
            {
                new BAPICUSTOMER_IDRANGE()
                {
                    SIGN = "I",
                    OPTION = "BT",
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
        //open client
        wsclient.Open();
        var response = await wsclient.BAPI_CUSTOMER_GETLISTAsync(request);
        var clientsFromSap = response.BAPI_CUSTOMER_GETLISTResponse.ADDRESSDATA;

        return Ok(clientsFromSap);

         /*const string _urlSuffix = "zbapi_customer_getlist/150/zbapi_customer_getlist/zbapi_customer_getlist";
         var serviceClient = new zBAPI_CUSTOMER_GETLISTClient(Helper.GetBinding(), Helper.GetEndpoint(_configuration, _urlSuffix),
        _configuration["Sap:Username"], _configuration["Sap:Password"]);
        var request = new BAPI_CUSTOMER_GETLIST
             {
                 MAXROWS = 100,
                 IDRANGE = new[]
                 {
                     new BAPICUSTOMER_IDRANGE()
                     {
                         SIGN = "I",
                         OPTION = "BT",
                         LOW = "0000000001",
                         HIGH = "0000000010"

                     }

                 },
                 ADDRESSDATA = new[]
                 {
                     new BAPICUSTOMER_ADDRESSDATA()
                     {

                     }
                 } 

             };        
        var response = await serviceClient.BAPI_CUSTOMER_GETLISTAsync(request);
        var clientsFromSap = response.BAPI_CUSTOMER_GETLISTResponse.ToString();
        return Ok(clientsFromSap);*/
    }
    [HttpPost]
    [Route("GetClientStatus")]
    public async Task<IActionResult> GetClientStatus([FromBody] SapSearchVMGetStatus searchVM)
    {
        String endpointurl = "http://ITCSAPWCT.grouphc.net:8000/sap/bc/srt/rfc/sap/zbapi_credit_account_getstatus/150/zbapi_credit_account_getstatus/zbapi_credit_account_getstatus";
        BasicHttpBinding binding = new BasicHttpBinding();
        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        EndpointAddress endpoint = new(endpointurl);
        var serviceClient = new ZBAPI_CREDIT_ACCOUNT_GETSTATUSClient(binding, endpoint);
        serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        serviceClient.ClientCredentials.UserName.Password = "azerty2023++";
        var request = new BAPI_CREDIT_ACCOUNT_GET_STATUS()
        {
            CUSTOMER = searchVM.customerSap,
            CREDITCONTROLAREA =searchVM.creditControlArea,
            CREDIT_ACCOUNT_OPEN_ITEMS = new[]
            {
                new BAPI1010_2()
                {

                }
            },
            CREDIT_ACCOUNT_DETAIL = new[]
            {
                new BAPI1010_1()
                {

                }
            }
        };
        serviceClient.Open();
        var response = await serviceClient.BAPI_CREDIT_ACCOUNT_GET_STATUSAsync(request);
        var clientsFromSap = response.BAPI_CREDIT_ACCOUNT_GET_STATUSResponse;
        return Ok(clientsFromSap);
    }
    [HttpPost]
    [Route("GetClientDetails")]
    public async Task<IActionResult> GetClientDetails([FromBody] SapSearchVMGetStatus searchVM)
    {
        // String endpointurl = "http://ITCSAPWCT.grouphc.net:8000/sap/bc/srt/rfc/sap/z_bapi_customer_details/150/z_bapi_customer_details/z_bapi_customer_details";
        var endpointurl = "http://GRPSAFRANU103.grouphc.net:8020/sap/bc/srt/rfc/sap/z_bapi_customer_details/150/z_bapi_customer_details/z_bapi_customer_details";
        BasicHttpBinding binding = new BasicHttpBinding();
        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        EndpointAddress endpoint = new(endpointurl);
        var serviceClient = new Z_bapi_customer_detailsClient(binding, endpoint);
        //serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        //serviceClient.ClientCredentials.UserName.Password = "azerty2023++";
        serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        serviceClient.ClientCredentials.UserName.Password = "PH8YYzPZiUiTdn]jSWUYnYdJAAwnUmeJlFeAwEza";
        var request = new Z_BAPI_CUSTOMER_CREDITDETAILS()
        {
            CUSTOMER = searchVM.customerSap,
            CREDIT_CONTROL_AREA =searchVM.creditControlArea,
          
        };
        serviceClient.Open();
        var response = await serviceClient.Z_BAPI_CUSTOMER_CREDITDETAILSAsync(request);
        var clientsFromSap = response.Z_BAPI_CUSTOMER_CREDITDETAILSResponse;
        return Ok(clientsFromSap);
    }
    [HttpPost]
    [Route("GetConsomation")]
    public async Task<IActionResult> GetConsomation([FromBody] SapSearchVMGetStatus searchVM)
    {
        var endpointurl = "http://ITCSAPWCT.grouphc.net:8000/sap/bc/srt/wsdl/flv_10002A111AD1/srvc_url/sap/bc/srt/rfc/sap/zsd_mcsi_s805_wsr/150/zsd_mcsi_s805_wsr/zsd_mcsi_s805_wsr?sap-client=150";
        BasicHttpBinding binding = new BasicHttpBinding();
        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        EndpointAddress endpoint = new(endpointurl);
        var serviceClient = new ZSD_MCSI_S805_WSRClient(binding, endpoint);
        //serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        serviceClient.ClientCredentials.UserName.Password = "azerty2023++";
        //serviceClient.ClientCredentials.UserName.Password = "PH8YYzPZiUiTdn]jSWUYnYdJAAwnUmeJlFeAwEza";

        var request = new ZSD_MCSI_S805()
        {
            ZKUNAG = new[]
            {
                 new ZSTKUNAG
                 {
                     OPTION = searchVM.KUNAG_Option,// "EQ",
                     SIGN = searchVM.KUNAG_Sign,// "I",
                     LOW = searchVM.KUNAG_Low,
                     HIGH = searchVM.KUNAG_High
                 }
            },
            ZSPTAG = new[]
            {
                new ZSTSPTAG
                {
                    OPTION = searchVM.SPTAG_Option,
                    SIGN = searchVM.SPTAG_Sign,
                    LOW = searchVM.SPTAG_Low,
                    HIGH = searchVM.SPTAG_High
                }
            },
            T_CUST_SALES = new[]
            {
                new ZSTCUST_SALES
                {

                }
            }

        };

        serviceClient.Open();
        var response = await serviceClient.ZSD_MCSI_S805Async(request);
        var clientsFromSap = response.ZSD_MCSI_S805Response;
        return Ok(clientsFromSap);
    }
    [HttpPost]
    [Route("GetConsomationProd")]
    public async Task<IActionResult> GetConsomationProd([FromBody] SapSearchVMGetStatus searchVM)
    {
        var endpointurl = "http://GRPSAFRANU103.grouphc.net:8020/sap/bc/srt/rfc/sap/zsd_mcsi_s805_wsr/150/zsd_mcsi_s805_wsr/zsd_mcsi_s805_wsr";
        BasicHttpBinding binding = new BasicHttpBinding();
        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        EndpointAddress endpoint = new(endpointurl);
        var serviceClient = new ZSD_MCSI_S805_WSRClient(binding, endpoint);
        //serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        //serviceClient.ClientCredentials.UserName.Password = "azerty2023++";
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
                            LOW = searchVM.KUNAG_Low,
                            HIGH = ""
                        }
                    },
            ZSPTAG = new[]
            {
                        new ZSTSPTAG()
                        {
                            SIGN = Constants.I,
                            OPTION = Constants.BT,
                            LOW = searchVM.SPTAG_Low,
                            HIGH = searchVM.SPTAG_High
                        }
                    }
        };

        serviceClient.Open();
        var response = await serviceClient.ZSD_MCSI_S805Async(request);
        var clientsFromSap = response.ZSD_MCSI_S805Response;
        return Ok(clientsFromSap);
    }




    //-------------------------------- Client liste réglement -----------------------------
    [HttpPost]
    [Route("ClientListeRéglement")]
    public async Task<IActionResult> ClientListeRéglement([FromBody] SapSearchVMGetStatus model)
    {
        try
        {
            string endpointurl = "http://GRPSAFRANU103.grouphc.net:8020/sap/bc/srt/rfc/sap/zfi_cust_items_wsdl/150/zfi_cust_items_wsdl/zfi_cust_items_wsdl";
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            //binding.SendTimeout = TimeSpan.FromMinutes(3);
            //binding.MaxReceivedMessageSize = 2147483647;

            EndpointAddress endpoint = new(endpointurl);
            var serviceClient = new zfi_cust_items_wsdlClient(binding, endpoint);
            serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
            serviceClient.ClientCredentials.UserName.Password = "PH8YYzPZiUiTdn]jSWUYnYdJAAwnUmeJlFeAwEza";

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
                            Low = model.customerSap,
                            High = ""
                        }
                    }
            };

            serviceClient.Open();
            var response = await serviceClient.ZfiCustItemsAsync(request);
            var clientsFromSap = response.ZfiCustItemsResponse;
            return Ok(response);
        }
        catch (Exception ex)
        {
            string message = "Caught an exception: " + ex;
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
    public async Task<IActionResult> ClientFacturesÉchues([FromBody] SapSearchVMGetStatus model)
    {
        try
        {
            string endpointurl = "http://GRPSAFRANU103.grouphc.net:8020/sap/bc/srt/rfc/sap/zfi_cust_items_wsdl/150/zfi_cust_items_wsdl/zfi_cust_items_wsdl";
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            //binding.SendTimeout = TimeSpan.FromMinutes(3);
            //binding.MaxReceivedMessageSize = 2147483647;

            EndpointAddress endpoint = new(endpointurl);
            var serviceClient = new zfi_cust_items_wsdlClient(binding, endpoint);
            serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
            serviceClient.ClientCredentials.UserName.Password = "PH8YYzPZiUiTdn]jSWUYnYdJAAwnUmeJlFeAwEza";

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
                            Low = model.customerSap,
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
    public async Task<IActionResult> ClientListeImpayés([FromBody] SapSearchVMGetStatus model)
    {
        try
        {
            string endpointurl = "http://GRPSAFRANU103.grouphc.net:8020/sap/bc/srt/rfc/sap/zfi_cust_items_wsdl/150/zfi_cust_items_wsdl/zfi_cust_items_wsdl";
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
           // binding.SendTimeout = TimeSpan.FromMinutes(3);
           // binding.MaxReceivedMessageSize = 2147483647;

            EndpointAddress endpoint = new(endpointurl);
            var serviceClient = new zfi_cust_items_wsdlClient(binding, endpoint);
            serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
            serviceClient.ClientCredentials.UserName.Password = "PH8YYzPZiUiTdn]jSWUYnYdJAAwnUmeJlFeAwEza";

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
                            Low = model.customerSap,
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
    public async Task<IActionResult> ClientListeFacture([FromBody] SapSearchVMGetStatus model)
    {
        try
        {
            string endpointurl = "http://GRPSAFRANU103.grouphc.net:8020/sap/bc/srt/rfc/sap/zfi_cust_items_wsdl/150/zfi_cust_items_wsdl/zfi_cust_items_wsdl";
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
           // binding.SendTimeout = TimeSpan.FromMinutes(3);
           // binding.MaxReceivedMessageSize = 2147483647;

            EndpointAddress endpoint = new(endpointurl);
            var serviceClient = new zfi_cust_items_wsdlClient(binding, endpoint);
            serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
            serviceClient.ClientCredentials.UserName.Password = "PH8YYzPZiUiTdn]jSWUYnYdJAAwnUmeJlFeAwEza";

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
                            Low = model.customerSap,
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


    [HttpPost]
    [Route("CUSTOMER_PARTNERFS_GET")]
    public async Task<IActionResult> CUSTOMER_PARTNERFS_GET([FromBody] SapSearchVMGetPartner sapSearchVM)
    {
        String endpointurl = "http://ITCSAPWCT.grouphc.net:8000/sap/bc/srt/rfc/sap/zcustomer_partnerfs_get/150/zcustomer_partnerfs_get/zcustomer_partnerfs_get";
        BasicHttpBinding binding = new BasicHttpBinding();
        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        EndpointAddress endpoint = new(endpointurl);
        var serviceClient = new ZCUSTOMER_PARTNERFS_GETClient(binding, endpoint);
        serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        serviceClient.ClientCredentials.UserName.Password = "azerty2023++";
        var request = new CUSTOMER_PARTNERFS_GET()
        {
          ET_E1KNVPM = new[]
          {
              new E1KNVPM()
              {

              }
          },
          IV_KUNNR =  sapSearchVM.IV_KUNNR, //"0001046236",
          IV_VKORG = sapSearchVM.IV_VKORG,  //"MA05",
          IV_VTWEG = sapSearchVM.IV_VTWEG,  // "01",
          IV_SPART = sapSearchVM.IV_SPART   //"03"
        };
        serviceClient.Open();
        var response = await serviceClient.CUSTOMER_PARTNERFS_GETAsync(request);
        var clientsFromSap = response.CUSTOMER_PARTNERFS_GETResponse;
        return Ok(clientsFromSap);
    }
    [HttpPost]
    [Route("SAP_CREATE_COMMANDE")]
    public async Task<IActionResult> SAP_CREATE_COMMANDE([FromBody] SapCreateCommande sapCreate)
    {
        String endpointurl = "http://ITCSAPWCT.grouphc.net:8000/sap/bc/srt/rfc/sap/zbapi_salesorder_createfromdat/150/zbapi_salesorder_createfromdat/zbapi_salesorder_createfromdat";
        BasicHttpBinding binding = new BasicHttpBinding();
        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        EndpointAddress endpoint = new(endpointurl);
        var serviceClient = new ZBAPI_SALESORDER_CREATEFROMDATClient(binding, endpoint);
        serviceClient.ClientCredentials.UserName.UserName = "MAR_DSSRMC";
        serviceClient.ClientCredentials.UserName.Password = "azerty2023++";
        var request = new BAPI_SALESORDER_CREATEFROMDAT2()
        {
            ORDER_HEADER_IN = new BAPISDHD1()
            {
                DOC_TYPE = "ZROB",  // Test "ZCOC", //Type de commande
                SALES_ORG = "MA15",   // Organisation commercial
                DISTR_CHAN = "01", // Canal de distribution
                DIVISION = "03",   //   Division Usine
                LINE_TIME = "12:04:35", // Heure de livraison
                //PRICE_DATE = "2023-07-12T00:00:00", // Date de prix from date to xml form to string,
                //PURCH_NO_C = "TEST" // Numero de commande client
            },
            ORDER_HEADER_INX = new BAPISDHD1X()
            {
                DOC_TYPE = "X", //Type de commande
                SALES_ORG = "X", // Organisation commercial
                DISTR_CHAN = "X", // Canal de distribution
                DIVISION = "X",   // Division Usine
                LINE_TIME = "X", // Heure de livraison
                //PRICE_DATE = "X", // Date de prix from date to string,
                //PURCH_NO_C = "X" // Numero de commande client
            },
            ORDER_PARTNERS = new[]
         {
                new BAPIPARNR()
                {
                    PARTN_ROLE = "AG", // Role du partenaire
                    PARTN_NUMB = sapCreate.DONNANT_ORDRE // Numero du partenaire code SAP chantier
                },
                new BAPIPARNR()
                {
                    PARTN_ROLE = "WE", // Role du partenaire
                    PARTN_NUMB = sapCreate.PARTN_NUMB  // Numero du partenaire For test Same client same chantier
                }
            },
            ORDER_ITEMS_IN = new[]
            {
                new BAPISDITM()
                {
                    MATERIAL = sapCreate.CodeArticle, //"000000000002002820", // Code article
                    PLANT = sapCreate.PLANT, // Code Sap chantier USINE division
                    // ITM_NUMBER = "000010", // Numero de poste For multiple articles
                }
            },
            ORDER_SCHEDULES_IN = new[]
         {
                new BAPISCHDL()
                {
                    REQ_QTY =  sapCreate.QuantiteArticle,//28 ,// Quantité article
                    REQ_TIME = "00:00:00", // Heure de livraison
                    DLV_TIME = "00:00:00",
                    TP_TIME = "00:00:00",
                    GI_TIME = "00:00:00",
                    LOAD_TIME = "00:00:00",
                    MS_TIME = "00:00:00"
                   // ITM_NUMBER = "000010", // Numero de poste For multiple articles
                }
         },
            TESTRUN = "X",
            RETURN = new[]
            {
                new BAPIRET2()
                {

                }
            }

        };
        serviceClient.Open();
        var response = await serviceClient.BAPI_SALESORDER_CREATEFROMDAT2Async(request);
        var resultCommande = response.BAPI_SALESORDER_CREATEFROMDAT2Response.RETURN;
        return Ok(resultCommande);
    }
}