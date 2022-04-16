# How I Made an API Project Using MongoDB ? 

## MongoDB Installation and Usage

- Since I want to keep my database locally, I downloaded the [Community Server](https://github.com/oguzhanKomcu/DATA_ACCESS/tree/master/DB_First). file from MongoDb's own site. Database operations can be done by subscribing to the cloud server, if desired.

- MongoDb can also be used directly with the CMD line if desired. However, if you want an interface like Sql, you need to download [ MongoDB Compass](https://github.com/oguzhanKomcu/DATA_ACCESS/tree/master/DB_First).

- After the installations are finished, I run MongoDB Compass directly.

- We first meet the "New Connection" page. Here we have the "ConnectionURI" address. Then I connect to MongoDB by saying "Conncect".

-I click on the "Databases" button on the Layout page. On the next page, I click on the "Create database" field and write my database name and the collection name. These fields and the names I will use in my Api project must be the same.

# Creating an API Project with Visual Studio 

- I am opening Visual Studio 2022. Here I am opening a "Blank Solution".

- Going to create a project in the solution, I create and open my project in the "Asp.Net Core Web API" type with a name I want.

- To use MongoDB features, I download "MongoDB.Driver" library from Nuget Package. So I can use MongoDb classes and methods.

- I am creating my class named "Reservation.cs" for my entity that I will create inside the model folder. I create my fields that I will use in my collection. Here I am applying the attribute indicating that my Id type is "BsonType.ObjectId".

 ```csharp
   public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        

        public string? Reservation_Date { get; set; }

        public string? Entry_Date { get; set; }
        public string? Exit_Date { get; set; }
        public string? Room_Number { get; set; }
        public string? Room_Type { get; set; }
        public string? Room_Price { get; set; }
        public string? Customer_Name { get; set; }
        public string? Customer_Email { get; set; }
        public string? Customer_Phone { get; set; }
        public string? Customer_City { get; set; }


    }
```

- I create the class "MongoDBSettings.cs" inside the model folder. This class will be used to store the property values of the file "appsettings.json". The important thing here is that the JSON and C# property names must be the same.

```csharp
  public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string CollectionName { get; set; } = string.Empty;

    }
```
- I go and configure the "MongoDBSettings.cs" class in Program.cs. In the "GetSection" method I gave, the desired "string key" parameter points to the name in my configuration document in "appsettings.json".

```csharp
 builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDBConnection"));

```
- Now in "appsettings.json" I am typing the corresponding names of "ConnectionString" , "DatabaseName" , "CollectionName" fields in MongoDb correctly.

```csharp
"AllowedHosts": "*",
  "MongoDBConnection": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "HotelReservationDB",
    "CollectionName": "Resevations"
  }

```
- One of the most important needs in Web API development is the need for documentation. Because what the API methods do and how they are used should be clear in the documentation. For this, I use the interface of "SWAGGER", which is now embedded with the CORE 6 version. An important purpose of Swagger is to provide an interface for RestApi. This allows both people and computers to see, examine and understand the features of RestApi without accessing the source code. I came to my Program.cs class and did the necessary action for SwaggerUI.


```csharp

app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoDB CRUD API V1");
    });
 

```

- Now I am doing my methods and injection operations that are necessary to do my CRUD operations in the database.

```csharp

app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MongoDB CRUD API V1");
    });
 

```
-I open my "Infrastructure" folder in my project. I open my "Repositories" folder in it. First of all, I create the interface of the repository named "IReservationRepository".

```csharp

 public interface IReservationRepository
    {
        Task<List<Reservation>> GetCustomers();


        Task<Reservation> GetCustomer(string id);


        Task<Reservation> Create(Reservation customer);


        Task Update(string id, Reservation customer);


        Task Delete(string id);
    }
 

```


-In "ReservationRepository.cs", I apply the methods necessary to do my CRUD operations in the database by inheriting from the interface. For the operations required for MongoDb in it, I use the "IMongoCollection" interface and give my carlik class, which I will operate as a type, and call my "MongoDBSettings" class that I created.

```csharp

 public class ReservationRepository : IReservationRepository
    {
        private readonly IMongoCollection<Reservation> _reservation;
        private readonly MongoDbSettings _settings;


        public ReservationRepository(IOptions<MongoDbSettings> mongoDBSettings)
        {
            _settings = mongoDBSettings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _reservation = database.GetCollection<Reservation>(_settings.CollectionName);
        }


        public async Task<List<Reservation>> GetCustomers()
        {

            return await _reservation.Find(customer => true).ToListAsync();
        }

        public async Task<Reservation> GetCustomer(string id)
        {
            return await _reservation.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Reservation> Create(Reservation customer)
        {
            await _reservation.InsertOneAsync(customer);
            return customer;

        }

        public async Task Update(string id, Reservation customer)
        {
            await _reservation.ReplaceOneAsync(x => x.Id == id, customer);

        }

        public async Task Delete(string id)
        {

            await _reservation.DeleteOneAsync(x => x.Id == id);

        }
 

```
