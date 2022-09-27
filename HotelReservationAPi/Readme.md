# How I Made an API Project Using MongoDB ? 

## MongoDB Installation and Usage

- Since I want to keep my database locally, I downloaded the [Community Server](https://https://www.mongodb.com/try/download/community). file from MongoDb's own site. Database operations can be done by subscribing to the cloud server, if desired.

- MongoDb can also be used directly with the CMD line if desired. However, if you want an interface like Sql, you need to download [ MongoDB Compass](https://www.mongodb.com/products/compass).

- After the installations are finished, I run MongoDB Compass directly.

- We first meet the "New Connection" page. Here we have the "ConnectionURI" address. Then I connect to MongoDB by saying "Conncect".

- I click on the "Databases" button on the Layout page. On the next page, I click on the "Create database" field and write my database name and the collection name. These fields and the names I will use in my Api project must be the same.

# Creating an API Project with Visual Studio 

- I am opening Visual Studio 2022. Here I am opening a "Blank Solution".

- Going to create a project in the solution, I create and open my project in the "Asp.Net Core Web API" type with a name I want.

- To use MongoDB features, I download "MongoDB.Driver" library from Nuget Package. So I can use MongoDb classes and methods.

- In the Model folder, I create my class named "Reservation.cs" , "User.cs" and my main entity "BaseEntity.cs" that inherits them. I create the fields that I will use in my collection myself. Here I am applying the attribute indicating that my identity type is "BsonType.ObjectId"


 ```csharp
   public class BaseEntity
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        private DateTime _createDate = DateTime.Now;

        public DateTime CreateDate
        { get { return _createDate; } set { _createDate = value; } }

        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        private Status _status = Status.Active;
        public Status Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
```
- I'm creating an enum type "Status.cs" class to determine the status states of my entity.
-
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

 ```csharp
  public class User : BaseEntity
    {
        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("password")]

        public string Password { get; set; }

    }
```

- Inside the model folder I create the class "MongoDBSettings.cs". This class will be used to store property values of "appsettings.json" file. The important thing here is that the JSON and C# property names must be the same. I created my "key" property that I will use for the jwt token in this class.

```csharp
  public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string CollectionName { get; set; } = string.Empty;
        public string? SecretKey { get; set; }

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
         options.SwaggerEndpoint("/swagger/RestAPI/swagger.json", "RestAPI");
    });
 

```

- Now I am doing my methods and injection operations that are necessary to do my CRUD operations in the database.

```csharp

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDBConnection"));
builder.Services.AddSingleton<MongoDbSettings>(options => options.GetRequiredService<IOptions<MongoDbSettings>>().Value);
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddControllers();
 

```
- I open my "Infrastructure" folder in my project. I open my "Repositories" folder in it. First of all, I create the interface of the repository named "IReservationRepository".

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


- In "ReservationRepository.cs", I apply the methods necessary to do my CRUD operations in the database by inheriting from the interface. For the operations required for MongoDb in it, I use the "IMongoCollection" interface and give my carlik class, which I will operate as a type, and call my "MongoDBSettings" class that I created.

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


- Then I go back to Program.cs and resolve my repository in my IOC container, which I resolve in my container as it will be used in my "MongoDbSettings" class.

```csharp

builder.Services.AddSingleton<MongoDbSettings>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
 

```

- Now it's time to create my API. For this, I add an ApiController in the "Controller" folder. I name it "ReservationController" like this. In it, I apply DI to use my methods in my repository, in the constructor method of my controller, that I want an object that uses the "IReservationRepository" interface.

- Later, I apply the "Http" methods required for developers who will use this api. I also include summaries of my texts so that developers can more easily understand which processes these methods are used for.


```csharp

 [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationsController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;

        }

        /// <summary>
        /// This function lists all made reservations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _reservationRepository.GetCustomers().ConfigureAwait(false));
        }
```
```csharp

        /// <summary>
        /// This function returns the reservation whose "id" is given.
        /// </summary>
        /// <param name="id">It is a required area and so type is int</param>
        /// <returns>If function is succeded will be return Ok, than will be return NotFound</returns>
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id)
        {
            var customer = await _reservationRepository.GetCustomer(id);

            if (customer is null)
            {
                return NotFound();
            }


            return Ok(customer);


        }
```

```csharp
        /// <summary>
        /// You can add a new reservation using this method.
        /// </summary>
        /// <param></param>
        /// <returns>If function is succeded will be return CreatedAtAction, than will be return Bad Request</returns>        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Reservation customer)
        {
            if (customer is null)
            {
               return BadRequest();
            }

            await _reservationRepository.Create(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }
```

```csharp
        /// <summary>
        /// Using this method, you can edit and update the reservation whose "id" is specified.
        /// </summary>
        /// <param name="id">It is a required area and so type is int</param>
        /// <returns>If function is succeded will be return NoContent, than will be return Bad Request</returns>
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Reservation customer)
        {
            var existingCustomer = await _reservationRepository.GetCustomer(id);
            if (existingCustomer is null)
            {
                return BadRequest();
            }

            await _reservationRepository.Update(id, customer);

            return NoContent();
        }
```

```csharp

        /// <summary>
        /// This function can remove your reservation. 
        /// </summary>
        /// <param name="id">It is a required area and so type is int</param>
        /// <returns>If function is succeded will be return NoContent, than will be return NotFound</returns>        
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _reservationRepository.GetCustomer(id);

            if (customer is null)
            {
                return NotFound();
            }

            await _reservationRepository.Delete(customer.Id);

            return NoContent();
        }
    }
    
```

- I apply the necessary codes in AddSwaggerGen() in program.cs so that my information and summary that will appear in my API can be seen by developers. After doing this, in order to activate the "xml" file, go to the property area of my project and go to the "OutPut" field in the Build tab in the ".csproj" file. I tick "Documentation File".

```csharp
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
            Name = "Oğuzhan Kömcü",
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

    
```
- I am running my project to test .SwaggerUI view is coming . The methods I wrote here are displayed separately thanks to swagger. I run my methods by giving the desired parameters.

<img src="https://user-images.githubusercontent.com/96787308/163675939-4083b9c1-b5eb-422e-afc4-2a50017ac592.png" width="900" height="500">   

- I want to enable access to my methods in my API, which I will create in my project, only for registered ones. I have already created a User class for this. Here, the user will first register, and then I will give access to the "JWT Token" that he will receive in the next "Login" operation. first i created it on an interfaceface.

```csharp
 public interface IUserRepository
    {
        string Authentication(string userName, string password);
        Task<User> Register(User user);
        Task<User> GetUser(string userName);

        Task<List<User>> GetUsers();
    }
 

```

- I install these packages before starting the process for authentication.

- Microsoft.AspNetCore.Authentication 
- Microsoft.AspNetCore.Authentication.JwtBearer
- System.IdentityModel.Tokens.Jwt


- Now I create a "UserRepository.cs" class and apply the necessary "Crud" operations for registration.

```csharp
    public class UserRepository : IUserRepository
    {
         private readonly IMongoCollection<User> _user;
        private readonly MongoDbSettings _settings;
        

        public UserRepository(IOptions<MongoDbSettings> mongoDBSettings, IConfiguration configuration)
        {
            _settings = mongoDBSettings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _user = database.GetCollection<User>("ConnectionName2"); // Here I specify the path of my "User" document and 
             // its field in the "appsettings.json" document, which I named "ConnectionName2".
             // Otherwise, my "Users" document cannot be found.
            _settings.SecretKey = configuration.GetSection("JwtKey").ToString(); // Here in the "appsettings.json" document,
            // specify the field that I keep for the jwt token.
        }

        public async Task<List<User>> GetUsers()
        {

            return await _user.Find(user => true).ToListAsync();
        }

        public string Authentication(string userName, string password) // Upon login, an Authentication key will be set
                                                                       // and its duration will be within the specified time.
        {
            var user = _user.Find(x => x.UserName == userName && x.Password == password).FirstOrDefault();
            if (user == null)
            {
                return null;
                
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenkey = Encoding.ASCII.GetBytes(_settings.SecretKey );
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.UserName)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);

            }
 
        }

        public async Task<User> GetUser(string userName)
        {
            return  await _user.Find<User>(User => User.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<User> Register(User user)
        {
            await _user.InsertOneAsync(user);
            return user;

        }
 

```
- Now I go and do the resolve in my "Program.cs" class.

```csharp
builder.Services.AddScoped<IUserRepository, UserRepository>();
 
```

- Now we need to configure our Jwt Authentication in program.cs as follows.


```csharp
builder.Services.AddAuthentication( x=>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtKey").ToString())),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

- Now to be able to use any type of Authentication we need to enable it in our project. For this I added app.UseAuthentication() and app.UseAuthorization() method in program.cs.

```csharp
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

```
- Configuration.GetSection(“JwtKey”) the key I'm using here comes from the value in appsettings.json. I went and added this to my json file.

  

```csharp
  
 "AllowedHosts": "*",
  "MongoDBConnection": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "HotelReservationDB",
    "CollectionName": "Resevations",
    "ConnectionName2": "Users"
  },

  "JwtKey": "This usedto sing in and verification jwt token"

```
- For Swagger UI to carry Json Web Tokens (JWT) in authorized APIs, authorization must be configured. This is how we add the security schema to the dependency container. 

  
```csharp
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
            Name = "Oğuzhan Kömcü",
            Url = new Uri("https://github.com/oguzhanKomcu")
        },
        License = new OpenApiLicense()
        {
            Name = "MIT License",
            Url = new Uri("http://opensource.org/licenses/MIT")
        }
    });
  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() 
    {
        Description = "JWT Authentication header using token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        
    });    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "Bearer",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });


    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
 });

