using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NewBeginnings.Common.PlayerBackgrounds.Containers;

internal class Inevitable : PlayerBackgroundContainer
{
    public override string LanguageKey => "Mods.NewBeginnings.Origins.Inevitable";
    public override MiscData Misc => new(300, 100, -1, ItemID.GoldBroadsword, ItemID.GoldPickaxe, ItemID.GoldAxe, stars: 3);
    public override EquipData Equip => new(ItemID.GoldHelmet, ItemID.GoldChainmail, ItemID.GoldGreaves);

    internal class InevitablePlayer : ModPlayer
    {
        Snail snail = null;

        public override void Load() => On_Main.DrawNPCs += DrawSnail;

        private void DrawSnail(On_Main.orig_DrawNPCs orig, Main self, bool behindTiles)
        {
            orig(self, behindTiles);

            if (!Main.gameMenu && Main.LocalPlayer.GetModPlayer<PlayerBackgroundPlayer>().HasBG("Inevitable"))
                Main.LocalPlayer.GetModPlayer<InevitablePlayer>().snail?.Draw();
        }

        public override void ResetEffects()
        {
            if (Player.GetModPlayer<PlayerBackgroundPlayer>().HasBG("Inevitable"))
            {
                if (Main.myPlayer == Player.whoAmI && !Main.gameMenu)
                {
                    snail ??= new Snail() { position = Player.Center - new Vector2(0, 800) };
                    snail.Update();
                }

                Player.GetDamage(DamageClass.Generic) += 0.1f;
                Player.GetDamage(DamageClass.Generic).Flat++;
            }
        }

        public override void UpdateDead() => snail?.Update();
    }

    internal class Snail : Entity
    {
        private static Asset<Texture2D> Tex = null;

        private int _timer = 0;
        private float _opacity = 0f;

        public void Update()
        {
            _timer++;

            if (Main.LocalPlayer.dead)
            {
                _opacity *= 0.9f;
                return;
            }

            if (_opacity < 0.1f)
                position = Main.LocalPlayer.Center - new Vector2(0, 800);

            _opacity = MathF.Min(_opacity + 0.05f, 1);

            Size = new Vector2(24);
            position += velocity;

            Vector2 baseSpeed = DirectionTo(Main.LocalPlayer.Center) * 1f;
            Vector2 modSpeed = (Main.LocalPlayer.Center - Center) * 0.0005f * MathF.Pow(MathF.Sin(_timer * 0.08f), 2) * 5;
            velocity = baseSpeed + modSpeed;

            if (Main.LocalPlayer.Hitbox.Intersects(Hitbox) && !Main.LocalPlayer.dead)
            {
                var text = NetworkText.FromKey("Mods.NewBeginnings.SnailDeath." + Main.rand.Next(3), Main.LocalPlayer.name);
                Main.LocalPlayer.KillMe(PlayerDeathReason.ByCustomReason(text), 9999, 0, false);
            }
        }

        public void Draw()
        {
            Tex ??= ModContent.Request<Texture2D>("NewBeginnings/Assets/Textures/Snail");

            float rot = velocity.ToRotation();
            SpriteEffects effect = SpriteEffects.FlipHorizontally;

            if (velocity.X < 0)
            {
                rot += MathHelper.Pi;
                effect = SpriteEffects.None;
            }

            float opacity = _opacity * (MathF.Pow(MathF.Sin(_timer * 0.08f), 2) * 0.75f + 0.25f);
            Main.spriteBatch.Draw(Tex.Value, Center - Main.screenPosition, null, Color.White * opacity, rot, Tex.Size() / 2f, 1f, effect, 0);
        }
    }
}
