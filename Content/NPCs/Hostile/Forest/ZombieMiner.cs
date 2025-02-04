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
    public class ZombieMiner : ModNPC
    {
        float AITimer = 0;
        float AttackTimer = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 40;
            NPC.lifeMax = 50;
            NPC.defense = 2;
            NPC.damage = 20;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 200;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            NPC.knockBackResist = 0.7f;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage * balance * 0.5f);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
                {
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                });
        }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255)
            {
                NPC.TargetClosest(false);
            }
            Lighting.AddLight(NPC.Center, 2, 2, 2);
            if (!NPC.HasValidTarget)
            {
                return;
            }
            Player player = Main.player[NPC.target];

            NPC.spriteDirection = Math.Sign(NPC.Center.DirectionTo(player.Center).X);

            if (AttackTimer == 180)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center) * 10, ModContent.ProjectileType<ThrownPickaxe>(), NPC.damage, 1f);
                }
                AttackTimer = 0;
            }

            if (AttackTimer < 180)
            {
                AttackTimer++;
            }
            AITimer++;
        }

        public override void FindFrame(int frameHeight)
        {
            int frameDur = 20;
            NPC.frameCounter += 1;
            if (NPC.frameCounter > frameDur)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
                if (NPC.frame.Y > 1 * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.dayTime)
            {
                return (spawnInfo.Player.ZoneForest && Main.bloodMoon) ? 0.06f : 0.005f;
            }
            else
            {
                return spawnInfo.Player.ZoneRockLayerHeight ? 0.02f : 0f;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Wood, minimumDropped: 1, maximumDropped: 3));
            npcLoot.Add(ItemDropRule.Common(ItemID.WoodenArrow, 5, minimumDropped: 2, maximumDropped: 3));
        }

        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
    }
}
