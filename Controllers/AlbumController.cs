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
    public async Task<IActionResult> Index(string searchString)
    {
      // Get userId from cookie using ClaimTypes (this is set in LoginController after successful login)
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var user = await _context.Users.FindAsync(int.Parse(userId));

      ViewBag.CurrentUserId = user.UserId;
      ViewBag.Borroweds = await _context.Borrowed.ToListAsync();
      ViewBag.BorrowedItems = await _context.BorrowedItem.ToListAsync();

      // Get album from context and populate related data
      var album = from a in _context.Albums
          .Include(album => album.Artist)
          .Include(album => album.Genre)
                  select a;

      if (album == null)
      {
        return NotFound();
      }

      // Search function. If string is empty it will not filter 
      if (!String.IsNullOrEmpty(searchString))
      {
        // Filter albums by title using searchString. 
        album = album.Where(search => search.Title!.Contains(searchString));
      }

      // Return filtered albums as list
      return View(await album.ToListAsync());
    }

    // GET: Album/Details/5
    public async Task<IActionResult> Details(int id)
    {
      if (!AlbumExists(id))
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
      // Populates dropdown list with Artist and Genre
      ViewData["ArtistName"] = new SelectList(_context.Set<Artist>(), "ArtistId", "Name");
      ViewData["GenreName"] = new SelectList(_context.Set<Genre>(), "GenreId", "Name");
      return View();
    }

    // POST: Album/Create (This stores it in the db)
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
    public async Task<IActionResult> Edit(int id)
    {
      if (!AlbumExists(id))
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("AlbumId,GenreId,ArtistId,Title,Price,IsAvailable,LentToUser")] Album album)
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
          return NotFound();
        }
        return RedirectToAction(nameof(Index));
      }

      ViewData["ArtistId"] = new SelectList(_context.Set<Artist>(), "ArtistId", "ArtistId", album.ArtistId);
      ViewData["GenreId"] = new SelectList(_context.Set<Genre>(), "GenreId", "GenreId", album.GenreId);

      return View(album);
    }

    // GET: Album/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
      if (!AlbumExists(id))
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
      try
      {
        var album = await _context.Albums.FindAsync(id);
        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();
      }
      catch
      {
        return NotFound();
      }

      return RedirectToAction(nameof(Index));
    }

    // GET: Album/Borrow/5
    public async Task<IActionResult> Borrow(int albumId)
    {
      if (!AlbumExists(albumId))
      {
        return NotFound();
      }

      var album = await _context.Albums
          .Include(a => a.Artist)
          .Include(a => a.Genre)
          .FirstOrDefaultAsync(m => m.AlbumId == albumId);

      if (album == null)
      {
        return NotFound();
      }

      return View(album);
    }

    // POST: Album/Borrow/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow([Bind("AlbumId")] Album albumToBorrow)
    {
      try
      {
        // Retrive album from database
        var album = await _context.Albums.FindAsync(albumToBorrow.AlbumId);

        album.IsAvailable = false;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.Users.FindAsync(int.Parse(userId));
        album.LentToUser = user.UserId;

        // Define borrow object for table Borrow in database
        var borrow = new Borrowed()
        {
          BorrowedAt = DateTime.Now,
          ReturnedAt = null,
          BorrowedItem = new BorrowedItem()
          {
            Album = album,
            User = user
          }
        };

        _context.Borrowed.Add(borrow);
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!AlbumExists(albumToBorrow.AlbumId))
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

    // GET: Album/Return/5
    public async Task<IActionResult> Return(int albumId, int borrowedItemId)
    {
      if (!AlbumExists(albumId))
      {
        return NotFound();
      }

      var album = await _context.Albums
          .Include(a => a.Artist)
          .Include(a => a.Genre)
          .FirstOrDefaultAsync(m => m.AlbumId == albumId);

      if (album == null)
      {
        return NotFound();
      }

      var borrowed = await _context.Borrowed
        .Include(b => b.BorrowedItem)
        .SingleOrDefaultAsync<Borrowed>(a => a.BorrowedItem.Album.AlbumId == albumId && a.ReturnedAt == null && a.BorrowedItem.BorrowedItemId == borrowedItemId);

      ViewBag.BorrowedId = borrowed.BorrowedId;

      return View(album);
    }

    // POST: Album/Return/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReturnConfirmed(int albumId, [Bind("BorrowedId")] int borrowedId)
    {
      if (!AlbumExists(albumId))
      {
        return NotFound();
      }

      try
      {
        // Similar to what happens in borrow action but "reversed"
        var album = await _context.Albums.FindAsync(albumId);
        var borrow = await _context.Borrowed
                      .Include(b => b.BorrowedItem)
                      .FirstOrDefaultAsync(b => b.BorrowedId == borrowedId);

        album.IsAvailable = true;
        album.LentToUser = null;

        if (borrow != null)
        {
          borrow.ReturnedAt = DateTime.Now;
          borrow.BorrowedId = borrowedId;

          _context.Borrowed.Update(borrow);
          await _context.SaveChangesAsync();
        }

      }
      catch (DbUpdateConcurrencyException)
      {
        return NotFound();
      }
      return RedirectToAction(nameof(Index));
    }

    // GET: LenderList
    public async Task<IActionResult> LenderList()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var user = await _context.Users.FindAsync(int.Parse(userId));
      ViewBag.CurrentUserId = user.UserId;

      var borrowed = _context.Borrowed
          .Include(borrowed => borrowed.BorrowedItem)
          .Include(borrowed => borrowed.BorrowedItem.Album)
          .Include(borrowed => borrowed.BorrowedItem.User);

      if (borrowed == null)
      {
        return NotFound();
      }

      return View(await borrowed.ToListAsync());
    }

    // GET: LenderHistory
    public async Task<IActionResult> LenderHistory()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var user = await _context.Users.FindAsync(int.Parse(userId));
      ViewBag.CurrentUserId = user.UserId;

      var borrowed = _context.Borrowed
          .Include(borrowed => borrowed.BorrowedItem)
          .Include(borrowed => borrowed.BorrowedItem.Album)
          .Include(borrowed => borrowed.BorrowedItem.User);

      if (borrowed == null)
      {
        return NotFound();
      }

      return View(await borrowed.ToListAsync());
    }

    private bool AlbumExists(int id)
    {
      return _context.Albums.Any(e => e.AlbumId == id);
    }
  }
}
