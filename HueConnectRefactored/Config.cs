using System;
using System.IO;
using ModKit.Internal;
using Newtonsoft.Json;

namespace HueConnectRefactored
{
    public class Config
    {
        public string ConfigVersion { get; set; } = "";
        public string LoginWebhookUrl { get; set; } = "";
        public bool CheckForUpdates { get; set; } = true;

        public static Config Load(string pluginFolder, string pluginVersion)
        {
            string path = Path.Combine(pluginFolder, "config.json");
            Config config = new Config();

            try
            {
                Directory.CreateDirectory(pluginFolder);

                if (File.Exists(path))
                {
                    JsonConvert.PopulateObject(File.ReadAllText(path), config);

                    if (config.ConfigVersion != pluginVersion)
                    {
                        string oldVersion = string.IsNullOrEmpty(config.ConfigVersion) ? "inconnue" : config.ConfigVersion;
                        Logger.LogWarning("HueConnect - Config", $"Config mise a jour : {oldVersion} -> {pluginVersion} (valeurs conservees).");
                    }
                    else
                    {
                        Logger.LogSuccess("HueConnect - Config", "Configuration chargee");
                    }
                }
                else
                {
                    Logger.LogWarning("HueConnect - Config", "Fichier config.json cree ! Configurez le webhook Discord.");
                }

                config.ConfigVersion = pluginVersion;
                File.WriteAllText(path, JsonConvert.SerializeObject(config, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.LogError("HueConnect - Config", ex.Message);
            }

            return config;
        }
    }
}
