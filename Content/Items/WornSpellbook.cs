using NewBeginnings.Common.UnlockabilitySystem;
using NewBeginnings.Content.Projectiles;
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
			Item.useTime = Item.useAnimation = 1;
			Item.useStyle = ItemUseStyleID.Shoot;
			//Item.shoot = ModContent.ProjectileType<FeintDagger>();
			Item.shootSpeed = 12f;
			Item.mana = 12;
			Item.knockBack = 1;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.noMelee = true;
		}

        public override bool? UseItem(Player player)
        {
			return true;
        }
    }
}