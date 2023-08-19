using Terraria.Localization;

namespace NewBeginnings.Common.UnlockabilitySystem;

internal class BaseUnlock
{
    public string Identifier;
    public bool Unlocked;

    public LocalizedText Name;
    public LocalizedText Description;
    public LocalizedText Rewards;

    public BaseUnlock(string langKey, string identifier, bool unlocked = false)
    {
        Identifier = identifier;
        Unlocked = unlocked;

        Name = Language.GetText(langKey + ".DisplayName");
        Description = Language.GetText(langKey + ".Description");
        Rewards = Language.GetText(langKey + ".Rewards");
    }
}
