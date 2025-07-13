using NewBeginnings.Common.UI;
using Terraria;
using System.Linq;
using Terraria.ID;

namespace NewBeginnings.Common.PlayerBackgrounds.Containers;

internal class Custom : PlayerBackgroundContainer
{
    public override string LanguageKey => "Mods.NewBeginnings.Origins.Custom";

    public static PlayerBackgroundData GetCustomBackground(Player player, bool resetLifeAndManaCrystals = false)
    {
        if (resetLifeAndManaCrystals)
        {
            player.ConsumedManaCrystals = 0;
            player.ConsumedLifeCrystals = 0;
        }

        PlayerBackgroundPlayer plr = player.GetModPlayer<PlayerBackgroundPlayer>();
        CustomOriginData customData = plr.CustomOriginData;

        string nameKey = "Mods.NewBeginnings.Origins.Custom";
        EquipData equips = new(customData.Helmet.Id, customData.Body.Id, customData.Legs.Id, [.. customData.Accessories.Select(x => x.Id)]);
        MiscData misc = new(customData.life, customData.mana, -1, ItemID.None, ItemID.None, ItemID.None, 0, 3);

        PlayerBackgroundData data = new(nameKey, "Custom", equips, misc, [.. customData.Hotbar.Select(x => (x.Id, x.Stack))]);
        return data;
    }
}
