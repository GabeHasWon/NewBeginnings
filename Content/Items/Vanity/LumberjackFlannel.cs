using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Content.Items.Vanity;

[AutoloadEquip(EquipType.Body)]
public class LumberjackFlannel : ModItem
{
	public override void SetStaticDefaults()
	{
		Tooltip.SetDefault("'Incredibly comfortable'");

		Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 20;
		Item.value = Item.sellPrice(0, 0, 0, 0);
		Item.rare = ItemRarityID.Green;
		Item.vanity = true;
	}
}