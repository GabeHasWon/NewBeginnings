using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Content.Items
{
	public class WornSpellbook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worn Spellbook");
			Tooltip.SetDefault("Trusty, albeit rather weak");
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Magic;
			Item.damage = 12;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.WaterBolt;
			Item.shootSpeed = 14f;
			Item.mana = 12;
			Item.knockBack = 1;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;
			Item.noMelee = true;
		}
	}
}