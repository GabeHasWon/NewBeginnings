using NewBeginnings.Content.Items.Weapon;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Common.PlayerBackgrounds.Containers;

internal class Pirate : PlayerBackgroundContainer
{
    public override string LanguageKey => "Mods.NewBeginnings.Origins.Pirate";
    public override (int type, int stack)[] Inventory => new (int, int)[] { (ItemID.Keg, 1), (ItemID.Sail, 200) };

    public override EquipData Equip => new(ItemID.EyePatch, 0, 0, ItemID.GoldRing, ItemID.Sextant);
    public override MiscData Misc => new(sword: ModContent.ItemType<RustyCutlass>(), stars: 2);

    public override bool HasSpecialSpawn() => true;

    public override Point16 GetSpawnPosition()
    {
        int x = WorldGen.genRand.NextBool() ? 240 : Main.maxTilesX - 240;
        int y = (int)(Main.worldSurface * 0.35f);

        while (!Main.tile[x, y].HasTile || !Main.tileSolid[Main.tile[x, y].TileType])
            y++;

        return new Point16(x, y);
    }
}
