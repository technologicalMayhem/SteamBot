using System;

namespace technologicalMayhem.SteamBot
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    sealed class AddonInitializer : System.Attribute
    {
        public int Priority
        {
            get { return priority; }
        }

        readonly int priority;

        public AddonInitializer()
        {
            priority = 0;
        }

        public AddonInitializer(int priority)
        {
            this.priority = priority;
        }
    }
}