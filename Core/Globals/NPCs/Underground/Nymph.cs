namespace TerrorMod.Core.Globals.NPCs.Underground;

public class Nymph : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.Nymph || entity.type == NPCID.LostGirl;
    }

    public override void PostAI(NPC npc)
    {
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];
        player.velocity += player.Center.DirectionTo(npc.Center) * 0.2f;
        for (float i = 0.05f; i < 1f; i += 0.05f)
        {
            Vector2 pos = Vector2.Lerp(npc.Center, player.Center, i);
            for (int j = 0; j < 3; j++)
            {
                Dust.NewDustDirect(pos, 1, 1, DustID.PinkCrystalShard).noGravity = true;
            }
        }
    }

    public override void UpdateLifeRegen(NPC npc, ref int damage)
    {
        npc.lifeRegen += 10;
    }
}
