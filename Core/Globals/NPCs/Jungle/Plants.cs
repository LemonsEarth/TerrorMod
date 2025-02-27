using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Utilities;
using TerrorMod.Common.Utils;

namespace TerrorMod.Core.Globals.NPCs.Jungle
{
    public class Plants : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.aiStyle == NPCAIStyleID.ManEater;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            npc.lifeRegen += (int)(npc.lifeMax / 100f);
        }
    }
}
