using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorMod.Core.Globals.NPCs.Forest
{
    public class Slimes : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.IceSlime
                || entity.type == NPCID.Slimer
                || entity.type == NPCID.Slimeling
                || entity.type == NPCID.SlimedZombie
                || entity.type == NPCID.BlueSlime
                || entity.type == NPCID.GreenSlime
                || entity.type == NPCID.RedSlime
                || entity.type == NPCID.YellowSlime
                || entity.type == NPCID.PurpleSlime
                || entity.type == NPCID.BlackSlime
                || entity.type == NPCID.MotherSlime
                || entity.type == NPCID.ShimmerSlime
                || entity.type == NPCID.JungleSlime
                || entity.type == NPCID.SandSlime;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OgreSpit, 60);
        }
    }
}
