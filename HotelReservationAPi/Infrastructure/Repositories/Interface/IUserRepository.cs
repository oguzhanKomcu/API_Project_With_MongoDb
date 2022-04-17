using HotelReservationAPi.Model.Entities;

namespace HotelReservationAPi.Infrastructure.Repositories.Interface
{
    public interface IUserRepository
    {
        string Authentication(string userName, string password);
        Task<User> Register(User user);
        Task<User> GetUser(string userName);

        Task<List<User>> GetUsers();
    }
}
