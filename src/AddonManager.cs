using System;
using System.Linq;

namespace technologicalMayhem.SteamBot
{
    public static class AddonManager
    {
        public static void LoadAddons()
        {

        }

        public static void InitializeAddons()
        {
            var methods = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(PluginInitializer), false).Length > 0))
                .ToList();
            methods.ForEach(x => x.Invoke(null, null));
        }
    }
}