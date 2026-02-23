using Terraria.Audio;

namespace TerrorMod.Content.Projectiles.Hostile;

public class MeteorHostile : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Meteor1;

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.Meteor1);
        AIType = ProjectileID.Meteor1;
        Projectile.aiStyle = ProjAIStyleID.Arrow;
        Projectile.hostile = true;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        SoundEngine.PlaySound(SoundID.Item89);
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            int num = Main.rand.Next(1, 4);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ProjectileType<BombExplosion>(), 20, 1);
            for (int i = 0; i < num; i++)
            {
                NPC.NewNPC(Projectile.GetSource_FromAI(), (int)Projectile.Center.X + Main.rand.Next(-20, 20), (int)Projectile.Center.Y + Main.rand.Next(-20, 20), NPCID.MeteorHead);
            }
        }
        return true;
    }
}
