﻿using System;
using System.Collections.Generic;

namespace Parser.Models
{
    public class Owner
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public List<string> PhoneNumbers { get; set; } = new List<string>();
        public string Description { get; set; }
        public string Site { get; set; }
        public string OtherInfo { get; set; }
    }
}