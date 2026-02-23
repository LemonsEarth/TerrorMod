namespace TerrorMod.Core.Globals.Tiles;

public class IndestructableTile : GlobalTile
{
    public override bool CanExplode(int i, int j, int type)
    {
        return type != TileID.Obsidian;
    }
}
