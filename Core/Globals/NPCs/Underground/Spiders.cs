using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Core.Globals.NPCs.Underground
{
    public class Spiders : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AttackTimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.BloodCrawler
                || entity.type == NPCID.BloodCrawlerWall
                || entity.type == NPCID.WallCreeper
                || entity.type == NPCID.WallCreeperWall
                || entity.type == NPCID.BlackRecluse
                || entity.type == NPCID.BlackRecluseWall
                || entity.type == NPCID.JungleCreeper
                || entity.type == NPCID.JungleCreeperWall;
        }

        public override void PostAI(NPC npc)
        {
            if (AttackTimer == 120)
            {
                if (npc.HasValidTarget)
                {
                    Player player = Main.player[npc.target];
                    if (Collision.CanHit(npc, player))
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.Center.DirectionTo(player.Center) * 5, ProjectileID.WebSpit, (int)(npc.damage * 0.4f), 1f);
                        }
                        AttackTimer = 0;
                    }
                }
            }

            if (AttackTimer < 120)
            {
                AttackTimer++;
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Venom, 60);
            target.AddBuff(ModContent.BuffType<ArachnophobiaDebuff>(), 600);
        }

        public override void OnKill(NPC npc)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < 8; i++)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY.RotatedBy(i * MathHelper.PiOver4) * 5, ProjectileID.WebSpit, (int)(npc.damage * 0.4f), 1f);
                }
            }
        }
    }
}
