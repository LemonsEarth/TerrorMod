using Terraria.DataStructures;
using TerrorMod.Content.NPCs.Hostile.Forest;
using TerrorMod.Common.Utils;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Globals.Tiles;

public class NPCTile : GlobalTile
{
    bool TileIsValid(int i, int j)
    {
        return !Main.tile[i + 1, j].HasTile && Main.tile[i + 1, j].WallType != WallID.None;
    }

    public override void RandomUpdate(int i, int j, int type)
    {
        if (!SkullSystem.briarSkullActive) return;
        if (type != TileID.Spikes && type != TileID.WoodenSpikes) return;
        if (WorldGen.InWorld(i, j, 5))
        {
            Tile tile = Main.tile[i, j];

            if (TileIsValid(i + 1, j))
            {
                WorldGen.PlaceTile(i + 1, j, tile.TileType);
            }
            if (TileIsValid(i - 1, j))
            {
                WorldGen.PlaceTile(i + 1, j, tile.TileType);
            }
            if (TileIsValid(i, j + 1))
            {
                WorldGen.PlaceTile(i + 1, j, tile.TileType);
            }
            if (TileIsValid(i, j - 1))
            {
                WorldGen.PlaceTile(i + 1, j, tile.TileType);
            }

        }
    }
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
                    NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, NPCType<TreeSpirit>());
                }
            }
        }

        if (type == TileID.ShadowOrbs)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.rand.NextBool(3))
                {
                    NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16, NPCID.ServantofCthulhu, ai3: 0.1f);
                }
            }
        }
    }
}
