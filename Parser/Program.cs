using Parser.Cian;
using System;
using System.IO;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            CianParser parser = new CianParser();

            string html;
            //html = parser.DownloadHtml("https://kazan.cian.ru/cat.php?currency=2&deal_type=sale&engine_version=2&foot_min=15&ipoteka=1&limit=20&maxprice=6000000&min_house_year=2010&mintarea=70&object_type%5B0%5D=1&offer_type=flat&only_foot=2&p=1&region=4777&room3=1&room4=1&with_neighbors=1&saved_search_id=21008055");
            //using (StreamWriter sw = new StreamWriter("Cian.txt", false, System.Text.Encoding.Default))
            //{
            //    sw.WriteLine(html);
            //}
            using (StreamReader sr = new StreamReader("Cian.txt"))
            {
                html = sr.ReadToEnd();
            }

            var x = parser.ParsingListAdvertisement(html);
            
            //Console.WriteLine(s);






            Scheduler.Start();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
