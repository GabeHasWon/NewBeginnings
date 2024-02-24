using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Common.PlayerBackgrounds.Containers;

internal class ReallyConfused : PlayerBackgroundContainer
{
    public override string LanguageKey => "Mods.NewBeginnings.Origins.ReallyConfused";

    public override MiscData Misc => new(swordReplace: ItemID.GoldBow, stars: 4);
    public override (int type, int stack)[] Inventory => new[] { ((int)ItemID.WoodenArrow, 100) };

    internal class ConfusedPlayer : ModPlayer
    {
        public override void PostUpdateEquips()
        {
            if (!Player.GetModPlayer<PlayerBackgroundPlayer>().HasBG(nameof(ReallyConfused)))
                return;

            var summon = Player.GetDamage(DamageClass.Summon);
            var summonDamage = new StatModifier(summon.Additive, summon.Multiplicative, summon.Flat, summon.Base);
            Player.GetDamage(DamageClass.Summon) = Player.GetDamage(DamageClass.Ranged);
            Player.GetDamage(DamageClass.Ranged) = Player.GetDamage(DamageClass.Magic);
            Player.GetDamage(DamageClass.Magic) = Player.GetDamage(DamageClass.Melee);
            Player.GetDamage(DamageClass.Melee).CombineWith(summonDamage);

            Player.GetAttackSpeed(DamageClass.Magic) = Player.GetAttackSpeed(DamageClass.Melee);
            Player.GetDamage(DamageClass.Melee) += (Player.maxMinions - 1) * 0.075f; //Adds extra summons to melee's damage
        }
    }
}
