using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds.Containers
{
    internal class Bereaved : PlayerBackgroundContainer
    {
        public override string Flavour => "The umbrella doesn't help when the storm still clouds your mind.";
        public override string Description => "Starts with a tragic umbrella and funeral set in a graveyard.";
        public override (int type, int stack)[] Inventory => new (int, int)[] { (ItemID.TragicUmbrella, 1) };

        public override EquipData Equip => new(ItemID.FuneralHat, ItemID.FuneralCoat, ItemID.FuneralPants);

        public override void ModifyWorldGenTasks(List<GenPass> list)
        {
            int index = list.FindIndex(x => x.Name == "Planting Trees");

            list.Insert(index - 1, new PassLegacy("Bereaved Spawn", Graveyard));
        }

        public static void Graveyard(GenerationProgress progress, Terraria.IO.GameConfiguration config)
        {
            int repeats = 0;

            for (int i = 0; i < 15; ++i)
            {
                if (repeats > 800)
                    break;

                int x = Main.spawnTileX + WorldGen.genRand.Next(-50, 50);
                int y = (int)(Main.worldSurface * 0.4f);

                while (!Main.tile[x, y].HasTile)
                    y++;

                if (!WorldGen.PlaceObject(x, y - 1, TileID.Tombstones, true, WorldGen.genRand.Next(6)))
                {
                    i--;
                    repeats++;
                    continue;
                }

                int sign = Sign.ReadSign(x, y - 1);
                if (sign >= 0)
                {
                    Sign.TextSign(sign, GetSignText());
                    NetMessage.SendData(MessageID.ReadSign, -1, -1, null, sign, 0f, (byte)new BitsByte(b1: true));
                }
            }
        }

        private static string GetSignText()
        {
            int random = WorldGen.genRand.Next(7);

            return random switch
            {
                0 => "The Final Update\nSeptember 28, 2022\n'Eh, it'll come back'",
                1 => "The Guide\nAlmost constantly\n'It was necessary!'",
                2 => "The Guide Again\nConstantly\n'I'm telling you officer, it was necessary!'",
                3 => "Quality Code\n(The date is scratched off)\n'Fear is all I know'",
                4 => "James \"The\" Clothier\nJune 11th, 2019\n'He got better after'",
                5 => "Ocram\nAugust 27, 2019\n'Fun fact: Same AI as the Eye of Cthulhu!'",
                _ => "Overseer\nApril 2nd, 2021\n'A necessary final boss until the end'"
            };
        }
    }
}
