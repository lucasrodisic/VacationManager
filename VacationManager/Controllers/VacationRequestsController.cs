using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using VacationManager.Data;
using VacationManager.Models;

namespace VacationManager.Controllers
{
    public class VacationRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public VacationRequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var requests = await _context.VacationRequests
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return View(requests);
        }

        
        public IActionResult Create()
        {
            return View(new VacationRequest());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacationRequest request)
        {
            var user = await _userManager.GetUserAsync(User);

            request.UserId = user.Id;
            request.CreatedAt = DateTime.Now;

            if (request.Type == VacationType.Sick && request.SickNoteUpload == null)
            {
                ModelState.AddModelError("SickNoteUpload", "При болничен е задължително да качите документ.");
                return View(request);
            }


            if (!ModelState.IsValid)
                return View(request);

            _context.VacationRequests.Add(request);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.VacationRequests.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (request == null || request.UserId != user!.Id || request.IsApproved)
                return Forbid();

            return View(request);
            
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.VacationRequests.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (request == null || request.UserId != user!.Id || request.IsApproved)
                return Forbid();

            return View(request);
        }
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _context.VacationRequests
                .Include(r => r.User)
                .ThenInclude(u => u.Team)
                .FirstOrDefaultAsync(r => r.Id == id);

            var approver = await _userManager.GetUserAsync(User);
            var isCEO = await _userManager.IsInRoleAsync(approver!, "CEO");
            var isTeamLead = request.User.Team?.TeamLeadId == approver!.Id;

           
            if (!isCEO && !isTeamLead)
                return Forbid();

            request.IsApproved = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var request = await _context.VacationRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var isOwner = request.UserId == user!.Id;
            var isCEO = await _userManager.IsInRoleAsync(user, "CEO");

            if (!isOwner && !isCEO)
                return Forbid();

            return View(request);
        }

    }
}
