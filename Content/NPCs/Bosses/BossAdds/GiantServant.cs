using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Content.NPCs.Bosses.BossAdds;

public class GiantServant : ModNPC
{
    int AITimer = 0;

    public override string Texture => $"Terraria/Images/NPC_" + NPCID.ServantofCthulhu;

    public override void SetStaticDefaults()
    {
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        NPCID.Sets.MPAllowedEnemies[Type] = true;
        NPCID.Sets.TrailCacheLength[NPC.type] = 5;
        NPCID.Sets.TrailingMode[NPC.type] = 3;
        Main.npcFrameCount[NPC.type] = 2;
        NPCID.Sets.CantTakeLunchMoney[Type] = true;
        NPCID.Sets.DontDoHardmodeScaling[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.ServantofCthulhu);
        AIType = NPCID.ServantofCthulhu;
        AnimationType = NPCID.ServantofCthulhu;
        NPC.lifeMax = 100;
        NPC.damage = 20;
        NPC.scale = 2.5f;
        NPC.noTileCollide = true;
    }

    public override void AI()
    {
        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
        {
            NPC.TargetClosest(false);
        }

        if (NPC.HasValidTarget && AITimer > 30)
        {
            NPC.MoveToPos(Main.player[NPC.target].Center, 0.006f, 0.006f, 0.3f + NPC.ai[3], 0.3f + NPC.ai[3]);
        }

        if (AITimer > 210)
        {
            NPC.SimpleStrikeNPC(NPC.lifeMax, 0, noPlayerInteraction: true);
        }

        AITimer++;
    }

    public override bool CheckActive()
    {
        return false;
    }

    public override void OnKill()
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, ProjectileType<ExplosionLarge>(), NPC.damage, 1f);
        }
    }

    public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
    {
        return false;
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    {
        target.AddBuff(BuffID.Darkness, 120);
        target.AddBuff(BuffType<FearDebuff>(), 20);
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
    {
        NPC.SimpleStrikeNPC(NPC.lifeMax, 0, noPlayerInteraction: true);
        modifiers.Cancel();
    }
}
