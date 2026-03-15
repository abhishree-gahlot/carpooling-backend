using CarpoolingSystem.Application.Interfaces;
using CarpoolingSystem.Application.Mappings;
using CarpoolingSystem.Application.Services;
using CarpoolingSystem.Domain.Repositories;
using CarpoolingSystem.Infrastructure.Data;
using CarpoolingSystem.Infrastructure.Repositories;
using CarpoolingSystem.Infrastructure.Services;
using CarpoolingSystem.Infrastructure.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRideSessionRepository, RideSessionRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IRideRequestRepository, RideRequestRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IRideSessionService, RideSessionService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
<<<<<<< HEAD
builder.Services.AddScoped<IRideRequestRepository, RideRequestRepository>();
builder.Services.AddScoped<IRideRequestService, RideRequestService>();
builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddSingleton<IDriverLocationStore, DriverLocationStoreService>();
builder.Services.AddAutoMapper(cfg => { }, typeof(VehicleProfile).Assembly);

=======
builder.Services.AddSignalR();
builder.Services.AddScoped<IHubService, HubService>();
builder.Services.AddSingleton<IDriverLocationStore, DriverLocationStoreService>();

builder.Services.AddAutoMapper(cfg => { }, typeof(VehicleProfile).Assembly);
>>>>>>> 63d4088178ddd58c602ceb1a0d56a06c21304fc8
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddSingleton<IDriverLocationStore, DriverLocationStoreService>();
builder.Services.AddScoped<ILocationService, LocationService>();

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

    options.Events = new JwtBearerEvents 
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(token) &&
                path.StartsWithSegments("/hubs/ride"))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
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
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();
app.UseCors("AngularPolicy");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<RideHub>("/hubs/ride");

app.Run();