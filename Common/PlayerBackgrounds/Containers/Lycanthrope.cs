using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NewBeginnings.Common.PlayerBackgrounds.Containers;

internal class Lycanthrope : PlayerBackgroundContainer
{
    internal class LycanthropePlayer : ModPlayer
    {
        internal static bool Mounting = false;

        public override void Load()
        {
            On_Player.QuickMount += BlockMount;
            On_Main.TryRemovingBuff += BlockMountBuff;
            On_Mount.DoSpawnDust += StopSpawnDustInMenu;
            On_Player.QuickGrapple += StopGrapple;
        }

        private void StopGrapple(On_Player.orig_QuickGrapple orig, Player self)
        {
            if (self.GetModPlayer<PlayerBackgroundPlayer>().HasBG("Lycanthrope"))
                return;

            orig(self);
        }

        private void StopSpawnDustInMenu(On_Mount.orig_DoSpawnDust orig, Mount self, Player mountedPlayer, bool isDismounting)
        {
            if (!Main.gameMenu)
                orig(self, mountedPlayer, isDismounting);
        }

        // Stop mount from being manually clicked off
        private void BlockMountBuff(On_Main.orig_TryRemovingBuff orig, int i, int b)
        {
            if (b == BuffID.WolfMount && Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().HasBG("Lycanthrope"))
                return;

            orig(i, b);
        }

        private void BlockMount(On_Player.orig_QuickMount orig, Player self)
        {
            if (self.GetModPlayer<PlayerBackgroundPlayer>().HasBG("Lycanthrope") && !Mounting)
                return;

            orig(self);
        }

        public override bool CanUseItem(Item item)
        {
            if (item.shoot > ProjectileID.None && ContentSamples.ProjectilesByType[item.shoot].aiStyle == ProjAIStyleID.Hook)
                return false;

            return item.mountType == -1 || !Player.GetModPlayer<PlayerBackgroundPlayer>().HasBG("Lycanthrope");
        }

        public override void PreUpdate()
        {
            // Force mount active if necessary
            if (!Player.mount.Active)
                Player.mount.SetMount(MountID.Wolf, Player);
        }
    }

    public override string LanguageKey => "Mods.NewBeginnings.Origins.Lycanthrope";

    public override MiscData Misc => new(150, 20, -1, ItemID.SilverBroadsword, ItemID.SilverPickaxe, ItemID.SilverAxe, 10, 4);
    public override EquipData Equip => EquipData.SingleAcc(ItemID.MoonCharm);

    public override void ModifyPlayerCreation(Player player) => player.miscEquips[3] = new Item(ItemID.WolfMountItem);
}
