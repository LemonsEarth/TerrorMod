using TerrorMod.Common.Utils;
using TerrorMod.Content.Projectiles.Hostile;
using System.Linq;
using TerrorMod.Core.Configs;
using TerrorMod.Content.NPCs.Bosses;
using Terraria.Chat;
using Terraria.Localization;

namespace TerrorMod.Core.Globals.NPCs.Bosses;

public class MoonLord : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.MoonLordCore
            || entity.type == NPCID.MoonLordHand
            || entity.type == NPCID.MoonLordHead;
    }

    public override void AI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];

        if (npc.type == NPCID.MoonLordCore)
        {
            if (AITimer == 540)
            {
                Vector2 pos = npc.Center;
                LemonUtils.DustCircle(pos, 32, 15, DustID.GemDiamond, 4f);
                LemonUtils.DustCircle(pos, 32, 10, DustID.GemDiamond, 4f);
                LemonUtils.DustCircle(pos, 32, 5, DustID.GemDiamond, 4f);
            }
            if (AITimer >= 600)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 pos = player.Center + new Vector2(Math.Sign(player.velocity.X) * 300, -1500);
                    Projectile.NewProjectile(npc.GetSource_FromAI(), pos, Vector2.Zero, ProjectileType<DoomSphere>(), 50, 2f, ai0: 2f);
                }
                AITimer = 0;
            }
        }
        else if (npc.type == NPCID.MoonLordHand)
        {
            if (npc.life <= npc.lifeMax * 0.25f)
            {
                if (npc.ai[0] != 2)
                {
                    npc.ai[1] += 4;
                }
                if (npc.ai[0] == 2)
                {
                    if (npc.ai[1] < 300)
                    {
                        npc.ai[1] += 7;
                    }
                }
            }
            else if (npc.life <= npc.lifeMax * 0.5f)
            {
                if (npc.ai[0] != 2)
                {
                    npc.ai[1] += 3;
                }
                if (npc.ai[0] == 2)
                {
                    if (npc.ai[1] < 300)
                    {
                        npc.ai[1] += 3;
                    }
                }
            }
            else if (npc.life <= npc.lifeMax * 0.75f)
            {
                if (npc.ai[0] != 2)
                {
                    npc.ai[1] += 1;
                }
                if (npc.ai[0] == 2)
                {
                    if (npc.ai[1] < 300)
                    {
                        npc.ai[1] += 1;
                    }
                }
            }
        }

        AITimer++;
    }

    bool placed = false;
    public override void OnKill(NPC npc)
    {
        if (NPC.downedMoonlord || placed) return;
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            int counter = 0;
            while (counter < 1000)
            {
                int x = WorldGen.genRand.Next((int)(Main.maxTilesX * 0.33f), (int)(Main.maxTilesX * 0.66f));
                int y = WorldGen.genRand.Next(80, (int)Main.worldSurface - 80);
                if (!Main.tile[x, y].HasTile && Main.tile[x, y].WallType == 0
                    && !Main.npc.Any(n => n.active && n.type == NPCType<InfiniteTerrorCage>()))
                {
                    NPC.NewNPC(npc.GetSource_FromAI(), x * 16, y * 16, NPCType<InfiniteTerrorCage>());

                    placed = true;
                }
                counter++;
                if (placed)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.TerrorMod.Messages.InfiniteTerrorCageSpawn.SpawnMessage"), Color.DarkGray);
                    break;
                }
            }
        }
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;

    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {

    }
}
