using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace NewBeginnings.Common.PlayerBackgrounds.Globals
{
    internal class AustralianPlayer : ModPlayer
    {
        public bool IsAustralian => Player is not null && Player.GetModPlayer<PlayerBackgroundPlayer>().HasBG("Australian"); //this is such a surreal property name

        public override void Load() => Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.DrawPlayerFull += LegacyPlayerRenderer_DrawPlayerFull;

        private static void LegacyPlayerRenderer_DrawPlayerFull(Terraria.Graphics.Renderers.On_LegacyPlayerRenderer.orig_DrawPlayerFull orig, LegacyPlayerRenderer self, Camera camera, Terraria.Player drawPlayer)
        {
            float oldGravDir = drawPlayer.gravDir;
            if (drawPlayer.GetModPlayer<AustralianPlayer>().IsAustralian)
                drawPlayer.gravDir = -1;

            orig(self, camera, drawPlayer);

            if (drawPlayer.GetModPlayer<AustralianPlayer>().IsAustralian)
                drawPlayer.gravDir = oldGravDir;
        }

        public override void PreUpdate() => Player.gravity = 0.4f;
    }
}
