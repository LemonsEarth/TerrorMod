using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using TerrorMod.Content.Buffs.Buffs;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Items.Consumables;

namespace TerrorMod.Content.NPCs.Hostile.Special;

public class HandSpirit : ModNPC
{
    float AITimer = 0;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 3;
    }

    public override void SetDefaults()
    {
        NPC.width = 50;
        NPC.height = 72;
        NPC.lifeMax = 100;
        NPC.defense = 6;
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

        NPC.spriteDirection = Math.Sign(NPC.Center.DirectionTo(player.Center).X) == 0 ? 1 : Math.Sign(NPC.Center.DirectionTo(player.Center).X);
        Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Smoke).noGravity = true;

        NPC.velocity /= 1.05f;
        if (AITimer % 120 == 0)
        {
            NPC.velocity = NPC.Center.DirectionTo(player.Center) * 20;
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

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    {
        if (Main.rand.NextBool(3)) target.AddBuff(BuffID.Cursed, 300);
        if (Main.rand.NextBool(3)) target.AddBuff(BuffID.WitheredArmor, 300);
        if (Main.rand.NextBool(3)) target.AddBuff(BuffID.WitheredWeapon, 600);
        if (Main.rand.NextBool(3)) target.AddBuff(BuffID.Bleeding, 600);
        if (Main.rand.NextBool(3)) target.AddBuff(BuffType<Weight>(), 300);
        target.velocity = Vector2.Zero;
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
