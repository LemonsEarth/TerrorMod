using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace TerrorMod.Core.Globals.Tiles
{
    public class NPCTile : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail) // Only proceed if the tile was actually broken
            {
                return;
            }
            if (type == TileID.Stone)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int roll = Main.rand.Next(1, 500);
                    if (roll > 1)
                    {
                        NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, NPCID.RockGolem);
                        for (int x = -3; x <= 3; x++)
                        {
                            for (int y = -3; y <= 3; y++)
                            {
                                if (Main.tile[i + x, j + y].HasTile && Main.tile[i + x, j + y].TileType == TileID.Stone)
                                {
                                    WorldGen.KillTile(i + x, j + y);
                                } 
                            }
                        }  
                    }
                    else if (roll < 5)
                    {
                        NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, NPCID.GraniteGolem);
                    }
                    else if (roll < 10)
                    {
                        NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, NPCID.GraniteFlyer);
                    }
                }
            }
        }
    }
}
