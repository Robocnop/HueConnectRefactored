using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Life.Network;
using ModKit.Helper.DiscordHelper;
using ModKit.Internal;

namespace HueConnectRefactored
{
    public class DiscordLogger
    {
        private readonly string _url;
        private readonly string _version;

        public DiscordLogger(string url, string version)
        {
            _url = url;
            _version = version;
        }

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(_url) && _url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);

        public async Task LogConnection(Player player)
        {
            if (!IsConfigured || player == null) return;

            try
            {
                int adminLevel = player.account?.adminLevel ?? 0;
                string steamId = player.steamId.ToString();

                List<string> names = new List<string> { "🎭 Nom RP", "💤 Compte", "🎮 Nom Steam", "💾 SteamID", "🔗 Profil" };
                List<string> values = new List<string>
                {
                    player.FullName,
                    player.account?.username ?? "Inconnu",
                    player.steamUsername,
                    steamId,
                    $"[Cliquez ici](https://steamcommunity.com/profiles/{steamId})"
                };

                if (adminLevel > 0)
                {
                    names.Add("🛡️ Admin");
                    values.Add($"Niveau {adminLevel}");
                }

                await DiscordHelper.SendEmbed(
                    new DiscordWebhookClient(_url),
                    adminLevel > 0 ? "#DD4B4E" : "#85E085",
                    adminLevel > 0 ? "🔔 Connexion d'un admin" : "🔔 Nouvelle connexion",
                    $"Log HueConnectRefactored v{_version}",
                    names,
                    values,
                    false,
                    true,
                    $"Zerox_Hue, refait par Robocnop • {DateTime.Now:HH:mm}"
                );
            }
            catch (Exception ex)
            {
                Logger.LogError("HueConnect - Discord Webhook", ex.Message);
            }
        }
    }
}
