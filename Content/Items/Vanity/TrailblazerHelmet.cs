using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Content.Items.Vanity;

[AutoloadEquip(EquipType.Head)]
public class TrailblazerHelmet : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 28;
		Item.height = 22;
		Item.value = Terraria.Item.sellPrice(0, 0, 25, 0);
		Item.rare = ItemRarityID.Green;
		Item.vanity = true;
	}
}
