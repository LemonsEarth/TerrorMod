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
using TerrorMod.Core.Systems;

namespace TerrorMod.Content.NPCs.Hostile.Special
{
    [AutoloadBossHead]
    public class MechanicalCore : ModNPC
    {
        ref float AITimer => ref NPC.ai[0];
        ref float Index => ref NPC.ai[1];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 88;
            NPC.height = 88;
            NPC.lifeMax = 15000;
            NPC.defense = 20;
            NPC.damage = 40;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 2000;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage);
        }

        public override bool CheckActive()
        {
            return false;
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
            Player player = Main.player[NPC.target];
            NPC.rotation = MathHelper.ToRadians(AITimer * 2);
            Lighting.AddLight(NPC.Center, 1, 1, 1);
            NPC.velocity = Vector2.Zero;

            AITimer++;
        }

        public override void OnKill()
        {
            EventSystem.mechanicalCorePositions[(int)Index] = Vector2.Zero;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.SoulofNight, 1, 3, 8));
            npcLoot.Add(ItemDropRule.Common(ItemID.SoulofLight, 1, 3, 8));
        }

        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
    }
}
