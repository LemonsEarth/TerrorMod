using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using TerrorMod.Content.Items.Accessories;
using TerrorMod.Common.Utils;

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
                || entity.type == NPCID.TacticalSkeleton
                || entity.type == NPCID.PossessedArmor;
        }

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.SkeletonArcher)
            {
                npc.ai[1] = MathHelper.Clamp(npc.ai[1], 0, 40);
            }
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (npc.type == NPCID.ArmoredSkeleton)
            {
                float mod = (float)(npc.lifeMax - npc.life) / npc.lifeMax;
                modifiers.FinalDamage *= MathHelper.Clamp(mod, 0.2f, 1f);
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (npc.type == NPCID.CursedSkull && npc.ai[3] == 1)
            {
                LemonUtils.DustCircle(npc.Center, 16, 5, DustID.GemDiamond, 1.3f);
                npc.active = false;
                return false;
            }
            return true;
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

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.AngryBones || npc.type == NPCID.DarkCaster || npc.type == NPCID.AngryBonesBig || npc.type == NPCID.AngryBonesBigHelmet || npc.type == NPCID.AngryBonesBigMuscle)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<UndeadAmulet>(), 100));
            }
        }
    }
}
