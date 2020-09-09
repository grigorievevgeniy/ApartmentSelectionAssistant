using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.DtoModels
{
    public class DtoOwner : EntityBase
    {
        public virtual Guid Id { get; set; }
        public virtual string Url { get; set; }
        public virtual string Name { get; set; }
        public virtual string PhoneNumbers { get; set; }
        public virtual string Description { get; set; }
        public virtual string Site { get; set; }
        public virtual string OtherInfo { get; set; }

        public virtual IList<DtoAdvertisement> Advertisements { get; set; }

        public DtoOwner()
        {
            Advertisements = new List<DtoAdvertisement>();
        }

        public virtual void AddAdvertisements(DtoAdvertisement advertisement)
        {
            advertisement.Owner = this;
            Advertisements.Add(advertisement);
        }
    }
}
