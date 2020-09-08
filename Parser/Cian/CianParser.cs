using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Parser.DtoModels;
using Parser.Helpers;
using Parser.Interfaces;
using Parser.Models;
using System;
using System.Linq;
using System.Net;

namespace Parser.Cian
{
    public class CianParser : IParser
    {
        AdvertisementRepository repository;

        public CianParser()
        {
            repository = new AdvertisementRepository();
        }

        public void Start(string url)
        {
            ListAdvertisements listAdvertisements = new ListAdvertisements();
            do
            {
                if (listAdvertisements.UrlNextPage != default) url = listAdvertisements.UrlNextPage;

                string html = DownloadHtml(url);
                listAdvertisements = ParsingListAdvertisement(html);

                repository.AddListAdvertisement(listAdvertisements);

            } while (listAdvertisements.ExistNextPage);

            // Парсим отдельно каждое объявление
            var urls = repository.GetUnfinishedAdvertisementUrls();
            foreach (var item in urls)
            {
                string html = DownloadHtml(item);
                Advertisement advertisement = ParsingAdvertisement(html);
                advertisement.Url = item;
                repository.AddAdvertisement(advertisement);
            }
        }

        public string DownloadHtml(string url)
        {
            if(FileInsteadWeb.CheckExistFile(url))
            {
                return FileInsteadWeb.ReadFile(url);
            }
            else
            {
                string html; 
                using (WebClient client = new WebClient())
                {
                    //client.Proxy = new WebProxy("83.97.23.90", 18080);
                    html = client.DownloadString(url);
                }
                FileInsteadWeb.SaveFile(url, html);

                return html;
            }
        }

        public ListAdvertisements ParsingListAdvertisement(string html)
        {
            IHtmlDocument document = new HtmlParser().ParseDocument(html);
            var rows = document.QuerySelectorAll("div[data-name='OfferCard']").ToArray();

            ListAdvertisements Advertisements = new ListAdvertisements();
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
                owner.PhoneNumbers = row.QuerySelector("div[data-name='PhoneButton']")?.TextContent;


                advertisement.DatePublishString = row.QuerySelector("div[data-name='TimeLabel'] .c6e8ba5398--absolute--9uFLj")?.TextContent;
                //ToDo Распарсить время "7 апр, 11:15", "вчера, 12:06", "сегодня, 12:06"
                //advertisement.DatePublish = ...

                advertisement.DateUpdate = DateTime.Now;
                advertisement.FullParse = false;
                advertisement.House = house;
                advertisement.Owner = owner;

                Advertisements.Advertisements.Add(advertisement);
            }

            var pages = document.QuerySelectorAll("div[data-name='Pagination'] ._93444fe79c--list-item--2KxXr");
            for (int i = 0; i < pages.Length - 1; i++)
            {
                if (!pages[i].OuterHtml.Contains("href"))
                {
                    Advertisements.ExistNextPage = true;
                    Advertisements.UrlNextPage = pages[i + 1].QuerySelector("a").GetAttribute("href");
                    if (!Advertisements.UrlNextPage.StartsWith("https://kazan.cian.ru"))
                    {
                        Advertisements.UrlNextPage = "https://kazan.cian.ru" + Advertisements.UrlNextPage;
                    }
                    break;
                }
            } 

