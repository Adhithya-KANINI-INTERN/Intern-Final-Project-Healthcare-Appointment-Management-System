using HAMSMicroservices.Models;
using HAMSMicroservices.Services;
using HAMSMicroservices.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connStr = builder.Configuration.GetConnectionString("ConnectStr");
builder.Services.AddDbContext<AppDBContext>(options => options.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTToken"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            NameClaimType = JwtRegisteredClaimNames.NameId,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Replace with your frontend URL
              .AllowAnyHeader() // Ensures Authorization header is allowed
              .AllowAnyMethod()
              .AllowCredentials(); // If you are using cookies for authentication
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();


app.UseCors("AllowSpecificOrigins");

app.UseAuthentication(); // Ensure this is before Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();



//using HAMSMicroservices.Models;
//using HAMSMicroservices.Services;
//using HAMSMicroservices.Services.Interfaces;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//string connStr = builder.Configuration.GetConnectionString("ConnectStr");
//builder.Services.AddDbContext<AppDBContext>(options => options.UseMySql(connStr, ServerVersion.AutoDetect(connStr)));
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<IDoctorService, DoctorService>();
//builder.Services.AddScoped<IAppointmentService, AppointmentService>();
//builder.Services.AddScoped<INotificationService, NotificationService>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
