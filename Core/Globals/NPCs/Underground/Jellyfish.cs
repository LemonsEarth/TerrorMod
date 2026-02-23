namespace TerrorMod.Core.Globals.NPCs.Underground;

public class Jellyfish : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.aiStyle == NPCAIStyleID.Jellyfish;
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        target.AddBuff(BuffID.Venom, 120);
        target.AddBuff(BuffID.Electrified, 60);
    }
}
