using Terraria.Audio;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.Projectiles.Hostile;

public class SkeletronGlock : ModProjectile
{
    ref float AITimer => ref Projectile.ai[0];
    ref float handIndex => ref Projectile.ai[1];

    public override void SetDefaults()
    {
        Projectile.width = 78;
        Projectile.height = 54;
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
            SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);
            LemonUtils.DustCircle(Projectile.Center, 8, 3, DustID.GemDiamond);
        }

        if (Main.npc[(int)handIndex] == null || !Main.npc[(int)handIndex].active || Main.npc[(int)handIndex].life <= 0 || Main.npc[(int)handIndex].type != NPCID.SkeletronHand)
        {
            Projectile.Kill();
        }

        Projectile.timeLeft = 10;

        if (Projectile.alpha > 0)
        {
            Projectile.alpha -= 7;
        }

        int sign = Main.npc[(int)handIndex].spriteDirection;
        if (AITimer % 60 == 0)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, sign * Vector2.UnitX.RotatedBy(Projectile.rotation) * 20, ProjectileID.BombSkeletronPrime, Projectile.damage, 1f);
            }
        }

        Projectile.spriteDirection = sign;
        Projectile.rotation = Main.npc[(int)handIndex].rotation + MathHelper.PiOver2 * sign;
        Projectile.Center = Main.npc[(int)handIndex].Center;

        AITimer++;
    }
}
