using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorMod.Core.Globals.NPCs.Dungeon
{
    public class Skeletons : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.AngryBones
                || entity.type == NPCID.Skeleton
                || entity.type == NPCID.UndeadMiner
                || entity.type == NPCID.UndeadViking
                || entity.type == NPCID.BoneThrowingSkeleton
                || entity.type == NPCID.CursedSkull
                || entity.type == NPCID.BoneSerpentHead
                || entity.type == NPCID.DarkCaster
                || entity.type == NPCID.GreekSkeleton
                || entity.type == NPCID.HeadacheSkeleton
                || entity.type == NPCID.MisassembledSkeleton
                || entity.type == NPCID.PantlessSkeleton;
        }

        public override void OnKill(NPC npc)
        {
            if (Main.rand.NextBool(4))
            {
                NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y, npc.type, npc.whoAmI);
            }
            else if (Main.rand.NextBool(4))
            {
                NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.CursedSkull, npc.whoAmI);
            }
        }
    }
}
