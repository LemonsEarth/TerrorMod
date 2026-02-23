using System.IO;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace TerrorMod.Core.Systems;

public class EventSystem : ModSystem
{
    public static bool hellbreachActive = false;
    public static bool finishedHellbreach = false;

    public override void PostUpdateWorld()
    {
        if (HellbreachStartCheck())
        {
            hellbreachActive = true;
            if (Main.netMode != NetmodeID.MultiplayerClient) ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.TerrorMod.Messages.Hellbreach.StartMessage"), Color.OrangeRed);
        }

        if (hellbreachActive && Utils.GetDayTimeAs24FloatStartingFromMidnight() > 19.50f)
        {
            hellbreachActive = false;
            finishedHellbreach = true;
            if (Main.netMode != NetmodeID.MultiplayerClient) ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.TerrorMod.Messages.Hellbreach.EndMessage"), Color.OrangeRed);
        }

        if ((int)Main.time == 1 && !Main.dayTime && DayCountSystem.dayCount == 3)
        {
            Main.bloodMoon = true;
            if (Main.netMode != NetmodeID.MultiplayerClient) ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("The Bloody Moon rises..."), Color.Red);
        }

        if ((int)Main.time == 1 && Main.dayTime && DayCountSystem.dayCount % 3 == 0 && Main.hardMode)
        {
            Main.eclipse = true;
            if (Main.netMode != NetmodeID.MultiplayerClient) ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("A Solar Eclipse has begun!"), Color.OrangeRed);
        }
    }

    bool HellbreachStartCheck()
    {
        int chanceDenominator = !finishedHellbreach ? 3 : 20;
        if (Math.Floor(Main.time) == 1 && Main.dayTime && DayCountSystem.dayCount > 4 && !hellbreachActive)
        {
            if (Main.rand.NextBool(chanceDenominator)) return true;
        }
        return false;
    }

    public override void ClearWorld()
    {
        hellbreachActive = false;
        finishedHellbreach = false;
    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag["hellbreachActive"] = hellbreachActive;
        tag["finishedHellbreach"] = finishedHellbreach;
    }

    public override void LoadWorldData(TagCompound tag)
    {
        hellbreachActive = tag.GetBool("hellbreachActive");
        finishedHellbreach = tag.GetBool("finishedHellbreach");
    }

    public override void NetSend(BinaryWriter writer)
    {
        writer.WriteFlags(hellbreachActive, finishedHellbreach);
    }

    public override void NetReceive(BinaryReader reader)
    {
        reader.ReadFlags(out hellbreachActive, out finishedHellbreach);
    }
}
