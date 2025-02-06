using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Corruption;

namespace TerrorMod.Core.Globals.NPCs.Corruption
{
    public class InfectedCorruptNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool infectedCorrupt = false;
        int corruptionTimer = 0;
        int maxCorruptionTimer = 600;

        public override void ResetEffects(NPC npc)
        {
            infectedCorrupt = false;
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return !entity.boss;
        }

        public override void PostAI(NPC npc)
        {
            if (!npc.dontTakeDamage && npc.type != NPCID.CultistArcherBlue && npc.type != NPCID.CultistDevote && npc.type != NPCID.OldMan && !npc.SpawnedFromStatue
                && npc.HasValidTarget && Main.player[npc.target].ZoneCorrupt && npc.life < 50)
            {
                npc.AddBuff(ModContent.BuffType<InfectedCorrupt>(), 180);
            }

            if (infectedCorrupt)
            {
                corruptionTimer++;
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Corruption);
            }

            if (corruptionTimer > maxCorruptionTimer)
            {
                LemonUtils.DustCircle(npc.Center, 8, 5, DustID.Corruption, 3f);
                SoundEngine.PlaySound(npc.DeathSound, npc.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC(npc.GetSource_Death("Corrupted"), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<CorruptCarrier>());
                }
                npc.active = false;
            }
        }
    }
}
