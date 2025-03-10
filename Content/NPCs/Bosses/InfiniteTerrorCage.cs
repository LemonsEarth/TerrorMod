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
using TerrorMod.Content.Buffs.Buffs;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Core.Systems;

namespace TerrorMod.Content.NPCs.Bosses
{
    [AutoloadBossHead]
    public class InfiniteTerrorCage : ModNPC
    {
        ref float AITimer => ref NPC.ai[0];
        ref float AttackTimer => ref NPC.ai[1];
        ref float AttackCount => ref NPC.ai[2];

        public override bool NeedSaving()
        {
            return true;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            
        }

        public override void SetDefaults()
        {
            NPC.width = 194;
            NPC.height = 190;
            NPC.lifeMax = 40000;
            NPC.defense = 100;
            NPC.damage = 40;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 20000;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.chaseable = false;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage);
            NPC.lifeMax = (int)(NPC.lifeMax * balance * 0.5f);
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
                {
                    BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                });
        }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255)
            {
                NPC.TargetClosest(false);
            }
            Player player = Main.player[NPC.target];
            NPC.velocity = Vector2.Zero;

            if (!Terraria.Graphics.Effects.Filters.Scene["TerrorMod:DesaturateShader"].IsActive() && Main.netMode != NetmodeID.Server)
            {
                Terraria.Graphics.Effects.Filters.Scene.Activate("TerrorMod:DesaturateShader");
            }

            if (AttackTimer > 0) AttackTimer--;

            AITimer++;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            
        }

        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }
    }
}
