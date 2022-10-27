using System.Diagnostics;
using System.Globalization;
using Domain.Models;
using Domain.Models.ApiModels;
using Domain.Models.Commande;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using WEBAPI.Tools;

namespace WEBAPI.Controllers;

[Route("[controller]")]

public class CommandeController : Controller
{
    private readonly ICommandeService _commandeService;
    private readonly IBlobService blobService;
    private readonly IAuthentificationService _authentificationService;

    public CommandeController(ICommandeService commandeService, IBlobService blobService,
        IAuthentificationService authentificationService)
    {
        _commandeService = commandeService;
        this.blobService = blobService;
        _authentificationService = authentificationService;
    }

    [HttpPost]
    [Route("CheckClient")]
    public IActionResult CheckClient([FromBody] CommandeSearchVm clientInFos)
    {
        var result = _commandeService.FindFormulaireClient(clientInFos.IceClient, clientInFos.CnieClient);
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

  public string ConvertFile([FromBody]  IFormFile? file)
  {
      if (file == null) return "";
      var mimeType = file.ContentType;
      byte[] fileData;
      using (var ms = new MemoryStream())
      {
          file.CopyTo(ms);
          fileData = ms.ToArray();
      }

      var str=  blobService.UploadFileToBlob(Guid.NewGuid() + "/" + file.FileName,
          "Beton Spécial", fileData, mimeType);
      return str;

  }

  [HttpPost]
    [Route("Create")]

public async Task<IActionResult> Create([FromBody] CommandeViewModel commandeViewModel, IFormFile? file)
    {
        var redirect = await _commandeService.CreateCommande(commandeViewModel);
        return redirect ? Ok("La prospection est crée avec succées") : Problem();
    }

    [HttpGet]
    [Route("FindUserRoleByEmail/{email}")]
    public async Task<IActionResult> FindUserRoleByEmail(string email)
    {
        var res = await _authentificationService.FindUserRoleByEmail(email);
        return string.IsNullOrEmpty(email) ? Problem(statusCode: StatusCodes.Status409Conflict, title:"Utilisateur introuvable!") : Ok(res);
    }
    [HttpPost("ListeCommandes")]
    public async Task<IActionResult> ListeCommandes([FromBody] CommandeSearchVm vm)
    {
        var client = await _commandeService.GetClients(vm.IceClient, vm.CnieClient, vm.RsClient);
        if (!client.Any())
            return Ok(new List<CommandeApiModel>());
        vm.IdClients = client.Select(x=>x.Client_Id).ToList();
        vm.CommandesAPI = vm.UserRole switch
        {
            "Commercial" => await _commandeService.GetCommandes(vm.IdClients, vm.DateCommande,vm.DateDebutSearch, vm.DateFinSearch),
            "Prescripteur technique" => await _commandeService.GetCommandesPT(vm.IdClients, vm.DateCommande ,vm.DateDebutSearch, vm.DateFinSearch),
            "DA BPE" => await _commandeService.GetCommandesDAPBE(vm.IdClients, vm.DateCommande,vm.DateDebutSearch, vm.DateFinSearch),
            "Responsable commercial" => await _commandeService.GetCommandesRC(vm.IdClients, vm.DateCommande,vm.DateDebutSearch, vm.DateFinSearch),
            "Chef de ventes" => await _commandeService.GetCommandesCV(vm.IdClients, vm.DateCommande,vm.DateDebutSearch, vm.DateFinSearch),
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
    }[HttpGet("GetZones")]
    public async Task<IActionResult> GetZones()
    {
        return Ok(await _commandeService.GetZones());
    }[HttpGet("GetArticles")]
    public async Task<IActionResult> GetArticles()
    {
        return Ok(await _commandeService.GetArticles());
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
    public async Task<IActionResult> GetVilles() =>  Ok(await _commandeService.GetVilles());
    
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
    public async Task<bool> FixationPrixTransport([FromBody] CommandeModifApi commandeModifApi)
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
            commandeModifApi.CommandeTarifBeton, commandeModifApi.UserEmail);
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
        vm.IdClients = client.Select(x=>x.Client_Id).ToList();
        vm.CommandesAPI =
            await _commandeService.GetCommandesValide(vm.IdClients, vm.DateCommande, vm.DateDebutSearch,
                vm.DateFinSearch);
        return Ok(vm.CommandesAPI);
    }

}