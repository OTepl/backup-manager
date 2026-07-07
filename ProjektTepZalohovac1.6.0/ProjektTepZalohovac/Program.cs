using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
namespace ProjektTepZalohovac
{
    
    
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string x = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var configs = JsonConfigManager.LoadConfigs(x + "\\config.json");

          var timeManager = new TimeManager();

            foreach (var config in configs)
            {
              var backupManager = new BackupManager(config);
              await timeManager.InitializeScheduler(config, backupManager.RunBackup);
            }
          await Task.Delay(-1);
        }
    }
}
