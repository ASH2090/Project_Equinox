using Project_Equinox.Models.Infrastructure;
using Project_Equinox.Models.Data.Repository;
using Microsoft.EntityFrameworkCore;                    

var builder = WebApplication.CreateBuilder(args);

// Add session services
builder.Services.AddMemoryCache();
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

// EF Core DI.
builder.Services.AddDbContext<EquinoxContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EquinoxContext")));

// Repository DI
builder.Services.AddScoped<IEquinoxRepository, EquinoxRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();


// Route for GymClass with pagination and sorting (regular routing)
app.MapControllerRoute(
    name: "GymClassPaginate",
    pattern: "GymClass/Page/{pageNumber}/Size/{pageSize}/Sort/{sortField}/{sortDirection}",
    defaults: new { controller = "GymClass", action = "Index" });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Enable attribute routing for Admin area
app.MapControllers();

app.Run();
