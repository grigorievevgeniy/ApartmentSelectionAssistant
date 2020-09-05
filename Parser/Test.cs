using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parser
{
    public class Test : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Random random = new Random();
            var x = DateTime.Now;
            Thread.Sleep(random.Next(1, 12000));
            Console.WriteLine(x);        
        }
    }
}
