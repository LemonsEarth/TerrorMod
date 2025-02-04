using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace TerrorMod.Common.Utils
{
    public static class LemonUtils
    {
        /// <summary>
        /// <para>Creates a circle of dust around a given position.</para>
        /// <para><paramref name="noGrav"/> - if false, dust will be affected by gravity.</para>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="amount"></param>
        /// <param name="speed"></param>
        /// <param name="dustID"></param>
        /// <param name="scale"></param>
        /// <param name="noGrav"></param>
        /// <param name="alpha"></param>
        /// <param name="newColor"></param>
        public static void DustCircle(Vector2 position, int amount, float speed, int dustID, float scale = 1, bool noGrav = true, int alpha = 0, Color newColor = default)
        {
            for (int i = 0; i < amount; i++)
            {
                var dust = Dust.NewDustDirect(position, 1, 1, dustID, Scale: scale);
                dust.velocity = new Vector2(0, -speed).RotatedBy(MathHelper.ToRadians(i * (360 / amount))).RotatedByRandom(MathHelper.Pi);
                if (noGrav)
                {
                    dust.noGravity = true;
                }

            }
        }

        public static NPC GetClosestNPC(Projectile projectile)
        {
            NPC closestEnemy = null;
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.CanBeChasedBy())
                {
                    if (closestEnemy == null)
                    {
                        closestEnemy = npc;
                    }
                    float distanceToNPC = Vector2.DistanceSquared(projectile.Center, npc.Center);
                    if (distanceToNPC < projectile.Center.DistanceSQ(closestEnemy.Center))
                    {
                        closestEnemy = npc;
                    }
                }
            }
            return closestEnemy;
        }

        public static int GetRandomNoStackItemID()
        {
            bool found = false;
            while (!found)
            {
                int randItemID = Main.rand.Next(0, 5455);
                Item randItem = ContentSamples.ItemsByType[randItemID];
                if (randItem.maxStack == 1)
                {
                    found = true;
                    return randItemID;
                }
            }
            return 0;
        }

        public static int GetRandomItemID()
        {
            int randItemID = Main.rand.Next(0, 5455);
            return randItemID;
        }

        /// <summary>
        /// Returns 1 for Small Worlds, 2 for Medium Worlds, 3 for Large Worlds (and bigger?)
        /// </summary>
        /// <returns></returns>
        public static int GetWorldSize()
        {
            switch (Main.maxTilesX)
            {
                case >= 8400:
                    return 3;
                case >= 6400:
                    return 2;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Returns 1 for Classic and Journey, 2 for Expert, 3 for Master.
        /// Doubles value if For the Worthy seed is active
        /// </summary>
        /// <returns></returns>
        public static int GetDifficulty()
        {
            int difficulty = 1;
            if (Main.expertMode) difficulty++;
            if (Main.masterMode) difficulty++;
            if (Main.getGoodWorld) difficulty *= 2;
            return difficulty;
        }
    }
}
