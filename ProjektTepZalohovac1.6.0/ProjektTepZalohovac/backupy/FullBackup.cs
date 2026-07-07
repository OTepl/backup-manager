using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektTepZalohovac.backupy;

namespace ProjektTepZalohovac.backupy
{
    public class FullBackup : Backup
    {
        public override void Execute(JsonBackupJobFileConfig config)
        {
            PerformBackup(config, _ => true, "full");
        }
    }
}

