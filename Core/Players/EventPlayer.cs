using System.Linq;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Players;

public class EventPlayer : ModPlayer
{
    public bool halloweenHorror = false;
    public bool gunpowderedSnow = false;
    public override void ResetEffects()
    {
        halloweenHorror = false;
        gunpowderedSnow = false;
    }

    public override void PostUpdate()
    {
        EventDebuffs();
    }

    void EventDebuffs()
    {
        if (EventSystem.hellbreachActive)
        {
            Player.AddBuff(BuffID.ShadowCandle, 2);
        }

        if (Main.bloodMoon)
        {
            Player.AddBuff(BuffID.Bleeding, 2);
            Player.AddBuff(BuffID.WaterCandle, 2);
        }

        if (Main.eclipse)
        {
            Player.AddBuff(BuffID.WaterCandle, 2);
            Player.AddBuff(BuffID.Battle, 2);
        }

        if (NPC.downedPlantBoss && !NPC.downedHalloweenKing)
        {
            Player.AddBuff(BuffType<HalloweenHorrorDebuff>(), 2);
        }

        if (NPC.downedPlantBoss && !NPC.downedChristmasIceQueen)
        {
            Player.AddBuff(BuffType<GunpowderedSnowDebuff>(), 2);
        }

        if (halloweenHorror)
        {
            if (Main.rand.NextBool(1000) && !Main.projectile.Any(proj => proj.active && proj.type == ProjectileType<PumpkingHeadProj>()))
            {
                if (Player.whoAmI == Main.myPlayer)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ProjectileType<PumpkingHeadProj>(), 1, 1f, Player.whoAmI);
                }
            }
        }

        if (gunpowderedSnow && TerrorPlayer.Timer % 600 == 0)
        {
            if (Main.rand.NextBool(30) && !NPC.AnyNPCs(NPCID.SnowBalla) && !NPC.AnyNPCs(NPCID.SnowmanGangsta) && !NPC.AnyNPCs(NPCID.MisterStabby))
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int amount = Main.rand.Next(4, 10);
                    for (int i = 0; i < amount; i++)
                    {
                        Vector2 pos = Player.Center + Main.rand.NextVector2CircularEdge(800, 800);
                        NPC.NewNPC(Player.GetSource_FromThis("TerrorMod:DebuffSpawn"), (int)pos.X, (int)pos.Y, Main.rand.Next(143, 146));
                    }
                }
            }
        }
    }
}