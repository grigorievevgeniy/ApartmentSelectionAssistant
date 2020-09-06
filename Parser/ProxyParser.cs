using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Parser
{
    class ProxyParser
    {
        public static List<Proxy> GetProxies()
        {
            using (var wc = new WebClient())
            {
                var html = wc.DownloadString("https://free-proxy-list.net/");
                IHtmlDocument document = new HtmlParser().ParseDocument(html);
                var rows = document.QuerySelectorAll("table.table-striped.table-bordered>tbody>tr").ToArray();

                List<Proxy> proxies = rows
                    .Where(x => x.Children[6].TextContent == "yes" && x.Children[4].TextContent == "elite proxy")
                    .Distinct()
                    .OrderBy(x => x.Children[0].TextContent)
                    .Select(x => new Proxy()
                    {
                        Ip = x.Children[0].TextContent,
                        Port = x.Children[1].TextContent,
                        Country = x.Children[3].TextContent,
                        Anonymity = x.Children[4].TextContent,
                        Https = x.Children[6].TextContent == "yes"
                    })
                    .ToList();

                return proxies;
            }
        }
        public static bool CheckProxy(string ip, int port)
        {
            string urlYandex = "https://yandex.ru/";
            string urlCian = "https://krasnodar.cian.ru/cat.php?deal_type=sale&engine_version=2&object_type%5B0%5D=1&offer_type=flat&region=4820&room1=1&room2=1";
            string urlAvito = "https://www.avito.ru/zelenodolsk/kvartiry/3-k_kvartira_42_m_13_et._1860325790";


            using (WebClient client = new WebClient())
            {
                client.Proxy = new WebProxy(ip, port);
                try
                {
                    var s = client.DownloadString(urlYandex);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }

                return true;
            }
        }
        public static bool CheckProxy(Proxy proxy) => CheckProxy(proxy.Ip, Convert.ToInt32(proxy.Port));
    }
}
