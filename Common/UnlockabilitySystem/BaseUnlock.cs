using Terraria.Localization;

namespace NewBeginnings.Common.UnlockabilitySystem;

internal class BaseUnlock(string langKey, string identifier, bool unlocked = false)
{
    public string Identifier = identifier;
    public bool Unlocked = unlocked;

    public LocalizedText Name = Language.GetText(langKey + ".DisplayName");
    public LocalizedText Description = Language.GetText(langKey + ".Description");
    public LocalizedText Rewards = Language.GetText(langKey + ".Rewards");
}
