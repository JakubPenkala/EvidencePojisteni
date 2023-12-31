using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using insurance.Data;
using insurance.Models;

namespace insurance.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext _context;

        public AccountsController
        (
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }
        private IActionResult RedirectToLocal(string? returnUrl = null)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        public IActionResult Login(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
 
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result =
                    await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                Console.WriteLine(model.Email + model.Password);
                if (result.Succeeded) {
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError("Login error", "Neplatné přihlašovací údaje.");
                return View(model);
            }
            return View(model);
        }
        public IActionResult Register(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name,Surname,Email,Password,ConfirmPassword,PhoneNumber,Street,City,ZipCode")] RegisterViewModel model, InsuredViewModel insured, string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { UserName = model.Email, Email = model.Email};
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _context.Add(insured);
                    insured.Name = insured.Name.Trim();
                    insured.Surname = insured.Surname.Trim();
                    insured.Street = insured.Street.Trim();
                    insured.City = insured.City.Trim();
                    insured.PhoneNumber = insured.PhoneNumber.Trim();
                    await _context.SaveChangesAsync();
                    await signInManager.SignInAsync(user, isPersistent: false); 
                    return RedirectToLocal(returnUrl);
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToLocal(null);
        }


    }
}

