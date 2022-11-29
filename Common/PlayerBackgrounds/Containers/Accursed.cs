using NewBeginnings.Common.UnlockabilitySystem;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds.Containers
{
    internal class Accursed : PlayerBackgroundContainer
    {
        public override string Description => "Starts with hardmode already enabled. Good luck!";

        public override EquipData Equip => new(ItemID.PearlwoodHelmet, ItemID.PearlwoodBreastplate, ItemID.PearlwoodGreaves);
        public override MiscData Misc => new(swordReplace: ItemID.GoldBroadsword, pickReplace: ItemID.GoldPickaxe, axeReplace: ItemID.GoldAxe, npcType: NPCID.Dryad);

        public override bool ClearCondition() => UnlockSaveData.Unlocked("Accursed");

        public override void ModifyWorldGenTasks(List<GenPass> list)
        {
            list.Add(new PassLegacy("Early Hardmode", (GenerationProgress p, Terraria.IO.GameConfiguration config) =>
            {
                p.Message = "Generating hardmode";
                WorldGen.smCallBack(null);

                Main.hardMode = true;
            }));
        }
    }
}
