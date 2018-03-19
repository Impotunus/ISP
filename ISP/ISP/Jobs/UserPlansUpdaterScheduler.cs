using System;
using Quartz;
using Quartz.Impl;

namespace ISP.Jobs
{
    public class UserPlansUpdaterScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = new StdSchedulerFactory().GetScheduler();
            scheduler.Start();

            IJobDetail userPlanUpdateJob = JobBuilder.Create<UserPlansUpdater>().Build();

            ITrigger userPlanUpdateTrigger = TriggerBuilder.Create()
                .WithIdentity("HourTrigger", "DefaultGroup")
                .StartAt(DateTime.UtcNow.AddSeconds(10))
                .WithSimpleSchedule(t => t.WithIntervalInMinutes(BLL.Utility.Constants.UserPlansUpdateJobRate).RepeatForever())
                .Build();

            scheduler.ScheduleJob(userPlanUpdateJob, userPlanUpdateTrigger);
        }
    }
}