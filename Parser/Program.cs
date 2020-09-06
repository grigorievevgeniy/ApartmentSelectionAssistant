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
            //html = parser.DownloadHtml("https://kazan.cian.ru/kupit-3-komnatnuyu-kvartiru/");
            //using (StreamWriter sw = new StreamWriter("Cian.txt", false, System.Text.Encoding.Default))
            //{
            //    sw.WriteLine(html);
            //}
            using (StreamReader sr = new StreamReader("Cian.txt"))
            {
                html = sr.ReadToEnd();
            }
            var x = parser.ParsingListAdvertisement(html);



            //html = parser.DownloadHtml("https://kazan.cian.ru/sale/flat/222765618/");
            //using (StreamWriter sw = new StreamWriter("Advertisement.txt", false, System.Text.Encoding.Default))
            //{
            //    sw.WriteLine(html);
            //}
            using (StreamReader sr = new StreamReader("Advertisement.txt"))
            {
                html = sr.ReadToEnd();
            }
            var t = parser.ParsingAdvertisement(html);


            //Console.WriteLine(s);






            Scheduler.Start();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