            return Advertisements;
        }
        public Advertisement ParsingAdvertisement(string html)
        {
            IHtmlDocument document = new HtmlParser().ParseDocument(html);

            Owner owner = new Owner();
            House house = new House();
            Advertisement advertisement = new Advertisement();

            house.FullAdress = document.QuerySelector("div[data-name='Geo'] span[itemprop='name']").GetAttribute("content");
            house.DistanceInfo = document.QuerySelector(".a10a3f92e9--undergrounds--2pop3")?.TextContent;

            var _aboutFlat = document.QuerySelectorAll("div[data-name='Description'] .a10a3f92e9--info--3XiXi");
            foreach (var item in _aboutFlat)
            {
                switch (item.Children[1].TextContent)
                {
                    case "Общая": { advertisement.TotalArea = double.Parse(string.Join("", item.Children[0].TextContent.Where(c => char.IsDigit(c) || char.IsLetter(',')))); } break;
                    case "Жилая": { advertisement.LivingArea = double.Parse(string.Join("", item.Children[0].TextContent.Where(c => char.IsDigit(c) || char.IsLetter(',')))); } break;
                    case "Кухня": { advertisement.KitchenArea = double.Parse(string.Join("", item.Children[0].TextContent.Where(c => char.IsDigit(c) || char.IsLetter(',')))); } break;
                    case "Этаж": { } break;
                    case "Построен": { house.YearBuilt = int.Parse(string.Join("", item.Children[0].TextContent.Where(c => char.IsDigit(c)))); } break;

                    default:
                        {
                            //Здесь нужно отследить новые поля
                        }
                        break;
                }
            }

            advertisement.Description = document.QuerySelector("div[data-name='Description'] p[itemprop='description']").TextContent;

            var _GeneralInformation = document.QuerySelectorAll(".a10a3f92e9--item--_ipjK");
            foreach (var item in _GeneralInformation)
            {
                switch (item.Children[0].TextContent)
                {
                    case "Тип жилья": { house.HouseType = item.Children[1].TextContent; } break;
                    case "Площадь комнат+ обозначение смежных комнат- обозначение изолированных комнат": { advertisement.RoomArea = item.Children[1].TextContent; } break;
                    case "Санузел": { advertisement.Bathroom = item.Children[1].TextContent; } break;
                    case "Балкон/лоджия": { advertisement.BalconyLoggia = item.Children[1].TextContent; } break;
                    case "Ремонт": { advertisement.Repairs = item.Children[1].TextContent; } break;
                    case "Вид из окон": { advertisement.ViewFromWindows = item.Children[1].TextContent; } break;
                    case "Планировка": { advertisement.Layout = item.Children[1].TextContent; } break;
                    case "Высота потолков": { advertisement.CeilingHeight = double.Parse(string.Join("", item.Children[1].TextContent.Where(c => char.IsDigit(c) || char.IsLetter(',')))); } break;

                    default:
                        {
                            //Здесь нужно отследить новые поля
                        }
                        break;
                }
            }

            var _BtiHouseData = document.QuerySelectorAll("div[data-name='BtiHouseData'] div[data-name='Item']");
            foreach (var item in _BtiHouseData)
            {
                switch (item.Children[0].TextContent)
                {
                    case "Год постройки": { house.YearBuilt = int.Parse(item.Children[1].TextContent); } break;
                    case "Строительная серия": { house.ConstructionSeries = item.Children[1].TextContent; } break;
                    case "Тип перекрытий": { house.CeilingType = item.Children[1].TextContent; } break;
                    case "Подъезды": { house.Entrances = item.Children[1].TextContent; } break;
                    case "Лифты": { house.Elevators = item.Children[1].TextContent; } break;
                    case "Аварийность": { house.Emergency = item.Children[1].TextContent; } break;
                    case "Парковка": { house.Parking = item.Children[1].TextContent; } break;
                    case "Мусоропровод": { house.GarbageChute = item.Children[1].TextContent; } break;
                    case "Газоснабжение": { house.GasSupply = item.Children[1].TextContent; } break;
                    case "Тип дома": { } break;
                    case "Отопление": { } break;

                    default:
                        {
                            //Здесь нужно отследить новые поля
                        }
                        break;
                }
            }

            decimal result;
            var elEPrice = document.QuerySelector("div[data-name='MarketPrice']");
            if (elEPrice != default)
            {
                if (decimal.TryParse(string.Join(" ", elEPrice.TextContent.Where(c => char.IsDigit(c) || char.IsLetter(','))), out result))
                    advertisement.EstimatedPrice = result;
            }

            var elPrice = document.QuerySelector("div[data-name='OfferTerms'] span[itemprop='price']");
            if (elPrice != default)
            {
                if (decimal.TryParse(string.Join(" ", elPrice.TextContent.Where(c => char.IsDigit(c) || char.IsLetter(','))), out result))
                    advertisement.Price = result;
            }

            owner.Url = document.QuerySelector("div[data-name='AuthorAsideBrand'] a[data-name='LinkWrapper']")?.GetAttribute("href");
            owner.Name = document.QuerySelector("div[data-name='AuthorAsideBrand'] a[data-name='LinkWrapper']")?.TextContent;

            advertisement.DateUpdate = DateTime.Now;
            advertisement.FullParse = true;
            advertisement.House = house;
            advertisement.Owner = owner;

            return advertisement;
        }
    }
}
