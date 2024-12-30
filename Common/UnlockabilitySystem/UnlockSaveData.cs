using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;

namespace NewBeginnings.Common.UnlockabilitySystem;

internal class UnlockSaveData
{
    public static Dictionary<string, BaseUnlock> achievementsByName = [];

    public static void Complete(string key, bool silent = false, bool noSave = false)
    {
        if (!achievementsByName.ContainsKey(key))
            throw new KeyNotFoundException($"No achievement of name {key} exists. Did you spell it right?");

        if (achievementsByName[key].Unlocked) //It's unlocked already, skip
            return;

        achievementsByName[key].Unlocked = true;

        if (!silent)
        {
            Main.NewText(Language.GetText("Mods.NewBeginnings.Unlocks.UnlockText").WithFormatArgs(achievementsByName[key].Name));
            Main.NewText(achievementsByName[key].Description, Color.Gray);
            Main.NewText(achievementsByName[key].Rewards);
        }

        if (!noSave)
            UnlockabilityIO.QuickSave(key);
    }

    internal static void Populate()
    {
        achievementsByName.Clear();

        AddBasic("Mods.NewBeginnings.Unlocks.Beginner", "Beginner");
        AddBasic("Mods.NewBeginnings.Unlocks.Accursed", "Accursed");
        AddBasic("Mods.NewBeginnings.Unlocks.Renewed", "Renewed");
        AddBasic("Mods.NewBeginnings.Unlocks.Terrarian", "Terrarian");
    }

    private static void AddBasic(string langKey, string identifier)
    {
        BaseUnlock newUnlock = new(langKey, identifier);
        achievementsByName.Add(newUnlock.Identifier, newUnlock);
    }

    public static bool Unlocked(string key)
    {
        if (!achievementsByName.TryGetValue(key, out BaseUnlock value))
            throw new KeyNotFoundException($"No achievement of name {key} exists. Did you spell it right?");

        return value.Unlocked;
    }
}
