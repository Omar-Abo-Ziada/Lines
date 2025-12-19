using System.ComponentModel.DataAnnotations.Schema;
using Lines.Domain.Models.Common;

namespace Lines.Domain.Models.Sites
{
    //[Table("Sectors", Schema = "Sites")]
    public class Sector : BaseModel
    {
        public string Name { get;  set; }
        public Guid CityId { get;  set; }
        public virtual City City { get;  set; }

        // Navigation properties
 

        // Constructor
        public Sector(string name, Guid cityId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Sector name cannot be empty.", nameof(name));
            
            if (cityId.Equals(Guid.Empty))
                throw new ArgumentException("City id cannot be empty.", nameof(cityId));
            Name = name;
            CityId = cityId;
        }


        // Just for data seeding
        public Sector()
        {

        }
        // Business Methods

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Sector name cannot be empty.", nameof(newName));
            Name = newName;
        }

        public void UpdateCity(Guid newCityId)
        {
            if(newCityId.Equals(Guid.Empty))
                throw new ArgumentException("City id cannot be empty.", nameof(newCityId));

            CityId = newCityId;
        }
    }
}