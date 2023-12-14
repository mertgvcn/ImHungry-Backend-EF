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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Swagger support for authorization
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ICryptionService, CryptionService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRestaurantService, RestaurantService>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<ICreditCardService, CreditCardService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IDbOperationHelperService, DbOperationHelperService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //need for automapper

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["AppSettings:ValidIssuer"],
        ValidAudience = builder.Configuration["AppSettings:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

//enable cors devamý
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //*eklendi

app.UseAuthorization();

app.MapControllers();

app.Run();
