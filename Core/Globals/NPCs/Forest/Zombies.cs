using Terraria.DataStructures;

namespace TerrorMod.Core.Globals.NPCs.Forest;

public class Zombies : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.Zombie
            || entity.type == NPCID.ZombieEskimo
            || entity.type == NPCID.ZombieDoctor
            || entity.type == NPCID.ZombieElf
            || entity.type == NPCID.ZombieMushroom
            || entity.type == NPCID.BaldZombie
            || entity.type == NPCID.PincushionZombie
            || entity.type == NPCID.SlimedZombie
            || entity.type == NPCID.SwampZombie
            || entity.type == NPCID.TwiggyZombie
            || entity.type == NPCID.FemaleZombie
            || entity.type == NPCID.ZombieRaincoat
            || entity.type == NPCID.ZombieMushroomHat
            || entity.type == NPCID.TheGroom
            || entity.type == NPCID.TheBride
            || entity.type == NPCID.DesertGhoul
            || entity.type == NPCID.DesertGhoulCorruption
            || entity.type == NPCID.DesertGhoulCrimson
            || entity.type == NPCID.DesertGhoulHallow
            || entity.type == NPCID.Mummy
            || entity.type == NPCID.BloodMummy
            || entity.type == NPCID.DarkMummy
            || entity.type == NPCID.LightMummy
            || entity.type == NPCID.ZombieMerman;
    }

    public override void OnKill(NPC npc)
    {
        if (Main.rand.NextBool(8))
        {
            NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.Skeleton, npc.whoAmI);
        }
    }

    public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
    {
        target.AddBuff(BuffID.Bleeding, 1800);
        target.AddBuff(BuffID.Weak, 600);

        if (hurtInfo.Damage > target.statLife)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(new EntitySource_SpawnNPC("ZombieKilledPlayer"), (int)target.Center.X, (int)target.Center.Y, NPCID.Zombie, npc.whoAmI);
            }
        }
    }

    public override void OnHitNPC(NPC npc, NPC target, NPC.HitInfo hit)
    {
        if (npc.isLikeATownNPC) hit.Damage *= 2;
        if (hit.Damage > target.life)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(new EntitySource_SpawnNPC("ZombieKilledPlayer"), (int)target.Center.X, (int)target.Center.Y, NPCID.Zombie, npc.whoAmI);
            }
        }
    }

    int dashTimer = 0;
    int maxDashTimer = 360;
    public override void AI(NPC npc)
    {
        if (dashTimer < maxDashTimer) dashTimer++;

        if (dashTimer == maxDashTimer && npc.HasValidTarget && Math.Abs(Main.player[npc.target].Center.Y - npc.Center.Y) < 64) // Only dash if the target is at similar height level
        {
            npc.velocity.X *= 7; // Dash attack
            dashTimer = 0;
        }

        if (dashTimer < 10)
        {
            npc.noTileCollide = true;
            npc.velocity.Y = 0;
        }
        else
        {
            npc.noTileCollide = false;
        }
    }
}
