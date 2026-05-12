using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NYAT_1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // EKLENEN KISIM BURASI
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
// --- ROL VE KULLANICI OLUÞTURMA ÝÞLEMÝNÝ TETÝKLEYEN KISIM ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Metot adýný güncelledik
        await NYAT_1.Data.Rol.SeedRolesAndUsersAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Roller oluþturulurken bir hata oluþtu: " + ex.Message);
    }
}
// ---------------------------------------------- // Bu satýr zaten en altta vardý, ona dokunma.
app.Run();
