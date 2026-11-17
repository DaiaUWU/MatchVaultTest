
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MatchVault.Data;
using MatchVault.Models;

namespace MatchVault.Controllers
{
    public class MatchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MatchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Matches
        // Search, Filter and Sort
        public async Task<IActionResult> Index(
            string? search,
            string? promotion,
            string? sort)
        {
            var query = _context.Matches
                .Include(m => m.WrestlingEvent)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(m =>
                    m.Title.Contains(search) ||
                    m.Wrestler1.Contains(search) ||
                    m.Wrestler2.Contains(search) ||
                    m.Event.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(promotion))
            {
                query = query.Where(m =>
                    m.Promotion == promotion);
            }

            query = sort switch
            {
                "oldest" => query
                    .OrderBy(m => m.MatchDate),

                "rating_high" => query
                    .OrderByDescending(m => m.Rating),

                "rating_low" => query
                    .OrderBy(m => m.Rating == null)
                    .ThenBy(m => m.Rating),

                "title" => query
                    .OrderBy(m => m.Title),

                _ => query
                    .OrderByDescending(m => m.MatchDate)
            };

            ViewBag.Promotions = await _context.Matches
                .Where(m => m.Promotion != "")
                .Select(m => m.Promotion)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();

            return View(await query.ToListAsync());
        }

        // GET: /Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var match = await _context.Matches
                .Include(m => m.WrestlingEvent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
                return NotFound();

            return View(match);
        }

        // GET: /Matches/Create
        public async Task<IActionResult> Create(int? eventId)
        {
            await LoadEvents(eventId);

            var match = new Match
            {
                MatchDate = DateTime.Today
            };

            if (eventId.HasValue)
            {
                var wrestlingEvent = await _context.Events
                    .FindAsync(eventId.Value);

                if (wrestlingEvent != null)
                {
                    match.EventId = wrestlingEvent.Id;
                    match.Event = wrestlingEvent.Name;
                    match.Promotion = wrestlingEvent.Promotion;
                    match.MatchDate = wrestlingEvent.EventDate;
                }
            }

            return View(match);
        }

        // POST: /Matches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Match match)
        {
            ModelState.Remove(nameof(Match.WrestlingEvent));

            await ApplySelectedEvent(match);

            if (!ModelState.IsValid)
            {
                await LoadEvents(match.EventId);
                return View(match);
            }

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            if (match.EventId.HasValue)
            {
                return RedirectToAction(
                    "Details",
                    "Events",
                    new { id = match.EventId.Value });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var match = await _context.Matches.FindAsync(id);

            if (match == null)
                return NotFound();

            await LoadEvents(match.EventId);

            return View(match);
        }

        // POST: /Matches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Match match)
        {
            if (id != match.Id)
                return NotFound();

            var existingMatch = await _context.Matches
                .FirstOrDefaultAsync(m => m.Id == id);

            if (existingMatch == null)
                return NotFound();

            ModelState.Remove(nameof(Match.WrestlingEvent));

            await ApplySelectedEvent(match);

            if (!ModelState.IsValid)
            {
                await LoadEvents(match.EventId);
                return View(match);
            }

            existingMatch.Title = match.Title;
            existingMatch.Wrestler1 = match.Wrestler1;
            existingMatch.Wrestler2 = match.Wrestler2;
            existingMatch.Promotion = match.Promotion;
            existingMatch.Event = match.Event;
            existingMatch.MatchDate = match.MatchDate;
            existingMatch.Description = match.Description;
            existingMatch.ImageUrl = match.ImageUrl;
            existingMatch.EventId = match.EventId;
            existingMatch.MatchType = match.MatchType;
            existingMatch.Winner = match.Winner;
            existingMatch.DurationMinutes = match.DurationMinutes;
            existingMatch.Rating = match.Rating;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(Details),
                new { id = match.Id });
        }

        // GET: /Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var match = await _context.Matches
                .Include(m => m.WrestlingEvent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
                return NotFound();

            return View(match);
        }

        // POST: /Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FindAsync(id);

            if (match == null)
                return NotFound();

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Apply event information
        private async Task ApplySelectedEvent(Match match)
        {
            if (!match.EventId.HasValue)
                return;

            var wrestlingEvent = await _context.Events
                .FindAsync(match.EventId.Value);

            if (wrestlingEvent == null)
            {
                ModelState.AddModelError(
                    nameof(Match.EventId),
                    "Selected event does not exist.");

                return;
            }

            match.Event = wrestlingEvent.Name;
            match.Promotion = wrestlingEvent.Promotion;
            match.MatchDate = wrestlingEvent.EventDate;

            ModelState.Remove(nameof(Match.Event));
            ModelState.Remove(nameof(Match.Promotion));
            ModelState.Remove(nameof(Match.MatchDate));
        }

        // Load event dropdown
        private async Task LoadEvents(int? selectedId = null)
        {
            var events = await _context.Events
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

            ViewBag.Events = new SelectList(
                events,
                "Id",
                "Name",
                selectedId);
        }
    }
}
