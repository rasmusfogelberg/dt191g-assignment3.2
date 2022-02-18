#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscoSaurus.Models;
using DiscoSaurus.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DiscoSaurus.Controllers
{
  [Authorize]
  public class AlbumController : Controller
  {
    private readonly DiscoSaurusContext _context;

    public AlbumController(DiscoSaurusContext context)
    {
      _context = context;
    }

    // GET: Album
    public async Task<IActionResult> Index()
    {
      var albumContext = _context.Albums.Include(a => a.Artist).Include(a => a.Genre);
      return View(await albumContext.ToListAsync());
    }

    // GET: Album/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var album = await _context.Albums
          .Include(a => a.Artist)
          .Include(a => a.Genre)
          .FirstOrDefaultAsync(m => m.AlbumId == id);
      if (album == null)
      {
        return NotFound();
      }

      return View(album);
    }

    // GET: Album/Create
    public IActionResult Create()
    {
      ViewData["ArtistName"] = new SelectList(_context.Set<Artist>(), "ArtistId", "Name");
      ViewData["GenreName"] = new SelectList(_context.Set<Genre>(), "GenreId", "Name");
      return View();
    }

    // POST: Album/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AlbumId,GenreId,ArtistId,Title,Price")] Album album)
    {
      if (ModelState.IsValid)
      {
        _context.Add(album);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      ViewData["ArtistId"] = new SelectList(_context.Set<Artist>(), "ArtistId", "ArtistId", album.ArtistId);
      ViewData["GenreId"] = new SelectList(_context.Set<Genre>(), "GenreId", "GenreId", album.GenreId);
      return View(album);
    }

    // GET: Album/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var album = await _context.Albums.FindAsync(id);
      if (album == null)
      {
        return NotFound();
      }
      ViewData["ArtistName"] = new SelectList(_context.Set<Artist>(), "ArtistId", "Name", album.ArtistId);
      ViewData["GenreName"] = new SelectList(_context.Set<Genre>(), "GenreId", "Name", album.GenreId);
      return View(album);
    }

    // POST: Album/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("AlbumId,GenreId,ArtistId,Title,Price")] Album album)
    {
      if (id != album.AlbumId)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(album);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!AlbumExists(album.AlbumId))
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
      ViewData["ArtistId"] = new SelectList(_context.Set<Artist>(), "ArtistId", "ArtistId", album.ArtistId);
      ViewData["GenreId"] = new SelectList(_context.Set<Genre>(), "GenreId", "GenreId", album.GenreId);
      return View(album);
    }

    // GET: Album/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var album = await _context.Albums
          .Include(a => a.Artist)
          .Include(a => a.Genre)
          .FirstOrDefaultAsync(m => m.AlbumId == id);
      if (album == null)
      {
        return NotFound();
      }

      return View(album);
    }

    // POST: Album/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var album = await _context.Albums.FindAsync(id);
      _context.Albums.Remove(album);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    // GET: Album/Borrow/5
    public async Task<IActionResult> Borrow(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var album = await _context.Albums
          .Include(a => a.Artist)
          .Include(a => a.Genre)
          .FirstOrDefaultAsync(m => m.AlbumId == id);
      if (album == null)
      {
        return NotFound();
      }

      return View(album);
    }

    // POST: Album/Borrow/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(int id, [Bind("AlbumId,UserId")] Album albumToBorrow)
    {
      var album = await _context.Albums.FindAsync(id);
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var user = await _context.Users.FindAsync(userId);

      // Add to "Borrow"-table: UserId and AlbumId
      string name = this.User.Identity.Name;

      var borrow = new Borrowed()
      {
        BorrowedAt = new DateTime(),
        ReturnedAt = null,
        BorrowedItem = new BorrowedItem() {
          Album = album,
          User = user
        }
      };

      _context.Borrowed.Add(borrow);
      await _context.SaveChangesAsync();
      
      return RedirectToAction(nameof(Index));
    }

    private bool AlbumExists(int id)
    {
      return _context.Albums.Any(e => e.AlbumId == id);
    }
  }
}
