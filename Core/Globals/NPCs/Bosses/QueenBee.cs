using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses;

public class QueenBee : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.QueenBee;
    }

    public override void AI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        // ai0 is state
        // 0 - dashing
        // 1 - summoning bees
        // 2 - flying around randomly?
        // 3 - firing stingers

        // ai1 is a timer
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];
        //if (!Terraria.Graphics.Effects.Filters.Scene["TerrorMod:DesaturateShader"].IsActive() && Main.netMode != NetmodeID.Server)
        //{
        //    Terraria.Graphics.Effects.Filters.Scene.Activate("TerrorMod:DesaturateShader");
        //}
        switch (npc.ai[0])
        {
            case 1:
                if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[1] == 0 && npc.life < npc.lifeMax * 0.7f)
                {
                    int chance = npc.life < npc.lifeMax * 0.25f ? 3 : 6;
                    if (Main.rand.NextBool(chance))
                    {
                        NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.Hornet);
                    }
                }
                break;
            case 2:
                if (AITimer % 10 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        npc.ai[0] = Main.rand.NextFromList(0, 1, 3);
                    }
                    npc.netUpdate = true;
                }
                break;
            case 3:
                int rate = npc.life < npc.lifeMax * 0.36f ? 60 : 120;
                if (npc.ai[1] % rate == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.Center.DirectionTo(player.Center) * 12, ProjectileID.BeeHive, npc.damage / 3, 1);
                    }
                }

                break;
        }

        //Main.NewText("ai0: " + npc.ai[0]);
        //Main.NewText("ai1: " + npc.ai[1]);
        //Main.NewText("ai2: " + npc.ai[2]);
        //Main.NewText("ai3: " + npc.ai[3]);
        AITimer++;
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        
    }
}
