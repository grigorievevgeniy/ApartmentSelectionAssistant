using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.DtoModels
{
    public class Owner
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string PhoneNumbers { get; set; }
        public string Description { get; set; }
        public string Site { get; set; }
        public string OtherInfo { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
