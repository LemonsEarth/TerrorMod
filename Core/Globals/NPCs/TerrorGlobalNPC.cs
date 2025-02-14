using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Conditions;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Items.Consumables;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Players;

namespace TerrorMod.Core.Globals.NPCs
{
    public class TerrorGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (target.GetModPlayer<TerrorPlayer>().leadArmorSet)
            {
                npc.AddBuff(BuffID.Poisoned, 600);
            }
        }
    }
}
