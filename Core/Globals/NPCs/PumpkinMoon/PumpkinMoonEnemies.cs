using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Forest;

namespace TerrorMod.Core.Globals.NPCs.PumpkinMoonEnemies;

public class PumpkinMoonEnemies : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;

    public override void SetDefaults(NPC entity)
    {

    }

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.Scarecrow1
            || entity.type == NPCID.Scarecrow2
            || entity.type == NPCID.Scarecrow3
            || entity.type == NPCID.Scarecrow4
            || entity.type == NPCID.Scarecrow5
            || entity.type == NPCID.Scarecrow6
            || entity.type == NPCID.Scarecrow7
            || entity.type == NPCID.Scarecrow8
            || entity.type == NPCID.Scarecrow9
            || entity.type == NPCID.Scarecrow10
            || entity.type == NPCID.Splinterling
            || entity.type == NPCID.Hellhound
            || entity.type == NPCID.Poltergeist
            || entity.type == NPCID.HeadlessHorseman
            || entity.type == NPCID.MourningWood
            || entity.type == NPCID.Pumpking
            || entity.type == NPCID.PumpkingBlade;
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        if (Main.rand.NextBool(3))
        {
            target.AddBuff(BuffType<FearDebuff>(), 60);
        }

        if (Main.rand.NextBool(3))
        {
            target.AddBuff(BuffType<Weight>(), 120);
        }
    }

    public override void AI(NPC npc)
    {
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];

        if (npc.type == NPCID.Poltergeist)
        {
            if (AITimer % 100 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.DirectionTo(player.Center) * 10, ProjectileID.LostSoulHostile, npc.damage / 4, 1f);
                }
            }
        }

        if (npc.type == NPCID.HeadlessHorseman)
        {
            if (npc.localAI[0] < 300) npc.localAI[0] = 300;
        }

        if (npc.type == NPCID.Hellhound || npc.type == NPCID.HeadlessHorseman)
        {
            if (AITimer % 60 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.rand.NextBool(4)) npc.velocity.Y -= 10;
                }
                npc.netUpdate = true;
            }
        }

        if (npc.type == NPCID.MourningWood)
        {
            if (npc.ai[0] == 0)
            {
                if (npc.ai[1] < 180) npc.ai[1] = 180;
            }
        }

        if (npc.type == NPCID.Pumpking)
        {
            if (npc.ai[2] < 240 && npc.ai[2] > 60)
            {
                npc.ai[2] = 240;
                npc.localAI[2] = 240;
            }

            if (AITimer % 180 == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileID.HorsemanPumpkin, npc.damage / 4, 1f);
                }
            }
        }

        if (npc.type == NPCID.PumpkingBlade)
        {
            if (npc.ai[3] > 0 && npc.ai[3] < 150)
            {
                npc.ai[3] = 150;
            }
        }

        AITimer++;
    }

    public override void OnKill(NPC npc)
    {
        if (npc.type == NPCID.MourningWood)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < 4; i++)
                {
                    NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.position.X + Main.rand.Next(0, npc.width), (int)npc.position.Y + Main.rand.Next(0, npc.height), NPCType<TreeSpirit>());
                }
            }
        }
    }

    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        if (npc.type == NPCID.MourningWood)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.rand.NextBool(20))
                {
                    NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.position.X + Main.rand.Next(0, npc.width), (int)npc.position.Y + Main.rand.Next(0, npc.height), NPCType<TreeSpirit>());
                }
            }
        }
    }
}
