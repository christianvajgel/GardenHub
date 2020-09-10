using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GardenHub.Domain.Account;
using Microsoft.AspNetCore.Http;
using GardenHub.Services.Account;
using GardenHub.Web.ViewModel.Account;
using Microsoft.AspNetCore.Authorization;

namespace GardenHub.Web.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService AccountService { get; set; }
        private IAccountIdentityManager AccountIdentityManager { get; set; }

        // Dependency Injection
        public AccountController(IAccountService accountService, IAccountIdentityManager accountIdentityManager)
        {
            this.AccountService = accountService;
            this.AccountIdentityManager = accountIdentityManager;
        }

        // Create GET
        public IActionResult Create()
        {
            return View();
        }

        // Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Account account)
        {
            try
            {
                await this.AccountService.Save(account);
                return RedirectToAction(nameof(Login));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("APP_ERROR", ex.Message);
                return View();
            }
        }

        // Update GET
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject"));
            if (user == null)
            {
                Redirect("/");
            }
            return View(await AccountService.FindById(new Guid(user.ToString())));
        }

        // Update POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Account account)
        {
            try
            {
                await this.AccountService.Update(account);
                return RedirectToAction(nameof(Edit));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("APP_ERROR", ex.Message);
                return View();
            }
        }

        // Delete GET
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            var user = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject"));
            if (user == null)
            {
                Redirect("/");
            }
            return View(await AccountService.FindById(new Guid(user.ToString())));
        }

        // Delete POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Account account)
        {
            try
            {
                await this.AccountService.Delete(account);
                await this.AccountIdentityManager.Logout();
                this.HttpContext.Session.Clear();
                return RedirectToAction(nameof(Create));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("APP_ERROR", ex.Message);
                return View();
            }
        }

        // Login GET
        public IActionResult Login(string returnUrl = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // Login POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                var result = await this.AccountIdentityManager.Login(model.UserName, model.Password);
                if (result.Succeeded)
                {
                    var userToSave = await this.AccountService.GetAccountByUserNamePassword(model.UserName, model.Password);
                    this.HttpContext.Session.SetString("UserObject", JsonConvert.SerializeObject(userToSave.Id));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Login ou senha inválidos");
                    return View(model);
                }
                if (!String.IsNullOrWhiteSpace(returnUrl))
                    return Redirect(returnUrl);
                //return Redirect("/");
                return RedirectToAction("Home", "Post");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro, por favor tente mais tarde.");
                return View(model);
            }
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await this.AccountIdentityManager.Logout();
            this.HttpContext.Session.Clear();
            return Redirect("Login");
        }

        // Index 
        public IActionResult Index()
        {
            return View();
        }

        // Home
        [Authorize]
        public async Task<IActionResult> Home()
        {
            try 
            {
                var user = JsonConvert.DeserializeObject(this.HttpContext.Session.GetString("UserObject")).ToString();
                return View(await AccountService.FindById(new Guid(user)));
            } 
            catch 
            {
                return View("Login");
            }
            
            //if (user == null)
            //{
            //    Redirect("Login");
            //}
            
        }
    }
}
