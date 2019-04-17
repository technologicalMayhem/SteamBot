using System;
using SteamKit2;
using Newtonsoft.Json;
using System.Linq;
using System.Management.Automation;

namespace technologicalMayhem.SteamBot.Permissions
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
            SubCommands.Add("get", typeof(GetPermissions));
            SubCommands.Add("add", typeof(AddPermissions));
            SubCommands.Add("remove", typeof(RemovePermissions));
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
                if (parameters.Length > 0 && parameters[0] != null)
                {
                    var userSteamID = new SteamID(parameters[0]);
                    if (userSteamID.IsValid)
                    {
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
                    else
                    {
                        ChatManager.AddTask(steamid, $"{parameters[0]} is not a valid Steam ID.");
                    }
                }
                else
                {
                    ChatManager.AddTask(steamid, "Syntax Error. Usage: permissions get <steamid>");
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
                if (parameters.Length > 0 && parameters[0] != null)
                {
                    var user = new SteamID(parameters[0]);
                    if (user.IsValid)
                    {
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
                                Console.WriteLine(item);
                                permissions.Permissions.Add(item);
                            }
                        }
                    }
                    else
                    {
                        ChatManager.AddTask(steamid, $"{parameters[0]} is not a valid Steam ID.");
                    }
                }
                else
                {
                    ChatManager.AddTask(steamid, "Syntax Error. Usage: permissions add <steamid> <permission>");
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
                if (parameters.Length > 0 && parameters[0] != null)
                {
                    var user = new SteamID(parameters[0]);
                    if (user.IsValid)
                    {
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
                    else
                    {
                        ChatManager.AddTask(steamid, $"{parameters[0]} is not a valid Steam ID.");
                    }
                }
                else
                {
                    ChatManager.AddTask(steamid, "Syntax Error. Usage: permissions remove <steamid> <permission>");
                }
            }
        }
    }
}