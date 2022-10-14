global using contactgroupAPIefMySQL.Models;
global using Microsoft.EntityFrameworkCore;
global using contactgroupAPIefMySQL;
global using Microsoft.AspNetCore.Authorization;  //Ei tartte lisää joka controlleriin
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;  //Add package for this one in visual studio

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authentization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();         //Matt Frear creator 
});
//Jwt Authentication
MyEnvironment.SetSecredKey();// This might be problem in production or in heroku/azure

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(System.Environment.GetEnvironmentVariable("SecredKey"))),    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
        }; 
    });
Console.WriteLine("Program.cs env variable securitykey="+System.Environment.GetEnvironmentVariable("SecredKey"));
//Context needs to added as service manually
builder.Services.AddDbContext<contactgroupContext>();

//builder.Services.AddScoped<JwtService>();
builder.Services.AddCors();     //Port are different in Front End and Back End 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    MyEnvironment.SetMySQLConnection();
    MyEnvironment.SetSecredKey();
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Console.WriteLine("Program.cs env variable securitykey=" + System.Environment.GetEnvironmentVariable("SecredKey"));

app.UseHttpsRedirection();

//JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
                                    //Frontti pyorimaan porttiin 3000 5555 Postman  btw Cookie requires ssl
app.UseCors(options => options
.WithOrigins(new[] {"http://localhost:3000", "http://localhost:5555", "http://localhost:8080", "https://localhost:4200","https://localhost:3000", "https://localhost:5555", "https://localhost:8080", "https://localhost:4200" })
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
);

app.Run();


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

