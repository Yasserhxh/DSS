using Domain.Models;
using Domain.Models.Client;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly ICommandeService commandeService;
        public ClientController(ICommandeService commandeService)
        {
            this.commandeService = commandeService;
        }
        public async Task<IActionResult> Index(ClientSearchVm vm)
        {
            vm.Clients = await commandeService.GetClients(vm.Ice, vm.Cnie, vm.RS);
            return View(vm);
        }
    }
}
