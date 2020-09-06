using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.DtoModels
{
    public class Advertisement
    {
        //Todo Нужно ли здесь поле Id
        public Guid Id { get; set; }

        public bool FullParse { get; set; }
        public DateTime DateUpdate { get; set; }

        public string Url { get; set; }
        public string DatePublishString { get; set; }
        public DateTime DatePublish { get; set; }
        public double TotalArea { get; set; }
        public double LivingArea { get; set; }
        public double KitchenArea { get; set; }

        public int RoomCount { get; set; }
        public int Floor { get; set; }

        public string HousingType { get; set; }
        public string RoomArea { get; set; }
        public double CeilingHeight { get; set; }
        public string Bathroom { get; set; }
        public string BalconyLoggia { get; set; }
        public string Repairs { get; set; }
        public string ViewFromWindows { get; set; }
        public string Description { get; set; }
        public string Layout { get; set; } //Планировка

        public decimal Price { get; set; }
        public decimal EstimatedPrice { get; set; }

        public Guid OwnerId { get; set; }
        public virtual Owner Owner { get; set; }
        public Guid HouseId { get; set; }
        public virtual House House { get; set; }

    }
}
