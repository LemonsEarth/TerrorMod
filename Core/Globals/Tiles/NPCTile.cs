using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TerrorMod.Content.NPCs.Hostile.Forest;
using TerrorMod.Common.Utils;

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
                    int roll = Main.rand.Next(1, 500 / LemonUtils.GetDifficulty());
                    if (roll <= 1 * LemonUtils.GetDifficulty())
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
                    else if (roll < 2)
                    {
                        NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, NPCID.GraniteGolem);
                        for (int x = -2; x <= 2; x++)
                        {
                            for (int y = -2; y <= 2; y++)
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
                        NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, NPCID.GraniteFlyer);
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                if (Main.tile[i + x, j + y].HasTile && Main.tile[i + x, j + y].TileType == TileID.Stone)
                                {
                                    WorldGen.KillTile(i + x, j + y);
                                }
                            }
                        }
                    }
                }
            }
            if (type == TileID.Trees)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (Main.rand.NextBool(10))
                    {
                        NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, ModContent.NPCType<TreeSpirit>());
                    }
                }
            }
        }
    }
}
