using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace NewBeginnings.Common.PlayerBackgrounds.Containers;

internal class Slayer : PlayerBackgroundContainer
{
    public override string LanguageKey => "Mods.NewBeginnings.Origins.Slayer";
    public override (int type, int stack)[] Inventory => new (int, int)[] { (ItemID.EndlessMusketPouch, 1) };
    public override MiscData Misc => new(140, 20, swordReplace: ItemID.Boomstick, stars: 4);

    public override bool HasSpecialSpawn() => true;

    public override Point16 GetSpawnPosition()
    {
        const int Offset = 400;

        int x = WorldGen.genRand.NextBool() ? Offset : Main.maxTilesX - Offset;
        int y = Main.maxTilesY - 160;
        bool success = false;

        while (!success)
        {
            for (int i = y; i < Main.maxTilesY - 40; i++)
            {
                bool validOpening = true;
                for (int j = 0; j < 3; ++j)
                    if (WorldGen.SolidTile(x, i - j - 1) || WorldGen.SolidTile(x + 1, i - j - 1))
                        validOpening = false;

                if (validOpening && WorldGen.SolidTile(x, i) && !Collision.LavaCollision(new Vector2(x - 1, i - 3) * 16, 3 * 16, 3 * 16))
                {
                    success = true;
                    y = i - 3;
                    break;
                }
            }

            if (!success)
                x += (x < Main.maxTilesX / 2) ? 2 : -2;
        }

        return new Point16(x, y);
    }
}
