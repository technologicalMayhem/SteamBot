using System;

namespace technologicalMayhem.SteamBot
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    sealed class AddonInitializer : System.Attribute
    {
        readonly int priority;

        public AddonInitializer()
        {
            priority = 0;
        }

        public AddonInitializer(int priority)
        {
            this.priority = priority;
        }
        
        public int Priority
        {
            get { return priority; }
        }
    }
}