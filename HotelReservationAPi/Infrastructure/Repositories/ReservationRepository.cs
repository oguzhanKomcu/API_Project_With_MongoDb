using HotelReservationAPi.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HotelReservationAPi.Infrastructure.Repositories
{
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
    }
}
