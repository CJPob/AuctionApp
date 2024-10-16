using AuctionApp.Areas.Identity.Data;
using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using AuctionApp.Data;
using AuctionApp.Persistence;
using AuctionApp.Persistence.Interfaces;
using AuctionApp.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

// auction
builder.Services.AddDbContext<AuctionDbContext>(options =>    
    options.UseMySQL(builder.Configuration.GetConnectionString("AuctionDbConnection")));


// identity
builder.Services.AddDbContext<AppIdentityDbContext>(options =>    
    options.UseMySQL(builder.Configuration.GetConnectionString("IdentityDbConnection")));
builder.Services.AddDefaultIdentity<AppIdentityUser>(options => 
    options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppIdentityDbContext>();

// dependency injection of service into controller for true
builder.Services.AddScoped<IAuctionService, AuctionService>();

//dependency inj for persistence into service  
builder.Services.AddScoped<IAuctionPersistence, MySqlAuctionPersistence>();



/////////////////////////////////////////////////////////////////////////////////////////////
// dependency inj for gen.iface and gen.repo 
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>(); 




// mapper
builder.Services.AddAutoMapper(typeof(Program));

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
app.MapRazorPages(); 
app.UseRouting();
app.UseAuthorization();

// Set AuctionsController's Index page as the default route /home /start page
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auctions}/{action=Index}/{id?}"); 

app.Run();