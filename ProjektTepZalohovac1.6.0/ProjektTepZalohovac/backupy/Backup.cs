using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjektTepZalohovac.backupy
{
    public abstract class Backup
    {
        public abstract void Execute(JsonBackupJobFileConfig config);

        protected void PerformBackup(JsonBackupJobFileConfig config, Func<FileInfo, bool> shouldCopy, string backupType)
        {
            foreach (var source in config.Sources)
            {
                foreach (var target in config.Targets)
                {
                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var sanitizedTimestamp = new string(timestamp.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());
                    var targetDir = Path.Combine(target, $"{backupType}_{sanitizedTimestamp}");

                    Directory.CreateDirectory(targetDir);

                    CopyWithCondition(source, targetDir, shouldCopy);

                    BackupUtils.SaveLastBackup(target, backupType, DateTime.Now);
                }
            }

            BackupUtils.ApplyRetention(config.Targets, config.Retention);
        }

        private void CopyWithCondition(string sourceDir, string targetDir, Func<FileInfo, bool> shouldCopy)
        {
            var dir = new DirectoryInfo(sourceDir);

            foreach (var file in dir.GetFiles())
            {
                if (shouldCopy(file))
                {
                    file.CopyTo(Path.Combine(targetDir, file.Name), true);
                }
            }

            foreach (var subDir in dir.GetDirectories())
            {
                var subDirTarget = Path.Combine(targetDir, subDir.Name);
                Directory.CreateDirectory(subDirTarget);
                CopyWithCondition(subDir.FullName, subDirTarget, shouldCopy);
            }
        }
    }
}




