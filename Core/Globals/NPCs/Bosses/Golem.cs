using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Projectiles.Hostile;
using System.Collections.Generic;
using System.Linq;
using TerrorMod.Core.Configs;
using Terraria.GameContent.ItemDropRules;
using TerrorMod.Content.Items.Accessories;

namespace TerrorMod.Core.Globals.NPCs.Bosses
{
    public class Golem : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        int AITimer = 0;
        int AttackTimer = 0;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Golem
                || entity.type == NPCID.GolemHead
                || entity.type == NPCID.GolemHeadFree
                || entity.type == NPCID.GolemFistLeft
                || entity.type == NPCID.GolemFistRight;
        }

        public override void AI(NPC npc)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];

           
            AITimer++;
        }
        
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (!TerrorServerConfigs.serverConfig.EnableBossChanges) return;
            if (npc.ai[1] == 1) modifiers.FinalDamage *= 0.5f;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Cursed, 75);
            target.AddBuff(BuffID.Weak, 120);
        }
    }
}
