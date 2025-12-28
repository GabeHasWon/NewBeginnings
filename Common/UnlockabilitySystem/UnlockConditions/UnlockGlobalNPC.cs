using NewBeginnings.Common.PlayerBackgrounds;
using NewBeginnings.Common.UnlockabilitySystem.Achievements;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Common.UnlockabilitySystem.UnlockConditions;

internal class UnlockGlobalNPC : GlobalNPC
{
    public override bool CheckDead(NPC npc)
    {
        bool anyBg = false;

        foreach (Player player in Main.ActivePlayers)
        {
            if (player.GetModPlayer<PlayerBackgroundPlayer>().HasBG())
            {
                anyBg = true;
                break;
            }
        }

        if (npc.type == NPCID.MoonLordCore)
            UnlockSaveData.Complete("Renewed");

        if (!anyBg)
            return true;

        if (npc.type == NPCID.WallofFlesh)
            UnlockSaveData.Complete("Accursed", AccursedAchievement.Condition);

        if (npc.type == NPCID.MoonLordCore)
            UnlockSaveData.Complete("Terrarian");

        return true;
    }
}
