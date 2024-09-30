using ContactManager_WebApp.DataAccess;
using Microsoft.EntityFrameworkCore;
using ContactManager_WebApp.BusinessLogic.Installers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ContactManagerContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCsvReaderService();

builder.Services.AddContactsRepository();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contacts}/{action=Index}/{id?}");

app.Run();
