using System;
using System.IO;
using System.Threading.Tasks;

namespace Aiva.Console {
    class Program {
        static async Task Main(string[] args) {
            //Console.WriteLine("Hello World!");

            if(!File.Exists(Core.Config.ConfigHandler.GetConfigPath())) {
                string clientId;
                do {
                    System.Console.WriteLine("Enter your ClientID: ");
                    clientId = System.Console.ReadLine();
                } while (string.IsNullOrEmpty(clientId));



                var auth = new Core.Twitch.Authentication();
                auth.SendRequestToBrowser(clientId);

                var result = await auth.GetAuthenticationValuesAsync().ConfigureAwait(false);

                var isValid = await Aiva.Core.Twitch.AivaClient.CheckBotuser(clientId, result.Token);

                if (isValid) {
                    new Core.Config.ConfigHandler(clientId, result.Token, "aivabot", "aeffchaen");
                } else {
                    throw new Exception("Failed to check twitch credentials");
                }
            }

            Core.Boot.Main();

            System.Console.WriteLine("Press crtl + c to quit");
            while (System.Console.ReadKey() != new ConsoleKeyInfo('c', ConsoleKey.C, false, false, true));
        }
    }
}
