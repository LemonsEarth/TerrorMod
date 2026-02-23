namespace TerrorMod.Content.NPCs.Bosses.BossAdds;

public class ForgottenSibling : ModNPC
{
    public override string Texture => $"Terraria/Images/NPC_" + NPCID.Probe;

    public override void SetStaticDefaults()
    {
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        NPCID.Sets.MPAllowedEnemies[Type] = true;
        NPCID.Sets.TrailCacheLength[NPC.type] = 5;
        NPCID.Sets.TrailingMode[NPC.type] = 3;
        Main.npcFrameCount[NPC.type] = 1;
        NPCID.Sets.CantTakeLunchMoney[Type] = true;
        NPCID.Sets.DontDoHardmodeScaling[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Probe);
        AIType = NPCID.Probe;
        NPC.lifeMax = 2500;
        NPC.color = Color.LightBlue;
        NPC.scale = 5f;
        NPC.noTileCollide = true;
        NPC.color = Color.Blue;
    }

    public override void AI()
    {
        if (!(NPC.AnyNPCs(NPCID.Spazmatism) || NPC.AnyNPCs(NPCID.Retinazer)))
        {
            NPC.velocity.Y -= 5;
        }
        NPC.color = Color.Blue;
        //NPC retinazer = Main.npc.First(npc => npc.type == NPCID.Retinazer);
        //NPC spazmatism = Main.npc.First(npc => npc.type == NPCID.Spazmatism);

        //if (retinazer != null && retinazer.active)
        //{
        //    for (float i = 0.05f; i < 1f; i += 0.05f)
        //    {
        //        Vector2 pos = Vector2.Lerp(NPC.Center, retinazer.Center, i);
        //        for (int j = 0; j < 3; j++)
        //        {
        //            Dust.NewDustDirect(pos, 1, 1, DustID.GemRuby).noGravity = true;
        //        }
        //    }
        //}

        //if (spazmatism != null && spazmatism.active)
        //{
        //    for (float i = 0.05f; i < 1f; i += 0.05f)
        //    {
        //        Vector2 pos = Vector2.Lerp(NPC.Center, spazmatism.Center, i);
        //        for (int j = 0; j < 3; j++)
        //        {
        //            Dust.NewDustDirect(pos, 1, 1, DustID.GemEmerald).noGravity = true;
        //        }
        //    }
        //}
    }

    public override bool CheckActive()
    {
        return true;
    }
}
