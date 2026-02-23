using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses;

public class LunaticCultist : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;
    bool summonedDragons = false;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.CultistBoss;
    }

    public override void AI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];

        if (npc.life <= npc.lifeMax / 2 && !summonedDragons)
        {
            summonedDragons = true;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X + 100, (int)npc.Center.Y, NPCID.CultistDragonHead);
                NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X - 100, (int)npc.Center.Y, NPCID.CultistDragonHead);
            }
        }

        switch (npc.ai[0])
        {
            case 3: // fireballs
                if (npc.ai[1] == 30)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 dir = Vector2.UnitY.RotatedBy(i * MathHelper.PiOver4);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 5, ProjectileID.CultistBossFireBallClone, npc.damage / 4, 1f);
                        }
                    }
                }
                break;
            case 4:
                if (npc.ai[1] == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Vector2 dir = Vector2.UnitY.RotatedBy(i * MathHelper.ToRadians(60));
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - Vector2.UnitY * 100, dir * 2, ProjectileID.CultistBossLightningOrb, npc.damage / 4, 1f);
                        }
                    }
                }
                break;
        }

        AITimer++;
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;

        modifiers.FinalDamage *= 0.6f;
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        if (Main.rand.NextBool(4)) target.AddBuff(BuffID.OnFire, 120);
        if (Main.rand.NextBool(4)) target.AddBuff(BuffID.Frostburn, 120);
        if (Main.rand.NextBool(4)) target.AddBuff(BuffID.CursedInferno, 120);
        if (Main.rand.NextBool(4)) target.AddBuff(BuffID.Ichor, 120);
        if (Main.rand.NextBool(4)) target.AddBuff(BuffID.ShadowFlame, 120);
        if (Main.rand.NextBool(4)) target.AddBuff(BuffID.WitheredArmor, 120);
        if (Main.rand.NextBool(4)) target.AddBuff(BuffID.WitheredWeapon, 120);
        if (Main.rand.NextBool(4)) target.AddBuff(BuffID.Obstructed, 120);
    }
}
