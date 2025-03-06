using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorMod.Core.Globals.NPCs.PirateInvasion
{
    public class Pirates : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override void SetDefaults(NPC entity)
        {
            
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Parrot
                || entity.type == NPCID.PirateCaptain
                || entity.type == NPCID.PirateCorsair
                || entity.type == NPCID.PirateCrossbower
                || entity.type == NPCID.PirateDeadeye
                || entity.type == NPCID.PirateDeckhand
                || entity.type == NPCID.PirateGhost
                || entity.type == NPCID.PirateShipCannon;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            int chanceDenominator = npc.type == NPCID.PirateCorsair || npc.type == NPCID.PirateDeckhand ? 1 : 4;
            if (Main.rand.NextBool(chanceDenominator))
            {
                Item heldItem = target.HeldItem;
                target.DropItem(npc.GetSource_OnHit(target), target.Center, ref heldItem);
            }
        }

        public override void AI(NPC npc)
        {
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

            if (npc.type == NPCID.PirateCrossbower)
            {
                if (npc.ai[1] > 50) npc.ai[1] = 50;
                if (npc.ai[1] > 10 && npc.ai[1] < 40) npc.ai[1] = 10;
            }

            if (npc.type == NPCID.PirateDeadeye)
            {
                if (npc.ai[1] == 21)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, npc.DirectionTo(player.Center).RotatedBy(i * MathHelper.ToRadians(5)) * 7, ProjectileID.BulletDeadeye, npc.damage / 2, 1);
                        }
                    }
                }
            }

            if (npc.type == NPCID.PirateCaptain)
            {
                if (AITimer % 1200 == 0)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitY * 10, ProjectileID.SharknadoBolt, npc.damage / 4, 1f);
                }
            }

            AITimer++;
        }

        public override void OnKill(NPC npc)
        {
            if (!Main.rand.NextBool(10)) return;
            if (npc.type != NPCID.PirateShipCannon && npc.type != NPCID.PirateCaptain && npc.type != NPCID.PirateGhost)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PirateGhost);
                }
            }
        }       
    }
}
