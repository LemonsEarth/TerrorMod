using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class KingSlime : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        bool canClone = true;
        int AITimer = 0;
        bool original = false;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.KingSlime;
        }

        public override void AI(NPC npc)
        {
            if (npc.life > npc.lifeMax * 0.9f)
            {
                original = true;
            }

            if (!npc.HasValidTarget) return;

            if (original)
            {
                OriginalAI(npc);
            }
            else
            {
                CloneAI(npc);
            }

            AITimer++;
        }

        void OriginalAI(NPC npc)
        {
            if (npc.life < npc.lifeMax * 0.5f && canClone)
            {
                canClone = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC clone = NPC.NewNPCDirect(npc.GetSource_FromAI("Half-Health"), (int)Main.player[npc.target].Center.X, (int)Main.player[npc.target].position.Y - 1000, npc.type);
                    clone.life = Main.masterMode ? 1200 : 800;
                    clone.color = Color.Green;
                    NetMessage.SendData(MessageID.SyncNPC, number: clone.whoAmI);
                }
            }

            int attackInterval = npc.life < npc.lifeMax * 0.5f ? 480 : 300;

            if (AITimer % attackInterval == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int amount = Main.masterMode ? 8 : 4;
                    for (int i = 0; i < amount; i++)
                    {
                        Vector2 pos = Main.player[npc.target].Center + (Vector2.UnitY * 800).RotatedBy(i * (MathHelper.Pi / amount));
                        LemonUtils.DustCircle(pos, 8, 5, DustID.Granite, 3f);
                        NPC slime = NPC.NewNPCDirect(npc.GetSource_FromAI("PeriodicSlimeSpawn"), (int)pos.X, (int)pos.Y, NPCID.SlimeSpiked);
                    }
                }
            }
        }

        void CloneAI(NPC npc)
        {
            npc.ai[0] += 3; // Hop faster

            if (npc.ai[2] < 10)
            {
                npc.ai[2] = 270; // Teleport more often
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (!original)
            {
                for (int i = 0; i < 8; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.TintableDust, newColor: Color.Blue, Scale: 4f);
                }
                npc.active = false;
                return false;
            }
            return true;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OgreSpit, 60);
        }
    }
}
