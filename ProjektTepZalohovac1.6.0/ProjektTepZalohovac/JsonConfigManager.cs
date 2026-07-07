using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ProjektTepZalohovac
{
    public class JsonConfigManager
    {
        public static List<JsonBackupJobFileConfig> LoadConfigs(string configFilePath)
        {
            var info = new FileInfo(configFilePath);
            if (!info.Exists)
                throw new FileNotFoundException($"Config file not found: {configFilePath}");

            try
            {
                var content = File.ReadAllText(info.FullName);
                var configs = JsonConvert.DeserializeObject<List<JsonBackupJobFileConfig>>(content);

                if (configs == null || configs.Count == 0)
                    throw new Exception("No valid configurations found in the file.");

                foreach (var config in configs)
                {
                    if (config.Sources == null || !config.Sources.All(Directory.Exists))
                        throw new Exception("Invalid or missing 'Sources' in configuration.");

                    if (config.Targets == null || !config.Targets.All(Directory.Exists))
                        throw new Exception("Invalid or missing 'Targets' in configuration.");

                    if (string.IsNullOrEmpty(config.Timing))
                        throw new Exception("Invalid or missing 'Timing' (Cron expression) in configuration.");
                }

                return configs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the backup configurations.", ex);
            }
        }
    }
}