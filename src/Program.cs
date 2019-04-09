using System;
using System.Collections;
using SteamKit2;
using SteamKit2.Internal;
using SteamAuth;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;

namespace technologicalMayhem.SteamBot
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Configuration.LoadConfig();
                UserManager.Start();
            }
            catch (ConfigurationException)
            {
                Console.WriteLine("Unable to configure application. Shutting down...");
                Environment.Exit(78);
            }
            CommandHandler.Start();
            //AddonManager.InitializeAddons();
            TaskManager.Start();
            ChatManager.Start();
            ConsoleManager.Start();
            while (!Globals.ReadyForShutdown.All(x => x))
            {

            }
            Environment.Exit(0);
        }
    }
}
