using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Core.Globals.NPCs.Underground
{
    public class Bats : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AttackTimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.CaveBat
                || entity.type == NPCID.IceBat
                || entity.type == NPCID.JungleBat
                || entity.type == NPCID.Hellbat
                || entity.type == NPCID.GiantBat
                || entity.type == NPCID.Lavabat
                || entity.type == NPCID.IlluminantBat;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Venom, 60);
            target.AddBuff(BuffID.Darkness, 300);
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextBool(3))
            {
                modifiers.FinalDamage *= 0;
                npc.Center += Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 64;
                SoundEngine.PlaySound(SoundID.Item131 with { PitchRange = (-0.2f, 0.2f) }, npc.Center);
            }
        }
    }
}
