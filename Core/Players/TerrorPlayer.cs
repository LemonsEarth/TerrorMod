﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TerrorMod.Content.Buffs.Debuffs;
using Terraria.Localization;
using TerrorMod.Common.Utils;
using System.Linq;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using TerrorMod.Core.Systems;
using Terraria.ModLoader.IO;
using System.IO;
using System.Collections.Generic;
using TerrorMod.Content.Buffs.Buffs;
using TerrorMod.Content.Items.Special;
using System.Collections;
using System;
using TerrorMod.Content.NPCs.Hostile.Special;
using TerrorMod.Content.Projectiles.Hostile;
using Terraria.WorldBuilding;
using TerrorMod.Content.NPCs.Bosses;

namespace TerrorMod.Core.Players
{
    public class TerrorPlayer : ModPlayer
    {
        int timer = 0;
        public bool infected = false;
        public bool overdosed = false;
        public bool leadArmorSet = false;
        public bool undeadAmulet = false;
        public bool ultimateTerror = false;

        public bool halloweenHorror = false;
        public bool gunpowderedSnow = false;

        public bool cobaltHead = false;
        public bool cobaltBody = false;
        public bool cobaltLegs = false;

        float infectedTimer = 0;
        float maxInfectedTimer = 600;

        public int curseLevel = 0;

        int savageSkullTimer = 0;

        public override void ResetEffects()
        {
            infected = false;
            overdosed = false;
            leadArmorSet = false;
            undeadAmulet = false;
            cobaltHead = false;
            cobaltBody = false;
            cobaltLegs = false;
            halloweenHorror = false;
            gunpowderedSnow = false;
            ultimateTerror = false;
        }

        public override void UpdateDead()
        {
            if (SkullSystem.vagrantSkullActive)
            {
                Player.RemoveSpawn();
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Player.HasBuff(ModContent.BuffType<TungstenPenetration>()))
            {
                modifiers.ArmorPenetration += 5;
            }
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            savageSkullTimer = 300;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (cobaltHead)
            {
                if (Main.rand.NextBool(10)) target.AddBuff(BuffID.Poisoned, 300);
                if (Main.rand.NextBool(10)) target.AddBuff(BuffID.Venom, 300);
            }

            if (cobaltBody)
            {
                if (Main.rand.NextBool(10)) target.AddBuff(BuffID.ShadowFlame, 300);
                if (Main.rand.NextBool(10)) target.AddBuff(BuffID.CursedInferno, 300);
            }

            if (cobaltLegs)
            {
                if (Main.rand.NextBool(10)) target.AddBuff(BuffID.Ichor, 300);
                if (Main.rand.NextBool(10)) target.AddBuff(BuffID.Oiled, 300);
            }
        }

        public override bool CanUseItem(Item item)
        {
            return !Player.HasBuff(BuffID.OgreSpit) && !Player.HasBuff(ModContent.BuffType<FearDebuff>());
        }

