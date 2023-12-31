using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using insurance.Data;
using insurance.Models;

namespace insurance.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class InsuranceEventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public InsuranceEventsController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }

        // GET: InsuranceEvents/Index
        [AllowAnonymous]
        public async Task<IActionResult> Index(InsuranceEventsDetailsViewModel insuranceEventsDetails)
        {
            var user = await userManager.GetUserAsync(User);
            if (User.IsInRole(UserRoles.Admin))
            {
                if (_context.InsuranceEvents != null)
                {
                    insuranceEventsDetails.GetInsuranceEvents = await _context.InsuranceEvents.ToListAsync();
                    return View(insuranceEventsDetails);
                }
            }
            if (signInManager.IsSignedIn(User))
            {
                var insured = await _context.Insured
                        .Where(p => p.Email == user.Email)
                        .FirstOrDefaultAsync();

                if (insured != null)
                {
                    var id = insured.Id;

                    var insuranceList = await _context.Insurance
                        .Where(p => p.Uid == id)
                        .ToListAsync();
                    List<InsuranceEventsViewModel> insuranceEventsList = new List<InsuranceEventsViewModel>();
                    foreach (var insurance in insuranceList)
                    {
                        var insuranceId = insurance.Id;

                        var insuranceEvents = await _context.InsuranceEvents
                            .Where(p => p.Iid == insuranceId)
                            .ToListAsync();
                        insuranceEventsList.AddRange(insuranceEvents);
                        
                    }
                    insuranceEventsDetails.GetInsuranceEvents = insuranceEventsList;
                    return View(insuranceEventsDetails);
                }            
            }
            return RedirectToAction("Login", "Accounts");
        }

        // GET: InsuranceEvents/Details
        [AllowAnonymous]
        public async Task<IActionResult> Details(InsuranceEventsDetailsViewModel insuranceEventsDetails, int? id)
        {
            if (id == null || _context.Insured == null)
            {
                return NotFound();
            }

            insuranceEventsDetails.InsuranceEvents = await _context.InsuranceEvents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceEventsDetails.InsuranceEvents == null)
            {
                return NotFound();
            }


            var insuranceEvents = await _context.InsuranceEvents.ToListAsync();
            var query = from InsuranceEvents in insuranceEvents
                        where InsuranceEvents.Iid == insuranceEventsDetails.Insurance?.Id
                        select InsuranceEvents;
            insuranceEventsDetails.GetInsuranceEvents = query;
            return View(insuranceEventsDetails);

        }

        // GET: InsuranceEvents/Create
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        // POST: InsuranceEvents/Create
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int Id, [Bind("Iid,Event,EventDate")] InsuranceEventsViewModel insuranceEvents)
        {
            if (ModelState.IsValid)
            {
                    _context.Add(insuranceEvents);
                    insuranceEvents.Iid = Id;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));              
            }
            return View(insuranceEvents);
        }

        // GET: InsuranceEvents/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InsuranceEvents == null)
            {
                return NotFound();
            }

            var insuranceEvents = await _context.InsuranceEvents.FindAsync(id);
            if (insuranceEvents == null)
            {
                return NotFound();
            }
            return View(insuranceEvents);
        }

        // POST: InsuranceEvents/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Iid,Event,EventDate")] InsuranceEventsViewModel insuranceEvents)
        {
            if (id != insuranceEvents.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuranceEvents);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceEventsExists(insuranceEvents.Id))
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
            return View(insuranceEvents);
        }

        // GET: InsuranceEvents/Delete
        public async Task<IActionResult> Delete(int? id,InsuranceEventsViewModel insuranceEvents)
        {
            if (id == null || _context.InsuranceEvents == null)
            {
                return NotFound();
            }

            insuranceEvents = await _context.InsuranceEvents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuranceEvents == null)
            {
                return NotFound();
            }

            return View(insuranceEvents);
        }

        // POST: InsuranceEvents/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, InsuranceEventsViewModel insuranceEvents, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (_context.InsuranceEvents == null)
            {
                return Problem("Entity set 'ApplicationDbContext.InsuranceEvents'  is null.");
            }
            insuranceEvents = await _context.InsuranceEvents.FindAsync(id);
            if (insuranceEvents != null)
            {
                _context.InsuranceEvents.Remove(insuranceEvents);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceEventsExists(int id)
        {
          return (_context.InsuranceEvents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
