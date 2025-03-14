using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
    public class DoomLaser : ModProjectile
    {
        int AITimer = 0;
        ref float Size => ref Projectile.ai[0];
        const string NoisePath = "TerrorMod/Common/Assets/Textures/NoiseTexture";
        static Asset<Texture2D> Noise;
        float scale = 1f;
        float laserLength = 9f;

        ref float Rotation => ref Projectile.ai[1];
        ref float RotPerSecond => ref Projectile.ai[2];

        public override void Load()
        {
            Noise = ModContent.Request<Texture2D>(NoisePath);
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 5000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 280;
            Projectile.height = 280;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = float.NaN;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Vector2.UnitY.RotatedBy(Rotation) * laserLength * Projectile.height, Projectile.width * 0.7f * Size, ref _);
        }

        public override void AI()
        {
            if (AITimer == 0)
            {
                SoundEngine.PlaySound(SoundID.Zombie104 with { MaxInstances = 0 }, Projectile.Center);
                if (Size == 0) Size = 1;
            }
            Projectile.velocity = Vector2.Zero;
            scale = (AITimer / 5f) * Size;
            Projectile.rotation = Rotation;
            Rotation += RotPerSecond;
            if (Projectile.timeLeft < 15)
            {
                scale = (Projectile.timeLeft * Size) / 5f;
            }
            scale = MathHelper.Clamp(scale, 0f, Size);
            AITimer++;
        }

        public override void OnKill(int timeLeft)
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Size().X / 2, 0f);
            Vector2 drawPos = Projectile.Center;

            //Main.EntitySpriteDraw(texture, drawPos - Main.screenPosition, texture.Frame(1, 3, 0, 0), Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None);
            var shader = GameShaders.Misc["TerrorMod:LaserShader"];
            shader.Shader.Parameters["moveSpeed"].SetValue(-2f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, shader.Shader, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.Textures[1] = Noise.Value;
            shader.Apply();
            Main.EntitySpriteDraw(texture, drawPos - Main.screenPosition, null, Color.Blue, Projectile.rotation, drawOrigin, new Vector2(scale, laserLength), SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public override void PostDraw(Color lightColor)
        {

        }
    }
}
