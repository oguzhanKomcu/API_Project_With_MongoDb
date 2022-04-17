using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelReservationAPi.Model.Entities
{
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
}
