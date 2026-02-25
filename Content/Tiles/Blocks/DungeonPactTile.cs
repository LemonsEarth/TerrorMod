using TerrorMod.Content.Buffs.Debuffs.Misc;

namespace TerrorMod.Content.Tiles.Blocks;

public class DungeonPactTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;

        DustType = DustID.GemDiamond;

        Main.tileMergeDirt[Type] = true;
        AddMapEntry(new Color(255, 255, 255));
    }

    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = fail ? 3 : 1;
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (closer) return;
        if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
        {
            Main.LocalPlayer.AddBuff(BuffType<UndeadPact>(), 30);
            for (float x = 0.1f; x < 1f; x += 0.1f)
            {
                Vector2 pos = Vector2.Lerp(new Vector2(i, j).ToWorldCoordinates(), Main.LocalPlayer.Center, x);
                for (int y = 0; y < 3; y++)
                {
                    Dust.NewDustDirect(pos, 1, 1, DustID.GemAmethyst).noGravity = true;
                }
            }
        }
    }
}
