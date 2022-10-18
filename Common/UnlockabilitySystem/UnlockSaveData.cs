using System;
using System.Collections.Generic;
using Terraria;

namespace NewBeginnings.Common.UnlockabilitySystem
{
    internal class UnlockSaveData
    {
        public static Dictionary<string, BaseUnlock> achievementsByName = new Dictionary<string, BaseUnlock>();

        public static void Complete(string key, bool silent = false, bool noSave = false)
        {
            if (achievementsByName[key].Unlocked) //It's unlocked already, skip
                return;

            achievementsByName[key].Unlocked = true;

            if (!silent)
                Main.NewText($"Unlocked {achievementsByName[key].Name}!");
            
            if (!noSave)
                UnlockabilityIO.QuickSave(key);
        }

        internal static void Populate()
        {
            achievementsByName.Clear();

            AddBasic("Beginner", "Default", "Use your first origin", "Unlocks the Builder origin");
            AddBasic("Accursed", "Default", "Defeat the Wall of Flesh while using an origin", "Unlocks the Accursed origin");
        }

        private static void AddBasic(string name, string tex, string desc, string ben)
        {
            BaseUnlock newUnlock = new(name, tex, desc, ben);
            achievementsByName.Add(newUnlock.Name, newUnlock);
        }

        public static bool Unlocked(string key)
        {
            if (!achievementsByName.ContainsKey(key))
                throw new KeyNotFoundException($"No achievement of name {key} exists. Did you spell it right?");

            return achievementsByName[key].Unlocked;
        }
    }
}
