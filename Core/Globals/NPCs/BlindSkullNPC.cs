using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Conditions;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Buffs;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Items.Consumables;
using TerrorMod.Content.Projectiles.Hostile;
using TerrorMod.Core.Players;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Globals.NPCs
{
    public class BlindSkullNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return true;
        }

        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (!SkullSystem.blindSkullActive) return null;
            return false;
        }
    }
}
