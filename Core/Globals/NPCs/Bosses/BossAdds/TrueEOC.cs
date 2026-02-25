using TerrorMod.Content.Buffs.Debuffs.Movement;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses.BossAdds;

public class TrueEOC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;
    bool canTeleport = false;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.MoonLordFreeEye;
    }

    public override void AI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];
        npc.ai[1]++;
        if (AITimer % 900 == 0 && AITimer > 0)
        {
            canTeleport = true;    
        }

        if (canTeleport && npc.ai[0] == 0)
        {
            canTeleport = false;
            Vector2 pos = player.Center + new Vector2(Math.Sign(player.velocity.X) * 600, 0);
            npc.Teleport(pos);
        }

        AITimer++;
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        if (Main.rand.NextBool(4)) target.AddBuff(BuffType<FearDebuff>(), 120);
        if (Main.rand.NextBool(4)) target.AddBuff(BuffType<Weight>(), 60);
    }
}
