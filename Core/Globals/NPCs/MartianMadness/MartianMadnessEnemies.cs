namespace TerrorMod.Core.Globals.NPCs.MartianMadness;

public class MartianMadnessEnemies : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;

    public override void SetDefaults(NPC entity)
    {

    }

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.BrainScrambler
            || entity.type == NPCID.RayGunner
            || entity.type == NPCID.GrayGrunt
            || entity.type == NPCID.MartianDrone
            || entity.type == NPCID.MartianEngineer
            || entity.type == NPCID.MartianOfficer
            || entity.type == NPCID.MartianWalker
            || entity.type == NPCID.ForceBubble
            || entity.type == NPCID.MartianSaucer
            || entity.type == NPCID.MartianSaucerCore
            || entity.type == NPCID.MartianSaucerTurret
            || entity.type == NPCID.MartianSaucerCannon
            || entity.type == NPCID.Scutlix
            || entity.type == NPCID.ScutlixRider
            || entity.type == NPCID.MartianTurret
            || entity.type == NPCID.GigaZapper;
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        if (Main.rand.NextBool(3))
        {
            target.AddBuff(BuffID.VortexDebuff, 90);
        }

        if (Main.rand.NextBool(3))
        {
            target.AddBuff(BuffID.Electrified, 90);
        }
    }

    public override void AI(NPC npc)
    {
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];

        if (npc.type == NPCID.ScutlixRider)
        {
            if (npc.ai[1] < 40) npc.ai[1] = 40;
        }

        else if (npc.type == NPCID.MartianTurret)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && AITimer % 150 == 0)
            {
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.DirectionTo(player.Center) * 8, ProjectileID.CultistBossLightningOrbArc, (int)(npc.damage * 1.5f), 1f, ai0: npc.DirectionTo(player.Center).ToRotation());
            }
        }

        if (npc.type == NPCID.MartianSaucerCore)
        {
            
        }

        AITimer++;
    }

    public override void OnKill(NPC npc)
    {
        
    }

    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        
    }
}
