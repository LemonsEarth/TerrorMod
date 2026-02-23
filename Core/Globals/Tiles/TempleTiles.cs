namespace TerrorMod.Core.Globals.Tiles;

public class TempleTiles : GlobalTile
{
    public override void NearbyEffects(int i, int j, int type, bool closer)
    {
        if (closer) return;

        if (type == TileID.Statues && Main.LocalPlayer.active && !Main.LocalPlayer.dead && Main.LocalPlayer.ZoneLihzhardTemple)
        {
            if (Main.tile[i, j].TileFrameX == 43 * 36) // Normal Statue
            {
                Main.LocalPlayer.AddBuff(BuffID.WaterCandle, 60);
            }

            if (Main.tile[i, j].TileFrameX == 44 * 36) // Watcher Statue
            {
                Main.LocalPlayer.AddBuff(BuffID.Blackout, 60);
            }

            if (Main.tile[i, j].TileFrameX == 45 * 36) // Guardian Statue
            {
                Main.LocalPlayer.AddBuff(BuffID.WitheredWeapon, 60);
                Main.LocalPlayer.AddBuff(BuffID.WitheredArmor, 60);
            }
        }
    }
}
