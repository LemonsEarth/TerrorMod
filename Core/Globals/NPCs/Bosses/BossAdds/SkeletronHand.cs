using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses.BossAdds
{
    public class SkeletronHand : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override void SetDefaults(NPC entity)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            entity.defense = 16;
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.SkeletronHand;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            if (AITimer == 0 || !Main.projectile.Any(proj => proj.active && proj.type == ModContent.ProjectileType<SkeletronGlock>() && proj.ai[1] == npc.whoAmI))
            { 
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<SkeletronGlock>(), npc.damage / 4, 1, ai1: npc.whoAmI);
                }
            }

            npc.rotation = npc.Center.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2;

            //Main.NewText("ai0: " + npc.ai[0]);
            //Main.NewText("ai1: " + npc.ai[1]);
            //Main.NewText("ai2: " + npc.ai[2]);
            //Main.NewText("ai3: " + npc.ai[3]);

            AITimer++;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Cursed, 75);
            target.AddBuff(BuffID.Weak, 120);
        }
    }
}