        public override void PostUpdate()
        {
            if (Main.netMode == NetmodeID.Server) return;
            EventDebuffs();
            BiomeDebuffs();
            PhobiaCheck();
            if (timer % 60 == 0 && Terraria.Graphics.Effects.Filters.Scene["TerrorMod:DesaturateShader"].IsActive() && !NPC.AnyNPCs(ModContent.NPCType<InfiniteTerrorCage>()))
            {
                Terraria.Graphics.Effects.Filters.Scene.Deactivate("TerrorMod:DesaturateShader");
            }

            if (timer % 60 == 0)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<InfiniteTerrorHead>()))
                {
                    if (SkyManager.Instance["TerrorMod:TerrorSky"].IsActive())
                    {
                        SkyManager.Instance.Deactivate("TerrorMod:TerrorSky");
                    }
                }
            }

            if (Player.HasBuff(BuffID.ChaosState))
            {
                if (timer % 60 == 0)
                {
                    Player.Teleport(Player.Center + Main.rand.NextVector2Circular(800, 800), 1);
                }
            }
            if (savageSkullTimer > 0) savageSkullTimer--;
            timer++;
        }

        void BiomeDebuffs()
        {
            int buffLimit = 4;

            if (NPC.downedBoss1) buffLimit++;
            if (NPC.downedQueenBee) buffLimit += 2;
            if (NPC.downedBoss3) buffLimit++;
            if (NPC.downedDeerclops) buffLimit++;
            if (NPC.downedPirates) buffLimit += 2;
            if (NPC.downedQueenSlime) buffLimit += 2;
            if (NPC.downedHalloweenKing) buffLimit += 2;
            if (NPC.downedChristmasIceQueen) buffLimit += 2;
            if (NPC.downedAncientCultist) buffLimit += 2;

            if (Player.buffType.Count(buff => buff != 0 && Main.debuff[buff] == false) > buffLimit)
            {
                Player.AddBuff(ModContent.BuffType<Overdosed>(), 2);
            }

            if (Player.ZoneUnderworldHeight) Player.AddBuff(BuffID.OnFire, 2);

            if (Player.ZoneCrimson) Player.AddBuff(ModContent.BuffType<InfectedCrimson>(), 3);
            if (Player.ZoneCorrupt) Player.AddBuff(ModContent.BuffType<InfectedCorrupt>(), 3);

            if (infected)
            {
                infectedTimer++;
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.Corruption, Scale: infectedTimer / (maxInfectedTimer / 2));
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.Crimson, Scale: infectedTimer / (maxInfectedTimer / 2));
            }
            else
            {
                if (infectedTimer > 0) infectedTimer--;
            }

            if (infectedTimer > maxInfectedTimer)
            {
                LemonUtils.DustCircle(Player.Center, 8, 5, DustID.Corruption, 3f);
                LemonUtils.DustCircle(Player.Center, 8, 5, DustID.Crimson, 3f);
                Player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TerrorMod.Buffs.InfectedCrimson.DeathMessage", Main.LocalPlayer.name)), 9999, 1);        
                infectedTimer = 0;
            }

            if (Player.ZoneSnow && !Player.HasBuff(BuffID.Campfire) && !Player.HasBuff(BuffID.Warmth) && !NPC.downedChristmasIceQueen)
            {
                Player.AddBuff(BuffID.Frostburn, 2);
                Player.AddBuff(BuffID.Chilled, 2);
                if (Main.rand.NextBool(1000))
                {
                    Player.AddBuff(BuffID.Frozen, 120);
                }
            }

            if (Player.ZoneDesert && !NPC.downedGolemBoss)
            {
                if (Main.dayTime &&
                    !(Player.armor[0].type == ItemID.CactusHelmet && Player.armor[1].type == ItemID.CactusBreastplate && Player.armor[2].type == ItemID.CactusLeggings))
                {
                    Player.AddBuff(BuffID.OnFire, 2);
                }

                if (!Main.dayTime && !Player.HasBuff(BuffID.Campfire) && !Player.HasBuff(BuffID.Warmth))
                {
                    Player.AddBuff(BuffID.Frostburn, 2);
                }
            }

            if (Player.ZoneJungle)
            {
                Player.AddBuff(BuffID.Darkness, 2);
                Player.AddBuff(BuffID.Wet, 2);
            }
        }

        void EventDebuffs()
        {
            if (EventSystem.hellbreachActive)
            {
                Player.AddBuff(BuffID.ShadowCandle, 2);
            }

            if (Main.bloodMoon)
            {
                Player.AddBuff(BuffID.Bleeding, 2);
                Player.AddBuff(BuffID.WaterCandle, 2);
            }

            if (Main.eclipse)
            {
                Player.AddBuff(BuffID.WaterCandle, 2);
                Player.AddBuff(BuffID.Battle, 2);
            }

            if (curseLevel > 0)
            {
                Player.AddBuff(ModContent.BuffType<CurseDebuff>(), 2);
            }

            if (NPC.downedPlantBoss && !NPC.downedHalloweenKing)
            {
                Player.AddBuff(ModContent.BuffType<HalloweenHorrorDebuff>(), 2);
            }

            if (NPC.downedPlantBoss && !NPC.downedChristmasIceQueen)
            {
                Player.AddBuff(ModContent.BuffType<GunpowderedSnowDebuff>(), 2);
            }

            if (halloweenHorror)
            {
                if (Main.rand.NextBool(1000) && !Main.projectile.Any(proj => proj.active && proj.type == ModContent.ProjectileType<PumpkingHeadProj>()))
                {
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<PumpkingHeadProj>(), 1, 1f, Player.whoAmI);
                    }
                }
            }

            if (gunpowderedSnow && timer % 600 == 0)
            {
                if (Main.rand.NextBool(30) && !NPC.AnyNPCs(NPCID.SnowBalla) && !NPC.AnyNPCs(NPCID.SnowmanGangsta) && !NPC.AnyNPCs(NPCID.MisterStabby))
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int amount = Main.rand.Next(4, 10);
                        for (int i = 0; i < amount; i++)
                        {
                            Vector2 pos = Player.Center + Main.rand.NextVector2CircularEdge(800, 800);
                            NPC.NewNPC(Player.GetSource_FromThis("TerrorMod:DebuffSpawn"), (int)pos.X, (int)pos.Y, Main.rand.Next(143, 146));
                        }
                    }
                }
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (SkullSystem.gluttonySkullActive)
            {
                Player.lifeRegen -= (int)(Player.manaSickReduction * 10) * 2;
            }
            if (overdosed)
            {
                Player.lifeRegen -= 7;
            }

            if (ultimateTerror)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegenTime = 0;
                Player.lifeRegen -= 150;
            }
        }

        public override void UpdateLifeRegen()
        {
            if (SkullSystem.savageSkullActive && savageSkullTimer <= 0)
            {
                Player.lifeRegenTime = 0;
            }
        }

        void PhobiaCheck()
        {
            // Phobia immunities

            if (Player.buffType.Any(buff => Main.vanityPet[buff] == true))
            {
                Player.buffImmune[ModContent.BuffType<NyctophobiaDebuff>()] = true; // Immune to fear of dark if any pet is active
            }

            if (Player.HasBuff(BuffID.SpiderMinion) || Player.HasBuff(BuffID.PetSpider)
                || Player.armor[0].type == ItemID.SpiderMask || Player.miscEquips[4].type == ItemID.WebSlinger)
            {
                Player.buffImmune[ModContent.BuffType<ArachnophobiaDebuff>()] = true; // Immune to fear of spiders if player has spider-related items
            }

            if (NPC.killCount[Item.NPCtoBanner(NPCID.BloodZombie)] > 50 || NPC.killCount[Item.NPCtoBanner(NPCID.Drippler)] > 50 || NPC.killCount[Item.NPCtoBanner(NPCID.FaceMonster)] > 50 || NPC.killCount[Item.NPCtoBanner(NPCID.Crimera)] > 50)
            {
                Player.buffImmune[ModContent.BuffType<HemophobiaDebuff>()] = true;
            }

            if (Player.noFallDmg || Player.equippedWings != null || Player.mount.Active)
            {
                Player.buffImmune[ModContent.BuffType<AcrophobiaDebuff>()] = true;
            }

            if (NPC.downedFishron)
            {
                Player.buffImmune[ModContent.BuffType<ThalassophobiaDebuff>()] = true;
            }


            if (Player.ZoneSkyHeight) Player.AddBuff(ModContent.BuffType<AcrophobiaDebuff>(), 2);
            if (Player.wet) Player.AddBuff(ModContent.BuffType<ThalassophobiaDebuff>(), 2);
            if (Player.ZoneCrimson || (Main.bloodMoon && Player.ZoneOverworldHeight)) Player.AddBuff(ModContent.BuffType<HemophobiaDebuff>(), 2);

            bool lightCheck = LemonUtils.CheckAllForLight(0.3f, Player.Center + new Vector2(32, 0), Player.Center - new Vector2(32, 0));
            if (Player.ZoneCorrupt || !lightCheck) Player.AddBuff(ModContent.BuffType<NyctophobiaDebuff>(), 2);
            if (Main.tile[(int)Player.Center.X / 16, (int)Player.Center.Y / 16].WallType == WallID.SpiderUnsafe)
            {
                Player.AddBuff(ModContent.BuffType<ArachnophobiaDebuff>(), 2);
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (Player.difficulty == 0 && !undeadAmulet) curseLevel++;
        }

        public override void PostUpdateMiscEffects()
        {
            for (int i = 0; i < curseLevel; i++)
            {
                if (Player.statLifeMax2 > 50)
                {
                    Player.statLifeMax2 -= 10;
                }
            }
        }

        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath)
        {
            itemsByMod["Terraria"].Clear();
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            return new Item[] {
                new Item(ItemID.Wood, 16, 1),
                new Item(ModContent.ItemType<LootToken>(), 16, 1)
            };
        }

        public override void Initialize()
        {
            curseLevel = 0;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["curseLevel"] = curseLevel;
        }

        public override void LoadData(TagCompound tag)
        {
            curseLevel = tag.GetInt("curseLevel");
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)TerrorMod.MessageType.CurseLevelSync);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)curseLevel);
            packet.Send(toWho, fromWho);
        }

        public void ReceivePlayerSync(BinaryReader reader)
        {
            curseLevel = reader.ReadByte();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            TerrorPlayer clone = (TerrorPlayer)targetCopy;
            clone.curseLevel = curseLevel;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            TerrorPlayer clone = (TerrorPlayer)clientPlayer;

            if (clone.curseLevel != curseLevel)
            {
                SyncPlayer(-1, Main.myPlayer, false);
            }
        }
    }
}