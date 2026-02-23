using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace TerrorMod.Content.NPCs.Hostile.Desert;

public class LivingCactus : ModNPC
{
    float AITimer = 0;
    float AttackTimer = 0;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 1;
    }

    public override void SetDefaults()
    {
        NPC.width = 32;
        NPC.height = 94;
        NPC.lifeMax = 100;
        NPC.defense = 6;
        NPC.damage = 60;
        NPC.HitSound = SoundID.Dig;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.value = 200;
        NPC.knockBackResist = 0f;
    }

    public override void OnSpawn(IEntitySource source)
    {
        NPC.spriteDirection = Main.rand.NextBool().ToDirectionInt();
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
            });
    }

    const int FIRE_RATE = 15;
    public override void AI()
    {
        NPC.velocity = Vector2.Zero;
        if (NPC.target < 0 || NPC.target == 255)
        {
            NPC.TargetClosest(false);
        }
        if (!NPC.HasValidTarget)
        {
            return;
        }
        Player player = Main.player[NPC.target];

        if (AttackTimer == FIRE_RATE && (NPC.Center.Distance(player.Center) < 200 || NPC.life < NPC.lifeMax))
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-60, 60))) * 6, ProjectileID.PineNeedleHostile, NPC.damage / 3, 1f);
                }
            }
            AttackTimer = 0;
        }

        if (AttackTimer < FIRE_RATE)
        {
            AttackTimer++;
        }
        AITimer++;
    }

    public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
    {
        return false;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return spawnInfo.Player.ZoneDesert ? 0.5f : 0f;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ItemID.Cactus, minimumDropped: 4, maximumDropped: 12));
    }

    public override bool? CanFallThroughPlatforms()
    {
        return false;
    }
}
