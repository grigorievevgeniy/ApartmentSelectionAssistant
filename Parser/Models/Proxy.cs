using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Models
{
    class Proxy
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Country { get; set; }
        public string Anonymity { get; set; }
        public bool Https { get; set; }


        //public DateTime Added { get; set; }
        //public DateTime Changed { get; set; }
        //public int GoodAvitoParsCount { get; set; } = 0;
        //public int BadAvitoParsCount { get; set; } = 0;
        //public int GoodCianParsCount { get; set; } = 0;
        //public int BadCianParsCount { get; set; } = 0;
        //public int GoodParsCount { get; set; } = 0;
        //public int BadParsCount { get; set; } = 0;
    }
}
