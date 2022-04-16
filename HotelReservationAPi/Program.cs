using HotelReservationAPi.Infrastructure.Repositories;
using HotelReservationAPi.Model;
using Microsoft.OpenApi.Models;
using Swashbuckle.Swagger;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDBConnection"));
builder.Services.AddSingleton<MongoDbSettings>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("RestAPI", new OpenApiInfo()
    {
        Title = "RestFul API",
        Version = "v1",
        Description = "Hotel Reservation RestFul API",
        Contact = new OpenApiContact()
        {
            Email = "komcuoguzz@gmail.com",
            Name = "Oðuzhan Kömcü",
            Url = new Uri("https://github.com/oguzhanKomcu")
        },
        License = new OpenApiLicense()
        {
            Name = "MIT License",
            Url = new Uri("http://opensource.org/licenses/MIT")
        }
    });

   
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoDB CRUD API V1");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
