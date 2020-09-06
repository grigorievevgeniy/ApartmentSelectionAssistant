﻿using Parser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Interfaces
{
    public interface IParser
    {
        public string DownloadHtml(string url);
        public ListAdvertisements ParsingListAdvertisement(string html);
        public Advertisement ParsingAdvertisement(string html);
    }
}
