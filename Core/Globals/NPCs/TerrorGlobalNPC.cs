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
    public class TerrorGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        bool legend = false;
        int AITimer = 0;

        public override void SetDefaults(NPC entity)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (entity.boss)
                {
                    entity.lifeMax += (int)(entity.lifeMax * 0.01f * ModLoader.Mods.Length);
                }
                else
                {
                    entity.lifeMax += (int)(entity.lifeMax * 0.05f * ModLoader.Mods.Length);
                }
            }        
        }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return !entity.SpawnedFromStatue && entity.CanBeChasedBy() && !NPCLists.SafeNPCs.Contains(entity.type) && lateInstantiation;
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.HasBuff<BeforeTheStormBuff>())
            {
                spawnRate = (int)(spawnRate * 5f);
                maxSpawns = (int)(maxSpawns * 0.5f);
            }

            if (player.HasBuff<TheStormDebuff>())
            {
                spawnRate = (int)(spawnRate * 0.8f);
                maxSpawns = (int)(maxSpawns * 1.3f);
            }
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            int chanceDenominator = 100;
            if (npc.rarity == 2) chanceDenominator = 50;
            if (npc.rarity >= 3) chanceDenominator = 20;
            if (npc.boss) chanceDenominator = 10;
            if (Main.rand.NextBool(chanceDenominator))
            {
                legend = true;
                if (npc.boss)
                {
                    npc.lifeMax *= 2;
                    npc.life = npc.lifeMax;
                    npc.damage = (int)(npc.damage * 1.25f);
                }
                else
                {
                    npc.lifeMax = (int)(npc.lifeMax * 2.5f);
                    npc.life = npc.lifeMax;
                    npc.damage = (int)(npc.damage * 1.5f);
                }
            }
        }


        int counter = 4;
        public override void OnKill(NPC npc)
        {
            if (legend && counter > 0)
            {
                counter--;
                npc.NPCLoot();
            }
        }

        Vector2 teleportPos = Vector2.Zero;
        public override void PostAI(NPC npc)
        {
            if (!legend) return;

            var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Torch, Scale: Main.rand.NextFloat(1.2f, 2f));
            dust.noGravity = true;

            if (!npc.HasValidTarget) return;
            Player player = Main.player[npc.target];
            if (!npc.boss)
            {
                if (AITimer > 0 && AITimer == 300)
                {
                    teleportPos = player.Center;
                    for (float i = 0.05f; i < 1f; i += 0.05f)
                    {
                        Vector2 pos = Vector2.Lerp(npc.Center, teleportPos, i);
                        for (int j = 0; j < 3; j++)
                        {
                            Dust.NewDustDirect(pos, 1, 1, DustID.GemRuby, Scale: 1.5f).noGravity = true;
                        }
                    }
                    AITimer = 0;
                }
            }
            if (AITimer == 60 && teleportPos != Vector2.Zero)
            {
                npc.Teleport(teleportPos);
                teleportPos = Vector2.Zero;
                npc.netUpdate = true;
            }

            AITimer++;
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (target.GetModPlayer<TerrorPlayer>().leadArmorSet)
            {
                npc.AddBuff(BuffID.Poisoned, 600);
            }
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (legend)
            {
                modifiers.FinalDamage *= 0.8f;
            }
        }
    }
}
