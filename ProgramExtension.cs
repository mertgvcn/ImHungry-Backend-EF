using ImHungryBackendER.Services.OtherServices.Interfaces;
using ImHungryBackendER.Services.OtherServices;
using WebAPI_Giris.Services.OtherServices.Interfaces;
using WebAPI_Giris.Services.OtherServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using ImHungryBackendER.Services.ControllerServices.CustomerServices;
using ImHungryBackendER.Services.ControllerServices.CustomerServices.Interfaces;
using ImHungryBackendER.Services.ControllerServices.NeutralServices.Interfaces;
using ImHungryBackendER.Services.ControllerServices.NeutralServices;
using ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices.Interfaces;
using ImHungryBackendER.Services.ControllerServices.RestaurantManagementServices;

namespace ImHungryBackendER
{
    public static class ProgramExtension
    {
        public static void AddAPI(this WebApplicationBuilder builder)
        {
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
        }

        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            //NeutralServices
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<IItemService, ItemService>();
            builder.Services.AddTransient<IRestaurantService, RestaurantService>();
            //RestaurantManagementServices
            builder.Services.AddTransient<IRestaurantManagerService, RestaurantManagerService>();
            builder.Services.AddTransient<IMenuServices, MenuService>();
            //UserServices
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<ICartService, CartService>();
            builder.Services.AddTransient<ILocationService, LocationService>();
            builder.Services.AddTransient<ICreditCardService, CreditCardService>();
            //Others
            builder.Services.AddTransient<ICryptionService, CryptionService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IDbOperationHelperService, DbOperationHelperService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //need for automapper
        }

        public static void ConfigureAuthorization(this WebApplicationBuilder builder)
        {
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
        }
    }
}
