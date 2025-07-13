using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds;

internal class PlayerBackgroundWorld : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        if (Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().HasBG()) //Adds an origin's specific worldgen info
        {
            var data = Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData;
            data.Delegates.ModifyWorldGenTasks(tasks);

            if (data.Delegates.HasSpecialSpawn())
                tasks.Add(new PassLegacy("Special Spawn", SetSpawnOrigin, 0.05f));
        }
    }

    private void SetSpawnOrigin(GenerationProgress progress, GameConfiguration configuration) => SetOriginSpawn(Main.LocalPlayer);

    public static Point16 SetOriginSpawn(Player player)
    {
        var data = player.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData;

        if (data.Delegates.GetSpawnPosition is null)
            return Point16.NegativeOne;

        Point16 newSpawn = data.Delegates.GetSpawnPosition();
        player.GetModPlayer<PlayerBackgroundPlayer>().SetOriginSpawn(newSpawn);
        return newSpawn;
    }

    public override void PostWorldGen()
    {
        if (Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().HasBG() && NPC.AnyNPCs(NPCID.Guide)) //Replaces guide if necessary
        {
            var data = Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData;
            if (data.Misc.SpecialFirstNPCType != -1)
            {
                NPC npc = Main.npc[NPC.FindFirstNPC(NPCID.Guide)];
                npc.SetDefaults(data.Misc.SpecialFirstNPCType);
            }
        }
    }
}
