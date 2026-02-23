namespace TerrorMod.Core.Globals.NPCs.Underground;

public class Mimics : GlobalNPC
{
    public override bool InstancePerEntity => true;
    int AITimer = 0;
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.aiStyle == NPCAIStyleID.Mimic;
    }

    public override void AI(NPC npc)
    {
        if (AITimer % 30 == 0)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.rand.NextBool(4))
                {
                    npc.velocity.X *= 4;
                }
            }
            npc.netUpdate = true;
        }

        npc.ai[2] = 20;
        AITimer++;
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        if (Main.rand.NextBool(4))
        {
            Item heldItem = target.HeldItem;
            target.TryDroppingSingleItem(npc.GetSource_OnHit(target), heldItem);
        }
    }
}
