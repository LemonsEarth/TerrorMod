using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Content.NPCs.Hostile.Forest
{
    public class WallOfZombies : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Zombie);
            NPC.width = 38;
            NPC.height = 276;
            NPC.lifeMax = 800;
            NPC.defense = 8;
            NPC.damage = 20;
            NPC.value = 1300;
            NPC.knockBackResist = 0f;
            AIType = NPCID.Zombie;
            NPC.aiStyle = NPCID.Zombie;
        }

        public override void AI()
        {
            if (!NPC.HasValidTarget) return;
            Player player = Main.player[NPC.target];
            NPC.spriteDirection = Math.Sign(NPC.Center.DirectionTo(player.Center).X);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
                {
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                });
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 3;
        }

        public override void FindFrame(int frameHeight)
        {
            int frameDur = 20;
            NPC.frameCounter += 1;
            if (NPC.frameCounter > frameDur)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
                if (NPC.frame.Y > 2 * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.Player.ZoneForest && !Main.dayTime) ? 0.05f : 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.BloodyMachete, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.ZombieArm, 10));
        }
    }
}
