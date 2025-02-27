using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorMod.Core.Globals.NPCs.Crimson
{
    public class Herpling : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.aiStyle == NPCAIStyleID.Herpling;
        }

        public override void AI(NPC npc)
        {
            npc.ai[0] = 200;
            npc.velocity.Y += 0.1f;
        }
    }
}
