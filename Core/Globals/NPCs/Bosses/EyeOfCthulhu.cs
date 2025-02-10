using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class EyeOfCthulhu : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override void SetDefaults(NPC entity)
        {
            entity.lifeMax = 3000;
            entity.defense = 16;
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.EyeofCthulhu;
        }

        public override void AI(NPC npc)
        {
            if (!npc.HasValidTarget) return;

            if (npc.ai[0] == 0) // if in phase 1
            {
                if (npc.ai[3] < 20) npc.ai[3] = 20; // Servant spawn timer in p1
            }
            else
            {
                if (npc.alpha < 255 && npc.life > npc.lifeMax / 4) npc.alpha++;

                if (npc.life <= npc.lifeMax / 4)
                {
                    npc.alpha = 0;
                    
                }
                if (npc.velocity.Length() > 14) // if dashing
                {
                    if (AITimer % 20 == 0)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu, npc.whoAmI, ai3: 0.1f);
                        }
                    }
                }
            }
            AITimer++;
        }

        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            return npc.ai[0] == 0;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Darkness, 120);
            target.AddBuff(ModContent.BuffType<FearDebuff>(), 60);
        }
    }
}
