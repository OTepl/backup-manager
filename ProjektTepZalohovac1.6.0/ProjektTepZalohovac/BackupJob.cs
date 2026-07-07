using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading.Tasks;
using Quartz;

namespace ProjektTepZalohovac
{
    public class BackupJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var backupTask = (Action)context.JobDetail.JobDataMap["backupTask"];
            backupTask.Invoke();
            return Task.CompletedTask;
        }
    }
}
