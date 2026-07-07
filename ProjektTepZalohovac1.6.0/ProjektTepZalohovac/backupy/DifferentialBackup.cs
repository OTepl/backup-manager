using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektTepZalohovac.backupy;

namespace ProjektTepZalohovac.backupy
{
    public class DifferentialBackup : Backup
    {
        public override void Execute(JsonBackupJobFileConfig config)
        {
            var lastFullBackup = BackupUtils.GetLastBackup("full", config.Targets);

            if (lastFullBackup == null)
            {
                PerformBackup(config, _ => true, "full");
                return;
            }

            PerformBackup(config, file => file.LastWriteTime > lastFullBackup, "differential");
        }
    }
}
