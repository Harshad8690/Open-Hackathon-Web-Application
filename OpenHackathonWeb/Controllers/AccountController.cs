using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using OpenHackathonWeb.Data;
using OpenHackathonWeb.Helpers;
using OpenHackathonWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenHackathonWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly HackathonDbContext _context;
        private readonly IToastNotification _toastNotification;

        public AccountController(HackathonDbContext context,
            IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        [NonAction]
        private async Task SignInUserAsync(Users user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(Constants.UserIdType, Convert.ToString(user.Id)),
                new Claim(Constants.FirstNameType, user.FirstName),
                new Claim(Constants.LastNameType,user.LastName),
                new Claim(Constants.WalletAddressType,user.WalletAddress),
                new Claim(Constants.UserRoleType, Convert.ToString(user.UserRole))
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IssuedUtc = DateTime.UtcNow,
                IsPersistent = false,
                AllowRefresh = false
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (user != null)
                {
                    await SignInUserAsync(user);
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            _toastNotification.AddErrorToastMessage("Invalid email or password.");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("login");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password,
                    WalletAddress = model.WalletAddress,
                    UserRole = (int)UserRoles.RegisteredMember
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("login");
            }
            return View(model);
        }
    }
}