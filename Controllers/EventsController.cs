

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatchVault.Data;
using MatchVault.Models;

namespace MatchVault.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Events
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }

        // GET: /Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var wrestlingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (wrestlingEvent == null)
                return NotFound();

            // Load matches linked to this event
            var eventMatches = await _context.Matches
                .Where(m => m.EventId == wrestlingEvent.Id)
                .OrderBy(m => m.Id)
                .ToListAsync();

            ViewBag.EventMatches = eventMatches;

            return View(wrestlingEvent);
        }

        // GET: /Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Promotion,EventDate,Venue,Location,Description,ImageUrl")]
            Event wrestlingEvent)
        {
            if (!ModelState.IsValid)
                return View(wrestlingEvent);

            _context.Events.Add(wrestlingEvent);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var wrestlingEvent = await _context.Events.FindAsync(id);

            if (wrestlingEvent == null)
                return NotFound();

            return View(wrestlingEvent);
        }

        // POST: /Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Promotion,EventDate,Venue,Location,Description,ImageUrl")]
            Event wrestlingEvent)
        {
            if (id != wrestlingEvent.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(wrestlingEvent);

            try
            {
                _context.Update(wrestlingEvent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Events.AnyAsync(e => e.Id == id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var wrestlingEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (wrestlingEvent == null)
                return NotFound();

            return View(wrestlingEvent);
        }

        // POST: /Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wrestlingEvent = await _context.Events.FindAsync(id);

            if (wrestlingEvent != null)
            {
                _context.Events.Remove(wrestlingEvent);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
