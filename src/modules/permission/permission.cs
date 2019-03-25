using System;
using SteamKit2;
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
            //TODO: Implement Stuff
            throw new NotImplementedException();
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