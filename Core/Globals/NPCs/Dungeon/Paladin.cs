namespace TerrorMod.Core.Globals.NPCs.Dungeon;

public class Paladin : GlobalNPC
{
    public override bool InstancePerEntity => true;
    int AITimer = 0;
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.Paladin;
    }

    public override void AI(NPC npc)
    {
        if (AITimer % 120 == 0)
        {
            foreach (var enemy in Main.ActiveNPCs)
            {
                if (enemy.CanBeChasedBy() && enemy.Distance(npc.Center) < 1000 && enemy.type != NPCID.Paladin)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int healAmount = enemy.life + (enemy.lifeMax / 100) <= enemy.lifeMax ? enemy.lifeMax / 20 : enemy.lifeMax - enemy.life;
                        healAmount = (int)MathHelper.Clamp(healAmount, 10, 40);
                        if (healAmount > 0)
                        {
                            enemy.HealEffect(healAmount);
                        }
                        enemy.life += healAmount;
                    }
                    enemy.netUpdate = true;
                }
            }
        }
        AITimer++;
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        modifiers.FinalDamage *= 0.75f;
    }
}
