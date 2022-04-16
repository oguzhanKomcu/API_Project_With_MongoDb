using HotelReservationAPi.Infrastructure.Repositories;
using HotelReservationAPi.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDBConnection"));
builder.Services.AddSingleton<MongoDbSettings>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
