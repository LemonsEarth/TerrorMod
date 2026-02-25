using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Common.Utils;
using System.Linq;
using TerrorMod.Content.Buffs.Debuffs.Phobias;

namespace TerrorMod.Core.Players;

public class PhobiaPlayer : ModPlayer
{
    public override void PostUpdate()
    {
        PhobiaCheckAndApply();
    }

    void PhobiaCheckAndApply()
    {
        // Phobia immunities

        if (Player.buffType.Any(buff => Main.vanityPet[buff] == true))
        {
            Player.buffImmune[BuffType<NyctophobiaDebuff>()] = true; // Immune to fear of dark if any pet is active
        }

        if (Player.HasBuff(BuffID.SpiderMinion) || Player.HasBuff(BuffID.PetSpider)
            || Player.armor[0].type == ItemID.SpiderMask || Player.miscEquips[4].type == ItemID.WebSlinger)
        {
            Player.buffImmune[BuffType<ArachnophobiaDebuff>()] = true; // Immune to fear of spiders if player has spider-related items
        }

        if (NPC.killCount[Item.NPCtoBanner(NPCID.BloodZombie)] > 50 || NPC.killCount[Item.NPCtoBanner(NPCID.Drippler)] > 50 || NPC.killCount[Item.NPCtoBanner(NPCID.FaceMonster)] > 50 || NPC.killCount[Item.NPCtoBanner(NPCID.Crimera)] > 50)
        {
            Player.buffImmune[BuffType<HemophobiaDebuff>()] = true;
        }

        if (Player.noFallDmg || Player.equippedWings != null || Player.mount.Active)
        {
            Player.buffImmune[BuffType<AcrophobiaDebuff>()] = true;
        }

        if (NPC.downedFishron)
        {
            Player.buffImmune[BuffType<ThalassophobiaDebuff>()] = true;
        }

        if (Player.ZoneSkyHeight) Player.AddBuff(BuffType<AcrophobiaDebuff>(), 2);
        if (Player.wet) Player.AddBuff(BuffType<ThalassophobiaDebuff>(), 2);
        if (Player.ZoneCrimson || (Main.bloodMoon && Player.ZoneOverworldHeight)) Player.AddBuff(BuffType<HemophobiaDebuff>(), 2);

        bool lightCheck = LemonUtils.CheckAllForLight(0.3f, Player.Center + new Vector2(32, 0), Player.Center - new Vector2(32, 0));
        if (Player.ZoneCorrupt || !lightCheck) Player.AddBuff(BuffType<NyctophobiaDebuff>(), 2);
        if (Main.tile[(int)Player.Center.X / 16, (int)Player.Center.Y / 16].WallType == WallID.SpiderUnsafe)
        {
            Player.AddBuff(BuffType<ArachnophobiaDebuff>(), 2);
        }
    }
}