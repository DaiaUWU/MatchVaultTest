
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatchVault.Data;
using MatchVault.Models;

namespace MatchVault.Controllers
{
    public class WrestlersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WrestlersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Wrestlers
        public async Task<IActionResult> Index()
        {
            var wrestlers = await _context.Wrestlers
                .OrderBy(w => w.Name)
                .ToListAsync();

            return View(wrestlers);
        }

        // GET: /Wrestlers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var wrestler = await _context.Wrestlers
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wrestler == null)
                return NotFound();

            return View(wrestler);
        }

        // GET: /Wrestlers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Wrestlers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Country,Promotion,Biography,ImageUrl,DebutDate")]
            Wrestler wrestler)
        {
            if (!ModelState.IsValid)
                return View(wrestler);

            _context.Wrestlers.Add(wrestler);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Wrestlers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var wrestler = await _context.Wrestlers.FindAsync(id);

            if (wrestler == null)
                return NotFound();

            return View(wrestler);
        }

        // POST: /Wrestlers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Country,Promotion,Biography,ImageUrl,DebutDate")]
            Wrestler wrestler)
        {
            if (id != wrestler.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(wrestler);

            try
            {
                _context.Update(wrestler);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Wrestlers
                    .AnyAsync(w => w.Id == id))
                    return NotFound();

                throw;
            }

            // Return to wrestler details after saving
            return RedirectToAction(
                nameof(Details),
                new { id = wrestler.Id }
            );
        }

        // GET: /Wrestlers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var wrestler = await _context.Wrestlers
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wrestler == null)
                return NotFound();

            return View(wrestler);
        }

        // POST: /Wrestlers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wrestler = await _context.Wrestlers.FindAsync(id);

            if (wrestler != null)
            {
                _context.Wrestlers.Remove(wrestler);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
