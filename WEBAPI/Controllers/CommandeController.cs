﻿using System.Diagnostics;
using Domain.Models;
using Domain.Models.Commande;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;

namespace WEBAPI.Controllers;
[Route("[controller]")]

public class CommandeController : ControllerBase
{
    private readonly ICommandeService _commandeService;

    public CommandeController(ICommandeService commandeService)
    {
        _commandeService = commandeService;
    }
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create([FromBody] CommandeViewModel commandeViewModel)
    {
       /* if (file != null)
        {
            var mimeType = file.ContentType;
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.ToArray();

            //commandeViewModel.Commande.ArticleFile = blobService.UploadFileToBlob(Guid.NewGuid().ToString() + "/" + file.FileName, "Beton Spécial", fileData, mimeType);
        }*/

        
        var redirect = await _commandeService.CreateCommande(commandeViewModel);
        return redirect ? Ok() : Problem();
    }
    [HttpPost("ListeCommandes")]
    public async Task<IActionResult> ListeCommandes([FromBody] CommandeSearchVm vm)
    {
        var client = await _commandeService.GetClients(vm.IceClient, vm.CnieClient, vm.RsClient);
        if (!client.Any())
            return Problem("Client est introuvable");
        vm.IdClient = client.FirstOrDefault()!.Client_Id;
        vm.CommandesAPI = vm.UserRole switch
        {
            "Commercial" => await _commandeService.GetCommandes(vm.IdClient, vm.DateCommande),
            "Prescripteur technique" => await _commandeService.GetCommandesPT(vm.IdClient, vm.DateCommande),
            "DA BPE" => await _commandeService.GetCommandesDAPBE(vm.IdClient, vm.DateCommande),
            "Responsable commercial" => await _commandeService.GetCommandesRC(vm.IdClient, vm.DateCommande),
            "Chef de ventes" => await _commandeService.GetCommandesCV(vm.IdClient, vm.DateCommande),
            "Responsable logistique" => await _commandeService.GetCommandesRL(vm.IdClient, vm.DateCommande),
            _ => vm.CommandesAPI
        };
        return !vm.CommandesAPI.Any() ? Problem("Aucune commande/prospection trouvée pour ce client") : Ok(vm.CommandesAPI);
    }   
    [HttpGet("DetailCommande/{commandeId}")]
    public async Task<IActionResult> DetailCommande(int? commandeId)
    {
        if (commandeId is null) return Problem("Veuillez selectionner une commande");
        var details = await _commandeService.GetCommandesDetails(commandeId);
        return !details.Any() ? Problem("Commande non trouvée") : Ok(details);

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

}