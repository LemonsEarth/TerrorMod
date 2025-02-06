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
    public class CrimsonCarrierProj : ModProjectile
    {
        string GlowMask_Path = "TerrorMod/Content/Projectiles/Hostile/CrimsonCarrierProj_Glow";

        ref float AITimer => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 7;
            }

            LemonUtils.DustCircle(Projectile.Center, 4, 3, DustID.Crimson);
            Projectile.rotation = MathHelper.ToRadians(AITimer * 6);

            AITimer++;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.Zero) * 3, ProjectileID.ViciousPowder, 0, 1);
            }   
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D glowmask = ModContent.Request<Texture2D>(GlowMask_Path, AssetRequestMode.ImmediateLoad).Value;
            Main.EntitySpriteDraw(glowmask, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, Color.White, Projectile.rotation, glowmask.Size() * 0.5f, 1f, SpriteEffects.None);
        }
    }
}
