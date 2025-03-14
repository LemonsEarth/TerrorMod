using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Projectiles.Hostile;


namespace TerrorMod.Content.NPCs.Bosses
{
    [AutoloadBossHead]
    public class InfiniteTerrorHead : ModNPC
    {
        ref float AITimer => ref NPC.ai[0];
        ref float Attack => ref NPC.ai[1];
        ref float AttackTimer => ref NPC.ai[2];
        ref float AttackCount => ref NPC.ai[3];
        int phase { get; set; } = 1;
        Vector2 savedPosition = Vector2.Zero;
        Vector2 savedDirection = Vector2.Zero;

        bool NotHost => Main.netMode != NetmodeID.MultiplayerClient;

        int HeadProj => ModContent.ProjectileType<InfiniteTerrorHeadProj>();
        int LaserSkull => ModContent.ProjectileType<LaserSkull>();
        int DoomSphere => ModContent.ProjectileType<DoomSphere>();

        float attackDuration = 0;
        int[] attackDurations = { 480, 600, 900, 1200, 600 };
        int[] attackDurations2 = { 900, 900, 720, 720, 900 };
        public Player player { get; private set; }

        public enum Attacks
        {
            DashWithFollowers,
            LaserSkulls
        }

        public enum Attacks2
        {

        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPCID.Sets.TrailingMode[NPC.type] = 2;
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.CantTakeLunchMoney[Type] = true;
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() { };
            /*{
                PortraitScale = 0.2f,
                PortraitPositionYOverride = -150
            };*/
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(),
                //new FlavorTextBestiaryInfoElement(this.GetLocalizedValue("Bestiary")),
            });
        }

        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.aiStyle = -1;
            NPC.width = 97;
            NPC.height = 97;
            NPC.Opacity = 1;
            NPC.lifeMax = 800000;
            NPC.defense = 40;
            NPC.damage = 70;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.value = 1000000;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0;
            NPC.noGravity = true;
            NPC.npcSlots = 10;
            NPC.SpawnWithHigherTime(30);

            if (!Main.dedServ)
            {
                //Music = MusicLoader.GetMusicSlot(Mod, "Content/Audio/Music/AnotherSamePlace");
            }
        }

        public override void Load()
        {

        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * balance * bossAdjustment * 0.5f);
            NPC.damage = (int)(NPC.damage * balance * 1f);
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackDuration);
            writer.Write(phase);
            writer.Write(NPC.Opacity);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackDuration = reader.ReadSingle();
            phase = reader.ReadInt32();
            NPC.Opacity = reader.ReadSingle();
        }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(false);
            }
            player = Main.player[NPC.target];

            DespawnCheck();
            if (AITimer == 0)
            {
                DustEffect();
            }
            Visuals();

            if (AITimer < IntroDuration)
            {
                Intro();
                AITimer++;
                return;
            }
            NPC.dontTakeDamage = false;

            if (attackDuration <= 0)
            {
                SwitchAttacks();
            }

            if (phase == 1)
            {
                switch (Attack)
                {
                    case (int)Attacks.DashWithFollowers:
                        DashWithFollowers();
                        break;
                    case (int)Attacks.LaserSkulls:
                        LaserSkulls();
                        break;
                }
            }
            else
            {
                /*NPC.defense = 100;
                switch (Attack)
                {
                    case (int)Attacks2.VoidEruptionSpin:
                        VoidEruptionSpin();
                        break;
                }*/

            }
            attackDuration--;
            AITimer++;
        }

        float DashSpeed = 40;
        const float DashWithFollower_Duration = 120;
        const float DashWithFollower_ChargeTime = 90;
        const float DashWithFollower_DashTime = 90;
        void DashWithFollowers()
        {
            switch (AttackTimer)
            {
                case DashWithFollower_Duration:
                    DustEffect();
                    if (NotHost)
                    {
                        savedPosition = player.Center + Main.rand.NextVector2CircularEdge(500, 300);
                    }
                    NPC.netUpdate = true;
                    NPC.Center = savedPosition;
                    DustEffect();
                    savedDirection = savedPosition.DirectionTo(player.Center);
                    NPC.rotation = savedDirection.ToRotation() + MathHelper.PiOver2;
                    SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                    break;
                case > DashWithFollower_ChargeTime:
                    NPC.velocity = -savedDirection;
                    break;
                case DashWithFollower_DashTime:
                    if (NotHost && Main.rand.NextBool(4))
                    {
                        AttackTimer = DashWithFollower_Duration;
                    }
                    NPC.netUpdate = true;
                    if (AttackTimer == DashWithFollower_Duration) return;
                    NPC.velocity = savedDirection * DashSpeed;
                    if (NotHost)
                    {
                        for (int i = -8; i <= 8; i++)
                        {
                            Vector2 perpendicularDirection = savedDirection.RotatedBy(MathHelper.PiOver2);
                            Vector2 pos = NPC.Center + perpendicularDirection * i * 400;
                            NewProj(pos, Vector2.Zero, HeadProj, ai0: savedDirection.ToRotation(), ai1: 30, ai2: DashSpeed);
                        }
                    }
                    break;
                case 0:
                    AttackTimer = DashWithFollower_Duration;
                    return;
            }

            AttackTimer--;
        }

        const float LaserSkullsDuration = 600;
        float rot => NPC.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(60) - MathHelper.PiOver2;
        float rotPerSecond => MathHelper.ToRadians(1f);
        void LaserSkulls()
        {
            switch (AttackTimer)
            {
                case LaserSkullsDuration:
                    DustEffect();
                    if (NotHost)
                    {
                        savedPosition = player.Center + Vector2.UnitY.RotatedBy(Main.rand.Next(4) * MathHelper.PiOver2) * 600;
                    }
                    NPC.netUpdate = true;
                    NPC.Center = savedPosition;
                    DustEffect();
                    SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                    break;
                case > 240:
                    if (AttackTimer % 60 == 0)
                    {
                        if (NotHost)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Vector2 pos = player.Center + Main.rand.NextVector2CircularEdge(400, 400);
                                NewProj(pos, Vector2.Zero, LaserSkull, ai0: 90, ai1: pos.DirectionTo(player.Center).ToRotation());
                            }
                        }
                    }
                    NPC.velocity = NPC.DirectionTo(player.Center) * 6;
                    NPC.Opacity = (float)(Math.Sin(AITimer / 4) * 0.5f + 0.5f);
                    NPC.rotation = rot;
                    break;
                case > 120:
                    NPC.velocity = NPC.DirectionTo(player.Center) * 6;
                    NPC.Opacity = (float)(Math.Sin(AITimer / 4) * 0.5f + 0.5f);
                    NPC.rotation = rot;
                    break;
                case 120:
                    NPC.velocity = Vector2.Zero;  
                    if (NotHost)
                    {
                        NewProj(NPC.Center, Vector2.Zero, DoomSphere, ai0: 2f, ai1: rot, ai2: rotPerSecond);
                    }
                    break;
                case > 0:
                    NPC.velocity = Vector2.Zero;
                    NPC.rotation += rotPerSecond;
                    break;
                case 0:
                    AttackTimer = LaserSkullsDuration;
                    return;
            }
            AttackTimer--;
        }

        void DustEffect()
        {
            LemonUtils.DustCircle(NPC.Center, 32, 15, DustID.GemDiamond, 4f);
            LemonUtils.DustCircle(NPC.Center, 32, 10, DustID.GemDiamond, 4f);
            LemonUtils.DustCircle(NPC.Center, 32, 5, DustID.GemDiamond, 4f);
        }

        Projectile NewProj(Vector2 position, Vector2 velocity, int type, int damage = 0, int knockback = 0, float ai0 = 0, float ai1 = 0, float ai2 = 0)
        {
            if (damage == 0) damage = NPC.damage / 4;
            return Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), position, velocity, type, damage, knockback, -1, ai0, ai1, ai2);
        }

        void DespawnCheck()
        {
            if (player.dead || !player.active || NPC.Center.Distance(player.MountedCenter) > 8000)
            {
                NPC.active = false;
                NPC.life = 0;
                NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {

        }

        void Visuals()
        {
            Lighting.AddLight(NPC.Center, 10, 10, 10);
            if (!Terraria.Graphics.Effects.Filters.Scene["TerrorMod:DesaturateShader"].IsActive() && Main.netMode != NetmodeID.Server)
            {
                Terraria.Graphics.Effects.Filters.Scene.Activate("TerrorMod:DesaturateShader");
            }

            foreach (var allPlayer in Main.ActivePlayers)
            {
                allPlayer.moonLordMonolithShader = true;
            }
        }

        int IntroDuration = 60;
        void Intro()
        {
            NPC.dontTakeDamage = true;
            NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 2;
            Attack = 0;
            attackDuration = attackDurations[0];
        }

        void SwitchAttacks()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Attack++;
                if (Attack > 1) Attack = 0;
                if (phase == 1) attackDuration = attackDurations[(int)Attack];
                else attackDuration = attackDurations2[(int)Attack];

                AttackCount = 0;
                AttackTimer = 0;
                NPC.Opacity = 1f;
            }

            NPC.netUpdate = true;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            /*int frameDur = 6;
            NPC.frameCounter += 1;
            if (NPC.frameCounter > frameDur)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
                if (NPC.frame.Y > 2 * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }*/
        }

        public override bool CheckDead()
        {

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            for (int k = NPC.oldPos.Length - 1; k > 0; k--)
            {
                Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition;
                Color color = NPC.GetAlpha(drawColor * 0.5f) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos + drawOrigin / 2, null, color * NPC.Opacity, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, NPC.Center - Main.screenPosition, null, Color.White * NPC.Opacity, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            /*LeadingConditionRule classicRule = new LeadingConditionRule(new Conditions.NotExpert());
            classicRule.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<VoidTremor>(), ModContent.ItemType<DevourerRift>()));
            classicRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Judgement>(), 4, 1, 1));
            npcLoot.Add(classicRule);
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TheNamelessBossBag>()));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.Furniture.TheNamelessRelic>()));*/
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void OnKill()
        {
            //NPC.SetEventFlagCleared(ref DownedBossSystem.downedTheNameless, -1);
        }
    }
}