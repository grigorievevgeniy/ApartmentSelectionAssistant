using Parser.Cian;
using Parser.DtoModels;
using System;
using System.IO;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            CianParser parser = new CianParser();
            ParserContext parserContext = new ParserContext();

            string html;
            using (StreamReader sr = new StreamReader("Advertisement.txt"))
            {
                html = sr.ReadToEnd();
            }
            var t = parser.ParsingAdvertisement(html);

            AdvertisementRepository parserRepository = new AdvertisementRepository();
            parserRepository.AddAdvertisement(t);



            Scheduler.Start();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
