using System;
using System.Reflection;
using System.Collections.Generic;
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
            List<MethodInfo> methods = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes()
                    .SelectMany(t => t.GetMethods())
                    .Where(m => m.GetCustomAttributes(typeof(PluginInitializer), false).Length > 0))
                    .ToList();
            methods.ForEach(x => x.Invoke(null, null));
        }

        public static IEnumerable<T> ReturnCount<T>(this IEnumerable<T> e)
        {
            Console.WriteLine(e.Count());
            return e;
        }
    }
}