using System.IO;
using Terraria.ModLoader.IO;

namespace TerrorMod.Core.Systems.World;

public class DayCountSystem : ModSystem
{
    public static int DayCount { get; set; } = 0;

    public override void ClearWorld()
    {
        DayCount = 0;
    }

    public override void LoadWorldData(TagCompound tag)
    {
        DayCount = tag.GetInt("dayCount");
    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag["dayCount"] = DayCount;
    }

    public override void PostUpdateWorld()
    {
        if ((int)Main.time == (int)Main.nightLength - 1)
        {
            DayCount++;
        }
    }

    public override void NetSend(BinaryWriter writer)
    {
        writer.Write(DayCount);
    }

    public override void NetReceive(BinaryReader reader)
    {
        reader.ReadInt32();
    }
}
