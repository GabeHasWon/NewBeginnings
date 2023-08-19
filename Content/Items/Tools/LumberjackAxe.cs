using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Content.Items.Tools;

internal class LumberjackAxe : ModItem
{
	public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;

	public override void SetDefaults()
	{
		Item.width = 36;
		Item.height = 30;
		Item.value = Item.buyPrice(0, 0, 20, 0);
		Item.rare = ItemRarityID.Blue;
		Item.axe = 11;
		Item.damage = 6;
		Item.knockBack = 4;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTime = 18;
		Item.useAnimation = 18;
		Item.DamageType = DamageClass.Melee;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.UseSound = SoundID.Item1;
	}
}
