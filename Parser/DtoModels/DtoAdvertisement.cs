using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.DtoModels
{
    public class DtoAdvertisement : EntityBase
    {
        public virtual Guid Id { get; set; }

        public virtual bool FullParse { get; set; }
        public virtual DateTime DateUpdate { get; set; }

        public virtual string Url { get; set; }
        public virtual string DatePublishString { get; set; }
        public virtual DateTime DatePublish { get; set; }
        public virtual double TotalArea { get; set; }
        public virtual double LivingArea { get; set; }
        public virtual double KitchenArea { get; set; }

        public virtual int RoomCount { get; set; }
        public virtual int Floor { get; set; }

        public virtual string HousingType { get; set; }
        public virtual string RoomArea { get; set; }
        public virtual double CeilingHeight { get; set; }
        public virtual string Bathroom { get; set; }
        public virtual string BalconyLoggia { get; set; }
        public virtual string Repairs { get; set; }
        public virtual string ViewFromWindows { get; set; }
        public virtual string Description { get; set; }
        public virtual string Layout { get; set; } //Планировка

        public virtual decimal Price { get; set; }
        public virtual decimal EstimatedPrice { get; set; }

        public virtual Guid OwnerId { get; set; }
        public virtual DtoOwner Owner { get; set; } = null;
        public virtual Guid HouseId { get; set; }
        public virtual DtoHouse House { get; set; } = null;

    }
}
