using System;
using System.Collections.Generic;
using technologicalMayhem.SteamBot;
using SteamKit2;
using System.Linq;

namespace technologicalMayhem.SteamBot.gmCommands
{
    public class gmCommand : CommandGroupBase
    {
        public gmCommand()
        {
            Properties = new ChatCommandProperties()
            {
                ShowInHelp = true,
                Command = "gmod",
                CommandSyntax = "gmod <subcommand> [parameter]"
            };
            SubCommands.Add("addons", typeof(Addons));
        }

        public class Addons : CommandGroupBase
        {
            public Addons()
            {
                Properties = new ChatCommandProperties()
                {
                    CommandSyntax = "gmod addons <subcommand> [parameter]",
                    CommandDescription = "Stellt Befehle zu managen der Addons auf dem GMod Server bereit"
                };
                SubCommands.Add("alert", typeof(Alert));
            }

            public class Alert : IChatCommand
            {
                public ChatCommandProperties Properties { get; } = new ChatCommandProperties()
                {
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
