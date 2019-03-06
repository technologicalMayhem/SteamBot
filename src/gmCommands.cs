using System;
using System.Collections.Generic;
using technologicalMayhem.SteamBot;
using SteamKit2;
using System.Linq;

namespace technologicalMayhem.SteamBot
{
    public class gmCommand : IChatCommand
    {
        public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
        {
            ShowInHelp = true,
            Command = "gmod",
            CommandSyntax = "gmod <subcommand> [parameter]"
        };

        public gmCommand()
        {
            var sub = new List<IChatCommand>();
            this.GetType().GetNestedTypes().ToList().ForEach(x => sub.Add((IChatCommand)Activator.CreateInstance(x)));
            Properties.Subcommands = sub.ToArray();
        }
        public void Execute(SteamID steamid, string[] parameters)
        {
            var command = Properties.Subcommands.FirstOrDefault(x => x.Properties.Command == parameters[0] || x.Properties.CommandAlias.Contains(parameters[0]));
            if (command != null)
            {
                command.Execute(steamid, parameters.Skip(1).ToArray());
            }
            else
            {
                ChatManager.AddTask(steamid, "Unbekannter Befehl. Benutze 'gmod help' für eine Liste aller Befehle von gmod oder 'help' für eine Liste aller Befehle.");
            }
        }

        public class Addons : IChatCommand
        {
            public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
            {
                ShowInHelp = false,
                Command = "addons",
                CommandSyntax = "gmod addons <subcommand> [parameter]",
                CommandDescription = "Stellt Befehle zu managen der Addons auf dem GMod Server bereit"
            };

            public Addons()
            {
                var sub = new List<IChatCommand>();
                this.GetType().GetNestedTypes().ToList().ForEach(x => sub.Add((IChatCommand)Activator.CreateInstance(x)));
                Properties.Subcommands = sub.ToArray();
            }

            public void Execute(SteamID steamid, string[] parameters)
            {
                var command = Properties.Subcommands.FirstOrDefault(x => x.Properties.Command == parameters[0] || x.Properties.CommandAlias.Contains(parameters[0]));
                if (command != null)
                {
                    command.Execute(steamid, parameters.Skip(1).ToArray());
                }
                else
                {
                    ChatManager.AddTask(steamid, "Unbekannter Befehl. Benutze 'gmod addons help' für eine Befehlsliste von 'gmod addons' oder 'help' für eine Liste aller Befehle.");
                }
            }

            public class Alert : IChatCommand
            {
                public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
                {
                    ShowInHelp = false,
                    Command = "alert",
                    CommandSyntax = "gmod addons alert [on|off]",
                    CommandDescription = "Zeigt den aktuellen Status des Addon Alarms oder legt ihn fest."
                };

                public void Execute(SteamID steamid, string[] parameters)
                {
                    
                }
            }
        }


        //Maybe not possible, due to how the chat works.
        public class Join : IChatCommand
        {
            public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
            {
                ShowInHelp = false,
                Command = "join",
                CommandSyntax = "gmod join",
                CommandDescription = "Zeigt den Steam-Verbindungs-Link für den TTT Server"
            };

            public void Execute(SteamID steamid, string[] parameters)
            {

            }
        }
    }

}
