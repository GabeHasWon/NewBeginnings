using System;

namespace NewBeginnings.Common.UnlockabilitySystem
{
    internal class BaseUnlock
    {
        public string Name;
        public string TexName;
        public string Description;
        public string Benefits;
        public bool Unlocked;

        public BaseUnlock(string name, string tex, string desc, string benefits, bool unlocked = false)
        {
            Name = name;
            TexName = tex;
            Description = desc;
            Benefits = benefits;
            Unlocked = unlocked;
        }
    }
}
