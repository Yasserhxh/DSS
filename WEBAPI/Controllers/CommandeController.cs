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

namespace WEBAPI.Controllers;

[Route("[controller]")]

public class CommandeController : Controller
{
    private readonly ICommandeService _commandeService;
    private readonly IBlobService blobService;
    private readonly IAuthentificationService _authentificationService;
    private readonly UserManager<ApplicationUser> _userManager;
    private IConfiguration _configuration;

    public CommandeController(ICommandeService commandeService, IBlobService blobService,
        IAuthentificationService authentificationService, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _commandeService = commandeService;
        this.blobService = blobService;
        _authentificationService = authentificationService;
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("CheckClient")]
    public IActionResult CheckClient([FromBody] CommandeSearchVm clientInFos)
    {
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
            "Commercial" => await _commandeService.GetCommandes(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            "Prescripteur technique" => await _commandeService.GetCommandesPT(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            "DA BPE" => await _commandeService.GetCommandesDAPBE(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            "Responsable commercial" => await _commandeService.GetCommandesRC(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            "Chef de ventes" => await _commandeService.GetCommandesCV(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            "Responsable logistique" => await _commandeService.GetCommandesRL(vm.IdClients, vm.DateCommande, vm.DateDebutSearch, vm.DateFinSearch),
            _ => vm.CommandesAPI
        };
        return Ok(vm.CommandesAPI);
    }
    [HttpGet("DetailCommande/{commandeId}")]
    public async Task<IActionResult> DetailCommande(int? commandeId)
    {
        if (commandeId is null) return Problem("Veuillez selectionner une commande");
        var details = await _commandeService.GetCommandesDetails(commandeId);
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
            commandeModifApi.CommandeTarifBeton, commandeModifApi.UserEmail, commandeModifApi.CommandeBetonArticleFile, commandeModifApi.CommandeCoutDeProdBeton);
        return res;
    }
    [HttpPost]
    [Route("FixationPrixRC")]
    public async Task<bool> FixationPrixRC([FromBody] JsonBetonModifApi betonModifApi)
    {
        var res = await _commandeService.FixationPrixRC(betonModifApi.CommandeModifVenteApis, betonModifApi.Useremail, betonModifApi.IdCommande);
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
            Controller controller = this;

            var lFileResult = await ConvertHTmlToPdf.ConvertCurrentPageToPdf(controller, commande, "Pdf",
                "Devis" + commande.IdCommande);

            var content = lFileResult as FileContentResult;
            var mimeType = content?.ContentType;
            return await blobService.UploadFileToBlobAsync(content!.FileDownloadName, Guid.NewGuid().ToString(), content.FileContents, mimeType!);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    [HttpGet("GeneratePDfChantier/{id:int}")]
    public async Task<string> GeneratePDfChantier(int id)
    {
        try
        {

            var commande = await _commandeService.GetCommande(id);
            Controller controller = this;

            var lFileResult = await ConvertHTmlToPdf.ConvertCurrentPageToPdf(controller, commande, "PdfChantier",
                "FicheChantier" + commande.IdCommande);

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
       String endpointurl = "http://ITCSAPWCT.grouphc.net:8000/sap/bc/srt/rfc/sap/zbapi_customer_getlist/150/zbapi_customer_getlist/zbapi_customer_getlist";
        BasicHttpBinding binding = new BasicHttpBinding();

        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly; 
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        EndpointAddress endpoint = new(endpointurl);
        var wsclient = new zBAPI_CUSTOMER_GETLISTClient(binding,endpoint);

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
            CUSTOMER = searchVM.customerSap,//"0001046236"
            CREDITCONTROLAREA =searchVM.creditControlArea, //"1474"
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
}