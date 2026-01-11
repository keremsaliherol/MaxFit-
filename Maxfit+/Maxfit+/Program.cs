using Microsoft.EntityFrameworkCore;
using Maxfit_.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 1) Veritabaný Servisini Ekle
builder.Services.AddDbContext<MaxFitContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Identity Servislerini Ekle
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    // Þifre kurallarý (Geliþtirme için esnek tutuldu)
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    // Oturum Ayarlarý
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<MaxFitContext>()
.AddDefaultTokenProviders();

// Cookie/Oturum Ayarlarý
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.SlidingExpiration = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// HTTP Pipeline Yapýlandýrmasý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3) Kimlik Doðrulama ve Yetkilendirme (Sýralama Deðiþmemeli!)
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// 4) SEED DATA (Rolleri ve Kullanýcýlarý Oluþturur)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        // Eksik Rolleri Tamamla
        string[] roleNames = { "Admin", "Staff", "Member" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // ÖRNEK ADMÝN (admin@maxfit.com / Admin123!)
        var adminEmail = "admin@maxfit.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var user = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, "Admin123!");
            if (result.Succeeded) await userManager.AddToRoleAsync(user, "Admin");
        }

        // ÖRNEK STAFF (staff@maxfit.com / Staff123!)
        var staffEmail = "staff@maxfit.com";
        var staffUser = await userManager.FindByEmailAsync(staffEmail);
        if (staffUser == null)
        {
            var user = new IdentityUser { UserName = staffEmail, Email = staffEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, "Staff123!");
            if (result.Succeeded) await userManager.AddToRoleAsync(user, "Staff");
        }

        // ÖRNEK MEMBER (member@maxfit.com / Member123!)
        var memberEmail = "member@maxfit.com";
        var memberUser = await userManager.FindByEmailAsync(memberEmail);
        if (memberUser == null)
        {
            var user = new IdentityUser { UserName = memberEmail, Email = memberEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, "Member123!");
            if (result.Succeeded) await userManager.AddToRoleAsync(user, "Member");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();