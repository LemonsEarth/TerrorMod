namespace TerrorMod.Core.Globals.NPCs.Underground;

public class GiantMimics : GlobalNPC
{
    public override bool InstancePerEntity => true;

    int AITimer = 0;
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.aiStyle == NPCAIStyleID.BiomeMimic;
    }

    public override void AI(NPC npc)
    {
        switch (npc.ai[0])
        {
            case 2: //hopping
                if (npc.ai[1] < 40 && npc.ai[2] < 240) npc.ai[1] = 40;
                if (AITimer % 30 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
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

                    if (Main.netMode != NetmodeID.MultiplayerClient)
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
            if (Main.netMode != NetmodeID.MultiplayerClient)
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
