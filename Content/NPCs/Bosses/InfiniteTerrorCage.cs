using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using TerrorMod.Content.NPCs.Bosses.Gores;
using TerrorMod.Core.Systems;

namespace TerrorMod.Content.NPCs.Bosses
{
    [AutoloadBossHead]
    public class InfiniteTerrorCage : ModNPC
    {
        ref float AITimer => ref NPC.ai[0];
        ref float AttackTimer => ref NPC.ai[1];
        ref float AttackCount => ref NPC.ai[2];
        ref float DeathTimer => ref NPC.ai[3];
        bool canDie = false;
        bool doDeath = false;

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
            NPC.lifeMax = 20000;
            NPC.defense = 100;
            NPC.damage = 40;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 10000;
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
            NPC.lifeMax = (int)(NPC.lifeMax * balance * 0.4f);
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

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (!Main.dedServ && NPC.life <= 0 && canDie)
            {
                for (int i = 0; i < 10; i++)
                {
                    Gore.NewGore(NPC.GetSource_Death(), new Vector2(Main.rand.NextFloat(NPC.position.X, NPC.position.X + NPC.width), Main.rand.NextFloat(NPC.position.Y, NPC.position.Y + NPC.height)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 10, ModContent.GoreType<CageGore>());
                }
            }

            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GemDiamond, Scale: Main.rand.NextFloat(1.5f, 2.5f));
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
            if (doDeath)
            {
                DeathTimer++;
                NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 2;
                int goreInterval = DeathTimer > 60 ? 20 : 60;
                if (DeathTimer % 20 == 0)
                {
                    LemonUtils.DustCircle(new Vector2(Main.rand.NextFloat(NPC.position.X, NPC.position.X + NPC.width), Main.rand.NextFloat(NPC.position.Y, NPC.position.Y + NPC.height)), 16, 8, DustID.GemDiamond, Main.rand.NextFloat(1.2f, 2.5f));
                    SoundEngine.PlaySound(SoundID.NPCHit2);
                    if (!Main.dedServ)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), new Vector2(Main.rand.NextFloat(NPC.position.X, NPC.position.X + NPC.width), Main.rand.NextFloat(NPC.position.Y, NPC.position.Y + NPC.height)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 10, ModContent.GoreType<CageGore>());
                    }
                }
                if (DeathTimer >= 180)
                {
                    canDie = true;
                    NPC.SimpleStrikeNPC(999, 0);
                }
            }
            AITimer++;
        }

        public override bool CheckDead()
        {
            if (!canDie)
            {
                NPC.dontTakeDamage = true;
                NPC.life = 1;
                doDeath = true;
                return false;
            }
            return true;
        }

        bool spawnedHead = false;
        public override void OnKill()
        {
            if (spawnedHead) return;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                spawnedHead = true;
                NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y + 30, ModContent.NPCType<InfiniteTerrorHead>());
            }
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
