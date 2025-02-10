using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;

namespace TerrorMod.Core.Globals.NPCs.Bosses.BossAdds
{
    public class Creeper : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Creeper;
        }

        public override void SetDefaults(NPC entity)
        {
            entity.lifeMax = 110;
        }

        public override void OnKill(NPC npc)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(new EntitySource_SpawnNPC(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu, npc.whoAmI, ai3: 0.1f);
            }
        }
    }
}
