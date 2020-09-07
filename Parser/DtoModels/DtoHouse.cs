using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.DtoModels
{
    public class DtoHouse
    {
        public Guid Id { get; set; }
        public string Url { get; set; }

        public string FullAdress { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string MicroDistrict { get; set; }
        public string Street { get; set; }

        public string NumberHouse { get; set; }

        public string DistanceInfo { get; set; }

        public int FloorCount { get; set; }
        public int YearBuilt { get; set; }
        public string ConstructionSeries { get; set; }
        public string HouseType { get; set; }
        public string CeilingType { get; set; }
        public string Entrances { get; set; } //Подъезды
        public string Elevators { get; set; }
        public string Heating { get; set; }
        public string Emergency { get; set; }
        public string Parking { get; set; }
        public string GarbageChute { get; set; }
        public string GasSupply { get; set; }

        public virtual ICollection<DtoAdvertisement> Advertisements { get; set; }
    }
}
