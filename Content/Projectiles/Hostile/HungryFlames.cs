using TerrorMod.Core.Systems;
using TerrorMod.Core.Systems.Particles;
using TerrorMod.Core.Systems.Particles.Renderers;

namespace TerrorMod.Content.Projectiles.Hostile
{
    public class HungryFlames : ModProjectile
    {
        int AITimer;
        ref float TimeLeft => ref Projectile.ai[0];

        public override string Texture => TerrorTextures.Empty100TexPath;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 600;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (AITimer == 0)
            {

            }

            for (int i = 0; i < 3; i++)
            {
                ParticleSystem.SpawnParticle<AfterDustParticleRendererGlowy>(
                    ParticleID.Glowy,
                    Projectile.RandomPos(),
                    Projectile.velocity * 0.5f,
                    Main.rand.NextFromList(Color.Red, Color.Orange),
                    1f,
                    Main.rand.NextFloat(0.5f, 1.2f),
                    30, 5, 20, 0.98f
                    );
            }

            if (AITimer > TimeLeft)
            {
                Projectile.Kill();
            }
            AITimer++;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }

        public override void OnKill(int timeLeft)
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }

        public override void PostDraw(Color lightColor)
        {

        }
    }
}
