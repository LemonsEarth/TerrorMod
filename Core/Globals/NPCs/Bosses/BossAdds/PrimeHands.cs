using Terraria.Audio;
using TerrorMod.Common.Utils;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses.BossAdds;

public class PrimeHands : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.PrimeCannon
            || entity.type == NPCID.PrimeLaser
            || entity.type == NPCID.PrimeVice
            || entity.type == NPCID.PrimeSaw;
    }

    public override void SetDefaults(NPC entity)
    {

    }

    public override bool PreAI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return true;
        if (npc.type == NPCID.PrimeLaser && AITimer > 300 && AITimer < 600) return false;
        return true;
    }

    public override void AI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];

        switch (npc.type)
        {
            case NPCID.PrimeVice:
                if (AITimer == 240)
                {
                    SoundEngine.PlaySound(SoundID.Roar with { Pitch = 1f, Volume = 0.5f });
                    LemonUtils.DustCircle(npc.Center, 16, 10, DustID.GemRuby, 2f);
                }
                if (AITimer >= 300)
                {
                    npc.velocity = Vector2.Zero;
                    npc.velocity = npc.Center.DirectionTo(player.Center) * 50;
                    AITimer = 0;
                }
                break;
            case NPCID.PrimeSaw:
                npc.velocity = npc.Center.DirectionTo(player.Center) * 5;
                float sinMovement = (float)Math.Sin(AITimer / 5) * 5;
                Vector2 normal = npc.velocity.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero);
                npc.velocity += normal * sinMovement;
                npc.rotation = npc.Center.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2;
                break;
            case NPCID.PrimeLaser:

                break;
            case NPCID.PrimeCannon:
                if (npc.ai[3] >= 600)
                {
                    if (AITimer % 120 == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = -2; i <= 2; i++)
                            {
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(i * 15)) * 20, ProjectileID.BombSkeletronPrime, npc.damage / 2, 1f);
                            }
                        }
                    }
                }
                break;
        }

        AITimer++;
    }

    public override void PostAI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];

        switch (npc.type)
        {
            case NPCID.PrimeVice:

                break;
            case NPCID.PrimeSaw:

                break;
            case NPCID.PrimeLaser:
                if (AITimer > 300 && AITimer < 600)
                {
                    npc.MoveToPos(player.Center - Vector2.UnitY * 200, 0.7f, 0.7f, 1f, 0.7f);
                    if (AITimer > 400 && AITimer % 30 == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY * 5, ProjectileID.DeathLaser, npc.damage / 2, 1f);
                        }
                    }
                    npc.rotation = Vector2.UnitY.ToRotation() - MathHelper.PiOver2;
                    AITimer++;
                }
                if (AITimer > 600) AITimer = 0;
                break;
        }
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        if (npc.type == NPCID.PrimeVice || npc.type == NPCID.PrimeSaw)
        {
            target.AddBuff(BuffID.Cursed, 120);
            target.AddBuff(BuffID.Weak, 120);
            target.AddBuff(BuffID.Bleeding, 240);
            target.AddBuff(BuffID.WitheredArmor, 300);
            target.maxMinions = 0;
        }
    }
}
