namespace TerrorMod.Content.Buffs.Debuffs;

public class Weight : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        BuffID.Sets.LongerExpertDebuff[Type] = false;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        if (Main.myPlayer == player.whoAmI)
        {
            player.velocity.Y += 2f;
            Tile tileBelow = Main.tile[(int)player.Center.X / 16, (int)player.Center.Y / 16 + 2];
            if (tileBelow.HasTile && tileBelow.TileType == TileID.Platforms)
            {
                Vector2 center = player.Center;
                center.Y += 0.1f;
                player.Center = center;
            }
        }
    }
}
