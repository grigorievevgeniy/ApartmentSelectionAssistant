using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public class Scheduler
    {
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<Test>().Build();

            var now = DateTime.Now;
            var NextDay = new DateTime(now.Year, now.Month, now.Day + 1);
            var NextMinute = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute + 1, 0);

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger1", "group1")     // идентифицируем триггер с именем и группой
                .StartAt(NextMinute)
                //.StartAt(NextDay)
                //.StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    //.WithIntervalInHours(24)
                    .WithIntervalInSeconds(4)
                    //.WithIntervalInMinutes(1)          // через 1 минуту
                    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер

            await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}
