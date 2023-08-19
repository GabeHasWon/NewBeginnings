using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Common.PlayerBackgrounds.Globals;

internal class PainterNPC : GlobalNPC
{
    public override void Load()
    {
        Terraria.GameContent.On_ShopHelper.GetShoppingSettings += ShopHelper_GetShoppingSettings;
    }

    private ShoppingSettings ShopHelper_GetShoppingSettings(Terraria.GameContent.On_ShopHelper.orig_GetShoppingSettings orig, Terraria.GameContent.ShopHelper self, Player player, NPC npc)
    {
        var settings = orig(self, player, npc);

        if (npc.type == NPCID.Painter && player.GetModPlayer<PlayerBackgroundPlayer>().HasBG("Painter"))
        {
            settings.PriceAdjustment *= 0.8f;
            settings.HappinessReport += "I'm always happy to help a fellow painter!";
        }

        return settings;
    }

    public override void GetChat(NPC npc, ref string chat)
    {
        if (npc.type == NPCID.Painter && Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().HasBG("Painter"))
        {
            if (Main.rand.NextBool(8))
                chat = "Ah! A fellow artist. Need to restock your paints?";

            if (Main.rand.NextBool(8))
                chat = "Perhaps we should work on a painting together?";
        }
    }
}