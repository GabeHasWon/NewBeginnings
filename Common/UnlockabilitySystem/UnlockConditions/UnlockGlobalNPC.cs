using NewBeginnings.Common.PlayerBackgrounds;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Common.UnlockabilitySystem.UnlockConditions;

internal class UnlockGlobalNPC : GlobalNPC
{
    public override bool CheckDead(NPC npc)
    {
        bool hasBG = Main.player.Take(Main.maxPlayers).Any(x => x.active && x.GetModPlayer<PlayerBackgroundPlayer>().HasBG());

        if (!hasBG)
            return true;

        if (npc.type == NPCID.WallofFlesh)
            UnlockSaveData.Complete("Accursed");

        return true;
    }
}
