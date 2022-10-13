using Microsoft.Xna.Framework;
using NewBeginnings.Common.Prim;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace NewBeginnings.Content.Projectiles
{
    public class FeintDagger : ModProjectile
    {
        public const int ShootAwayTimer = 30;

        public ref float Timer => ref Projectile.ai[0];
        public ref float DaggerID => ref Projectile.ai[1];

        private List<Vector2> points = new List<Vector2>();
        private Vector2 _originalSpeed = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.timeLeft = 60 * 8; //8 seconds
        }

        public override void OnSpawn(IEntitySource source)
        {
            DaggerID = Main.projectile.Take(Main.maxProjectiles).Where(x => x.active && x.owner == Projectile.owner && x.ai[0] < ShootAwayTimer).Count();
            _originalSpeed = Projectile.velocity;
 
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            Timer++;

            if (Timer < ShootAwayTimer)
            {
                const float MaxRadians = 0.8f;

                int daggers = Main.projectile.Take(Main.maxProjectiles).Where(x => x.active && x.owner == Projectile.owner && x.ai[0] < ShootAwayTimer).Count();
                float maxRadii = MaxRadians / daggers;
                float rotation = maxRadii * DaggerID;
                float offsetRotation = MaxRadians;

                Vector2 followPos = new Vector2(0, 50).RotatedBy(rotation * 2 - offsetRotation);
                float dist = Projectile.Distance(Main.player[Projectile.owner].Center - followPos);

                if (dist > 12f)
                    dist = 12f;

                Vector2 newVel = Projectile.DirectionTo(Main.player[Projectile.owner].Center - followPos) * dist;
                Projectile.velocity = !newVel.HasNaNs() ? newVel : Vector2.Zero;
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, _originalSpeed.ToRotation() + MathHelper.PiOver2, 0.1f);
            }
            else if (Timer == ShootAwayTimer)
                Projectile.velocity = _originalSpeed;
            else
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            points.Add(Projectile.Center);

            if (points.Count > 8)
                points.RemoveAt(0);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            const int _widthStart = 10;

            TrailDrawer.Draw(points, _widthStart);
            return true;
        }
    }
}
