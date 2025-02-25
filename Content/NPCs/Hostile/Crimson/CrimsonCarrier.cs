using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Content.NPCs.Hostile.Crimson
{
    public class CrimsonCarrier : ModNPC
    {
        ref float AITimer => ref NPC.ai[0];
        ref float AttackTimer => ref NPC.ai[1];

        readonly string GlowMask_Path = "TerrorMod/Content/NPCs/Hostile/Crimson/CrimsonCarrier_Glow";
        static Asset<Texture2D> GlowMask;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<InfectedCrimson>()] = true;
            GlowMask = ModContent.Request<Texture2D>(GlowMask_Path);
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.lifeMax = 170;
            NPC.defense = 12;
            NPC.damage = 20;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 100;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0.4f;
            NPC.noGravity = true;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
                {
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                });
        }

        const int FIRE_RATE = 180;

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
            NPC.spriteDirection = -Math.Sign(NPC.Center.DirectionTo(player.Center).X);
            if (AITimer % 3 == 0) Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Crimson);
            if (AttackTimer == FIRE_RATE)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = NPC.Center.Distance(player.Center) < 600 ? NPC.Center.DirectionTo(player.Center) : Vector2.UnitY;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 2, ModContent.ProjectileType<CrimsonCarrierProj>(), NPC.damage / 3, 1f);
                    LemonUtils.DustCircle(NPC.Center, 8, 6, DustID.Crimson);
                }
                AttackTimer = 0;
            }
            Vector2 posAbovePlayer = player.Center - Vector2.UnitY * 200;
            Vector2 pos = posAbovePlayer;
            if (!Collision.CanHit(NPC.Center, 16, 16, posAbovePlayer, 16, 16)) // If point is not reachable
            {
                pos = player.Center;
            }
            NPC.MoveToPos(pos, 0.05f, 0.05f, 0.05f, 0.05f);

            if (AttackTimer < FIRE_RATE)
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
                if (NPC.frame.Y > 2 * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneCrimson ? 0.05f : 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, minimumDropped: 2, maximumDropped: 8));
            npcLoot.Add(ItemDropRule.Common(ItemID.ViciousMushroom, minimumDropped: 2, maximumDropped: 4));
        }

        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(GlowMask.Value, NPC.Center - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, effects);
        }
    }
}
