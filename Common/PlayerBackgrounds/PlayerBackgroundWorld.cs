using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds
{
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

        private void SetSpawnOrigin(GenerationProgress progress, GameConfiguration configuration)
        {
            var data = Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData;
            Point16 newSpawn = data.Delegates.GetSpawnPosition();

            Main.spawnTileX = newSpawn.X;
            Main.spawnTileY = newSpawn.Y;
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
}
