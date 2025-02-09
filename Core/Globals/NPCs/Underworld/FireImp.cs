using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Core.Globals.NPCs.Underworld
{
    public class FireImp : GlobalNPC
    {
        int AITimer = 0;

        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.FireImp;
        }

        public override void AI(NPC npc)
        {
            if (AITimer % 180 == 0 && npc.HasValidTarget)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.Center.DirectionTo(Main.player[npc.target].Center) * 10, ModContent.ProjectileType<FireballClone>(), npc.damage / 3, 1f);
                }
            }

            AITimer++;
        }
    }
}
