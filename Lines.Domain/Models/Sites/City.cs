using System.ComponentModel.DataAnnotations.Schema;
using Lines.Domain.Models.Common;
using Lines.Domain.Models.Vehicles;

namespace Lines.Domain.Models.Sites
{
    //[Table("Cities", Schema = "Sites")] 
    public class City : BaseModel
    {
        public string Name { get;  set; }
        public double Latitude { get;  set; }
        public double Longitude { get;  set; }
        public ICollection<CityVehicleType> CityVehicleTypes { get; set; }
        // Navigation properties
        public virtual ICollection<Sector> Sectors { get;  set; } = new List<Sector>();

        // Constructor
        public City(string name, double latitude, double longitude)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("City name cannot be empty.", nameof(name));
            
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Sectors = new List<Sector>();
        }


        // Just for data seeding
        public City()
        {

        }

        // Business Methods
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("City name cannot be empty.", nameof(newName));
            Name = newName;
        }
        
        
    }
}