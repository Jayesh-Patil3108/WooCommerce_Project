using Microsoft.EntityFrameworkCore;
using SaaS.Application.IServices.Auth;
using SaaS.Application.Services.Auth;
using SaaS.Infrastructure.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient("SaaSApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/"); // Adjust to SaaS.Api URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddDbContext<SaaSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("SaaS.Infrastructure")));
builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Home/Error");
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapGet("/", () => Results.Redirect("/Login"));

app.Run();
