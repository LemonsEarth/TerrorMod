using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using TerrorMod.Content.Buffs.Buffs;
using TerrorMod.Content.Buffs.Debuffs.Misc;
using TerrorMod.Content.Items.Consumables;

namespace TerrorMod.Content.NPCs.Hostile.Special;

public class EvilSpirit : ModNPC
{
    float AITimer = 0;
    ref float speed => ref NPC.ai[0];

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 2;
    }

    public override void SetDefaults()
    {
        NPC.width = 50;
        NPC.height = 72;
        NPC.lifeMax = 120;
        NPC.defense = 10;
        NPC.damage = 40;
        NPC.HitSound = SoundID.NPCHit36;
        NPC.DeathSound = SoundID.NPCDeath39;
        NPC.value = 1280;
        NPC.aiStyle = -1;
        NPC.knockBackResist = 0.9f;
        NPC.Opacity = 0.7f;
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

        if (AITimer % 60 == 0)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                speed = Main.rand.NextFloat(1, 5);
            }
            NPC.netUpdate = true;
        }
        NPC.velocity = NPC.Center.DirectionTo(player.Center) * speed;
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
            if (NPC.frame.Y > 1 * frameHeight)
            {
                NPC.frame.Y = 0;
            }
        }
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    {
        if (Main.rand.NextBool(3)) target.AddBuff(BuffID.Cursed, 300);
        if (Main.rand.NextBool(3)) target.AddBuff(BuffID.WitheredArmor, 600);
        if (Main.rand.NextBool(3)) target.AddBuff(BuffID.WitheredWeapon, 300);
        speed = 0;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return (!spawnInfo.PlayerInTown && !spawnInfo.Player.HasBuff(BuffType<BeforeTheStormBuff>()) && Main.hardMode && spawnInfo.Player.HasBuff(BuffType<TheStormDebuff>())) ? 0.1f : 0f;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ItemType<LifeEssence>(), chanceDenominator: 100, minimumDropped: 1, maximumDropped: 4));
    }
}
