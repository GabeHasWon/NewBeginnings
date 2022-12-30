using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace NewBeginnings.Common.PlayerBackgrounds.Containers;

internal class Adventurer : PlayerBackgroundContainer
{
    public override string Description => "Comes with a thirst for exploration and a penchant for boulder traps!";
    public override (int type, int stack)[] Inventory => new (int, int)[] { (ItemID.GrapplingHook, 1), (ItemID.Torch, 100), (ItemID.TrapsightPotion, 5), (ItemID.SpelunkerPotion, 5) };

    public override EquipData Equip => new EquipData(ItemID.ArchaeologistsHat, ItemID.ArchaeologistsJacket, ItemID.ArchaeologistsPants);

    public override void ModifyWorldGenTasks(List<GenPass> list)
    {
        int trapsIndex = list.FindIndex(x => x.Name == "Traps");

        if (trapsIndex != -1)
            list.Insert(trapsIndex + 1, new PassLegacy("Traps2", BoulderStep));
    }

	public void BoulderStep(GenerationProgress progress, Terraria.IO.GameConfiguration config)
	{
		for (int i = 0; i < 750; ++i)
		{
			int x = WorldGen.genRand.Next(Main.maxTilesX / 5, (Main.maxTilesX / 5) * 4);
			int y = WorldGen.genRand.Next((int)Main.worldSurface, (int)(Main.maxTilesY / 1.5f));
			int count = 0;

			for (int l = x - 1; l < x + 3; ++l)
				for (int k = y; k < y + 10; ++k)
					if (!Main.tile[l, k].HasTile)
						count++;

			if (count > 8)
				BoulderTrap(x, y);
			else
				i--;
		}
	}

	/// <summary>
	/// Copied from vanilla source; ripped out of WorlGen.placeTrap(int, int).
	/// </summary>
    private bool BoulderTrap(int x, int y)
    {
		int adjY = y;
		int adjX = x;
		int realY = adjY - 8;
		adjX += WorldGen.genRand.Next(-1, 2);

		Tile tile;

		while (true)
		{
			bool canExit = true;
			int repeats = 0;

			for (int i = adjX - 2; i <= adjX + 3; i++)
			{
				for (int j = realY; j <= realY + 3; j++)
				{
					if (!WorldGen.SolidTile(i, j))
						canExit = false;

					tile = Main.tile[i, j];

					if (!tile.HasTile)
						continue;

					tile = Main.tile[i, j];
					if (tile.TileType == 226)
					{
						WorldGen.trapDiag[1, 0]++;
						return false;
					}
					tile = Main.tile[i, j];
					if (tile.TileType != 0)
					{
						tile = Main.tile[i, j];
						if (tile.TileType != 1)
						{
							tile = Main.tile[i, j];
							if (tile.TileType != 59)
								continue;
						}
					}
					repeats++;
				}
			}

			realY--;

			if (realY < Main.worldSurface)
			{
				WorldGen.trapDiag[1, 0]++;
				return false;
			}

			if (canExit && repeats > 2)
				break;
		}

		if (adjY - realY <= 5 || adjY - realY >= 40)
		{
			WorldGen.trapDiag[1, 0]++;
			return false;
		}

		for (int i = adjX; i <= adjX + 1; i++)
			for (int j = realY; j <= adjY; j++)
				WorldGen.KillTile(i, j);

		for (int i = adjX - 2; i <= adjX + 3; i++)
		{
			for (int j = realY - 2; j <= realY + 3; j++)
			{
				if (WorldGen.SolidTile(i, j))
				{
					tile = Main.tile[i, j];
					tile.TileType = TileID.Stone;
				}
			}
		}

		WorldGen.PlaceTile(x, adjY, TileID.PressurePlates, mute: true, forced: true, -1, 7);
		WorldGen.PlaceTile(adjX, realY + 2, TileID.ActiveStoneBlock, mute: true);
		WorldGen.PlaceTile(adjX + 1, realY + 2, TileID.ActiveStoneBlock, mute: true);
		WorldGen.PlaceTile(adjX + 1, realY + 1, TileID.Boulder, mute: true);

		realY += 2;
		tile = Main.tile[adjX, realY];
		tile.RedWire = true;
		tile = Main.tile[adjX + 1, realY];
		tile.RedWire = true;
		realY++;

		WorldGen.PlaceTile(adjX, realY, TileID.ActiveStoneBlock, mute: true);
		WorldGen.PlaceTile(adjX + 1, realY, TileID.ActiveStoneBlock, mute: true);

		tile = Main.tile[adjX, realY];
		tile.RedWire = true;
		tile = Main.tile[adjX + 1, realY];
		tile.RedWire = true;

		WorldGen.PlaceTile(adjX, realY + 1, TileID.ActiveStoneBlock, mute: true);
		WorldGen.PlaceTile(adjX + 1, realY + 1, TileID.ActiveStoneBlock, mute: true);

		tile = Main.tile[adjX, realY + 1];
		tile.RedWire = true;
		tile = Main.tile[adjX + 1, realY + 1];
		tile.RedWire = true;

		int num10 = x;
		int num11 = adjY;

		while (num10 != adjX || num11 != realY)
		{
			tile = Main.tile[num10, num11];
			tile.RedWire = true;

			if (num10 > adjX)
				num10--;

			if (num10 < adjX)
				num10++;

			tile = Main.tile[num10, num11];
			tile.RedWire = true;

			if (num11 > realY)
				num11--;

			if (num11 < realY)
				num11++;

			tile = Main.tile[num10, num11];
			tile.RedWire = true;
		}
		WorldGen.trapDiag[1, 1]++;
		return true;
	}
}
