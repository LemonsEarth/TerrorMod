using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Content.Buffs.Debuffs;

namespace TerrorMod.Core.Globals.NPCs.Underground
{
    public class Jellyfish : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AttackTimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.aiStyle == NPCAIStyleID.Jellyfish;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Venom, 120);
            target.AddBuff(BuffID.Electrified, 60);
        }
    }
}
