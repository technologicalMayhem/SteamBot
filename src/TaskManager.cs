using System;
using SteamAuth;

namespace technologicalMayhem.SteamBot
{
    static class TaskManager
    {
        public static void Start()
        {
            Globals.SteamLogin = new UserLogin(Globals.Username, Globals.Password);
            LoginResult loginResult = Globals.SteamLogin.DoLogin();
        }
    }
}