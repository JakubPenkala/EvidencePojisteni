using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using insurance.Classes;
using insurance.Data;
using insurance.Models;
using System.Data;

namespace insurance.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class InsuredController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public InsuredController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }

        // GET: Insured/Index
        [AllowAnonymous]
        public async Task<IActionResult> Index(InsuredDetailsViewModel insuredDetails)
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                if (User.IsInRole(UserRoles.Admin))
                {
                    insuredDetails.GetInsured = await _context.Insured.ToListAsync();
                    return View(insuredDetails);
                }
                else if (signInManager.IsSignedIn(User))
                {
                    insuredDetails.GetInsured = await _context.Insured
                                                .Where(p => p.Email == user.Email)
                                                .ToListAsync();
                    var id = insuredDetails.GetInsured.FirstOrDefault()?.Id;
                    return RedirectToAction("Details", "Insured", new { id });
                }
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: Insured/Details
        [AllowAnonymous]
        [HttpGet, ActionName ("Details")]
        public async Task<IActionResult> Details(InsuredDetailsViewModel insuredDetails, int? id)
        {
            if (id == null || _context.Insured == null)
            {
                return NotFound();
            }

            insuredDetails.Insured = await _context.Insured
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuredDetails.Insured == null)
            {
                return NotFound();
            }
            

            var insurance = await _context.Insurance.ToListAsync();   
            var query = from Insurance in insurance
                        where Insurance.Uid == insuredDetails.Insured?.Id
                        select Insurance;
            insuredDetails.GetInsurance = query;
            return View(insuredDetails);      
        }

        // GET: Insured/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insured/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel register, InsuredViewModel insured, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                
                IdentityUser user = new IdentityUser {UserName = register.Email, Email = register.Email};
                IdentityResult result = await userManager.CreateAsync(user, register.Password);    

                if (result.Succeeded)
                {
                    _context.Add(insured);
                    insured.Name = insured.Name.Trim();
                    insured.Surname = insured.Surname.Trim();
                    insured.Street = insured.Street.Trim();
                    insured.City = insured.City.Trim();
                    insured.PhoneNumber = insured.PhoneNumber.Trim();
                    await _context.SaveChangesAsync();
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(insured);
        }

        // GET: Insured/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insured = await _context.Insured.FindAsync(id);
            if (insured == null)
            {
                return NotFound();
            }
            return View(insured);
        }

        // POST: Insured/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Email,PhoneNumber,Street,City,ZipCode")] InsuredViewModel insured)
        {
            if (id != insured.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insured);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuredExists(insured.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(insured);
        }

        // GET: Insured/Delete
        public async Task<IActionResult> Delete(int? id, InsuredViewModel insured)
        {
            if (id == null || _context.Insured == null)
            {
                return NotFound();
            }
            insured = await _context.Insured
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insured == null)
            {
                return NotFound();
            }

            return View(insured);
        }

        // POST: Insured/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Insured == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Insured'  is null.");
            }
            var insured = await _context.Insured.FindAsync(id);
            if (insured != null)
            {
                
                DeleteFromDatabase deleteFromDatabase = new DeleteFromDatabase();
                DeleteUser deleteUser = new DeleteUser();
                deleteFromDatabase.Delete("Insurance","Uid", insured.Id);
                deleteFromDatabase.Delete("InsuranceEvents", "Iid", insured.Id);
                deleteUser.Delete(insured.Email);
                _context.Insured.Remove(insured);
            }      
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Insured");
        }

        private bool InsuredExists(int id)
        {
          return (_context.Insured?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
