using Terraria.Audio;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Corruption;

namespace TerrorMod.Core.Globals.NPCs.Corruption;

public class InfectedCorruptNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;
    public bool infectedCorrupt = false;
    int corruptionTimer = 0;
    int maxCorruptionTimer = 600;

    public override void ResetEffects(NPC npc)
    {
        infectedCorrupt = false;
    }

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return !entity.boss
            && entity.type != NPCID.Creeper
            && entity.type != NPCID.ServantofCthulhu
            && entity.type != NPCID.EaterofWorldsBody
            && entity.type != NPCID.EaterofWorldsTail
            && entity.type != NPCID.SkeletronHand;
    }

    public override void PostAI(NPC npc)
    {
        if (!npc.dontTakeDamage && !NPCLists.SafeNPCs.Contains(npc.type) && !npc.SpawnedFromStatue
            && npc.HasPlayerTarget && Main.player[npc.target].ZoneCorrupt && npc.life < 50 && !NPCID.Sets.ProjectileNPC[npc.type])
        {
            npc.AddBuff(BuffType<InfectedCorrupt>(), 180);
        }

        if (infectedCorrupt)
        {
            corruptionTimer++;
            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Corruption);
        }

        if (corruptionTimer > maxCorruptionTimer)
        {
            LemonUtils.DustCircle(npc.Center, 8, 5, DustID.Corruption, 3f);
            SoundEngine.PlaySound(npc.DeathSound, npc.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(npc.GetSource_Death("Corrupted"), (int)npc.Center.X, (int)npc.Center.Y, NPCType<CorruptCarrier>());
            }
            npc.active = false;
        }
    }
}
