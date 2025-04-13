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
using TerrorMod.Content.Buffs.Buffs;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Items.Consumables;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Content.NPCs.Hostile.Special
{
    public class ArrowSpirit : ModNPC
    {
        float AITimer = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 72;
            NPC.lifeMax = 70;
            NPC.defense = 3;
            NPC.damage = 40;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.value = 1280;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0.6f;
            NPC.Opacity = 0.9f;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
                {
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard
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

            Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Smoke).noGravity = true;

            NPC.MoveToPos(player.Center, 0.05f, 0.05f, 0.3f, 0.3f);
            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
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
                if (NPC.frame.Y > 4 * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(3)) target.AddBuff(BuffID.Cursed, 300);
            if (Main.rand.NextBool(3)) target.AddBuff(BuffID.WitheredArmor, 300);
            if (Main.rand.NextBool(3)) target.AddBuff(BuffID.WitheredWeapon, 600);
            if (Main.rand.NextBool(3)) target.AddBuff(BuffID.Bleeding, 600);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (!spawnInfo.PlayerInTown && !spawnInfo.Player.HasBuff(ModContent.BuffType<BeforeTheStormBuff>()) && Main.hardMode && spawnInfo.Player.HasBuff(ModContent.BuffType<TheStormDebuff>())) ? 0.1f : 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LifeEssence>(), chanceDenominator: 100, minimumDropped: 1, maximumDropped: 4));
        }
    }
}
