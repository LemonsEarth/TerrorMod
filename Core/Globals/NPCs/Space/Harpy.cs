using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Core.Globals.NPCs.Space
{
    public class Harpy : GlobalNPC
    {
        int AITimer = 0;

        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Harpy;
        }

        public override void SetDefaults(NPC entity)
        {
            entity.knockBackResist = 0f;
            entity.scale *= 1.5f;
        }

        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (Main.hardMode)
            {
                npc.lifeMax *= 2;
            }
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (Main.hardMode)
            {
                modifiers.FinalDamage /= 2;
            }
        }

        public override void AI(NPC npc)
        {
            if (npc.ai[0] > 100) npc.ai[0] = 0;

            AITimer++;
        }
    }
}
