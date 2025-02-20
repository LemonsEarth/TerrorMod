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
                || entity.type == NPCID.ArmoredViking
                || entity.type == NPCID.BoneThrowingSkeleton
                || entity.type == NPCID.CursedSkull
                || entity.type == NPCID.BoneSerpentHead
                || entity.type == NPCID.DarkCaster
                || entity.type == NPCID.GreekSkeleton
                || entity.type == NPCID.HeadacheSkeleton
                || entity.type == NPCID.MisassembledSkeleton
                || entity.type == NPCID.PantlessSkeleton
                || entity.type == NPCID.ArmoredSkeleton
                || entity.type == NPCID.SkeletonArcher
                || entity.type == NPCID.Tim
                || entity.type == NPCID.RuneWizard
                || entity.type == NPCID.GiantCursedSkull
                || entity.type == NPCID.Necromancer
                || entity.type == NPCID.DiabolistRed
                || entity.type == NPCID.DiabolistWhite
                || entity.type == NPCID.RaggedCaster
                || entity.type == NPCID.HellArmoredBones
                || entity.type == NPCID.HellArmoredBonesMace
                || entity.type == NPCID.HellArmoredBonesSpikeShield
                || entity.type == NPCID.HellArmoredBonesSword
                || entity.type == NPCID.BlueArmoredBones
                || entity.type == NPCID.BlueArmoredBonesMace
                || entity.type == NPCID.BlueArmoredBonesSword
                || entity.type == NPCID.BoneLee
                || entity.type == NPCID.SkeletonSniper
                || entity.type == NPCID.SkeletonCommando
                || entity.type == NPCID.TacticalSkeleton;
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
