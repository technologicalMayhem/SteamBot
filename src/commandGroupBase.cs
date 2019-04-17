using System;
using System.Collections.Generic;
using System.Linq;
using SteamKit2;

namespace technologicalMayhem.SteamBot
{
    public abstract class CommandGroupBase : IChatCommand
    {
        public virtual ChatCommandProperties Properties { get; set; }
        public Dictionary<string, Type> SubCommands { get => subCommands; set => subCommands = value; }
        private Dictionary<string, Type> subCommands = new Dictionary<string, Type>();

        public virtual void Execute(SteamID steamid, string[] parameters)
        {
            if (SubCommands.Count > 0)
            {
                if (parameters.Length < 1)
                {
                    NoParameters();
                }
                else
                {
                    try
                    {
                        var command = (IChatCommand)Activator.CreateInstance(SubCommands[parameters[0]]);
                        CommandHandler.AddPreparedTask(steamid, parameters.Skip(1).ToArray(), command);
                    }
                    catch (KeyNotFoundException)
                    {
                        NotFound();
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

        public virtual void NotFound()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Warning: {this.GetType()} has not implemented a notFound method.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public virtual void NoParameters()
        {
        }
    }
}