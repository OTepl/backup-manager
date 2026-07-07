using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektTepZalohovac.backupy;

namespace ProjektTepZalohovac
{
    internal class BackupManager
    {
        private readonly JsonBackupJobFileConfig _config;

        public BackupManager(JsonBackupJobFileConfig config)
        {
            _config = config;
        }

        public void RunBackup()
        {
            try
            {
                Backup backup = _config.Method.ToLower() switch
                {
                    "full" => new FullBackup(),
                    "differential" => new DifferentialBackup(),
                    "incremental" => new IncrementalBackup(),
                    _ => throw new ArgumentException("Invalid backup method in config.json.")
                };

                backup.Execute(_config);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Backup failed: {ex.Message}");
            }
        }
    }
}


