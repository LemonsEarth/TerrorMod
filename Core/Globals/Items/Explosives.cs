using System.Collections.Generic;

namespace TerrorMod.Core.Globals.Items;

public class Explosives : GlobalItem
{
    public override bool InstancePerEntity => true;
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.Bomb || entity.type == ItemID.Dynamite;
    }

    public override void UpdateInventory(Item item, Player player)
    {
        if (Main.rand.NextBool(500))
        {
            item.stack--;
            if (item.type == ItemID.Bomb)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, ProjectileID.Bomb, item.damage, 1f);
                }
            }
            else if (item.type == ItemID.Dynamite)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, ProjectileID.Dynamite, item.damage, 1f);
                }
            }
        }
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        var line = new TooltipLine(Mod, "TerrorMod:SlipperyExplosives", "They are a bit slippery, hard to hold on to...");
        tooltips.Add(line);
    }
}
