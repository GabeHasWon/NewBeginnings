using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;

namespace NewBeginnings.Content.Items
{
    [AutoloadEquip(EquipType.Waist)]
    internal class DeprivedLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Lights up when placed in inventory\nInfinite duration");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        
        public override void SetDefaults()
        {
            Item.Size = new Vector2(18, 36);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = Item.buyPrice(gold: 2);
            Item.accessory = true;
        }

        public override void UpdateInventory(Player player)
        {
            Lighting.AddLight(player.Center + new Vector2(0, 8), new Vector3(0.4f, 0.48f, 0.48f));
            player.waist = (sbyte)EquipLoader.GetEquipSlot(Mod, nameof(DeprivedLantern), EquipType.Waist);
        }

        public override void UpdateEquip(Player player)
        {
            Lighting.AddLight(player.Center + new Vector2(0, 8), new Vector3(0.4f, 0.48f, 0.48f));
            player.waist = (sbyte)EquipLoader.GetEquipSlot(Mod, nameof(DeprivedLantern), EquipType.Waist);
        }
    }
}
