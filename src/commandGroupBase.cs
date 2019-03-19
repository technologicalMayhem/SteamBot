using System;
using System.Collections.Generic;
using System.Linq;
using SteamKit2;

namespace technologicalMayhem.SteamBot
{
    public abstract class commandGroupBase : IChatCommand
    {
        public virtual ChatCommandProperties Properties { get; set; }
        public Dictionary<string, Type> subCommands = new Dictionary<string, Type>();

        public virtual void Execute(SteamID steamid, string[] parameters)
        {
            if (subCommands.Count > 0)
            {
                if (parameters.Length < 1)
                {
                    noParameters();
                }
                else
                {
                    try
                    {
                        var command = (IChatCommand)Activator.CreateInstance(subCommands[parameters[0]]);
                        command.Execute(steamid, parameters.Skip(1).ToArray());
                    }
                    catch (KeyNotFoundException)
                    {
                        notFound();
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Warning: {this.GetType()} has no suscommands despite being a command group.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public virtual void notFound()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Warning: {this.GetType()} has not implemented a notFound method.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public virtual void noParameters()
        {
        }
    }
}