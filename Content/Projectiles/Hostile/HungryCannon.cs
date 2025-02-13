using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Projectiles.Hostile
{
    public class HungryCannon : ModProjectile
    {
        ref float AITimer => ref Projectile.ai[0];
        ref float directionY => ref Projectile.ai[1]; // 1 is up, 0 is down

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 36;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
        }

        public override void AI()
        {
            if (AITimer == 0)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath10 with { PitchRange = (0.3f, 0.6f)}, Projectile.Center);
                LemonUtils.DustCircle(Projectile.Center, 8, 3, DustID.GemRuby);
            }

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 7;
            }

            int rot = directionY == 1 ? 1 : -1;
            Projectile.rotation = rot * MathHelper.PiOver2;

            if (AITimer % 10 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY.RotatedBy(directionY * MathHelper.Pi) * 5, ModContent.ProjectileType<EyeFireButFire>(), Projectile.damage, 1f);
                }
            }

            Projectile.velocity = Vector2.Zero;

            AITimer++;
        }
    }
}
