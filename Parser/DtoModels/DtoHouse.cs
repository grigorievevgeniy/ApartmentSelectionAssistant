using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.DtoModels
{
    public class DtoHouse : EntityBase
    {
        public virtual Guid Id { get; set; }
        public virtual string Url { get; set; }

        public virtual string FullAdress { get; set; }
        public virtual string Region { get; set; }
        public virtual string City { get; set; }
        public virtual string District { get; set; }
        public virtual string MicroDistrict { get; set; }
        public virtual string Street { get; set; }

        public virtual string NumberHouse { get; set; }

        public virtual string DistanceInfo { get; set; }

        public virtual int FloorCount { get; set; }
        public virtual int YearBuilt { get; set; }
        public virtual string ConstructionSeries { get; set; }
        public virtual string HouseType { get; set; }
        public virtual string CeilingType { get; set; }
        public virtual string Entrances { get; set; } //Подъезды
        public virtual string Elevators { get; set; }
        public virtual string Heating { get; set; }
        public virtual string Emergency { get; set; }
        public virtual string Parking { get; set; }
        public virtual string GarbageChute { get; set; }
        public virtual string GasSupply { get; set; }

        public virtual IList<DtoAdvertisement> Advertisements { get; set; }

        public DtoHouse()
        {
            Advertisements = new List<DtoAdvertisement>();
        }

        public virtual void AddAdvertisement(DtoAdvertisement advertisement)
        {
            advertisement.House = this;
            Advertisements.Add(advertisement);
        }
    }
}
