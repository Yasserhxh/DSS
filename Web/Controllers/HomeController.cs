using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.IServices;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ICommandeService _commandeService;
    public HomeController(ICommandeService commandeService)
    {
        _commandeService = commandeService;
    }
    public async Task<IActionResult> Index()
    {
        ViewData["FormeJuridique"] = new SelectList(await _commandeService.GetFormeJuridiques(), "FormeJuridique_Id", "FormeJuridique_Libelle");
        ViewData["TypeChantier"] = new SelectList(await _commandeService.GetTypeChantiers(), "Tc_Id", "Tc_Libelle");
        ViewData["Zone"] = new SelectList(await _commandeService.GetZones(), "Zone_Id", "Zone_Libelle");
        ViewData["Article"] = new SelectList(await _commandeService.GetArticles(), "Article_Id", "Designation");
        ViewData["DelaiPaiement"] = new SelectList(await _commandeService.GetDelaiPaiements(), "Delai_Id", "Delai_Libelle");
        ViewData["Centrale"] = new SelectList(await _commandeService.GetCentraleBetons(), "Ctr_Id", "Ctr_Nom");
        ViewData["Pompe"] = new SelectList(await _commandeService.GetTarifPompeRefs(), "Tpr_Id", "LongFleche_Libelle");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public async Task<double> GetTarifArticle(int id)
    {
        return await _commandeService.GetTarifArticle(id);
    }
    public async Task<double> GetTarifZone(int id)
    {
        return await _commandeService.GetTarifZone(id);
    }
    public async Task<double> GetTarifPompe(int id)
    {
        return await _commandeService.GetTarifPompe(id);
    }
    public new IActionResult Unauthorized()
    {
        return View();
    }

    /*[HttpPost]
    public void LogJavaScriptError(string message)
    {
        string strPath = "Errors/JavascriptLog.txt";
        if (!System.IO.File.Exists(strPath))
        {
            System.IO.File.Create(strPath).Dispose();
        }

        using var sw = System.IO.File.AppendText(strPath);
        sw.WriteLine("=============Error Logging ===========");
        sw.WriteLine("===========Start============= " + DateTime.Now);
        sw.WriteLine("Error Message: " + message);
        sw.WriteLine("===========End============= " + DateTime.Now + "\r\n");
    }*/
}