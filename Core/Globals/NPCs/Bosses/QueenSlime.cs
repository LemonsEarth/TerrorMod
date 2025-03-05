using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.NPCs.Hostile.Forest;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class QueenSlime : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.QueenSlimeBoss;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;

            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            //ai0 is state

            switch (npc.ai[0])
            {
                case 0: //nothing
                    //if (Main.netMode != NetmodeID.MultiplayerClient)
                    //{
                    //    npc.ai[0] = 3;
                    //}
                    //npc.netUpdate = true;
                    break;
                case 3: // jumping 
                    if (npc.ai[1] > -40 && npc.ai[1] < -30)
                    {
                        npc.ai[1] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = -1; i <= 1; i++)
                            {
                                Vector2 dir = -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(15) * i);
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 10, ProjectileID.QueenSlimeGelAttack, npc.damage / 4, 1f);
                            }
                        }
                    }
                    break;
                case 4:
                    if (npc.ai[1] == 30)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                Vector2 pos = player.Center + new Vector2(Main.rand.Next(-500, 500), Main.rand.Next(-900, -700));
                                Projectile.NewProjectile(npc.GetSource_FromAI(), pos, Vector2.UnitY * 3, ProjectileID.QueenSlimeMinionPinkBall, npc.damage / 4, 1f);
                            }
                        }
                        foreach (var allPlayers in Main.ActivePlayers)
                        {
                            allPlayers.AddBuff(ModContent.BuffType<Weight>(), 60);
                        }
                    }
                    break;
                case 5:
                    //if (npc.ai[1] < 50) npc.ai[1] = 50;
                    break;
            }

            npc.ai[3] = 0;

            AITimer++;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OgreSpit, 60);
            target.AddBuff(ModContent.BuffType<Weight>(), 60);
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {

        }

        public override bool PreAI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return true;
            return true;
        }

        public override void PostAI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        }
    }
}
