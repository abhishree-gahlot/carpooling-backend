using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Application.Mappings;
using CarpoolingSystem.Application.Services;
using CarpoolingSystem.Domain.Repositories;
using CarpoolingSystem.Infrastructure.Data;
using CarpoolingSystem.Infrastructure.Repositories;
using CarpoolingSystem.Infrastructure.Services;
using CarpoolingSystem.Infrastructure.Configuration;
using CarpoolingSystem.Infrastructure.Data;
using CarpoolingSystem.Infrastructure.Repositories;
using CarpoolingSystem.Infrastructure.Services;
using CarpoolingSystem.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IRideSessionRepository, RideSessionRepository>();
builder.Services.AddScoped<IRideSessionService, RideSessionService>();
builder.Services.AddScoped<CarpoolingSystem.Domain.Repositories.IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
//builder.Services.AddScoped<CarpoolingSystem.Application.Interfaces.IVehicleRepository, VehicleRepository>();

builder.Services.AddAutoMapper(
    typeof(CarpoolingSystem.Application.Mappings.VehicleProfile).Assembly);
builder.Services.Configure<ReverseGeoCodingOptions>
    (
        builder.Configuration.GetSection("ExternalServices:ReverseGeocoding")
    );
builder.Services.AddHttpClient<IReverseGeocodingService, ReverseGeoCodingService>();
builder.Services.AddScoped<LocationService>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AngularPolicy");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();