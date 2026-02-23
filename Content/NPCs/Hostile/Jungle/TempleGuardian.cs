using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using TerrorMod.Content.Items.Consumables;

namespace TerrorMod.Content.NPCs.Hostile.Jungle;

public class TempleGuardian : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 1;
    }

    public override void SetDefaults()
    {
        NPC.width = 110;
        NPC.height = 110;
        NPC.lifeMax = 99999;
        NPC.defense = 9999;
        NPC.damage = 99999;
        NPC.HitSound = SoundID.NPCHit4;
        NPC.DeathSound = SoundID.NPCDeath14;
        NPC.value = 20000;
        NPC.aiStyle = NPCAIStyleID.SkeletronHead;
        AIType = NPCID.DungeonGuardian;
        NPC.knockBackResist = 0f;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
        NPC.SuperArmor = true;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>()
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheTemple,
            });
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (!NPC.downedPlantBoss)
        {
            return spawnInfo.Player.ZoneLihzhardTemple ? 0.5f : 0f;
        }
        return 0f;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ItemType<LifeEssence>(), 2, 5, 500));
    }

    public override bool? CanFallThroughPlatforms()
    {
        return true;
    }
}
