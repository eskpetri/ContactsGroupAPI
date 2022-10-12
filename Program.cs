global using contactgroupAPIefMySQL.Models;
global using Microsoft.EntityFrameworkCore;
global using contactgroupAPIefMySQL;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Context needs to added as service manually
builder.Services.AddDbContext<contactgroupContext>();

//builder.Services.AddScoped<JwtService>();
builder.Services.AddCors();     //Port are different in Front End and Back End 

//JWT
/*
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});*/




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    MyEnvironment.SetMySQLConnection();
    MyEnvironment.SetSecredKey();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
                                    //Frontti py�rim��n porttiin 3000
app.UseCors(options => options
.WithOrigins(new[] {"http://localhost:3000", "http://localhost:8080", "http://localhost:4200" })
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
);

app.Run();
