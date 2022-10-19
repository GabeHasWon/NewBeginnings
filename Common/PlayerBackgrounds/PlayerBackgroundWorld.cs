using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds
{
    internal class PlayerBackgroundWorld : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            if (Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().HasBG()) //Adds an origin's specific worldgen info
                Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData.Delegates.ModifyWorldGenTasks(tasks);
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
