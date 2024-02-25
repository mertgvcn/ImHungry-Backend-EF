using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ImHungryBackendER;
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
