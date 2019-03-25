using System;
using System.Collections.Generic;
using SteamKit2;
using Newtonsoft.Json;
using technologicalMayhem.SteamBot;

namespace technologicalMayhem.SteamBot
{
    public static class Permissions
    {
        [PluginInitializer]
        public static void Initialize()
        {
            CommandHandler.CommandReceived += CheckPermissions;
        }

        public static void CheckPermissions(ref CommandHandler.OnCommandReceivedEventArgs e)
        {
            var userData = UserManager.GetUserData(e.steamID);
            UserPermissions permissions;
            try
            {
                permissions = JsonConvert.DeserializeObject<UserPermissions>(userData["permissions"]);
            }
            catch (KeyNotFoundException)
            {
                permissions = new UserPermissions();
            }
        }

        class UserPermissions
        {
            public int rank;
            public List<string> permissions;

            public UserPermissions()
            {
                rank = 0;
                permissions = new List<string>();
            }
        }
    }

    public class InsufficientPermissions : IChatCommand
    {
        public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
        {

        };

        public void Execute(SteamID steamid, string[] parameters)
        {
            ChatManager.AddTask(steamid, "Nicht genug Rechte.");
        }
    }
}