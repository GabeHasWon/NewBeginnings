using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace NewBeginnings.Content.Projectiles.Weapon;

public class KylieProjectile : ModProjectile
{
    ref float Timer => ref Projectile.ai[0];

    Player Owner => Main.player[Projectile.owner];

    public override void SetStaticDefaults() => Main.projFrames[Type] = 1;

    public override void SetDefaults()
    {
        Projectile.width = 22;
        Projectile.height = 22;
        Projectile.penetrate = -1;
        Projectile.aiStyle = 0;
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.friendly = true;
        Projectile.timeLeft = 4;
        Projectile.tileCollide = true;
    }

    public override void AI()
    {
        const int Cutoff = 25;
        const float MaxSpeed = 10;

        Timer++;
        Projectile.timeLeft++;
        Projectile.rotation += 0.12f;
        Projectile.tileCollide = Collision.CanHit(Projectile, Owner);

        if (Timer > Cutoff)
        {
            float factor = 0.8f * MathHelper.Min((Timer - Cutoff) / (Cutoff + 20f), 1f);
            float angle = Utils.AngleLerp(Projectile.velocity.ToRotation(), Projectile.AngleTo(Owner.Center), factor) - Projectile.velocity.ToRotation();
            Projectile.velocity = Projectile.velocity.RotatedBy(angle);

            if (Projectile.velocity.LengthSquared() > MaxSpeed * MaxSpeed)
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * MaxSpeed;

            if (Projectile.Hitbox.Intersects(Owner.Hitbox))
                Projectile.Kill();
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.velocity = Projectile.DirectionTo(Owner.Center) * 8;
        return false;
    }
}
