using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using TerrorMod.Content.Buffs.Debuffs.Movement;
using TerrorMod.Content.NPCs.Bosses.BossAdds;
using TerrorMod.Content.NPCs.Hostile.Special;
using TerrorMod.Core.Configs;

namespace TerrorMod.Core.Globals.NPCs.Bosses;

public class Twins : GlobalNPC
{
    public override bool InstancePerEntity => true;
    string WhiteCirclePath => "TerrorMod/Common/Assets/Textures/WhiteCircle";
    static Asset<Texture2D> WhiteCircle;

    int AITimer = 0;

    public override void Load()
    {
        WhiteCircle = Request<Texture2D>(WhiteCirclePath);
    }

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.Retinazer || entity.type == NPCID.Spazmatism;
    }

    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        if (npc.type == NPCID.Retinazer)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCType<ForgottenSibling>());
            }
        }
    }

    public override void AI(NPC npc)
    {
        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
        if (!npc.HasValidTarget) return;
        Player player = Main.player[npc.target];
        if (player.Distance(npc.Center) > 5000) player.AddBuff(BuffID.CursedInferno, 2);
        if (npc.type == NPCID.Retinazer)
        {
            RetinazerAI(npc);
        }

        if (npc.type == NPCID.Spazmatism)
        {
            SpazmatismAI(npc);
        }

        AITimer++;
    }

    void RetinazerAI(NPC npc)
    {
        if (npc.ai[0] != 3)
        {
            if (npc.ai[1] == 0) // laser
            {
                if (npc.ai[3] > 0 && npc.ai[3] < 40)
                {
                    npc.ai[3] = 40; // laser timer
                }
            }
            else if (npc.ai[1] == 2) //dashing
            {
                if (npc.ai[2] <= 2)
                {
                    npc.velocity *= 1.2f;
                }
            }
        }
        else
        {
            if (npc.ai[1] == 1) // spam
            {
                if (npc.localAI[1] < 40)
                {
                    npc.localAI[1] = 40;
                }
            }
        }
    }

    void SpazmatismAI(NPC npc)
    {
        if (npc.ai[0] == 3) // if p2
        {
            if (npc.ai[1] == 2) // dashing
            {
                if (npc.ai[2] < 2)
                {
                    npc.velocity *= 2f;
                }
            }
            else if (npc.ai[1] == 0) // flame
            {
                if (npc.localAI[2] == 0 && npc.ai[2] < 300)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = -2; i <= 2; i++)
                        {
                            Vector2 dir = npc.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(15 * i));
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, dir * 10, ProjectileID.CursedFlameHostile, npc.damage / 4, 1f);
                        }
                    }
                }
            }
        }
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (NPC.AnyNPCs(NPCType<MechanicalCore>()))
        {
            modifiers.FinalDamage *= 0;
        }

        if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;

        if (NPC.AnyNPCs(NPCType<ForgottenSibling>()))
        {
            modifiers.FinalDamage *= 0.5f;
        }
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        target.AddBuff(BuffID.Darkness, 120);
        if (Main.rand.NextBool(3))
        {
            target.AddBuff(BuffType<FearDebuff>(), 60);

        }
    }

    public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (NPC.AnyNPCs(NPCType<ForgottenSibling>()))
        {
            spriteBatch.Draw(WhiteCircle.Value, npc.position + WhiteCircle.Size() * 0.25f - screenPos, null, Color.Blue * 0.2f, 0f, WhiteCircle.Size(), 0.3f, SpriteEffects.None, 0);
        }
    }
}
