using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ModKit.Internal;
using Newtonsoft.Json.Linq;

namespace HueConnectRefactored
{
    public static class UpdateChecker
    {
        private const string LatestReleaseApi = "https://api.github.com/repos/Robocnop/HueConnectRefactored/releases/latest";
        private const string ReleasesUrl = "https://github.com/Robocnop/HueConnectRefactored/releases";

        public static async Task CheckAsync(string currentVersion)
        {
            try
            {
                // Certains runtimes Mono anciens n'activent pas TLS 1.2 par defaut (requis par GitHub)
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    // L'API GitHub rejette les requetes sans User-Agent
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("HueConnectRefactored");

                    string json = await client.GetStringAsync(LatestReleaseApi);
                    string tag = JObject.Parse(json)["tag_name"]?.ToString();

                    if (!TryParseVersion(tag, out Version latest) || !TryParseVersion(currentVersion, out Version current))
                        return;

                    if (latest > current)
                        Logger.LogWarning("HueConnect - Update", $"⚠ Version {tag} disponible (vous utilisez v{currentVersion}). Telechargement : {ReleasesUrl}");
                    else
                        Logger.LogSuccess("HueConnect - Update", $"Plugin a jour (v{currentVersion}).");
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning("HueConnect - Update", $"Verification de mise a jour impossible : {ex.Message}");
            }
        }

        private static bool TryParseVersion(string raw, out Version version)
        {
            version = null;
            if (string.IsNullOrWhiteSpace(raw)) return false;

            raw = raw.Trim().TrimStart('v', 'V');
            if (raw.IndexOf('.') < 0) raw += ".0";

            return Version.TryParse(raw, out version);
        }
    }
}
