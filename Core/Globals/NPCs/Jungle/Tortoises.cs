using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ReLogic.Utilities;
using TerrorMod.Common.Utils;
using System;

namespace TerrorMod.Core.Globals.NPCs.Jungle
{
    public class Tortoises : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        bool appliedBoost = false;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.aiStyle == NPCAIStyleID.GiantTortoise;
        }

        public override void AI(NPC npc)
        {
            switch (npc.ai[0]) // ai0 represents state
            {
                case 0: //roaming
                    
                    if (npc.ai[1] < 360) npc.ai[1] = 360;
                    break;
                case 1:
                    npc.ai[1] += 3;
                    break;
                case 2:
                    
                    break;
                case 3:
                    if (Math.Abs(npc.ai[3]) < 11)
                    {
                        npc.ai[3] *= 3;
                    }
                    break;
            }
        }
    }
}
