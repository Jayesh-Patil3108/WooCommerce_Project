using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SaaS.Api.Middleware;
using SaaS.Application.IServices.Auth;
using SaaS.Application.IServices.WhatsAppService;
using SaaS.Application.IServices.WooCommerce;
using SaaS.Application.Services.Auth;
using SaaS.Application.Services.WhatsAppService;
using SaaS.Application.Services.WooCommerce;
using SaaS.Infrastructure.DbContexts;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SaaS.Api", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<SaaSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
     sqlOptions => sqlOptions.MigrationsAssembly("SaaS.Infrastructure")
));

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero,
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated for user: " + context.Principal?.Identity?.Name);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpClient<IWooCommerceService, WooCommerceService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("WooCommerce:TimeoutSeconds"));
});
builder.Services.AddHttpClient<IWhatsAppService, WhatsAppService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WhatsApp:ApiBaseUrl"]);
    client.Timeout = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("WhatsApp:TimeoutSeconds"));
});


// Add logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SaaS.Api v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//app.UseTokenRefreshMiddleware();


// Temporarily comment out TenantFilterMiddleware
app.UseMiddleware<TenantFilterMiddleware>();


app.MapControllers();

app.Run();
