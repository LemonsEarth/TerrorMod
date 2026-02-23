namespace TerrorMod.Core.Globals.NPCs.Meteorite;

public class MeteorHead : GlobalNPC
{
    int AITimer = 0;

    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.MeteorHead;
    }

    public override void SetDefaults(NPC entity)
    {
        entity.knockBackResist = 0f;
    }

    public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
    {
        if (Main.hardMode)
        {
            npc.lifeMax *= 4;
        }
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (Main.hardMode)
        {
            modifiers.FinalDamage /= 2;
        }
    }

    public override void AI(NPC npc)
    {
        if (AITimer % 180 == 0 && npc.HasValidTarget)
        {
            npc.velocity *= 6;
        }

        AITimer++;
    }
}
