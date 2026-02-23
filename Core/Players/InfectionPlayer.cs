using Terraria.DataStructures;
using TerrorMod.Content.Buffs.Debuffs;
using Terraria.Localization;
using TerrorMod.Common.Utils;

namespace TerrorMod.Core.Players;

public class InfectionPlayer : ModPlayer
{
    int infectedTimer = 0;
    int maxInfectedTimer = 600;

    public override void PostUpdate()
    {
        if (Player.ZoneCrimson) Player.AddBuff(BuffType<InfectedCrimson>(), 3);
        if (Player.ZoneCorrupt) Player.AddBuff(BuffType<InfectedCorrupt>(), 3);

        if (Player.HasBuff<InfectedCorrupt>())
        {
            infectedTimer++;
            Dust.NewDust(Player.position, Player.width, Player.height, DustID.Corruption, Scale: infectedTimer / (maxInfectedTimer / 2));
            Dust.NewDust(Player.position, Player.width, Player.height, DustID.Crimson, Scale: infectedTimer / (maxInfectedTimer / 2));
        }
        else
        {
            if (infectedTimer > 0) infectedTimer--;
        }

        if (infectedTimer > maxInfectedTimer)
        {
            LemonUtils.DustCircle(Player.Center, 8, 5, DustID.Corruption, 3f);
            LemonUtils.DustCircle(Player.Center, 8, 5, DustID.Crimson, 3f);
            Player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TerrorMod.Buffs.InfectedCrimson.DeathMessage", Main.LocalPlayer.name)), 9999, 1);
            infectedTimer = 0;
        }
    }
}