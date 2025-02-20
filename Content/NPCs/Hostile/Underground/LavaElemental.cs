using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.NPCs.Hostile.Underground
{
    public class LavaElemental : ModNPC
    {
        ref float AITimer => ref NPC.ai[0];
        ref float AttackTimer => ref NPC.ai[1];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 61;
            NPC.height = 61;
            NPC.lifeMax = 130;
            NPC.defense = 12;
            NPC.damage = 40;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.value = 200;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0.3f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void FindFrame(int frameHeight)
        {
            int frameDur = 15;
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

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
                {
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                });
        }

        const int FIRE_RATE = 45;
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255)
            {
                NPC.TargetClosest(false);
            }
            if (!NPC.HasValidTarget)
            {
                return;
            }
            Player player = Main.player[NPC.target];
            NPC.rotation = MathHelper.ToRadians(AITimer * 2);

            if (AttackTimer == FIRE_RATE)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 2, Main.rand.Next(ProjectileID.GreekFire1, ProjectileID.GreekFire3), NPC.damage / 4, 1f);
                }
                AttackTimer = 0;
            }

            NPC.MoveToPos(player.Center, 0.02f, 0.02f, 0.01f, 0.01f);

            if (AttackTimer < FIRE_RATE)
            {
                AttackTimer++;
            }
            AITimer++;
        }

        public override void OnKill()
        {
            int tileX = (int)NPC.Center.X / 16;
            int tileY = (int)NPC.Center.Y / 16;
            if (WorldGen.InWorld(tileX, tileY) && Main.netMode != NetmodeID.MultiplayerClient)
            {
                WorldGen.PlaceLiquid(tileX, tileY, (byte)LiquidID.Lava, 255);
                NetMessage.SendTileSquare(-1, tileX, tileY);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Point16 playerTile = spawnInfo.Player.Center.ToTileCoordinates16();
            int lavaTiles = 0;
            for (int x = -30; x <= 30; x++)
            {
                for (int y = -30; y <= 30; y++)
                {
                    int coordsX = playerTile.X + x;
                    int coordsY = playerTile.Y + y;
                    if (!WorldGen.InWorld(coordsX, coordsY))
                    {
                        continue;
                    }
                    Tile tile = Main.tile[coordsX, coordsY];
                    if (tile.LiquidAmount > 50 && tile.LiquidType == (byte)LiquidID.Lava)
                    {
                        lavaTiles++;
                        if (lavaTiles >= 30) break;
                    }
                }
                if (lavaTiles >= 30) break;
            }
            float denominator = spawnInfo.Player.ZoneOverworldHeight ? 50f : 500f;
            return lavaTiles / denominator;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.ObsidianRose, 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.LavaCharm, 100));
        }

        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
    }
}
