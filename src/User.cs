using System;
using System.Collections.Generic;
using SteamKit2;

namespace technologicalMayhem.SteamBot
{
    public class User
    {
        public SteamID steamID {get; set;}
        public Dictionary<string, string> data {get;set;}
    }
}