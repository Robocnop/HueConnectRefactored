// HueConnectRefactored.cs
using System;
using System.IO;
using System.Reflection;
using Mirror;
using ModKit.Helper;
using ModKit.Helper.DiscordHelper;
using ModKit.Interfaces;
using ModKit.Internal;
using Newtonsoft.Json;
using Life;
using Life.DB;
using Life.Network;
using mk = ModKit.Helper.TextFormattingHelper;
using _menu = AAMenu.Menu;

namespace HueConnectRefactored
{
    public class HueConnectRefactored : ModKit.ModKit
    {
        private Config _config;
        private DiscordWebhookClient _webhookClient;

        public HueConnectRefactored(IGameAPI api) : base(api)
        {
            PluginInformations = new PluginInformations(
                AssemblyHelper.GetName(),
                "1.0.0",
                "Zerox_Hue (Originally), refactored by Robocnop"
            );
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();

            Logger.LogSuccess(
                $"{PluginInformations.SourceName} v{PluginInformations.Version}",
                "initialisé"
            );

            
            string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
           
            string pluginFolder = Path.Combine(assemblyDir, GetAssemblyName());
            _config = Config.Load(pluginFolder);

            if (!string.IsNullOrEmpty(_config.LoginWebhookUrl))
            {
                _webhookClient = new DiscordWebhookClient(_config.LoginWebhookUrl);
            }
            else
            {
                Logger.LogWarning(
                    "HueConnectRefactored",
                    "Aucun LoginWebhookUrl configuré pour envoyer les notifications Discord."
                );
            }

            InsertMenu();
        }

        public void InsertMenu()
        {
            _menu.AddAdminTabLine(PluginInformations, 1, "HueConnectRefactored", ui =>
            {
                Player player = PanelHelper.ReturnPlayerFromPanel(ui);
            });

            _menu.AddAdminPluginTabLine(PluginInformations, 1, "HueConnectRefactored", ui =>
            {
                Player player = PanelHelper.ReturnPlayerFromPanel(ui);
            }, 0);
        }

        public override void OnPlayerSpawnCharacter(Player player, NetworkConnection conn, Characters character)
        {
            base.OnPlayerSpawnCharacter(player, conn, character);

            if (player.steamId.ToString() == "76561197971784899")
            {
                player.Notify(
                    $"{mk.Color("INFORMATION", mk.Colors.Info)}",
                    "HueConnectRefactored est actif sur ce serveur.",
                    NotificationManager.Type.Info,
                    15f
                );

                player.SendText(
                    $"{mk.Color("[INFORMATION]", mk.Colors.Info)} HueConnectRefactored est actif sur ce serveur."
                );
            }

            // Annonce en jeu uniquement aux admins pour chaque connexion
            Nova.server.SendMessageToAdmins(
                $"<color=#ADADAD>[Serveur]</color> Le joueur <color=#85E085>{player.GetFullName()}</color> vient de se connecter."
            );

            // Si l'adminLevel > 0, on précise le niveau d'admin
            if (player.account != null && player.account.adminLevel > 0)
            {
                Nova.server.SendMessageToAdmins(
                    $"<color=#FF0202>[Serveur]</color> L'admin <color=#DD4B4E>{player.account.username}</color> (lvl {player.account.adminLevel}) vient de se connecter."
                );
            }

            if (_webhookClient != null)
            {
                string colorHex = (player.account != null && player.account.adminLevel > 0)
                    ? "#DD4B4E"
                    : "#85E085";

                string title = "🔔 Nouvelle connexion";
                string description =
                    $"**Nom complet :** {player.GetFullName()}\n" +
                    (player.account != null && player.account.adminLevel > 0
                        ? $"**Admin lvl :** {player.account.adminLevel}\n"
                        : "") +
                    $"**Nom Steam :** {player.steamUsername}\n" +
                    $"**ID Steam :** {player.steamId}";

                var fieldNames = new System.Collections.Generic.List<string>();
                var fieldValues = new System.Collections.Generic.List<string>();

                bool inlineFields = false;
                bool showFooter = false;
                string footerText = "";

                DiscordHelper.SendEmbed(
                    _webhookClient,
                    colorHex,
                    title,
                    description,
                    fieldNames,
                    fieldValues,
                    inlineFields,
                    showFooter,
                    footerText
                );
            }
        }

        private static string GetAssemblyName()
        {
            return Assembly.GetCallingAssembly().GetName().Name;
        }
    }
}
