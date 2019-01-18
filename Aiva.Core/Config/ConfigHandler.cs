using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aiva.Core.Config {
    public class ConfigHandler {
        public static Models.Config.Model.Root Config { get; private set; }

        public ConfigHandler() {
            if(File.Exists(GetConfigPath())) {
                Config = Models.Config.Model.Root.FromJson(
                    File.ReadAllText(GetConfigPath()));
            } else {
                LoadDefaultConfigFile();

                Config = Models.Config.Model.Root.FromJson(
                    File.ReadAllText(GetConfigPath()));
            }
        }

        /// <summary>
        /// Move default config file to "real" file
        /// </summary>
        private void LoadDefaultConfigFile()
            => File.Move(GetSampleConfigPath(), GetConfigPath());

        /// <summary>
        /// Save config to disc
        /// </summary>
        public void SaveConfig() {
            var json = JsonConvert.SerializeObject(Config);
            File.WriteAllText(GetConfigPath(), json);
        }

        /// <summary>
        /// Get sample Config Path
        /// </summary>
        /// <returns></returns>
        public static string GetSampleConfigPath() {
            return Path.Combine(
                AppContext.BaseDirectory,
                "Config",
                "config.json.default");
        }

        /// <summary>
        /// Get Config Path
        /// </summary>
        /// <returns></returns>
        public static string GetConfigPath() {
            return Path.Combine(
                AppContext.BaseDirectory,
                "Config",
                "config.json");
        }
    }
}
