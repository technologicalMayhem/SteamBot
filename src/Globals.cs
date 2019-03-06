using System;
using SteamAuth;

namespace technologicalMayhem.SteamBot
{
    public static class Globals
    {
        /// <summary>
        /// If this is set to false the application will start to shut down.
        /// </summary>
        public static bool IsRunning = true;
        /// <summary>
        /// <c>ReadyForShutdown</c> checks whether or not all parts of the prorgram are ready for a proper shutdown. The array has multiple booleans, each representing a certain part of the program.
        /// <para>0 - Task Manager</para> 
        /// <para>1 - Chat Manager</para> 
        /// </summary>
        public static bool[] ReadyForShutdown = new bool[]{false,false};
        /// <summary>
        /// Username of the bot.
        /// </summary>
        public static string Username;
        /// <summary>
        /// Password for the bot account.
        /// </summary>
        public static string Password;
        /// <summary>
        /// The UserLogin class for SteamAuth
        /// </summary>
        public static UserLogin SteamLogin;
    }
}