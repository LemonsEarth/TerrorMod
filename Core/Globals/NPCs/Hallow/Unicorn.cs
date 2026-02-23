namespace TerrorMod.Core.Globals.NPCs.Hallow;

public class Unicorn : GlobalNPC
{
    public override bool InstancePerEntity => true;
    int AITimer = 0;
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.Unicorn;
    }

    public override void AI(NPC npc)
    {
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];
        if (AITimer % 300 == 0)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileID.FairyQueenLance, npc.damage / 4, 1f, ai0: npc.DirectionTo(player.Center).ToRotation());
            }
        }
        AITimer++;
    }
}
