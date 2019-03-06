using System;
using System.IO;
using System.Collections;
using Newtonsoft.Json;

namespace technologicalMayhem.SteamBot
{
    public static class Configuration
    {
        /// <summary>
        /// Loads and assigns all values from config.cfg
        /// </summary>
        public static void LoadConfig()
        {
            if (File.Exists("config.cfg"))
            {
                var config = JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText("config.cfg"));
                Globals.Username = config.username;
                Globals.Password = config.password;
            }
            else
            {
                SaveConfig();
                LoadConfig();
            }
        }
        /// <summary>
        /// Saves current configuration to config.cfg
        /// </summary>
        public static void SaveConfig()
        {
            var config = new ConfigFile(){
                username = Globals.Username,
                password = Globals.Password
            };
            File.WriteAllText("config.cfg", JsonConvert.SerializeObject(config));
        }
        /// <summary>
        /// Simple struct to represent the config file
        /// </summary>
        struct ConfigFile
        {
            public string username;
            public string password;
        }
    }
}