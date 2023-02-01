using Microsoft.Xna.Framework;
using NewBeginnings.Common.UnlockabilitySystem;
using NewBeginnings.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace NewBeginnings.Content.Items.Weapon
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
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<FeintDagger>();
            Item.shootSpeed = 12f;
            Item.mana = 12;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.noMelee = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position.Y -= 30;
            velocity = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;

            Point16 size = new();
            StructureHelper.Generator.GetDimensions("Assets/Structures/FrozenIgloo" + Main.rand.Next(2), Mod, ref size, false);
            StructureHelper.Generator.GenerateStructure("Assets/Structures/FrozenIgloo" + Main.rand.Next(2), new Point16(Main.spawnTileX - (size.X / 2), Main.spawnTileY - size.Y - 1), Mod);
        }
    }
}