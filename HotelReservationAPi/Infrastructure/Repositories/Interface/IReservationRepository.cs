using HotelReservationAPi.Model;
using HotelReservationAPi.Model.Entities;

namespace HotelReservationAPi.Infrastructure.Repositories
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetCustomers();


        Task<Reservation> GetCustomer(string id);


        Task<Reservation> Create(Reservation customer);


        Task Update(string id, Reservation customer);


        Task Delete(string id);
    }
}
