using System;

namespace technologicalMayhem.SteamBot
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    sealed class PluginInitializer : System.Attribute
    {
        readonly int priority;

        public PluginInitializer()
        {
            priority = 0;
        }

        public PluginInitializer(int priority)
        {
            this.priority = priority;
        }
        
        public int Priority
        {
            get { return priority; }
        }
    }
}