using BytexDigital.Steam.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FASTER.Models
{
    public static class SteamWebApi
    {
        private const string V2 = "&steamids=";
        private const string V3 = "&publishedfileids[0]=";


        // Get mod info for single mod
        public static async Task<JObject> GetSingleFileDetailsAsync(uint modId)
        {
            try
            {
                var response = await ApiCallAsync("https://api.steampowered.com/IPublishedFileService/GetDetails/v1?key=" + GetApiKey() + V3 + modId);
                return (JObject)response?.SelectToken("response.publishedfiledetails[0]");
            }
            catch (Exception ex)
            {
                if (Environment.UserInteractive)
                {
                    MessageBox.Show("Cannot reach Steam API \n\nCheck https://steamstat.us/", $"Steam API Error:'{ex.Message}'", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    Console.WriteLine($"Cannot reach Steam API \n\nCheck https://steamstat.us/! Steam API Error:'{ex.Message}'");
                }
                return null;
            }
        }


        // Gets user info
        public static async Task<JObject> GetPlayerSummariesAsync(string playerId)
        {
            try
            {
                var response = await ApiCallAsync("https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v1?key=" + GetApiKey() + V2 + playerId);
                return (JObject)response?.SelectToken("response.players.player[0]");
            }
            catch (Exception ex)
            {
                if (Environment.UserInteractive)
                {
                    MessageBox.Show("Cannot reach Steam API \n\nCheck https://steamstat.us/", $"Steam API Error:'{ex.Message}'", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    Console.WriteLine($"Cannot reach Steam API \n\nCheck https://steamstat.us/! Steam API Error:'{ex.Message}'");
                }
                return null;
            }
        }


        /// <summary>
        /// Calls to Steam API Endpoint and returns the result as JSON Object.
        /// </summary>
        /// <param name="uri">Url</param>
        /// <returns>Json object</returns>
        /// <exception cref="ArgumentException"></exception>
        private static async Task<JObject> ApiCallAsync(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException($"'{nameof(uri)}' cannot be null or empty.", nameof(uri));
            }

            try
            {
                using HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(100);

                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JObject.Parse(content);
                }
                else if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(5000);
                    return await ApiCallAsync(uri); // Retry the API call after the delay
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while making the API call.", ex);
            }
        }

        private static string GetApiKey()
        {
            return !string.IsNullOrEmpty(Properties.Settings.Default.SteamAPIKey)
                ? Properties.Settings.Default.SteamAPIKey
                : StaticData.SteamApiKey;
        }
    }

    internal class AuthCodeProvider : SteamAuthenticationCodesProvider
    {
        public override string GetEmailAuthenticationCode(SteamCredentials steamCredentials)
        {
            MainWindow.Instance.SteamUpdaterViewModel.Parameters.Output += "\nPlease enter your email auth code: ";


            var input = MainWindow.Instance.SteamUpdaterViewModel.SteamGuardInput().Result;

            MainWindow.Instance.SteamUpdaterViewModel.Parameters.Output += "\nRetrying... ";

            return input;
        }

        public override string GetTwoFactorAuthenticationCode(SteamCredentials steamCredentials)
        {
            MainWindow.Instance.SteamUpdaterViewModel.Parameters.Output += "\nPlease enter your 2FA code: ";

            var input = MainWindow.Instance.SteamUpdaterViewModel.SteamGuardInput().Result;

            MainWindow.Instance.SteamUpdaterViewModel.Parameters.Output += "\nRetrying... ";

            return input;
        }
    }
}
