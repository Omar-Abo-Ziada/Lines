//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Lines.Infrastructure.Entities
//{
//    internal class CityEntity
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }

//        // Navigation properties
//        public virtual ICollection<VehicleTypeEntity>? AllowedVehicleTypes { get; set; }
//        public virtual ICollection<LocationEntity> Locations { get; set; } = new List<LocationEntity>();
//        public virtual ICollection<SectorEntity> Sectors { get; set; } = new HashSet<SectorEntity>();
//    }
//}
