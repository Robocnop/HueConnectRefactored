// Config.cs
using System;
using System.IO;
using Newtonsoft.Json;

namespace HueConnectRefactored
{
    public class Config
    {
        public string LoginWebhookUrl { get; set; }

        public static Config Load(string pluginFolder)
        {
            string configPath = Path.Combine(pluginFolder, "config.json");
            if (!File.Exists(configPath))
            {
                var defaultConfig = new Config
                {
                    LoginWebhookUrl = ""  // À renseigner manuellement après premier lancement
                };
                Directory.CreateDirectory(pluginFolder);
                File.WriteAllText(configPath, JsonConvert.SerializeObject(defaultConfig, Formatting.Indented));
            }
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
        }
    }
}
