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
    public class BombSlime : ModNPC
    {
        float AITimer = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 34;
            NPC.lifeMax = 50;
            NPC.defense = 2;
            NPC.damage = 20;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 200;
            NPC.aiStyle = NPCAIStyleID.Slime;
            NPC.knockBackResist = 0.2f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
                {
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                });
        }

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

            if (NPC.life < NPC.lifeMax)
            {
                AITimer++;
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);
                SoundEngine.PlaySound(SoundID.Item13, NPC.Center);
            }

            if (AITimer >= 300)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<BombExplosion>(), NPC.damage, 1f);
                }
                NPC.SimpleStrikeNPC(1000, 0, noPlayerInteraction: true);
            }
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
            return (spawnInfo.Player.ZoneForest) ? 0.1f : 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Bomb, minimumDropped: 1, maximumDropped: 4));
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, minimumDropped: 2, maximumDropped: 4));
        }
    }
}
