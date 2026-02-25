using TerrorMod.Content.Buffs.Debuffs.Movement;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses.BossAdds;

public class WallOfFleshEye : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.WallofFleshEye;
    }

    public override void AI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];
        //ai1 is a timer for lasers (goes to ~600 and then ~60 between each laser)

        //Main.NewText("ai0: " + npc.ai[0]);
        //Main.NewText("ai1: " + npc.ai[1]);
        //Main.NewText("ai2: " + npc.ai[2]);
        //Main.NewText("ai3: " + npc.ai[3]);
        //Main.NewText("ai0: " + npc.localAI[0]);
        //Main.NewText("ai1: " + npc.localAI[1]);
        //Main.NewText("ai2: " + npc.localAI[2]);
        //Main.NewText("ai3: " + npc.localAI[3]);
        AITimer++;
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        //if (npc.ai[1] == 1) modifiers.FinalDamage *= 0.5f; 
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        target.AddBuff(BuffType<FearDebuff>(), 90);
        target.AddBuff(BuffID.Weak, 120);
    }
}
