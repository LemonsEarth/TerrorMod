using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorMod.Core.Globals.NPCs.Underground
{
    public class GraniteElemental : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.GraniteFlyer;
        }

        public override void PostAI(NPC npc)
        {
            npc.noTileCollide = true;
        }
    }
}
