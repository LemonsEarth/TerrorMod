using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TerrorMod.Content.Buffs.Debuffs;
using Terraria.Localization;
using Humanizer;
using TerrorMod.Common.Utils;

namespace TerrorMod.Core.Players
{
    public class TerrorPlayer : ModPlayer
    {
        public bool infected = false;

        float infectedTimer = 0;
        float maxInfectedTimer = 600;

        public override void ResetEffects()
        {
            infected = false;
        }

        public override bool CanUseItem(Item item)
        {
            return !Player.HasBuff(BuffID.OgreSpit);
        }

        public override void PostUpdate()
        {
            if (Player.ZoneCrimson) Player.AddBuff(ModContent.BuffType<InfectedCrimson>(), 3);
            if (Player.ZoneCorrupt) Player.AddBuff(ModContent.BuffType<InfectedCorrupt>(), 3);

            if (infected)
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
                Player.KillMe(PlayerDeathReason.ByCustomReason(Language.GetText("Mods.TerrorMod.Buffs.InfectedCrimson.DeathMessage").Format(Main.LocalPlayer.name)), 9999, 0);
                infectedTimer = 0;
            }
        }

        public override void PostUpdateBuffs()
        {
            
        }

        public override void PostUpdateEquips()
        {
            
        }
    }
}