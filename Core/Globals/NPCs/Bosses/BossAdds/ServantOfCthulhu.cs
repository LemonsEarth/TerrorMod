using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses.BossAdds;

public class ServantOfCthulhu : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.ServantofCthulhu;
    }

    public override void SetStaticDefaults()
    {
        NPCID.Sets.NeverDropsResourcePickups[NPCID.ServantofCthulhu] = true;
    }

    public override void SetDefaults(NPC entity)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        entity.lifeMax = 32;
        entity.DeathSound = null;
    }

    public override void AI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (npc.HasValidTarget && AITimer > 30)
        {
            npc.MoveToPos(Main.player[npc.target].Center, 0.001f, 0.001f, 0.2f + npc.ai[3], 0.2f + npc.ai[3]);
        }

        if (AITimer > 120)
        {
            npc.SimpleStrikeNPC(100, 0, noPlayerInteraction: true);
        }

        AITimer++;
    }

    public override void OnKill(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, Vector2.Zero, ProjectileType<ExplosionSmall>(), npc.damage, 1f);
        }
    }

    public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
    {
        return false;
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        target.AddBuff(BuffID.Darkness, 120);
        target.AddBuff(BuffType<FearDebuff>(), 20);
    }

    public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        npc.SimpleStrikeNPC(100, 0, noPlayerInteraction: true);
        modifiers.Cancel();
    }
}
