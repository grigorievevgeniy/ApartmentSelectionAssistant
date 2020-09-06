using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Parser.Interfaces;
using Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Parser.Cian
{
    class CianParser : IParser
    {
        public string DownloadHtml(string url)
        {
            using (WebClient client = new WebClient())
            {
                //client.Proxy = new WebProxy("83.97.23.90", 18080);
                return client.DownloadString(url);
            }
        }

        public ICollection<Advertisement> ParsingListAdvertisement(string html)
        {
            IHtmlDocument document = new HtmlParser().ParseDocument(html);
            var rows = document.QuerySelectorAll("div[data-name='OfferCard']").ToArray();

            List<Advertisement> Advertisements = new List<Advertisement>();
            foreach (var row in rows)
            {
                Owner owner = new Owner();
                House house = new House();
                Advertisement advertisement = new Advertisement();

                advertisement.Url = row.QuerySelector(".c6e8ba5398--header--1fV2A").GetAttribute("href");

                #region Парсим титул "3-комн. кв., 97,4 м², 1/16 этаж"
                var tempTitle = row.QuerySelector("div[data-name='Title']")?.TextContent;
                if (tempTitle == null) tempTitle = row.QuerySelector("div[data-name='TopTitle']")?.TextContent;

                var tempTitleArray = tempTitle?.Split(", ");

                if (tempTitleArray != null)
                {
                    foreach (var item in tempTitleArray)
                    {
                        if (item.Contains("-комн. кв."))
                            advertisement.RoomCount = int.Parse(string.Join("", item.Where(c => char.IsDigit(c))));
                        if (item.Contains("м²"))
                        {
                            advertisement.TotalArea = double.Parse(string.Join("", item.Where(c => char.IsDigit(c) || c == ',')));
                        }
                        if (item.Contains("этаж"))
                        {
                            advertisement.Floor = int.Parse(item.Substring(0, item.IndexOf('/')));
                            house.FloorCount = int.Parse(item.Substring(item.IndexOf('/') + 1, item.IndexOf(' ') - item.IndexOf('/') - 1));
                        }
                    }
                }
                #endregion

                advertisement.Price = decimal.Parse(string.Join("", row.QuerySelector(".c6e8ba5398--header--1df-X").TextContent.Where(c => char.IsDigit(c))));
                house.DistanceInfo = row.QuerySelector(".c6e8ba5398--underground-container--1exfN")?.TextContent;

                #region Парсим адресс "Татарстан респ., Казань, р-н Советский, ул. Аделя Кутуя, 83"
                house.FullAdress = row.QuerySelector("div[data-name='AddressItem']")?.TextContent;
                var tempAdressArray = house.FullAdress?.Split(", ");
                
                //ToDo Надо пересмотреть, т.к. порядок часто разный
                house.Region = tempAdressArray[0];
                house.City = tempAdressArray[1];
                house.District = tempAdressArray[2];
                house.Street = tempAdressArray[3];
                house.NumberHouse = tempAdressArray[4];

                for (int i = 0; i < tempAdressArray.Length; i++)
                {
                    if (tempAdressArray[i].Contains("р-н")) house.District = tempAdressArray[i];
                    if (tempAdressArray[i].Contains("мкр.")) house.MicroDistrict = tempAdressArray[i];
                    //ToDo рассмотреть другие варианты
                }
                #endregion

                advertisement.Description = row.QuerySelector("div[data-name='Description']")?.TextContent;

                owner.Name = row.QuerySelector("div[data-name='AgentBrandMainInfoComponent']")?.TextContent;
                owner.PhoneNumbers.Add(row.QuerySelector("div[data-name='PhoneButton']")?.TextContent);


                advertisement.DatePublishString = row.QuerySelector("div[data-name='TimeLabel'] .c6e8ba5398--absolute--9uFLj")?.TextContent;
                //ToDo Распарсить время "7 апр, 11:15", "вчера, 12:06", "сегодня, 12:06"
                //advertisement.DatePublish = ...

                advertisement.DateUpdate = DateTime.Now;
                advertisement.FullParse = false;
                advertisement.House = house;
                advertisement.Owner = owner;

                Advertisements.Add(advertisement);
            }

            return Advertisements;
        }

        public Advertisement ParsingAdvertisement(string html)
        {
            throw new NotImplementedException();
        }
    }
}
