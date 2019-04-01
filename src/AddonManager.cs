using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace technologicalMayhem.SteamBot
{
    public static class AddonManager
    {
        static AppDomain addons = AppDomain.CreateDomain("addons");

        //Loads all dll files frot the addons folder and makes them available in the addons domain
        public static void LoadAssemblies()
        {
            string[] files;
            try
            {
                files = Directory.GetFiles("addons", "*.dll");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Directory.CreateDirectory("addons");
                return;
            }
            foreach (var dll in files)
            {
                addons.Load(dll);
            }
            Console.WriteLine(addons.GetAssemblies().Count() + " Addons loaded.");
        }

        public static void ReloadAssemblies()
        {
            Console.WriteLine("Attempting to unload addons...");
            AppDomain.Unload(addons);
            Console.WriteLine("Unloaded all addons. Reloading...");
            LoadAssemblies();
        }

        public static void UnloadAssemblies()
        {
            AppDomain.Unload(addons);
        }

        public static void InitializeAddons()
        {
            List<MethodInfo> methods = AppDomain.CurrentDomain.GetAssemblies()
                .Concat(addons.GetAssemblies())
                .SelectMany(a => a.GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(AddonInitializer), false).Length > 0))
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