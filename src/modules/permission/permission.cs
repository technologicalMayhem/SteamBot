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

        [AddonInitializer]
        public static void Initialize()
        {
            CommandHandler.CommandReceived += CheckPermissions;
        }

        static void CheckPermissions(ref CommandHandler.OnCommandReceivedEventArgs e)
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

        public static UserPermissions GetUserPermissions(SteamID steamid)
        {
            var userData = UserManager.GetUserData(steamid);
            if (userData.ContainsKey("permission"))
            {
                return JsonConvert.DeserializeObject<UserPermissions>(userData["permission"]);
            }
            else
            {
                return new UserPermissions();
            }
        }

        public static void SetUserPermissions(UserPermissions permissions, SteamID steamid)
        {
            UserManager.GetUserData(steamid)["permission"] = JsonConvert.SerializeObject(permissions);
        }

        public static string[] GetAllPermissions()
        {
            var commands = new List<string>();
            foreach (var c in CommandHandler.Commands)
            {
                commands.Add(c.type.FullName);
            }
            return commands.ToArray();
        }

    }

    public class Rank
    {
        public int RankID { get; set; }
        public List<string> Permissions { get; set; }
    }

    public class UserPermissions
    {
        public int Rank { get; set; }
        public List<string> Permissions { get; set; }

        public UserPermissions()
        {
            Rank = 0;
            Permissions = new List<string>();
        }

        public string SaveChanges()
        {
            return JsonConvert.SerializeObject(this);
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