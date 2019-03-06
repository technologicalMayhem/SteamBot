using System;
using SteamKit2;

namespace technologicalMayhem.SteamBot
{
    public interface IChatCommand
    {
        ChatCommandProperties Properties { get; }
        void Execute(SteamID steamid, string[] parameters);
    }

    public interface IPlugin{
        void Initialize();
        void Shutdown();
    }
}