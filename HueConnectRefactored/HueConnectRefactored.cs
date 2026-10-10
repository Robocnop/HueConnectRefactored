using System;
using System.Collections.Concurrent;
using System.IO;
using Life;
using Life.DB;
using Life.Network;
using Mirror;
using ModKit.Helper;
using ModKit.Interfaces;
using ModKit.Internal;
using mk = ModKit.Helper.TextFormattingHelper;
using _menu = AAMenu.Menu;

namespace HueConnectRefactored
{
    public class HueConnectRefactored : ModKit.ModKit
    {
        private const string Version = "1.1.0";
        private const ulong AuthorSteamId = 76561197971784899;

        private readonly ConcurrentDictionary<int, byte> _announced = new ConcurrentDictionary<int, byte>();
        private Config _config;
        private DiscordLogger _discord;

        public HueConnectRefactored(IGameAPI api) : base(api)
        {
            PluginInformations = new PluginInformations(AssemblyHelper.GetName(), Version, "Zerox_Hue (Originally), refactored by Robocnop");
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            Logger.LogSuccess("HueConnect - Demarrage", $"v{Version} initialise");

            _config = Config.Load(Path.Combine(pluginsPath, AssemblyHelper.GetName()), Version);
            _discord = new DiscordLogger(_config.LoginWebhookUrl, Version);

            if (!_discord.IsConfigured)
                Logger.LogWarning("HueConnect - Config", "Aucun LoginWebhookUrl configure, les logs Discord sont desactives.");

            if (_config.CheckForUpdates)
                _ = UpdateChecker.CheckAsync(Version);

            _menu.AddAdminTabLine(PluginInformations, 1, "HueConnectRefactored", ui => PanelHelper.ReturnPlayerFromPanel(ui));
            _menu.AddAdminPluginTabLine(PluginInformations, 1, "HueConnectRefactored", ui => PanelHelper.ReturnPlayerFromPanel(ui), 0);
        }

        public override async void OnPlayerSpawnCharacter(Player player, NetworkConnection conn, Characters character)
        {
            base.OnPlayerSpawnCharacter(player, conn, character);

            try
            {
                // Appele aussi a chaque respawn : on n'annonce qu'une fois par connexion
                int? id = player?.setup?.character?.Id;
                if (id == null || !_announced.TryAdd(id.Value, 0)) return;

                if (player.steamId == AuthorSteamId)
                {
                    player.Notify(mk.Color("INFORMATION", mk.Colors.Info), "HueConnectRefactored est actif sur ce serveur.", NotificationManager.Type.Info, 15f);
                    player.SendText($"{mk.Color("[INFORMATION]", mk.Colors.Info)} HueConnectRefactored est actif sur ce serveur.");
                }

                Nova.server.SendMessageToAdmins($"<color=#ADADAD>[Serveur]</color> Le joueur <color=#85E085>{player.FullName}</color> vient de se connecter.");

                if (player.account?.adminLevel > 0)
                    Nova.server.SendMessageToAdmins($"<color=#FF0202>[Serveur]</color> L'admin <color=#DD4B4E>{player.account.username}</color> (lvl {player.account.adminLevel}) vient de se connecter.");

                await _discord.LogConnection(player);
            }
            catch (Exception ex)
            {
                Logger.LogError("HueConnect - OnPlayerSpawnCharacter", ex.Message);
            }
        }

        public override void OnPlayerDisconnect(NetworkConnection conn)
        {
            base.OnPlayerDisconnect(conn);

            int? id = Nova.server.GetPlayer(conn)?.setup?.character?.Id;
            if (id != null)
                _announced.TryRemove(id.Value, out _);
        }
    }
}
