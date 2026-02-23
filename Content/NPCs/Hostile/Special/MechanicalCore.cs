using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Buffs;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Content.NPCs.Hostile.Special;

[AutoloadBossHead]
public class MechanicalCore : ModNPC
{
    ref float AITimer => ref NPC.ai[0];
    ref float AttackTimer => ref NPC.ai[1];
    ref float AttackCount => ref NPC.ai[2];
    bool spawnedProbes = false;

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

    public override void SetDefaults()
    {
        NPC.width = 88;
        NPC.height = 88;
        NPC.lifeMax = 7500;
        NPC.defense = 20;
        NPC.damage = 40;
        NPC.HitSound = SoundID.NPCHit4;
        NPC.DeathSound = SoundID.NPCDeath14;
        NPC.value = 2000;
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

    const int ATTACK_CD_NORMAL = 60;
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
        if (NPC.Center.Distance(player.Center) < 1000)
        {
            if (AttackTimer == 0)
            {
                switch (AttackCount)
                {
                    case < 2:
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center) * 10, ProjectileID.EyeBeam, NPC.damage / 5, 1f);
                        }
                        AttackTimer = ATTACK_CD_NORMAL;
                        break;
                    case < 4:
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center) * 8, ProjectileID.EyeBeam, NPC.damage / 5, 1f);
                        }
                        AttackTimer = ATTACK_CD_NORMAL / 3;
                        break;
                    case 5:
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center).RotatedBy(i * MathHelper.PiOver4) * 8, ProjectileID.EyeBeam, NPC.damage / 5, 1f);
                            }
                        }
                        AttackTimer = ATTACK_CD_NORMAL * 2;
                        AttackCount = -1;
                        break;
                }
                AttackCount++;
            }
        }

        foreach (var allPlayer in Main.ActivePlayers)
        {
            if (!allPlayer.HasBuff(BuffType<BeforeTheStormBuff>()) && NPC.Distance(allPlayer.Center) > 3000)
            {
                allPlayer.AddBuff(BuffType<TheStormDebuff>(), 2);    
            }
        }

        if (AttackTimer > 0) AttackTimer--;

        if (NPC.life < NPC.lifeMax * 0.5f && !spawnedProbes)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = NPC.Center + (Vector2.UnitY * 100).RotatedBy(i * MathHelper.ToRadians(120));
                LemonUtils.DustCircle(pos, 8, 5, DustID.Granite, 3f);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPCDirect(NPC.GetSource_FromAI("ProbeSpawn"), (int)pos.X, (int)pos.Y, NPCID.Probe);
                }
            }
            spawnedProbes = true;
        }

        AITimer++;
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
