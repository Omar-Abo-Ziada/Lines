using Lines.Domain.Models.Common;
using Lines.Domain.Models.Passengers;

namespace Lines.Domain.Models.Sites
{
    //[Table("Locations", Schema = "Sites")]
    public class FavoriteLocation : BaseModel
    {
        // Basic Properties
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid CityId { get; set; }
        public virtual City City { get; set; }
        public Guid PassengerId { get; set; }
        public virtual Passenger  Passenger { get; set; }
        public Guid SectorId { get;  set; }
        public virtual Sector Sector { get;  set; } 

        public FavoriteLocation(string name, double latitude, double longitude,Guid passengerId, Guid cityId, Guid sectorId)
        {
            ValidateLocation(latitude, longitude);
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            CityId = cityId;
            SectorId = sectorId;
            PassengerId = passengerId;
        }


        // Just for data seeding
        public FavoriteLocation()
        {

        }
        
        public void Update(string name, double latitude, double longitude,Guid passengerId, Guid cityId, Guid sectorId)
        {
            ValidateLocation(latitude, longitude);
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            CityId = cityId;
            SectorId = sectorId;
            PassengerId = passengerId;
        }
        void ValidateLocation(double latitude, double longitude)
        {
            if (!IsValidLatitude(latitude))
                throw new ArgumentException("Invalid latitude. Must be between -90 and 90 degrees.");

            if (!IsValidLongitude(longitude))
                throw new ArgumentException("Invalid longitude. Must be between -180 and 180 degrees.");
        }

        bool IsValidLatitude(double latitude)
        {
            return latitude >= -90 && latitude <= 90;
        }

        bool IsValidLongitude(double longitude)
        {
            return longitude >= -180 && longitude <= 180;
        }

    }
}