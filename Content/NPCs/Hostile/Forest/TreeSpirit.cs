using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using TerrorMod.Common.Utils;

namespace TerrorMod.Content.NPCs.Hostile.Forest;

public class TreeSpirit : ModNPC
{
    ref float AITimer => ref NPC.ai[0];
    ref float AttackTimer => ref NPC.ai[1];

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 1;
    }

    public override void SetDefaults()
    {
        NPC.width = 16;
        NPC.height = 16;
        NPC.lifeMax = 24;
        NPC.defense = 2;
        NPC.damage = 10;
        NPC.HitSound = SoundID.Dig;
        NPC.DeathSound = SoundID.Dig;
        NPC.value = 10;
        NPC.aiStyle = -1;
        NPC.knockBackResist = 1f;
        NPC.noGravity = true;
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
            });
    }

    const int FIRE_RATE = 90;
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
        NPC.rotation = NPC.Center.DirectionTo(player.Center).ToRotation();

        if (AttackTimer == FIRE_RATE)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.Center.DirectionTo(player.Center) * 10, ProjectileID.WoodenArrowHostile, NPC.damage, 1f);
                LemonUtils.DustCircle(NPC.Center, 8, 6, DustID.WoodFurniture);
            }
            AttackTimer = 0;
        }

        NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 1f / 60f);

        if (AttackTimer < FIRE_RATE)
        {
            AttackTimer++;
        }
        AITimer++;
    }


    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return 0f;
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
