using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.ItemDropRules;
using TerrorMod.Content.Items.Consumables;

namespace TerrorMod.Core.Globals.NPCs.Forest
{
    public class Undead : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Zombie
                || entity.type == NPCID.ZombieEskimo
                || entity.type == NPCID.ZombieDoctor
                || entity.type == NPCID.ZombieElf
                || entity.type == NPCID.ZombieMushroom
                || entity.type == NPCID.BaldZombie
                || entity.type == NPCID.PincushionZombie
                || entity.type == NPCID.SlimedZombie
                || entity.type == NPCID.SwampZombie
                || entity.type == NPCID.TwiggyZombie
                || entity.type == NPCID.FemaleZombie
                || entity.type == NPCID.ZombieRaincoat
                || entity.type == NPCID.ZombieMushroomHat
                || entity.type == NPCID.Skeleton
                || entity.type == NPCID.AngryBones
                || entity.type == NPCID.UndeadMiner
                || entity.type == NPCID.UndeadViking
                || entity.type == NPCID.BoneThrowingSkeleton
                || entity.type == NPCID.CursedSkull
                || entity.type == NPCID.BoneSerpentHead
                || entity.type == NPCID.DarkCaster
                || entity.type == NPCID.GreekSkeleton
                || entity.type == NPCID.HeadacheSkeleton
                || entity.type == NPCID.MisassembledSkeleton
                || entity.type == NPCID.PantlessSkeleton
                || entity.type == NPCID.BloodZombie;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LifeEssence>(), 500, 1, 3));
        }
    }
}
