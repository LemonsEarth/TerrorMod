namespace TerrorMod.Core.Globals.NPCs.Hallow;

public class HallowNPC : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.ChaosElemental
            || entity.type == NPCID.Pixie
            || entity.type == NPCID.Unicorn
            || entity.type == NPCID.DesertGhoulHallow
            || entity.type == NPCID.SandsharkHallow
            || entity.type == NPCID.IlluminantBat
            || entity.type == NPCID.IlluminantSlime
            || entity.type == NPCID.Gastropod
            || entity.type == NPCID.LightMummy
            || entity.type == NPCID.PigronHallow
            || entity.type == NPCID.QueenSlimeMinionBlue
            || entity.type == NPCID.QueenSlimeMinionPurple
            || entity.type == NPCID.QueenSlimeMinionPink
            || entity.type == NPCID.QueenSlimeBoss
            || entity.type == NPCID.HallowBoss;
    }

    public override void OnKill(NPC npc)
    {
        if (!npc.HasValidTarget) return;
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            if (Main.rand.NextBool(3))
            {
                Vector2 pos = Main.player[npc.target].Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 900;
                NPC.NewNPC(npc.GetSource_FromAI(), (int)pos.X, (int)pos.Y, NPCID.EnchantedSword);
            }    
        }
    }
}
