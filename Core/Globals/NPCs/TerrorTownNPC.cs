﻿using Microsoft.Xna.Framework;
using System.Configuration;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Conditions;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Buffs;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Items.Consumables;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Players;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Globals.NPCs
{
    public class TerrorTownNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return lateInstantiation && entity.townNPC;
        }

        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            if (!SkullSystem.greedSkullActive) return;
            foreach (Item item in items)
            {
                if (item == null) continue;
                int originalPrice = item.shopCustomPrice == null ? item.value : item.shopCustomPrice.Value;
                item.shopCustomPrice = originalPrice * 2;
            }
        }

        bool spawnedSkelly = false;
        public override void OnKill(NPC npc)
        {
            int chance = 8;
            if (!Main.dayTime) chance = 4;
            if (Main.getGoodWorld) chance = 2;
            if (Main.zenithWorld) chance = 1;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!spawnedSkelly && Main.rand.NextBool(chance) )
                {
                    NPC.NewNPC(npc.GetSource_FromThis(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.SkeletronHead);
                    spawnedSkelly = true;
                }
            }

        }
    }
}
