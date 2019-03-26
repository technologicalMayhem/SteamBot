using System;
using System.Collections.Generic;
using SteamKit2;
using Newtonsoft.Json;
using technologicalMayhem.SteamBot;

namespace technologicalMayhem.SteamBot
{
    public static class Permissions
    {
        static List<Rank> ranks;

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
            if (!permissions.Permissions.Contains(e.command.GetType().ToString()))
            {
                e.command = new InsufficientPermissions();
            }
        }

        class UserPermissions
        {
            public int Rank {get; set;}
            public List<string> Permissions {get; set;}

            public UserPermissions()
            {
                Rank = 0;
                Permissions = new List<string>();
            }
        }

        class Rank
        {
            public int RankID {get; set;}
            public List<string> Permissions {get; set;}
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