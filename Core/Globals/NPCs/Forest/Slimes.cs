using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Core.Globals.NPCs.Forest
{
    public class Slimes : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.BlueSlime
                || entity.type == NPCID.Slimer
                || entity.type == NPCID.MotherSlime
                || entity.type == NPCID.Crimslime
                || entity.type == NPCID.CorruptSlime
                || entity.type == NPCID.IlluminantSlime
                || entity.type == NPCID.Slimer
                || entity.type == NPCID.QueenSlimeMinionBlue
                || entity.type == NPCID.QueenSlimeMinionPink
                || entity.type == NPCID.QueenSlimeMinionPurple
                || entity.type == NPCID.IceSlime
                || entity.type == NPCID.ShimmerSlime
                || entity.type == NPCID.SandSlime
                || entity.type == NPCID.SlimeSpiked
                || entity.type == NPCID.SpikedIceSlime
                || entity.type == NPCID.SpikedJungleSlime;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OgreSpit, 60);
            if (npc.netID == NPCID.Pinky)
            {
                target.AddBuff(BuffID.VortexDebuff, 120);
            }

            if (npc.type == NPCID.MotherSlime || npc.netID == NPCID.BlackSlime || npc.netID == NPCID.RedSlime)
            {
                target.AddBuff(ModContent.BuffType<Weight>(), 240);
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.netID == NPCID.Pinky)
            {
                npc.ai[0] = -2000; // setting it to anything negative just makes the slime jump constantly, at -2000 is the highest jump a slime can do
            }
            else if (npc.type == NPCID.BlueSlime)
            {
                npc.ai[0] += 3; // ai0 is the jump timer for slimes. It decrements every frame for 120 frames before jumping, this just speeds it up
            }
            return true;
        }
    }
}
