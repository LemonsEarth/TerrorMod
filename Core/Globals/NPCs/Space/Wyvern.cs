using Terraria.DataStructures;

namespace TerrorMod.Core.Globals.NPCs.Space;

public class Wyvern : GlobalNPC
{
    int AITimer = 0;

    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.WyvernBody
            || entity.type == NPCID.WyvernBody2
            || entity.type == NPCID.WyvernBody3
            || entity.type == NPCID.WyvernHead
            || entity.type == NPCID.WyvernLegs
            || entity.type == NPCID.WyvernTail;
    }

    public override void SetDefaults(NPC entity)
    {
        entity.knockBackResist = 0f;
        entity.scale *= 3f;
    }

    public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
    {
        
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (Main.hardMode && (npc.type == NPCID.WyvernBody || npc.type == NPCID.WyvernBody2 || npc.type == NPCID.WyvernBody3 || npc.type == NPCID.WyvernLegs))
        {
            modifiers.FinalDamage /= 3;
        }
    }

    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        
    }

    public override void AI(NPC npc)
    {
        

        AITimer++;
    }
}
