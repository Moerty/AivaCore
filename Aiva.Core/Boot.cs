using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Core {
    public static class Boot {
        public static void Main() {
            CheckIfDatabaseExists();
            var config = new ConfigHandler();
            var twitchClient = new Twitch.AivaClient();
            var twitchTasks = new Twitch.Functions.EventListener();
            twitchTasks.SetEvents();
        }

        private static void CheckIfDatabaseExists() {
            using (var context = new Database.Context()) {
                context.Database.EnsureCreated();
            }
        }
    }
}
