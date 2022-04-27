using HotelReservationAPi.Infrastructure.Repositories.Interface;
using HotelReservationAPi.Model;
using HotelReservationAPi.Model.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelReservationAPi.Infrastructure.Repositories.Concrete
{
    public class UserRepository : IUserRepository
    {
         private readonly IMongoCollection<User> _user;
        private readonly MongoDbSettings _settings;
        

        public UserRepository(IOptions<MongoDbSettings> mongoDBSettings, IConfiguration configuration)
        {
            _settings = mongoDBSettings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _user = database.GetCollection<User>("ConnectionName2");
            _settings.SecretKey = configuration.GetSection("JwtKey").ToString();
        }

        public async Task<List<User>> GetUsers()
        {

            return await _user.Find(user => true).ToListAsync();
        }

        public string Authentication(string userName, string password)
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
    }
}
