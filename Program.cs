using DiscoSaurus.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add cookie authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
        options =>
        {
            // Set cookie expiration time
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.Cookie.Name = ".DiscoSaurus";
            options.LoginPath = "/Login";
        });

// Connection string for database
builder.Services.AddDbContext<DiscoSaurusContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDbString")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Album/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Album}/{action=Index}/{id?}");

app.Run();
