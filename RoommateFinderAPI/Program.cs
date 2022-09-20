using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoommateFinderAPI.Core;
using RoommateFinderAPI.Entities.Models;
using RoommateFinderAPI.Entities.Resources;
using RoommateFinderAPI.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(Policies.Admin, Policies.Policy(Policies.Admin));
    config.AddPolicy(Policies.Moderator, Policies.Policy(Policies.Moderator));
    //config.AddPolicy(Policies.Member, Policies.Policy(Policies.Member));
});
//services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddDbContext<RoommateFinderDbContext>(options =>
            options.UseSqlite("Filename=../RommateFinder.db"));

builder.Services.AddIdentity<User, IdentityRole>()
   .AddEntityFrameworkStores<RoommateFinderDbContext>();
// Add services to the container.


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<RoommateFinderDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    context.Database.Migrate();
    Seed.SeedUsers(userManager, roleManager);
}

app.Run();