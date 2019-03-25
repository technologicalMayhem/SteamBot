using System;
using SteamKit2;
using technologicalMayhem.SteamBot;

namespace technologicalMayhem.SteamBot
{
    public static class Permissions
    {
        [PluginInitializer]
        static void Initialize()
        {
            CommandHandler.CommandReceived += CheckPermissions;
        }

        public static void CheckPermissions(CommandHandler.OnCommandReceivedEventArgs e)
        {
            Console.WriteLine($"The command being executed is: {e.command.GetType()}. Replacing it now!");
            e.command = new InsufficientPermissions();
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