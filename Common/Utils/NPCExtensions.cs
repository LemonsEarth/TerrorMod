using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace TerrorMod.Common.Utils
{
    public static class NPCExtensions
    {
        /// <summary>
        /// Accelerates an NPC towards a position
        /// </summary>
        /// <param name="NPC"></param>
        /// <param name="pos">The position to accelerate towards</param>
        /// <param name="xDecel">The "turning speed" on the X axis. Increase this value if you want the NPC to decelerate faster if its not moving in the desired direction</param>
        /// <param name="yDecel">Same as xAccel, just on the Y axis</param>
        /// <param name="xAccel">The desired acceleration on the X axis</param>
        /// <param name="yAccel">The desired acceleration on the Y axis</param>
        public static void MoveToPos(this NPC NPC, Vector2 pos, float xDecel = 1f, float yDecel = 1f, float xAccel = 1f, float yAccel = 1f)
        {
            Vector2 direction = NPC.Center.DirectionTo(pos);
            if (direction.HasNaNs())
            {
                return;
            }
            float XaccelMod = Math.Sign(direction.X) - Math.Sign(NPC.velocity.X);
            float YaccelMod = Math.Sign(direction.Y) - Math.Sign(NPC.velocity.Y);
            NPC.velocity += new Vector2(XaccelMod * xDecel + xAccel * Math.Sign(direction.X), YaccelMod * yDecel + yAccel * Math.Sign(direction.Y));
        }
    }
}
