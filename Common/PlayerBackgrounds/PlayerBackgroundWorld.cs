using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds
{
    internal class PlayerBackgroundWorld : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            if (Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().HasBG())
                Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().BackgroundData.Delegates.ModifyWorldGenTasks(tasks);
        }
    }
}
