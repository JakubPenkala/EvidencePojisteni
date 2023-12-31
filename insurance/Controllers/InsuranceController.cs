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
    public class InsuranceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public InsuranceController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }
        // GET: Insurance/Index
        [AllowAnonymous]
        public async Task<IActionResult> Index(InsuranceDetailsViewModel insuranceDetails)
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                    if (User.IsInRole(UserRoles.Admin))
                    {
                    insuranceDetails.GetInsurance = await _context.Insurance.ToListAsync();
                        return View(insuranceDetails);
                    }
                    else if (signInManager.IsSignedIn(User))
                    {
                        var insured = await _context.Insured
                        .Where(p => p.Email == user.Email)
                        .ToListAsync();
                        var id = insured.First().Id;
                    var pojisteni = await _context.Insurance
                                                        .Where(p => p.Uid == id)
                                                        .ToListAsync();
                    insuranceDetails.GetInsurance = pojisteni;
                        return View(insuranceDetails);
                    }
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: Insurance/Details
        [AllowAnonymous]
        public async Task<IActionResult> Details(InsuranceDetailsViewModel insuranceDetails, int? id)
        {
            if (id == null || _context.Insurance == null)
            {
                return NotFound();
            }

            insuranceDetails.Insurance = await _context.Insurance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceDetails.Insurance == null)
            {
                return NotFound();
            }
            var insuranceEvents = await _context.InsuranceEvents.ToListAsync();
            var query = from InsuranceEvents in insuranceEvents
                        where InsuranceEvents.Iid == insuranceDetails.Insurance?.Id
                        select InsuranceEvents;
            insuranceDetails.GetInsuranceEvents = query;
            return View(insuranceDetails);
        }

        // GET: Insurance/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            InsuranceTypes insuranceTypes = new InsuranceTypes();
            var insurances = insuranceTypes.GetInsuranceTypes();
            return View(insurances);
        }

        // POST: Insurance/Create
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int Id, [Bind("Uid,InsuranceType,Sum,SubjectOfInsurance,ValidFrom,ValidUntil")] InsuranceViewModel insurance)
        {
            if (ModelState.IsValid)
            {
                    _context.Add(insurance);
                    insurance.Uid = Id;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));          
            }
            InsuranceTypes insuranceTypes = new InsuranceTypes();
            insurance.InsuranceList = insuranceTypes.GetInsuranceTypes().InsuranceList;
            return View(insurance);
        }

        // GET: Insurance/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Insurance == null)
            {
                return NotFound();
            }
            InsuranceTypes insuranceTypes = new InsuranceTypes();          
            var insurance = await _context.Insurance.FindAsync(id);
            insurance.InsuranceList = insuranceTypes.GetInsuranceTypes().InsuranceList;
            if (insurance == null)
            {
                return NotFound();
            }
            return View(insurance);
        }

        // POST: Insurance/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Uid,InsuranceType,Sum,SubjectOfInsurance,ValidFrom,ValidUntil")] InsuranceViewModel insurance)
        {
            if (id != insurance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.Id))
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
            InsuranceTypes insuranceTypes = new InsuranceTypes();
            insurance.InsuranceList = insuranceTypes.GetInsuranceTypes().InsuranceList;
            return View(insurance);
        }

        // GET: Insurance/Delete
        public async Task<IActionResult> Delete(int? id, InsuranceViewModel insurance)
        {
            if (id == null || _context.Insurance == null)
            {
                return NotFound();
            }

            insurance = await _context.Insurance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // POST: Insurance/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, InsuranceViewModel insurance)
        {
            if (_context.Insurance == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Insurance'  is null.");
            }
            insurance = await _context.Insurance.FindAsync(id);
            if (insurance != null)
            {
                DeleteFromDatabase deleteDeleteFromDatabase = new DeleteFromDatabase();
                deleteDeleteFromDatabase.Delete("InsuranceEvents", "Iid", insurance.Id);
                _context.Insurance.Remove(insurance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceExists(int id)
        {
          return (_context.Insurance?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
