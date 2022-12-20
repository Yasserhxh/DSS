using Domain.Models;
using Domain.Models.Commande;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class CommandeController : Controller
    {
        private readonly ICommandeService commandeService;
        private readonly IBlobService _blobService;
        public CommandeController(ICommandeService commandeService, IBlobService blobService)
        {
            this.commandeService = commandeService;
            _blobService = blobService;
        }
        

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CommandeViewModel commandeViewModel, IFormFile? file)
        {
            if (file != null)
            {
                var mimeType = file.ContentType;
                byte[] fileData;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileData = ms.ToArray();
                }

                commandeViewModel.Commande.ArticleFile = _blobService.UploadFileToBlob(Guid.NewGuid()+ "/" + file.FileName, "Beton Spécial", fileData, mimeType);
            }
            var redirect = await commandeService.CreateCommande(commandeViewModel);
            if (redirect)
            {
                TempData["Creation"] = "OK";
                return Redirect("/Commande/ListeCommandes");
            }

            TempData["Creation"] = "KO";
            return Redirect("/Home/Index");
        }

        [Authorize(Roles = "Admin,Commercial")]
        public async Task<IActionResult> ListeCommandes(CommandeSearchVm vm)
        {
            vm.CLients = await commandeService.GetClients();
            //vm.Commandes = await commandeService.GetCommandes(vm.IdClient, vm.DateCommande);
            return View(vm);
        }
        [Authorize(Roles = "Admin,Prescripteur technique")]
        public async Task<IActionResult> ListeCommandesPT(CommandeSearchVm vm)
        {
            vm.CLients = await commandeService.GetClients();
            //vm.Commandes = await commandeService.GetCommandesPT(vm.IdClient, vm.DateCommande);
            return View(vm);
        }
        [Authorize(Roles = "Admin,DA BPE")]
        public async Task<IActionResult> ListeCommandesDABPE(CommandeSearchVm vm)
        {
            vm.CLients = await commandeService.GetClients();
            //vm.Commandes = await commandeService.GetCommandesDAPBE(vm.IdClient, vm.DateCommande);
            return View(vm);
        }
        [Authorize(Roles = "Admin,Responsable commercial")]
        public async Task<IActionResult> ListeCommandesRC(CommandeSearchVm vm)
        {
            vm.CLients = await commandeService.GetClients();
            //vm.Commandes = await commandeService.GetCommandesRC(vm.IdClient, vm.DateCommande);
            return View(vm);
        }
        [Authorize(Roles = "Admin,Chef de ventes")]
        public async Task<IActionResult> ListeCommandesCV(CommandeSearchVm vm)
        {
            vm.CLients = await commandeService.GetClients();
            //vm.Commandes = await commandeService.GetCommandesCV(vm.IdClient, vm.DateCommande);
            return View(vm);
        }
        [Authorize(Roles = "Admin,Responsable logistique")]
        public async Task<IActionResult> ListeCommandesRL(CommandeSearchVm vm)
        {
            vm.CLients = await commandeService.GetClients();
            //vm.Commandes = await commandeService.GetCommandesRL(vm.IdClient, vm.DateCommande);
           // return View(vm);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            var commande = await commandeService.GetCommande(id);
            return View(commande);
        }
        [Authorize(Roles = "Admin,Prescripteur technique")]
        public async Task<IActionResult> DetailPT(int? id)
        {
            var commande = await commandeService.GetCommande(id);
            return View(commande);
        }
        [Authorize(Roles = "Admin,DA BPE")]
        public async Task<IActionResult> DetailDABPE(int? id)
        {
            var commande = await commandeService.GetCommande(id);
            return View(commande);
        }
        [HttpPost]
        public async Task<bool> PorposerPrix(int id, decimal Tarif)
        {
            var UserName = User.Identity.Name;
            var success = await commandeService.ProposerPrix(id, Tarif, UserName);
            return success;
        }
        [HttpPost]
        public async Task<bool> PorposerPrixDABPE(int id, decimal Tarif)
        {
            var success = await commandeService.ProposerPrixDABPE(id, Tarif);
            return success;
        }
        [HttpPost]
        public async Task<bool> ValiderCommande(int id, string Commentaire)
        {
            var UserName = User.Identity.Name;
            var success = await commandeService.ValiderCommande(id, Commentaire, UserName);
            return success;
        }
        [HttpPost]
        public async Task<bool> RefuserCommande(int id, string Commentaire)
        {
            var UserName = User.Identity.Name;
            var success = await commandeService.RefuserCommande(id, Commentaire, UserName);
            return success;
        }
        public async Task<IActionResult> ModifierCommande(int id)
        {
            ViewData["FormeJuridique"] = new SelectList( await commandeService.GetFormeJuridiques(), "FormeJuridique_Id", "FormeJuridique_Libelle");
            ViewData["TypeChantier"] = new SelectList(await commandeService.GetTypeChantiers(), "Tc_Id", "Tc_Libelle");
            ViewData["Zone"] = new SelectList( await commandeService.GetZones(), "Zone_Id", "Zone_Libelle");
            ViewData["Article"] = new SelectList( await commandeService.GetArticles(), "Article_Id", "Designation");
            ViewData["DelaiPaiement"] = new SelectList( await commandeService.GetDelaiPaiements(), "Delai_Id", "Delai_Libelle");
            ViewData["Centrale"] = new SelectList(await commandeService.GetCentraleBetons(), "Ctr_Id", "Ctr_Nom");
            ViewData["Pompe"] = new SelectList(await commandeService.GetTarifPompeRefs(), "Tpr_Id", "LongFleche_Libelle");

            var commande = await commandeService.GetCommande(id);
            var Model = new CommandeViewModel()
            {
                Commande = commande,
                DetailCommandes = commande.DetailCommandes,
                Chantier = commande.Chantier,
                Client = commande.Client
            };

            return View(Model);
        }
        [HttpPost]
        public async Task<IActionResult> ModifierCommande(int id,CommandeViewModel commandeViewModel)
        {
            var UserName = User.Identity?.Name;
            var redirect = await commandeService.UpdateCommande(id, commandeViewModel, UserName);
            if (redirect)
            {
                TempData["Modification"] = "OK";
                return Redirect("/Commande/ListeCommandes");
            }
            ViewData["FormeJuridique"] = new SelectList(await commandeService.GetFormeJuridiques(), "FormeJuridique_Id", "FormeJuridique_Libelle");
            ViewData["TypeChantier"] = new SelectList(await commandeService.GetTypeChantiers(), "Tc_Id", "Tc_Libelle");
            ViewData["Zone"] = new SelectList(await commandeService.GetZones(), "Zone_Id", "Zone_Libelle");
            ViewData["Article"] = new SelectList(await commandeService.GetArticles(), "Article_Id", "Designation");
            ViewData["DelaiPaiement"] = new SelectList(await commandeService.GetDelaiPaiements(), "Delai_Id", "Delai_Libelle");
            ViewData["Centrale"] = new SelectList(await commandeService.GetCentraleBetons(), "Ctr_Id", "Ctr_Nom");
            ViewData["Pompe"] = new SelectList(await commandeService.GetTarifPompeRefs(), "Tpr_Id", "LongFleche_Libelle");
            return View();
            
        }
        [HttpPost]
        public async Task<bool> FixationPrixTransport(int id, double VenteT, double VenteP)
        {
            var success = await commandeService.FixationPrixTransport(id, VenteT, VenteP);
            return success;
        }
    }
}
