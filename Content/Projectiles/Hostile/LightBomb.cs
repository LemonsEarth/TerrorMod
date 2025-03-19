using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Common.Utils.Prim;

namespace TerrorMod.Content.Projectiles.Hostile
{
    public class LightBomb : ModProjectile
    {
        float AITimer = 0;
        ref float xPos => ref Projectile.ai[0];
        ref float yPos => ref Projectile.ai[1];
        ref float Time => ref Projectile.ai[2];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
        }

        public override void OnKill(int timeLeft)
        {
            /*if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<StardustExplosion>(), Projectile.damage * 2, Projectile.knockBack);
            }*/
            LemonUtils.DustCircle(Projectile.Center, 8, 8f, DustID.GemDiamond, 1.25f);
        }

        Vector2 pos = Vector2.Zero;
        Vector2 spawnPos = Vector2.Zero;
        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 7;
            }
            if (AITimer == 0)
            {
                pos = new Vector2(xPos, yPos);
                spawnPos = Projectile.Center;
                SoundEngine.PlaySound(SoundID.Item29, Projectile.Center);
                Projectile.timeLeft = (int)Time;
            }

            Vector2 controlpoint = spawnPos + (spawnPos.DirectionTo(pos) * spawnPos.Distance(pos) * 0.75f).RotatedBy(MathHelper.PiOver2);
            Projectile.Center = LemonUtils.BezierCurve(spawnPos, pos, controlpoint, AITimer / Time);

            AITimer++;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            var shader = GameShaders.Misc["TerrorMod:ProjectileLightShader"];
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, shader.Shader, Main.GameViewMatrix.TransformationMatrix);
            shader.Shader.Parameters["time"].SetValue(AITimer / 60f);
            shader.Shader.Parameters["color"].SetValue(Color.White.ToVector4());
            shader.Apply();
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
