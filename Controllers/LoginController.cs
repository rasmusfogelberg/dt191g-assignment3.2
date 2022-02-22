using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using DiscoSaurus.Models;
using DiscoSaurus.Data;
using Microsoft.EntityFrameworkCore;

namespace DiscoSaurus.Controllers;

public class LoginController : Controller
{
  private readonly DiscoSaurusContext _context;

  public LoginController(DiscoSaurusContext context)
  {
    _context = context;
  }

  public IActionResult Index()
  {
    return View();
  }

  // Checks so login credentials are valid
  private bool IsValidLogin(string userName, string password)
  {
    return userName.Equals("admin") && password.Equals("password");
  }

  // If the user enters valid credentials s/he is logged in
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Login([Bind] User user)
  {

    if (ModelState.IsValid)
    { 
      var validUser = await _context.Users.Where(userInDb => userInDb.Username == user.Username && userInDb.Password == user.Password).FirstOrDefaultAsync();
      
      if (validUser == null)
      {
        ModelState.AddModelError(string.Empty, "Invalid username or password");
        return View("index");
      }
      
      // Creates a list of claims containg claim-types for name and role
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, validUser.Username),
        new Claim(ClaimTypes.NameIdentifier, validUser.UserId.ToString()),
        new Claim(ClaimTypes.Role, "Admin")
      };

      // A claimed identity is a collection of claims. These claims are stored in a cookie
      var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      // A collection of identities that identifies the user. A ClaimsPrincipal can have multiple ClaimsIdentity
      var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
      // Sets configuration values about the authenticated cookie
      var authProperties = new AuthenticationProperties
      {
        // Allows the refresh of the cookie
        AllowRefresh = true,
        // Allows the cookie to be stored between requests
        IsPersistent = true,
      };

      // Sign and create cookie based on claims and authproperties
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

      return LocalRedirect("/");
    }

    return View("Index");
  }

  [HttpGet]
  public async Task<IActionResult> Logout()
  { // Invalidates and removes the cookie
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Redirect("/");
  }
}
