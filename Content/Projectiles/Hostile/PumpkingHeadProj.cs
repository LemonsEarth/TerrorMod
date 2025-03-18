using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorMod.Content.Projectiles.Hostile
{
    public class PumpkingHeadProj : ModProjectile
    {
        int AITimer = 0;
        int frame = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.scale = 1f;
            Projectile.hide = true;
        }

        public override void AI()
        {
            if (AITimer == 0)
            {
                SoundEngine.PlaySound(TerrorMod.Jumpscare, Projectile.Center);
                Projectile.Opacity = 0f;
                frame = Main.rand.Next(0, 3);
            }

            Projectile.velocity = Vector2.Zero;
            Projectile.Center = Main.player[Projectile.owner].Center;
            
            if (Projectile.timeLeft > 100)
            {
                Projectile.Opacity = MathHelper.Lerp(0, 1, AITimer / 5f);
            }
            else if (Projectile.timeLeft < 30)
            {
                Projectile.Opacity = MathHelper.Lerp(0, 1, Projectile.timeLeft / 30f);
            }

            AITimer++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = texture.Frame(1, 10, 0, Projectile.frame).Size() * 0.5f;
            Main.EntitySpriteDraw(texture, new Vector2(Main.screenWidth, Main.screenHeight / 4) * 0.5f, texture.Frame(1, 3, 0, frame), Color.White * Projectile.Opacity, Projectile.rotation, drawOrigin, Projectile.scale * 10, SpriteEffects.None);
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }
    }
}
