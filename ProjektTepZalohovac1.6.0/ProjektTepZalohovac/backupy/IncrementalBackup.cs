using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektTepZalohovac.backupy;

namespace ProjektTepZalohovac.backupy
{
    public class IncrementalBackup : Backup
    {
        public override void Execute(JsonBackupJobFileConfig config)
        {
            var lastBackup = BackupUtils.GetLastBackup("incremental", config.Targets) ??
                             BackupUtils.GetLastBackup("full", config.Targets);

            if (lastBackup == null)
            {
                PerformBackup(config, _ => true, "full");
                return;
            }

            PerformBackup(config, file => file.LastWriteTime > lastBackup, "incremental");
        }
    }
}

