using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;

namespace TerrorMod.Content.Projectiles.Hostile;

public class DoomSphere : ModProjectile
{
    int AITimer = 0;
    ref float Size => ref Projectile.ai[0];
    const string NoisePath = "TerrorMod/Common/Assets/Textures/NoiseTexture";
    static Asset<Texture2D> Noise;
    float scale = 1f;

    ref float Rotation => ref Projectile.ai[1];
    ref float RotPerSecond => ref Projectile.ai[2];

    public override void Load()
    {
        Noise = Request<Texture2D>(NoisePath);
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(Projectile.timeLeft);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        Projectile.timeLeft = reader.ReadInt32();
    }

    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 3;
        ProjectileID.Sets.DrawScreenCheckFluff[Type] = 5000;
    }

    public override void SetDefaults()
    {
        Projectile.width = 200;
        Projectile.height = 200;
        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 120;
        Projectile.penetrate = -1;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Rotation;
    }

    public override void ModifyDamageHitbox(ref Rectangle hitbox)
    {
        
    }

    public override void AI()
    {
        if (AITimer == 0)
        {
            if (Size == 0) Size = 1;
            SoundEngine.PlaySound(SoundID.Zombie104 with { MaxInstances = 0}, Projectile.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ProjectileType<DoomLaser>(), Projectile.damage, 2f, ai0: Size / 2, ai1: Rotation, ai2: RotPerSecond);
                proj.timeLeft = Projectile.timeLeft;
                NetMessage.SendData(MessageID.SyncProjectile, number: proj.whoAmI);
            }
        }
        Projectile.damage = 10;
        Projectile.rotation = Rotation;
        Rotation += RotPerSecond;
        scale = AITimer / 5f;
        if (Projectile.timeLeft < 15) scale = Projectile.timeLeft / 5f;
        scale = MathHelper.Clamp(scale, 0f, 1f);
        AITimer++;
    }

    public override void OnKill(int timeLeft)
    {

    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Vector2 drawOrigin = texture.Size() / 2;
        Vector2 drawPos = Projectile.Center;

        //Main.EntitySpriteDraw(texture, drawPos - Main.screenPosition, texture.Frame(1, 3, 0, 0), Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None);
        var shader = GameShaders.Misc["TerrorMod:SphereShader"];
        shader.Shader.Parameters["moveSpeed"].SetValue(-2f);
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, shader.Shader, Main.GameViewMatrix.TransformationMatrix);
        Main.instance.GraphicsDevice.Textures[1] = Noise.Value;
        shader.Apply();
        Main.EntitySpriteDraw(texture, drawPos - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Size, SpriteEffects.None, 0);
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        return false;
    }

    public override void PostDraw(Color lightColor)
    {

    }
}
