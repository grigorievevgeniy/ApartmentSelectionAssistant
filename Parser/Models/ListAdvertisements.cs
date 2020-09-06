using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Models
{
    public class ListAdvertisements
    {
        public ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
        public bool ExistNextPage { get; set; }
        public string UrlNextPage { get; set; }
    }
}
