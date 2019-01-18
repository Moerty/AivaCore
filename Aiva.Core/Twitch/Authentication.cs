using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Aiva.Core.Twitch {
    public class Authentication {
        private readonly HttpListener TwitchListener;
        private const string ReturnUrl = "http://localhost:56207";

        public Authentication() {
            TwitchListener = new HttpListener();
            TwitchListener.Prefixes.Add(ReturnUrl + "/");
        }

        /// <summary>
        /// Start the listener
        /// </summary>
        /// <returns>returns IsListening Value</returns>
        private bool StartListener() {
            try {
                TwitchListener.Start();
            } catch (HttpListenerException ex) {
                throw new Exception("Cant start listener for TwitchAuthentication" + Environment.NewLine + ex);
            }

            return TwitchListener.IsListening;
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        private void StopListener() {
            TwitchListener.Stop();
        }

        /// <summary>
        /// Get Twitch Auths
        /// </summary>
        /// <returns></returns>
        public async Task<Models.Twitch.AuthenticationModel> GetAuthenticationValuesAsync() {
            StartListener();

            Models.Twitch.AuthenticationModel Values = null;
            while (TwitchListener.IsListening) {
                var context = await TwitchListener.GetContextAsync();

                if (context.Request.QueryString.HasKeys()) {
                    if (context.Request.RawUrl.Contains("access_token")) {
                        Uri myUri = new Uri(context.Request.Url.OriginalString);
                        string scope = HttpUtility.ParseQueryString(myUri.Query).Get("scope");
                        string access_token = HttpUtility.ParseQueryString(myUri.Query).Get(0).Replace("access_token=", "");

                        if (!String.IsNullOrEmpty(scope) && !String.IsNullOrEmpty(access_token)) {
                            Values = GetModel(access_token, scope);
                        }
                    }
                }

                byte[] b = Encoding.UTF8.GetBytes(GetResponse());
                context.Response.StatusCode = 200;
                context.Response.KeepAlive = false;
                context.Response.ContentLength64 = b.Length;

                var output = context.Response.OutputStream;
                output.Write(b, 0, b.Length);
                context.Response.Close();

                if (Values != null) {
                    StopListener();
                    return Values;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates the Response for TwitchOAuth
        /// </summary>
        /// <returns>Response</returns>
        private string GetResponse() {
            StringBuilder builder = new StringBuilder();

            builder.Append("<html>");
            builder.Append(Environment.NewLine);
            builder.Append("<head>");
            builder.Append(Environment.NewLine);
            builder.Append("<title>AivaBot Twitch Oauth</title>");
            builder.Append(Environment.NewLine);
            builder.Append("<script language=\"JavaScript\">");
            builder.Append(Environment.NewLine);
            builder.Append("if(window.location.hash) {");
            builder.Append(Environment.NewLine);
            builder.Append("window.location.href = window.location.href.replace(\"/#\",\"?=\");");
            builder.Append(Environment.NewLine);
            builder.Append("}");
            builder.Append(Environment.NewLine);
            builder.Append("</script>");
            builder.Append(Environment.NewLine);
            builder.Append("</head>");
            builder.Append(Environment.NewLine);
            builder.Append("<body>You can close this tab</body>");
            builder.Append(Environment.NewLine);
            builder.Append("</html>");

            return builder.ToString();
        }

        /// <summary>
        /// Creates the Model to return
        /// </summary>
        /// <param name="token">Twitch Token</param>
        /// <param name="scopes">Twitch Scopes</param>
        /// <returns></returns>
        private Models.Twitch.AuthenticationModel GetModel(string token, string scopes) {
            return new Models.Twitch.AuthenticationModel {
                Token = token,
                Scopes = scopes
            };
        }

        /// <summary>
        /// Starts the Request for the User to authorize for twitch
        /// </summary>
        /// <param name="ClientID"></param>
        public void SendRequestToBrowser(string ClientID) {
            Thread.Sleep(500);

            var url = GetUrl(ClientID);

            // https://stackoverflow.com/questions/38576854/how-do-i-launch-the-web-browser-after-starting-my-asp-net-core-application
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                Process.Start(new ProcessStartInfo("cmd", $"/c start \"\" \"{url}\"")); // Works ok on windows
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                Process.Start("xdg-open", url);  // Works ok on linux
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                Process.Start("open", url); // Not tested
            } else {
                throw new Exception("Operating System not found");
            }
        }

        /// <summary>
        /// Returns the URL which we call to create a oauth token
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        private static string GetUrl(string ClientID) {
            var sb = new StringBuilder();
            sb.Append("https://api.twitch.tv/kraken/oauth2/authorize");
            sb.Append("?response_type=token");
            sb.Append("&client_id=").Append(ClientID);
            sb.Append("&redirect_uri=").Append(ReturnUrl);
            sb.Append("&scope=user_read+user_blocks_edit+user_blocks_read+user_follows_edit+channel_read+channel_editor+channel_commercial+channel_stream+channel_subscriptions+user_subscriptions+channel_check_subscription+chat_login+channel_feed_read+channel_feed_edit");
            return sb.ToString();
        }
    }
}

