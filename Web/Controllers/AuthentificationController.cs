using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Authentication;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Service.IServices;
using Service.Services;

namespace Web.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly IAuthentificationService authentificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AuthentificationController(IAuthentificationService authentificationService, UserManager<ApplicationUser> userManager
            , IMapper mapper)
        {
            this.authentificationService = authentificationService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user)
        {
            var userAuth = await this.authentificationService.Login(user);

            if (userAuth == null)
            {
                TempData["LoginError"] = "Login ou mot de passe incorrect";
                return View();
            }
            var roles = await _userManager.GetRolesAsync(userAuth);
            if (roles.Contains("Prescripteur technique"))
                return RedirectToAction("ListeCommandesPT", "Commande");
            else if (roles.Contains("Admin"))
                return RedirectToAction("ListeCommandes", "Commande");
            else if (roles.Contains("Chef de ventes"))
                return RedirectToAction("ListeCommandesCV", "Commande");
            else if (roles.Contains("Responsable commercial"))
                return RedirectToAction("ListeCommandesRC", "Commande");
            else if (roles.Contains("DA BPE"))
                return RedirectToAction("ListeCommandesDABPE", "Commande");
            else if (roles.Contains("Responsable logistique"))
                return RedirectToAction("ListeCommandesRL", "Commande");
            else if (roles.Contains("Commercial"))
                return RedirectToAction("Index", "Client");
            else
                return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            var success = await this.authentificationService.Register(user);

            if (!success)
            {
                TempData["RegisterError"] = "Une erreur s'est produite";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<bool> Logout()
        {
            return await this.authentificationService.Logout();
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["MailError"] = "Format email invalide";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
            {
                TempData["MailError"] = "Email non trouvé";
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Authentification", new { token, email = user.Email }, Request.Scheme);

            var writer = new StringWriter();
            var engine =
                HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            var view = engine.FindView(ControllerContext, "ForgotPasswordEmail", true);
            var viewContext = new ViewContext(
                ControllerContext,
                view.View,
                ViewData,
                TempData,
                writer,
                new HtmlHelperOptions()
            );
            var forgotPassEmailVm = new ForgotPasswordEmailVm()
            {
                Link = link
            };
            var currentRequestModel = ViewData.Model;
            ViewData.Model = forgotPassEmailVm;
            await view.View.RenderAsync(viewContext);
            ViewData.Model = currentRequestModel;
            var html = writer.GetStringBuilder().ToString();
            var subject = "Récupération de mot de passe";


            TempData["MailConfirmation"] = "L'email a été envoyé. Veuillez vérifier votre boîte email pour réinitialiser votre mot de passe.";

            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
            {
                TempData["MailError"] = "Les mots de passe ne correspondent pas";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                TempData["MailError"] = "Email non trouvé";
                return View();
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);

            if (!resetPassResult.Succeeded)
            {
                TempData["MailError"] = "Une erreur s'est produite";

                return View();
            }

            TempData["MailReset"] = "le mot de passe a été réinitialisé. Veuillez vous connecter.";

            return View();
        }
        public async Task<IActionResult> ListeDesUtilisateurs()
        {
            var users = await authentificationService.getListUsers();
            return View(users);
        }

        public async Task<bool> ResetPasswordByAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return false;

            try
            {
                var writer = new StringWriter();
                var engine =
                    HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                var view = engine.FindView(ControllerContext, "ResetPassswordEmail", true);
                var viewContext = new ViewContext(
                    ControllerContext,
                    view.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );
                //var tempPassword = GenerateRandomPassword.Generate(10);
                var forgotPassEmailVm = new ForgotPasswordEmailVm()
                {
                    //Password = tempPassword,
                    UserName = user.UserName
                };
                var currentRequestModel = ViewData.Model;
                ViewData.Model = forgotPassEmailVm;
                await view.View.RenderAsync(viewContext);
                ViewData.Model = currentRequestModel;
                var hasher = new PasswordHasher<ApplicationUser>();
                //user.PasswordHash = hasher.HashPassword(user, tempPassword);
                await _userManager.UpdateAsync(user);
                var html = writer.GetStringBuilder().ToString();
                var subject = "Rénitialisation de mot de passe";

                //await _sendGridMailService.SendEmaiAsync(user.Email, html, subject);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<bool> EnableDisableUser(string id, int code)
        {
            return await authentificationService.EnableDisableUser(id, code);
        }
        public async Task<IActionResult> AjouterUnUtilisateur()
        {
            var Roles = await authentificationService.GetRoles();
            ViewData["Roles"] = new SelectList(Roles, "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AjouterUnUtilisateur([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Required error";
                var Roles = await authentificationService.GetRoles();
                ViewData["Roles"] = new SelectList(Roles, "Name", "Name");
                return View();
            }
            var response = await authentificationService.AjouterUnUtilisateur(model);

            if (!response.Success)
            {
                var Roles = await authentificationService.GetRoles();
                ViewData["Roles"] = new SelectList(Roles, "Name", "Name");
                TempData["Erreur"] = response.Message;
                return View();
            }

            try
            {
                var writer = new StringWriter();
                var engine =
                    HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                var view = engine.FindView(ControllerContext, "AddUserEmail", true);
                var viewContext = new ViewContext(
                    ControllerContext,
                    view.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );
                var forgotPassEmailVm = new ForgotPasswordEmailVm()
                {
                    Password = model.Password,
                    UserName = model.UserName
                };
                var currentRequestModel = ViewData.Model;
                ViewData.Model = forgotPassEmailVm;
                await view.View.RenderAsync(viewContext);
                ViewData.Model = currentRequestModel;
                var html = writer.GetStringBuilder().ToString();
                var subject = "Création de compte";

                //await _sendGridMailService.SendEmaiAsync(model.Email, html, subject);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            TempData["Creation"] = "OK";
            return RedirectToAction("ListeDesUtilisateurs", "Authentification");
        }

        public async Task<IActionResult> ModifierUnUtilisateurByAdmin(string id)
        {           
            var User = await _userManager.FindByIdAsync(id);
            var Roles = await _userManager.GetRolesAsync(User);
            var userModel = _mapper.Map<ApplicationUser, UserModel>(User);
            userModel.Role = Roles.FirstOrDefault();
            ViewData["Roles"] = new SelectList(await authentificationService.GetRoles(), "Name", "Name");
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> ModifierUnUtilisateurByAdmin([FromForm] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Required error";
                ViewData["Roles"] = new SelectList(await authentificationService.GetRoles(), "Name", "Name");
                return View();
            }
            var result = await authentificationService.UpdateUser(model);
            if (!result.Success)
            {
                ViewData["Roles"] = new SelectList(await authentificationService.GetRoles(), "Name", "Name");
                TempData["Erreur"] = result.Message;
                return View();
            }
            TempData["Creation"] = "OKM";
            return RedirectToAction("ListeDesUtilisateurs", "Authentification");
        }

        public async Task<IActionResult> ModifierUnUtilisateur()
        {
            var User = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var userModel = _mapper.Map<ApplicationUser, UpdateUserModel>(User);
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> ModifierUnUtilisateur([FromForm] UpdateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Required error";
                return View();
            }
            var result = await authentificationService.UpdateProfil(model);
            if (!result.Success)
            {
                TempData["Erreur"] = result.Message;
                return View();
            }
            TempData["Modification"] = "Profil modifié avec succès";
            return View();
        }
    }
}