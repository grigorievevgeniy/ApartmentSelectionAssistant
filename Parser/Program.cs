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

            parser.Start("https://kazan.cian.ru/cat.php?currency=2&deal_type=sale&engine_version=2&foot_min=10&maxprice=5000000&offer_type=flat&only_foot=2&region=4777&room3=1&room4=1");


            Scheduler.Start();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
