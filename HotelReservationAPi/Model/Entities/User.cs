using MongoDB.Bson.Serialization.Attributes;

namespace HotelReservationAPi.Model.Entities
{
    public class User : BaseEntity
    {
        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("password")]

        public string Password { get; set; }

    }
}
