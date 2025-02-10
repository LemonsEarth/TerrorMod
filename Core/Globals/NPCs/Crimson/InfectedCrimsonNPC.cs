using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Crimson;

namespace TerrorMod.Core.Globals.NPCs.Crimson
{
    public class InfectedCrimsonNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool infectedCrimson = false;
        int crimsonTimer = 0;
        int maxCrimsonTimer = 420;

        public override void ResetEffects(NPC npc)
        {
            infectedCrimson = false;
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return !entity.boss
                && entity.type != NPCID.Creeper
                && entity.type != NPCID.ServantofCthulhu
                && entity.type != NPCID.EaterofWorldsBody
                && entity.type != NPCID.EaterofWorldsTail
                && entity.type != NPCID.SkeletronHand;
        }

        public override void PostAI(NPC npc)
        {
            if (!npc.dontTakeDamage && !NPCLists.SafeNPCs.Contains(npc.type) && !npc.SpawnedFromStatue
                && npc.HasValidTarget && Main.player[npc.target].ZoneCrimson && npc.life < 50)
            {
                npc.AddBuff(ModContent.BuffType<InfectedCrimson>(), 180);
            }

            if (infectedCrimson)
            {
                crimsonTimer++;
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Crimson);
            }

            if (crimsonTimer > maxCrimsonTimer)
            {
                LemonUtils.DustCircle(npc.Center, 8, 1, DustID.GemRuby, 2f);
                SoundEngine.PlaySound(npc.DeathSound, npc.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC(npc.GetSource_Death("Crimsoned"), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<CrimsonCarrier>());
                }
                npc.active = false;
            }
        }
    }
}
