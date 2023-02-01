using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using System.Linq;
using System;
using Terraria.ModLoader;

namespace NewBeginnings.Common.PlayerBackgrounds.Containers
{
    internal class Frozen : PlayerBackgroundContainer
    {
        private static Point16 spawnPos = new Point16();

        public override string Name => "Frozen";
        public override string Flavour => "After a thousands years frozen (and a couple days making the igloo), they're ready for action.";
        public override string Description => "Epic";
        public override (int type, int stack)[] Inventory => new (int type, int stack)[] { (ItemID.Snowball, 999) };

        public override EquipData Equip => new(ItemID.PinkEskimoHood, ItemID.PinkEskimoCoat, ItemID.PinkEskimoPants);
        public override MiscData Misc => new MiscData(swordReplace: ItemID.SnowballCannon);

        public override bool HasSpecialSpawn() => true;
        public override Point16 GetSpawnPosition() => spawnPos;

        public override void ModifyWorldGenTasks(List<GenPass> list)
        {
            int index = list.FindIndex(x => x.Name == "Planting Trees");

            list.Insert(index - 1, new PassLegacy("Igloo", Igloo));
        }

        public static void Igloo(GenerationProgress progress, Terraria.IO.GameConfiguration config)
        {
            int min = WorldGen.snowMinX[0];
            int max = WorldGen.snowMaxX[Array.IndexOf(WorldGen.snowMaxX, 0) - 1];

            int x = (min + max) / 2;
            int y = (int)(Main.worldSurface * 0.3f);

            while (!Main.tile[x, y].HasTile || (Main.tile[x, y].TileType != TileID.IceBlock && Main.tile[x, y].TileType != TileID.SnowBlock))
                y++;

            Point16 size = new();
            var mod = ModLoader.GetMod("NewBeginnings");
            int type = Main.rand.Next(2);

            StructureHelper.Generator.GetDimensions("Assets/Structures/FrozenIgloo" + type, mod, ref size, false);
            spawnPos = new Point16(x - (size.X / 2), y - size.Y - 1);
            StructureHelper.Generator.GenerateStructure("Assets/Structures/FrozenIgloo" + type, spawnPos, mod);
        }
    }
}
