namespace TerrorMod.Common.Utils;

public static partial class LemonUtils
{
    public static bool Alive(this Player player)
    {
        return (player != null || player.active || !player.dead || !player.ghost);
    }

    public static float GetLifePercent(this Player player)
    {
        return (float)player.statLife / player.statLifeMax2;
    }

    public static bool HasAnyFireDebuff(this Player player)
    {
        return player.HasBuff(BuffID.OnFire) || player.HasBuff(BuffID.Burning) || player.HasBuff(BuffID.OnFire3)
            || player.HasBuff(BuffID.Frostburn) || player.HasBuff(BuffID.Frostburn2) || player.HasBuff(BuffID.ShadowFlame);
    }

    public static bool HasAnyPoisonDebuff(this Player player)
    {
        return player.HasBuff(BuffID.Poisoned) || player.HasBuff(BuffID.Venom);
    }

    public static Vector2 RandomPos(this Player player, float fluffX = 0, float fluffY = 0)
    {
        Vector2 pos = player.position + new Vector2(Main.rand.NextFloat(-fluffX, player.width + fluffX), Main.rand.NextFloat(-fluffY, player.height + fluffY));
        return pos;
    }

    public static bool IsGrounded(this Player player)
    {
        Tile tileBelow = Main.tile[(int)(player.Center.X / 16), (int)(player.Center.Y / 16) + 2];
        return (Main.tileSolid[tileBelow.TileType] || Main.tileSolid[tileBelow.TileType]) && player.velocity.Y == 0;
    }

    public static void RestoreMana(this Player player, int value)
    {
        if (player.statMana + value > player.statManaMax2)
        {
            value = player.statManaMax2 - player.statMana;
        }
        player.statMana += value;
        player.ManaEffect(value);
    }

    public static void DOTDebuff(this Player player, int damagePerSecond)
    {
        if (player.lifeRegen > 0) player.lifeRegen = 0;
        player.lifeRegenTime = 0;
        player.lifeRegen -= damagePerSecond * 2;
    }
}
