﻿using Terraria;
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
                || entity.type == NPCID.BloodZombie
                || entity.type == NPCID.PantlessSkeleton
                || entity.type == NPCID.ArmoredSkeleton
                || entity.type == NPCID.SkeletonArcher
                || entity.type == NPCID.Tim
                || entity.type == NPCID.ArmoredViking
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
                || entity.type == NPCID.PossessedArmor
                || entity.type == NPCID.DesertGhoul
                || entity.type == NPCID.DesertGhoulCorruption
                || entity.type == NPCID.DesertGhoulCrimson
                || entity.type == NPCID.DesertGhoulHallow
                || entity.type == NPCID.Mummy
                || entity.type == NPCID.BloodMummy
                || entity.type == NPCID.DarkMummy
                || entity.type == NPCID.LightMummy
                || entity.type == NPCID.ZombieMerman;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LifeEssence>(), 400, 1, 3));
        }
    }
}
