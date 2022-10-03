﻿using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;

namespace Web.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly IAuthentificationService authentificationService;

        public AuthentificationController(IAuthentificationService authentificationService)
        {
            this.authentificationService = authentificationService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user)
        {
            var success = await this.authentificationService.Login(user);

            if (success is not null)
            {
                TempData["LoginError"] = "Login ou mot de passe incorrect";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            var success = await authentificationService.Register(user);

            if (!success)
            {
                TempData["RegisterError"] = "Une erreur s'est produite";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //public async Task<bool> Logout()
        //{
        //    return await this.authentificationService.Logout();
        //}
    }
}