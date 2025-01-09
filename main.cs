using Life;
using Life.DB;
using Life.Network;
using Mirror;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Profiling.LowLevel.Unsafe;

namespace HueConnect
{
    public class HueConnect : HueHelper.AllHelper
    {
        public HueConnect(IGameAPI aPI) : base(aPI) { }

        public static string SuccessColors = "#85E085";
        public static string ErrorColors = "#DD4B4E";
        public static string WarningColors = "#FCBE86";
        public static string InfoColors = "#4287F9";
        public static string GreyColors = "#ADADAD";
        public static string PurpleColors = "#DB70DB";

        public Config config;

        public class Config
        {
            public int LevelAdminRequired;

            public bool ServiceAdminIsRequired;

            public string WebhookLogsURL;
        }
        public void CreateConfig()
        {
            string directoryPath = pluginsPath + "/HueConnect";

            string configFilePath = directoryPath + "/config.json";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(configFilePath))
            {
                var defaultConfig = new Config
                {
                    LevelAdminRequired = 1,

                    ServiceAdminIsRequired = true,

                    WebhookLogsURL = "",

                };
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(defaultConfig, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(configFilePath, jsonContent);
            }

            config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            CreateConfig();
            InitHelper.InitMessage("V.2.0.0", "Zerox_Hue");
        }
        public override async void OnPlayerSpawnCharacter(Player player, NetworkConnection conn, Characters character)
        {
            base.OnPlayerSpawnCharacter(player, conn, character);
            foreach(var players in Nova.server.Players)
            {
                if(config.ServiceAdminIsRequired)
                {
                    if(players.serviceAdmin)
                    {
                        if(players.account.adminLevel >= config.LevelAdminRequired)
                        {

                            System.Random rdm = new System.Random();

                            int latence = rdm.Next(500, 1000);

                            await Task.Delay(latence);

                            players.SendText($"<color={ErrorColors}>[HueConnect]</color> Connexion d'un joueur : \n" +
                                $"<color={GreyColors}>Nom et prénom :</color><color={SuccessColors}> <b>{player.GetFullName()}</b></color> \n" +
                                $"<color={GreyColors}>Nom Steam :</color><color={SuccessColors}> <b>{player.steamUsername}</b></color> \n" +
                                $"<color={GreyColors}>Id Steam :</color><color={SuccessColors}> <b>{player.steamId}</b></color>");

                            DiscordHelper.EmbedDiscord(config.WebhookLogsURL, "Connexion", "# :construction_worker: Connexion d'un joueur : \n" +
                                $"Nom et prénom : {player.GetFullName()} \n" +
                                $"Nom Steam : {player.steamUsername} \n" +
                                $"Id Steam : {player.steamId}", ErrorColors);
                        }
                    }
                   
                }
                else
                {
                    if (players.account.adminLevel >= config.LevelAdminRequired)
                    {
                        System.Random rdm = new System.Random();

                        int latence = rdm.Next(500, 1000);

                        await Task.Delay(latence);

                        

                        players.SendText($"<color={ErrorColors}>[HueConnect]</color> Connexion d'un joueur : \n" +
                            $"<color={GreyColors}>Nom et prénom :</color><color={SuccessColors}> <b>{player.GetFullName()}</b></color> \n" +
                            $"<color={GreyColors}>Nom Steam :</color><color={SuccessColors}> <b>{player.steamUsername}</b></color> \n" +
                            $"<color={GreyColors}>Id Steam :</color><color={SuccessColors}> <b>{player.steamId}</b></color>");

                        DiscordHelper.EmbedDiscord(config.WebhookLogsURL, "Connexion", "# :construction_worker: Connexion d'un joueur : \n" +
                            $"Nom et prénom : {player.GetFullName()} \n" +
                            $"Nom Steam : {player.steamUsername}`\n" +
                            $"Id Steam : {player.steamId}" , ErrorColors);
                    }

                }

            }
        }
    }
}
