using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
namespace ProjektTepZalohovac
{
    internal class TimeManager
    {
         private readonly IScheduler _scheduler;

        public TimeManager()
        {
            var factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler().Result;
        }

        public async Task InitializeScheduler(JsonBackupJobFileConfig config, Action backupTask)
        {
            await _scheduler.Start();

            var job = JobBuilder.Create<BackupJob>()
                .WithIdentity($"backupJob_{Guid.NewGuid()}", "backupGroup")
                .Build();

            job.JobDataMap["backupTask"] = backupTask;

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"backupTrigger_{Guid.NewGuid()}", "backupGroup")
                .WithCronSchedule(config.Timing)
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }
    }
}
