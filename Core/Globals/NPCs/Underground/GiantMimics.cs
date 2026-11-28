using Terraria.Audio;

namespace TerrorMod.Core.Globals.NPCs.Underground;

public class GiantMimics : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int PreAITimer = 0;
    int AITimer = 0;
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.aiStyle == NPCAIStyleID.BiomeMimic;
    }

    public override void SetDefaults(NPC entity)
    {

    }

    public override bool PreAI(NPC npc)
    {
        PreAITimer++;
        if (PreAITimer <= 240)
        {
            Dust.NewDustPerfect(npc.RandomPos(), DustID.Granite, -Vector2.UnitY * Main.rand.NextFloat(1, 5)).noGravity = true;
            npc.velocity.X = Main.rand.NextFloat(-1, 1);
            npc.dontTakeDamage = true;
        }
        return PreAITimer > 240;
    }

    public override void AI(NPC npc)
    {
        if (AITimer == 0)
        {
            npc.dontTakeDamage = false;
            int scaleFactor = 10;
            npc.scale = scaleFactor;
            npc.width *= scaleFactor;
            npc.height *= scaleFactor;
            npc.position.Y -= npc.height;
            SoundEngine.PlaySound(SoundID.Zombie92 with { PitchRange = (-1f, -0.5f)}, npc.Center);
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { PitchRange = (-0.5f, -0.2f)}, npc.Center);
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion with { PitchRange = (0f, 0.2f)}, npc.Center);

            for (int i = 0; i < scaleFactor * 2; i++)
            {
                Dust.NewDustPerfect(npc.RandomPos(), DustID.Granite, -Vector2.UnitY * Main.rand.NextFloat(1, 5)).noGravity = true;
            }
            for (int i = 0; i < 3 * scaleFactor; i++)
            {
                LemonUtils.SmokeGore(npc.GetSource_FromAI(), npc.RandomPos(), 5, 10);
            }
        }
        switch (npc.ai[0])
        {
            case 2: //hopping
                if (npc.ai[1] < 40 && npc.ai[2] < 240) npc.ai[1] = 40;
                if (AITimer % 30 == 0)
                {
                    if (LemonUtils.NotClient())
                    {
                        if (Main.rand.NextBool(4))
                        {
                            npc.velocity.X *= 4;
                        }
                    }
                    npc.netUpdate = true;
                }
                break;
            case 3:
                if ((int)npc.ai[1] % 30 == 0)
                {
                    int projType = npc.type switch
                    {
                        NPCID.BigMimicCorruption => ProjectileID.CursedFlameHostile,
                        NPCID.BigMimicCrimson => ProjectileID.GoldenShowerHostile,
                        NPCID.BigMimicHallow => ProjectileID.QueenSlimeMinionPinkBall,
                        _ => ProjectileID.WebSpit
                    };

                    if (LemonUtils.NotClient())
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, -Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 5, projType, npc.damage / 4, 1f);
                        }
                    }
                }
                break;
        }

        if (AITimer % 600 == 0)
        {
            if (LemonUtils.NotClient())
            {
                int weaponType = npc.type switch
                {
                    NPCID.BigMimicCorruption => NPCID.CursedHammer,
                    NPCID.BigMimicCrimson => NPCID.CrimsonAxe,
                    NPCID.BigMimicHallow => NPCID.EnchantedSword,
                    _ => NPCID.RockGolem
                };
                NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, weaponType);
            }
        }
        AITimer++;
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        if (Main.rand.NextBool(2))
        {
            Item heldItem = target.HeldItem;
            target.TryDroppingSingleItem(npc.GetSource_OnHit(target), heldItem);
        }

        int debuffType = npc.type switch
        {
            NPCID.BigMimicCorruption => BuffID.CursedInferno,
            NPCID.BigMimicCrimson => BuffID.Ichor,
            NPCID.BigMimicHallow => BuffID.WitheredWeapon,
            _ => BuffID.WitheredArmor
        };
        target.AddBuff(debuffType, 600);
    }
}
