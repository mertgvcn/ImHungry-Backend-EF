using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Npgsql;
using System.Data.Common;
using WebAPI_Giris;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI_Giris.Services.OtherServices.Interfaces;
using WebAPI_Giris.Services.OtherServices;
using WebAPI_Giris.Services.ControllerServices.Interfaces;
using WebAPI_Giris.Services.ControllerServices;
using Microsoft.EntityFrameworkCore;
using ImHungryBackendER;
using AutoMapper;
using ImHungryBackendER.Services.OtherServices.Interfaces;
using ImHungryBackendER.Services.OtherServices;
using ImHungryBackendER.Services.ControllerServices.Interfaces;
using ImHungryBackendER.Services.ControllerServices;
using ImHungryLibrary;

var builder = WebApplication.CreateBuilder(args);

//Entity Framework Context Connection
builder.Services.AddDbContext<ImHungryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("default"), b => b.MigrationsAssembly("ImHungryLibrary")));

// Json Serializer
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

//Enable CORS(dýþarýdan gerçekleþen iþlemlere izin)
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});



// Add services to the container.
builder.AddAPI();
builder.ConfigureServices();
builder.ConfigureAuthorization();


var app = builder.Build();

//enable cors devamý
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AutoMigrateDatabase();

app.UseHttpsRedirection();

app.UseAuthentication(); //*eklendi

app.UseAuthorization();

app.MapControllers();

app.Run();
