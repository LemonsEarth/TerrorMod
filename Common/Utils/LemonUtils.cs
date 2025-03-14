using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Common.Utils
{
    public static class LemonUtils
    {
        public static Vector2 BezierCurve(Vector2 pointA, Vector2 pointB, Vector2 controlPoint, float fracComplete)
        {
            Vector2 AToControl = Vector2.Lerp(pointA, controlPoint, fracComplete);
            Vector2 ControlToB = Vector2.Lerp(controlPoint, pointB, fracComplete);
            Vector2 finalPoint = Vector2.Lerp(AToControl, ControlToB, fracComplete);

            return finalPoint;
        }

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

        public static bool CheckAllForLight(float lightLevel, params Vector2[] positions)
        {
            foreach (Vector2 pos in positions)
            {
                float h = Lighting.Brightness((int)pos.X / 16, (int)pos.Y / 16);
                if (Lighting.Brightness((int)pos.X / 16, (int)pos.Y / 16) < lightLevel && !Main.tile[(int)pos.X / 16, (int)pos.Y / 16].HasTile)
                {
                    return false;
                }
            }
            return true;
        }

        public static void AddPhobiaDebuffs(Player player, float mul = 1f)
        {
            if (mul <= 0) mul = 1;
            if (Main.rand.NextBool((int)(300 / mul))) player.AddBuff(BuffID.Weak, (int)(300 * mul));
            if (Main.rand.NextBool((int)(500 / mul))) player.AddBuff(BuffID.Blackout, (int)(180 * mul));
            if (Main.rand.NextBool((int)(300 / mul))) player.AddBuff(BuffID.Slow, (int)(300 * mul));
            if (Main.rand.NextBool((int)(1200 / mul))) player.AddBuff(BuffID.Silenced, (int)(90 * mul));
            if (Main.rand.NextBool((int)(1600 / mul))) player.AddBuff(BuffID.Cursed, (int)(90 * mul));
            if (Main.rand.NextBool((int)(2000 / mul))) player.AddBuff(BuffID.Confused, (int)(180 * mul));
            if (Main.rand.NextBool((int)(3000 / mul))) player.AddBuff(BuffID.Stoned, 180);
            if (Main.rand.NextBool((int)(5000 / mul))) player.AddBuff(ModContent.BuffType<FearDebuff>(), (int)(120 * mul));
        }
    }
}
