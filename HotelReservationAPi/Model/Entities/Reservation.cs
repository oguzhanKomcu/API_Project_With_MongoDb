using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelReservationAPi.Model.Entities
{
    public class Reservation :BaseEntity
    {
        

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
}
