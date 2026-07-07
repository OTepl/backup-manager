using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektTepZalohovac
{
    public static class BackupUtils
    {
        private const string BackupLogFileName = "LastBackupLog.txt";

        public static void SaveLastBackup(string targetDir, string backupType, DateTime backupDate)
        {
            var logFilePath = Path.Combine(targetDir, BackupLogFileName);

            var logEntry = $"{backupDate:o} | {backupType}";

            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }

        public static DateTime? GetLastBackup(string backupType, IEnumerable<string> targetDirs)
        {
            foreach (var targetDir in targetDirs)
            {
                var logFilePath = Path.Combine(targetDir, BackupLogFileName);

                if (File.Exists(logFilePath))
                {
                    var lines = File.ReadAllLines(logFilePath);
                    foreach (var line in lines.Reverse()) 
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 2 &&
                            DateTime.TryParse(parts[0].Trim(), null, DateTimeStyles.RoundtripKind, out var date) &&
                            string.Equals(parts[1].Trim(), backupType, StringComparison.OrdinalIgnoreCase))
                        {
                            return date;
                        }
                    }
                }
            }
            return null;
        }

        public static void ApplyRetention(IEnumerable<string> targetDirs, Retention retention)
        {
            foreach (var targetDir in targetDirs)
            {
                var dirInfo = new DirectoryInfo(targetDir);
                if (!dirInfo.Exists) continue;

                var backupPackages = dirInfo.GetDirectories()
                                            .OrderBy(d => d.CreationTime)
                                            .ToList();

                if (backupPackages.Count > retention.Size)
                {
                    var newFullBackup = MergeBackups(backupPackages);

                    foreach (var backup in backupPackages)
                    {
                        backup.Delete(true);
                    }

                    newFullBackup.MoveTo(Path.Combine(targetDir, $"full_{DateTime.Now:yyyyMMddHHmmss}"));

                    continue;
                }

                while (backupPackages.Count > retention.Count)
                {
                    var oldest = backupPackages.First();
                    oldest.Delete(true);
                    backupPackages.RemoveAt(0);
                }
            }
        }

        private static DirectoryInfo MergeBackups(List<DirectoryInfo> backupPackages)
        {
            var tempDir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            tempDir.Create();

            foreach (var backup in backupPackages)
            {
                CopyDirectory(backup, tempDir, overwrite: true);
            }

            return tempDir;
        }

        private static void CopyDirectory(DirectoryInfo sourceDir, DirectoryInfo targetDir, bool overwrite = false)
        {
            foreach (var file in sourceDir.GetFiles())
            {
                file.CopyTo(Path.Combine(targetDir.FullName, file.Name), overwrite);
            }

            foreach (var subDir in sourceDir.GetDirectories())
            {
                var newTargetSubDir = targetDir.CreateSubdirectory(subDir.Name);
                CopyDirectory(subDir, newTargetSubDir, overwrite);
            }
        }



    }
}