```
- Now I have created a "UserController" api in my "Controler" folder.

```csharp
 [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        
        
        [Route("authenticate")]
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
           var token = _userRepository.Authentication(user.UserName, user.Password); //For the verification token to be generated when login
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(new { token, user });
 
        }



        /// <summary>
        /// This function lists all made user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userRepository.GetUsers().ConfigureAwait(false));
        }

        /// <summary>
        /// This function returns the user whose "id" is given.
        /// </summary>
        /// <param name="id">It is a required area and so type is string</param>
        /// <returns>If function is succeded will be return Ok, than will be return NotFound</returns>

        [HttpGet("{id:length(24)}")]
        public  ActionResult<User> GetUser(string id)
        {
            var user =  _userRepository.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// You can add a new user using this method.
        /// </summary>
        /// <returns>If function is succeded will be return CreatedAtAction, than will be return Bad Request</returns>    
        [HttpPost("Register") ]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user is null)
            {
                return BadRequest();
            }

            await _userRepository.Register(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

    }

```

- Now I add [Authorize] atributte per my "ReservationsController" controller so that unregistered and expired users cannot access my Resevation methods.

```csharp
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase

```

Now let's see how it is tested.I first got all the users by making a get list and copied their id.Then I entered my username and password in the "POST/api/User/authenticate" and got my token.

<img src="https://user-images.githubusercontent.com/96787308/165599747-e47a3e61-cfab-41cf-a81e-974f85d6403e.png" width="900" height="400">

<img src="https://user-images.githubusercontent.com/96787308/165600835-20fd561f-67a3-4204-8747-159be73a76f6.png" width="900" height="400">

After that, I click on the "Authorize" button and enter my token so that "Bearer" appears at the beginning.

<img src="https://user-images.githubusercontent.com/96787308/165601351-33d5253f-7955-459d-8055-1cb3676c1957.png" width="900" height="400">

Now I can access my methods in my "Reservation" controls, which are closed to outside access.

<img src="https://user-images.githubusercontent.com/96787308/165601894-f6a662c1-e3cb-49cb-a68f-f2c93cdc2bcb.png" width="900" height="400">
