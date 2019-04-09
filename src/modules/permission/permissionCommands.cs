using System;
using SteamKit2;
using Newtonsoft.Json;
using System.Linq;
using System.Management.Automation;

namespace technologicalMayhem.SteamBot
{
    public class permCommand : CommandGroupBase
    {
        public permCommand()
        {
            Properties = new ChatCommandProperties()
            {
                ShowInHelp = true,
                Command = "permissions",
                CommandAlias = new string[] { "perm" },
                CommandSyntax = "permissions <subcommand> [parameter]",
            };
        }

        public class GetPermissions : IChatCommand
        {
            public ChatCommandProperties Properties
            {
                get
                {
                    return new ChatCommandProperties()
                    {
                        Command = "get"
                    };
                }
            }

            public void Execute(SteamID steamid, string[] parameters)
            {
                var userSteamID = new SteamID(parameters[0]);
                var permissions = Permissions.GetUserPermissions(userSteamID);
                if (permissions.Permissions.Count >= 1)
                {
                    var url = PastebinAPI.Paste.CreateAsync(String.Join("\n", permissions.Permissions), "Permissions for " + parameters[0]).Result.Url;
                    ChatManager.AddTask(steamid, url);
                }
                else
                {
                    ChatManager.AddTask(steamid, $"{steamid} has no permissions to return.");
                }
            }
        }

        public class AddPermissions : IChatCommand
        {
            public ChatCommandProperties Properties
            {
                get
                {
                    return new ChatCommandProperties()
                    {
                        Command = "add"
                    };
                }
            }

            public void Execute(SteamID steamid, string[] parameters)
            {
                var user = new SteamID(parameters[0]);
                var permissions = Permissions.GetUserPermissions(steamid);
                var pattern = new WildcardPattern(parameters[1]);
                var newPermissions = Permissions.GetAllPermissions().ToList();
                //Filter the permission list
                var filtered = newPermissions.FindAll(x => pattern.IsMatch(x));
                //Add new permissions to the list
                foreach (var item in filtered)
                {
                    if (!permissions.Permissions.Contains(item))
                    {
                        permissions.Permissions.Add(item);
                    }
                }
            }
        }

        public class RemovePermissions : IChatCommand
        {
            public ChatCommandProperties Properties
            {
                get
                {
                    return new ChatCommandProperties()
                    {
                        Command = "remove",
                        CommandAlias = new string[] { "rm" }
                    };
                }
            }

            public void Execute(SteamID steamid, string[] parameters)
            {
                var user = new SteamID(parameters[0]);
                var permissions = Permissions.GetUserPermissions(steamid);
                var pattern = new WildcardPattern(parameters[1]);
                var newPermissions = Permissions.GetAllPermissions().ToList();
                //Filter the permission list
                var filtered = newPermissions.FindAll(x => pattern.IsMatch(x));
                //Add new permissions to the list
                foreach (var item in filtered)
                {
                    if (permissions.Permissions.Contains(item))
                    {
                        permissions.Permissions.Remove(item);
                    }
                }
            }
        }
    }
}