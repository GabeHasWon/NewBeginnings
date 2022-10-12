using System;
using System.Collections.Generic;
using Terraria;

namespace NewBeginnings.Common.UnlockabilitySystem
{
    internal class UnlockSaveData
    {
        public static Dictionary<string, BaseUnlock> achievementsByName = new Dictionary<string, BaseUnlock>();

        public static void CompleteAchievement(string key)
        {
            achievementsByName[key].Unlocked = true;
            Main.NewText($"Unlocked {achievementsByName[key].Name}!");
            UnlockabilityIO.QuickSave(key);
        }

        internal static void Populate()
        {
            achievementsByName.Clear();

            AddBasic("Beginner", "Default", "Use your first origin", "Unlocks the Builder origin");
        }

        private static void AddBasic(string name, string tex, string desc, string ben)
        {
            BaseUnlock newUnlock = new BaseUnlock(name, tex, desc, ben);
            achievementsByName.Add(newUnlock.Name, newUnlock);
        }
    }
}
