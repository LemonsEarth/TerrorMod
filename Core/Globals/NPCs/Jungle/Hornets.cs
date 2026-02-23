using Terraria.Audio;
using Terraria.DataStructures;
using TerrorMod.Common.Utils;

namespace TerrorMod.Core.Globals.NPCs.Jungle;

public class Hornets : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int soundMeter = 0;
    int maxSoundTolerance = 100;
    Vector2 lastSoundPosition;
    int AITimer = 0;
    bool playedSound = false;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.Hornet 
            || entity.type == NPCID.MossHornet 
            || entity.type == NPCID.HornetFatty
            || entity.type == NPCID.HornetHoney
            || entity.type == NPCID.HornetLeafy
            || entity.type == NPCID.HornetStingy
            || entity.type == NPCID.Bee 
            || entity.type == NPCID.BeeSmall;
    }

    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient) return;
        if (npc.ai[3] == 0) // Swarm spawn
        {
            for (int i = 0; i < 3; i++)
            {
                if (Main.rand.NextBool(2))
                {
                    NPC.NewNPC(source, (int)npc.Center.X + Main.rand.Next(-32, 32), (int)npc.Center.Y + Main.rand.Next(-32, 32), npc.type, npc.whoAmI, ai3: 2);
                }
            }
        }
    }

    public override void AI(NPC npc)
    {
        if (soundMeter < maxSoundTolerance)
        {
            ActiveSound doubleJump = SoundEngine.FindActiveSound(SoundID.DoubleJump);
            ActiveSound dig = SoundEngine.FindActiveSound(SoundID.Dig);
            ActiveSound hurt = SoundEngine.FindActiveSound(SoundID.PlayerHit);
            if (doubleJump != null && Vector2.Distance(npc.Center, doubleJump.Position.Value) < 1000)
            {
                soundMeter++;
                lastSoundPosition = doubleJump.Position.Value;
                SoundEngine.PlaySound(SoundID.Item173 with
                { PitchRange = (0.6f, 0.9f), Volume = 0.2f, MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, npc.Center);
            }
            if (dig != null && Vector2.Distance(npc.Center, dig.Position.Value) < 1000)
            {
                soundMeter++;
                lastSoundPosition = dig.Position.Value;
                SoundEngine.PlaySound(SoundID.Item173 with
                { PitchRange = (0.6f, 0.9f), Volume = 0.2f, MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, npc.Center);
            }
            if (hurt != null && Vector2.Distance(npc.Center, hurt.Position.Value) < 1000)
            {
                soundMeter++;
                lastSoundPosition = hurt.Position.Value;
                SoundEngine.PlaySound(SoundID.Item173 with
                { PitchRange = (0.6f, 0.9f), Volume = 0.2f, MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, npc.Center);
            }
        }
        

        if (soundMeter >= maxSoundTolerance)
        {
            if (!playedSound)
            {
                SoundEngine.PlaySound(SoundID.Item173 with { PitchRange = (0.3f, 0.6f), Volume = 0.5f, MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest}, npc.Center);
                playedSound = true;
            }
            npc.noTileCollide = true;
            
            if (npc.type == NPCID.Hornet || npc.type == NPCID.MossHornet)
            {
                HornetAI(npc);
            }

            Player target = null;

            foreach (Player player in Main.ActivePlayers)
            {
                if (target == null) target = player;
                if (player.Distance(lastSoundPosition) < 800 && player.Distance(lastSoundPosition) <= target.Distance(lastSoundPosition))
                {
                    target = player;
                }
            }

            npc.MoveToPos(target.Center, 0.05f, 0.05f, 0.05f, 0.05f);
        }

        AITimer++;
    }

    void HornetAI(NPC npc)
    {
        if (npc.ai[1] < 100)
        {
            npc.ai[1] = 100;
        }
    }
}
