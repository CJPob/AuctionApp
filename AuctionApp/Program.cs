using AuctionApp.Areas.Identity.Data;
using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using AuctionApp.Data;
using AuctionApp.Persistence;
using AuctionApp.Persistence.Interfaces;
using AuctionApp.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Auction database context
builder.Services.AddDbContext<AuctionDbContext>(options =>    
    options.UseMySQL(builder.Configuration.GetConnectionString("AuctionDbConnection")));

// Identity database context
builder.Services.AddDbContext<AppIdentityDbContext>(options =>    
    options.UseMySQL(builder.Configuration.GetConnectionString("IdentityDbConnection")));
builder.Services.AddDefaultIdentity<AppIdentityUser>(options => 
    options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Include role support 
    .AddEntityFrameworkStores<AppIdentityDbContext>();

// Dependency injection for services and repositories
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>(); 
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserPersistence, UserPersistence>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Initialize roles
CreateRolesAsync(app.Services).Wait();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorPages(); 
app.UseRouting();   
app.UseAuthorization();

// Set default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auctions}/{action=Index}/{id?}");

app.Run();

//  create roles, fill the table, set admin rodle to user admin@admin.se
async Task CreateRolesAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var roleManager = 
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    
    var userManager = 
        scope.ServiceProvider.GetRequiredService<UserManager<AppIdentityUser>>();

    string email = "admin2@admin.se"; 
    string password = "Test-1234!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new AppIdentityUser { UserName = email, Email = email };
        user.EmailConfirmed = true;
        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
        
    }   
}
