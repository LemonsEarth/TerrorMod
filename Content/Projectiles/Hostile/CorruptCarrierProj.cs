﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Common.Utils.Prim;

namespace TerrorMod.Content.Projectiles.Hostile
{
    public class CorruptCarrierProj : ModProjectile
    {
        string GlowMask_Path => Texture + "_Glow";
        static Asset<Texture2D> GlowMask;

        ref float AITimer => ref Projectile.ai[0];

        static BasicEffect BasicEffect;
        GraphicsDevice GraphicsDevice => Main.instance.GraphicsDevice;

        public override void Load()
        {
            Main.RunOnMainThread(() =>
            {
                BasicEffect = new BasicEffect(GraphicsDevice)
                {
                    TextureEnabled = true,
                    VertexColorEnabled = true,
                };
            });
            
        }

        public override void Unload()
        {
            BasicEffect?.Dispose();
            BasicEffect = null;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            GlowMask = ModContent.Request<Texture2D>(GlowMask_Path);
        }

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
            if (AITimer == 0)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath13, Projectile.Center);
            }

            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 7;
            }

            Projectile.velocity *= 1.05f;

            LemonUtils.DustCircle(Projectile.Center, 4, 3, DustID.Corruption);
            Projectile.rotation = MathHelper.ToRadians(AITimer * 6);

            AITimer++;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.Zero), ProjectileID.VilePowder, 0, 1);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            PrimHelper.DrawBasicProjectilePrimTrail(Projectile, 12, Color.MediumPurple, Color.Black, BasicEffect, GraphicsDevice);

            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            Main.EntitySpriteDraw(GlowMask.Value, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, Color.White, Projectile.rotation, GlowMask.Value.Size() * 0.5f, 1f, SpriteEffects.None);
        }
    }
}
