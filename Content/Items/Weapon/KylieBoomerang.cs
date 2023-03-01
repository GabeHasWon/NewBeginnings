using Microsoft.Xna.Framework;
using NewBeginnings.Content.Projectiles.Weapon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Content.Items.Weapon
{
    public class KylieBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Kylie");
            Tooltip.SetDefault("'Surprisingly effective in combat!'");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 16;
            Item.width = 20;
            Item.height = 46;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<KylieProjectile>();
            Item.shootSpeed = 10f;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => velocity = velocity.RotatedByRandom(0);
    }
}