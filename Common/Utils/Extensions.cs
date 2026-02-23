using TerrorMod.Core.Players;

namespace TerrorMod.Common.Utils;

public static class Extensions
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

    public static void MoveToPos(this Projectile proj, Vector2 pos, float xDecel = 1f, float yDecel = 1f, float xAccel = 1f, float yAccel = 1f)
    {
        Vector2 direction = proj.Center.DirectionTo(pos);
        if (direction.HasNaNs())
        {
            return;
        }
        float XaccelMod = Math.Sign(direction.X) - Math.Sign(proj.velocity.X);
        float YaccelMod = Math.Sign(direction.Y) - Math.Sign(proj.velocity.Y);
        proj.velocity += new Vector2(XaccelMod * xDecel + xAccel * Math.Sign(direction.X), YaccelMod * yDecel + yAccel * Math.Sign(direction.Y));
    }

    public static TerrorPlayer TerrorPlayer(this Player player)
    {
        return player.GetModPlayer<TerrorPlayer>();
    }

    public static TemperaturePlayer TemperaturePlayer(this Player player)
    {
        return player.GetModPlayer<TemperaturePlayer>();
    }

    public static bool CheckHead(this Player player, int helmetItemType)
    {
        return player.armor[0].type == helmetItemType;
    }

    public static bool CheckBody(this Player player, int chestItemType)
    {
        return player.armor[1].type == chestItemType;
    }

    public static bool CheckLegs(this Player player, int bootsItemType)
    {
        return player.armor[2].type == bootsItemType;
    }
}
