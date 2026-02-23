using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Effects;
using TerrorMod.Common.Utils;
using TerrorMod.Content.Buffs.Debuffs;
using TerrorMod.Content.Items.Accessories;
using TerrorMod.Content.NPCs.Bosses.BossAdds;
using TerrorMod.Content.NPCs.Bosses.Gores;
using TerrorMod.Content.Projectiles.Hostile;


namespace TerrorMod.Content.NPCs.Bosses;

[AutoloadBossHead]
public class InfiniteTerrorHead : ModNPC
{
    ref float AITimer => ref NPC.ai[0];
    ref float Attack => ref NPC.ai[1];
    ref float AttackTimer => ref NPC.ai[2];
    ref float AttackCount => ref NPC.ai[3];
    Vector2 savedPosition = Vector2.Zero;
    Vector2 savedDirection = Vector2.Zero;

    bool canDie = false;
    bool doDeath = false;
    int deathTimer = 0;

    bool NotClient => Main.netMode != NetmodeID.MultiplayerClient;

    int HeadProj => ProjectileType<InfiniteTerrorHeadProj>();
    int LaserSkull => ProjectileType<LaserSkull>();
    int DoomSphere => ProjectileType<DoomSphere>();
    int Chain => ProjectileType<ChainProj>();
    int LightBomb => ProjectileType<LightBomb>();
    int HungryCannon => ProjectileType<HungryCannon>();

    float attackDuration = 0;
    int[] attackDurations = { 480, 600, 600, 720, 600, 780, 660, 600, 900 };
    public Player player { get; private set; }

    public enum Attacks
    {
        DashWithFollowers,
        LaserSkulls,
        ChainSpam,
        LightningLances,
        LaserFlinging,
        HungryMaze,
        TearsOfFire,
        PhantasmalBarrage,
        LaserSpiral
    }

    public enum Attacks2
    {

    }

    bool hard => Main.masterMode || NPC.life < NPC.lifeMax / 2;

    public override void SetStaticDefaults()
    {
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
        NPCID.Sets.MPAllowedEnemies[Type] = true;
        NPCID.Sets.TrailCacheLength[NPC.type] = 10;
        NPCID.Sets.TrailingMode[NPC.type] = 3;
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
        NPC.lifeMax = 400000;
        NPC.defense = 40;
        NPC.damage = 100;
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
            Music = MusicLoader.GetMusicSlot(Mod, "Common/Assets/Audio/Music/ChampionVIP");
        }
    }

    public override void Load()
    {

    }

    public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
    {
        NPC.lifeMax = (int)(NPC.lifeMax * balance * bossAdjustment * 0.6f);
        NPC.damage = 100;
        if (Main.expertMode) NPC.damage = 130;
        if (Main.masterMode) NPC.damage = 170;
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(attackDuration);
        writer.Write(NPC.Opacity);
        writer.Write(savedPosition.X);
        writer.Write(savedPosition.Y);
        writer.Write(savedDirection.X);
        writer.Write(savedDirection.Y);
        writer.Write(canDie);
        writer.Write(doDeath);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        attackDuration = reader.ReadSingle();
        NPC.Opacity = reader.ReadSingle();
        savedPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());
        savedDirection = new Vector2(reader.ReadSingle(), reader.ReadSingle());
        canDie = reader.ReadBoolean();
        doDeath = reader.ReadBoolean();
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
            SoundEngine.PlaySound(SoundID.Roar with { PitchRange = (-0.7f, -0.4f) }, player.Center);
            SoundEngine.PlaySound(SoundID.Roar with { PitchRange = (-0.7f, -0.4f) }, player.Center);
            SoundEngine.PlaySound(SoundID.NPCDeath10 with { PitchRange = (-0.7f, -0.4f) }, player.Center);
        }
        Visuals();

        if (AITimer < IntroDuration)
        {
            Intro();
            AITimer++;
            return;
        }
        if (!doDeath)
        {
            NPC.dontTakeDamage = false;
        }

        if (!doDeath)
        {
            if (attackDuration <= 0)
            {
                SwitchAttacks();
            }

            switch (Attack)
            {
                case (int)Attacks.DashWithFollowers:
                    DashWithFollowers();
                    break;
                case (int)Attacks.LaserSkulls:
                    LaserSkulls();
                    break;
                case (int)Attacks.ChainSpam:
                    ChainSpam();
                    break;
                case (int)Attacks.LightningLances:
                    LightningLances();
                    break;
                case (int)Attacks.LaserFlinging:
                    LaserFlinging();
                    break;
                case (int)Attacks.HungryMaze:
                    HungryMaze();
                    break;
                case (int)Attacks.TearsOfFire:
                    TearsOfFire();
                    break;
                case (int)Attacks.PhantasmalBarrage:
                    PhantasmalBarrage();
                    break;
                case (int)Attacks.LaserSpiral:
                    LaserSpiral();
                    break;
            }
        }
        else
        {
            FinalPhase();
        }

        if (AITimer % 10 == 0)
        {
            foreach (var ply in Main.ActivePlayers)
            {
                if (ply.Distance(NPC.Center) > 4000)
                {
                    ply.AddBuff(BuffID.CursedInferno, 60);
                    ply.AddBuff(BuffID.ShadowFlame, 60);
                    ply.AddBuff(BuffID.OnFire, 60);
                    ply.AddBuff(BuffID.Ichor, 60);
                    ply.AddBuff(BuffID.WitheredArmor, 60);
                    ply.AddBuff(BuffID.WitheredWeapon, 60);
                    ply.AddBuff(BuffID.Frostburn, 60);
                    ply.AddBuff(BuffID.Venom, 60);
                    ply.AddBuff(BuffID.Poisoned, 60);
                    ply.AddBuff(BuffID.Burning, 60);
                    ply.AddBuff(BuffID.Obstructed, 180);
                }
            }
        }

        attackDuration--;
        AITimer++;
    }

    float finalArenaDistance = 800;
    void FinalPhase()
    {
        switch (deathTimer)
        {
            case 0:
                SwitchAttacks();
                DeleteProjectiles();
                DustEffect();
                NPC.Center = player.Center - Vector2.UnitY * 600;
                DustEffect();
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                NPC.rotation = 0;
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Roar with { PitchRange = (-0.7f, -0.4f) });
                SoundEngine.PlaySound(SoundID.Roar with { PitchRange = (-0.7f, -0.4f) });
                SoundEngine.PlaySound(SoundID.NPCDeath10 with { PitchRange = (-0.7f, -0.4f) });
                PunchCameraModifier mod1 = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 15f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(mod1);
                break;
            case < 90:
                PunchCameraModifier mod2 = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 15f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(mod2);
                if (deathTimer % 10 == 0)
                {
                    LemonUtils.DustCircle(new Vector2(Main.rand.NextFloat(NPC.position.X, NPC.position.X + NPC.width), Main.rand.NextFloat(NPC.position.Y, NPC.position.Y + NPC.height)), 16, 8, DustID.GemDiamond, Main.rand.NextFloat(1.2f, 2.5f));
                    SoundEngine.PlaySound(SoundID.NPCHit2);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), new Vector2(Main.rand.NextFloat(NPC.position.X, NPC.position.X + NPC.width), Main.rand.NextFloat(NPC.position.Y, NPC.position.Y + NPC.height)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 10, GoreType<CageGore>());
                    }
                }
                break;
            case < 360:
                if (deathTimer < 240)
                    if (deathTimer % 10 == 0)
                    {
                        if (NotClient)
                        {
                            Vector2 pos = NPC.Center - Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 800;
                            NewProj(pos, Vector2.Zero, LaserSkull, ai0: (240 - deathTimer) + 180, ai1: pos.DirectionTo(NPC.Center).ToRotation());
                        }
                    }
                break;
            case 480:
                SoundEngine.PlaySound(SoundID.Roar with { PitchRange = (-0.7f, -0.4f) });
                SoundEngine.PlaySound(SoundID.Roar with { PitchRange = (-0.7f, -0.4f) });
                SoundEngine.PlaySound(SoundID.NPCDeath10 with { PitchRange = (-0.7f, -0.4f) });
                break;
            case < 1020 and > 480:
                if (deathTimer % 120 == 0)
                {
                    if (NotClient)
                    {
                        for (int i = -4; i <= 4; i++)
                        {
                            float speed = Main.rand.NextFloat(4, 6);
                            float oscSpeed = hard ? 14 : 11;
                            for (int j = 1; j <= 2; j++)
                            {
                                Vector2 pos = NPC.Center + new Vector2(800 * i, -600 - j * 200).RotatedBy(MathHelper.PiOver2 * AttackCount);
                                NewProj(pos, Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * AttackCount) * speed, Chain, ai0: oscSpeed, ai1: oscSpeed);
                                NewProj(pos, Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * AttackCount) * speed, Chain, ai0: oscSpeed, ai1: -oscSpeed);
                            }
                        }
                    }
                    AttackCount++;
                }
                break;
            case < 1060:
                AttackCount = 0;
                break;
            case 1060:
                DeleteProjectiles();
                if (NotClient)
                {
                    NewProj(NPC.Center, Vector2.Zero, ProjectileType<DoomSphereLong>(), ai0: 2, ai1: NPC.rotation, ai2: MathHelper.ToRadians(1f));
                }
                break;
            case < 1860:
                if (deathTimer % 100 == 0)
                {
                    if (NotClient)
                    {
                        int randProj = Main.rand.NextFromList(ProjectileID.InsanityShadowHostile, ProjectileID.DeathLaser, ProjectileID.CursedFlameHostile, ProjectileID.EyeBeam, ProjectileID.PhantasmalBolt);
                        for (int i = 0; i < 16; i++)
                        {
                            float speed = 6;
                            if (randProj == ProjectileID.PhantasmalBolt) speed = 3;
                            if (randProj == ProjectileID.InsanityShadowHostile) speed = 8;
                            NewProj(NPC.Center, Vector2.UnitY.RotatedBy((MathHelper.PiOver4 / 2) * i) * speed, randProj);
                        }
                    }
                    AttackCount++;
                }
                NPC.rotation += rotPerSecond;
                break;
            case 1860:
                if (NotClient)
                {
                    NewProj(NPC.Center, Vector2.Zero, ProjectileType<DoomSphereLong>(), ai0: 2, ai1: NPC.rotation, ai2: MathHelper.ToRadians(1.8f));
                }
                break;
            case < 2660:
                if (deathTimer % 100 == 0)
                {
                    if (NotClient)
                    {
                        int randProj = Main.rand.NextFromList(ProjectileID.InsanityShadowHostile, ProjectileID.DeathLaser, ProjectileID.CursedFlameHostile, ProjectileID.EyeBeam, ProjectileID.PhantasmalBolt);
                        for (int i = 0; i < 16; i++)
                        {
                            float speed = 8;
                            if (randProj == ProjectileID.PhantasmalBolt) speed = 4;
                            if (randProj == ProjectileID.InsanityShadowHostile) speed = 10;
                            NewProj(NPC.Center, Vector2.UnitY.RotatedBy((MathHelper.PiOver4 / 2) * i) * speed, randProj);
                        }
                    }
                    AttackCount++;
                }
                NPC.rotation += rotPerSecond;
                break;
            case 2660:
                SoundEngine.PlaySound(SoundID.Roar with { PitchRange = (-0.7f, -0.4f) });
                SoundEngine.PlaySound(SoundID.Roar with { PitchRange = (-0.7f, -0.4f) });
                SoundEngine.PlaySound(SoundID.NPCDeath10 with { PitchRange = (-0.7f, -0.4f) });
                if (NotClient)
                {
                    NewProj(NPC.Center, Vector2.Zero, DoomSphere, ai0: 2, ai1: NPC.rotation, ai2: MathHelper.ToRadians(2.5f));
                }
                break;
            case < 2760:
                NPC.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 2;
                NPC.rotation = MathHelper.ToRadians(deathTimer * 3);
                if (deathTimer % 10 == 0)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Gore.NewGore(NPC.GetSource_Death(), new Vector2(Main.rand.NextFloat(NPC.position.X, NPC.position.X + NPC.width), Main.rand.NextFloat(NPC.position.Y, NPC.position.Y + NPC.height)), Vector2.UnitY.RotatedByRandom(MathHelper.Pi * 2) * 10, GoreType<CageGore>());
                        }
                    }
                }
                PunchCameraModifier mod3 = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 15f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(mod3);
                break;
            case > 2760:
                canDie = true;
                NPC.SimpleStrikeNPC(9999999, 1);
                break;
        }

        foreach (var ply in Main.ActivePlayers)
        {
            if (ply.Distance(NPC.Center) > finalArenaDistance)
            {
                ply.velocity += ply.DirectionTo(NPC.Center) * 2f;
            }
        }
        for (int i = 0; i < 16; i++)
        {
            Vector2 pos = NPC.Center + Vector2.UnitY.RotatedBy(((2 * MathHelper.Pi) / 16) + MathHelper.ToRadians(deathTimer) * i) * finalArenaDistance;
            Vector2 directionToCenter = pos.DirectionTo(NPC.Center);
            Dust.NewDustDirect(pos, 2, 2, DustID.GemDiamond, directionToCenter.X * 5, directionToCenter.Y * 5, Scale: Main.rand.NextFloat(0.8f, 2f)).noGravity = true;
        }
        deathTimer++;
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
                if (NotClient)
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
                if (NotClient && Main.rand.NextBool(4))
                {
                    AttackTimer = DashWithFollower_Duration;
                }
                NPC.netUpdate = true;
                if (AttackTimer == DashWithFollower_Duration) return;
                NPC.velocity = savedDirection * DashSpeed;
                if (NotClient)
                {
                    for (int i = -8; i <= 8; i++)
                    {
                        Vector2 perpendicularDirection = savedDirection.RotatedBy(MathHelper.PiOver2);
                        Vector2 pos = NPC.Center + perpendicularDirection * i * 400;
                        NewProj(pos, Vector2.Zero, HeadProj, ai0: savedDirection.ToRotation(), ai1: 30, ai2: DashSpeed);
                    }

                    if (hard)
                    {
                        for (int i = -8; i <= 8; i++)
                        {
                            Vector2 perpendicularDirection = savedDirection;
                            Vector2 pos = player.Center + player.DirectionTo(savedPosition).RotatedBy(MathHelper.PiOver2) * 500 + perpendicularDirection * i * 400;
                            NewProj(pos, Vector2.Zero, HeadProj, ai0: savedDirection.RotatedBy(MathHelper.PiOver2).ToRotation(), ai1: 60, ai2: DashSpeed * 1.4f);
                        }
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
    const float SkullSpamTime = 240;
    const float BigLaserTime = 120;
    void LaserSkulls()
    {
        switch (AttackTimer)
        {
            case LaserSkullsDuration:
                DustEffect();
                if (NotClient)
                {
                    savedPosition = player.Center + Vector2.UnitY.RotatedBy(Main.rand.Next(4) * MathHelper.PiOver2) * 600;
                }
                NPC.netUpdate = true;
                NPC.Center = savedPosition;
                DustEffect();
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                break;
            case > SkullSpamTime:
                if (AttackTimer % 60 == 0)
                {
                    if (NotClient)
                    {
                        int amount = 3;
                        if (NPC.life < NPC.lifeMax / 2) amount++;
                        if (Main.masterMode) amount++;
                        for (int i = 0; i < amount; i++)
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
            case > BigLaserTime:
                NPC.velocity = NPC.DirectionTo(player.Center) * 6;
                NPC.Opacity = (float)(Math.Sin(AITimer / 4) * 0.5f + 0.5f);
                NPC.rotation = rot;
                break;
            case BigLaserTime:
                NPC.velocity = Vector2.Zero;
                if (NotClient)
                {
                    NewProj(NPC.Center, Vector2.Zero, DoomSphere, ai0: 2f, ai1: rot, ai2: rotPerSecond);
                }
                break;
            case > 0:
                NPC.velocity = Vector2.Zero;
                NPC.rotation += rotPerSecond;
                for (int i = 0; i < 16; i++)
                {
                    Vector2 pos = NPC.Center + Vector2.UnitY.RotatedBy(((2 * MathHelper.Pi) / 16 + MathHelper.ToRadians(AttackTimer)) * i) * 900;
                    Vector2 directionToCenter = pos.DirectionTo(NPC.Center);
                    Dust.NewDustDirect(pos, 2, 2, DustID.GemDiamond, directionToCenter.X * 5, directionToCenter.Y * 5, Scale: Main.rand.NextFloat(0.8f, 2f)).noGravity = true;
                }
                if (hard)
                {
                    if (AttackTimer == BigLaserTime / 2)
                    {
                        NewProj(NPC.Center, Vector2.Zero, DoomSphere, ai0: 2f, ai1: NPC.rotation, ai2: rotPerSecond * 1.3f);
                    }
                }
                if (AttackTimer % 10 == 0)
                {
                    foreach (var ply in Main.ActivePlayers)
                    {
                        if (NPC.Center.Distance(ply.Center) > 900)
                        {
                            ply.AddBuff(BuffType<UltimateTerror>(), 12);
                        }
                    }
                }
                if (AttackTimer % 30 == 0)
                {
                    for (int i = -6; i <= 6; i++)
                    {
                        if (NotClient)
                        {
                            NewProj(NPC.Center, Vector2.UnitX.RotatedBy(NPC.rotation + i * MathHelper.ToRadians(30)) * 8, ProjectileID.LostSoulHostile);
                        }
                    }
                }
                break;
            case 0:
                AttackTimer = LaserSkullsDuration;
                return;
        }
        AttackTimer--;
    }

    const float ChainSpamDuration = 600;
    const float ChainFireFromNPCTime = 480;
    const float ChainFireFromSkyTime = 240;
    const float ServantSpawnTime = 60;
    void ChainSpam()
    {
        switch (AttackTimer)
        {
            case ChainSpamDuration:
                NPC.Center = player.Center - Vector2.UnitY * 300;
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                DustEffect();
                NPC.rotation = 0;
                AttackCount = 1;
                break;
            case > ChainFireFromNPCTime:
                int cd = hard ? 10 : 20;
                if (AttackTimer % cd == 0)
                {
                    if (NotClient)
                    {
                        NewProj(NPC.Center, NPC.DirectionTo(player.Center) * Main.rand.NextFloat(AttackCount, AttackCount + 6), Chain, ai0: 6, ai1: 6);
                    }
                    AttackCount += 2;
                }
                if (AttackTimer % 10 == 0)
                {
                    // Uppies
                    if (NotClient)
                    {
                        NewProj(NPC.Center, -Vector2.UnitY.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-15, 15))) * Main.rand.NextFloat(24, 36), Chain, ai0: 6, ai1: 6);
                    }
                }
                NPC.velocity = Vector2.UnitY.RotatedBy(MathHelper.ToRadians(AttackTimer * 4)) * 2;
                NPC.rotation = 0;
                break;
            case > ChainFireFromSkyTime:
                if (AttackTimer % 60 == 0)
                {
                    if (NotClient)
                    {
                        for (int i = -4; i <= 4; i++)
                        {
                            Vector2 pos = player.Center + new Vector2(600 * i, -800);
                            float speed = Main.rand.NextFloat(3, 7);
                            float oscSpeed = hard ? 12 : 10;
                            NewProj(pos, Vector2.UnitY * speed, Chain, ai0: oscSpeed, ai1: oscSpeed);
                            NewProj(pos, Vector2.UnitY * speed, Chain, ai0: oscSpeed, ai1: -oscSpeed);
                        }
                    }
                }
                NPC.velocity = Vector2.UnitY.RotatedBy(MathHelper.ToRadians(AttackTimer * 8)) * 2;
                NPC.rotation = 0;
                break;
            case > ServantSpawnTime:
                if (AttackTimer % 15 == 0)
                {
                    if (NotClient)
                    {
                        Vector2 pos = NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height));
                        NewNPC(pos, NPCType<GiantServant>(), ai3: 0.1f);
                    }
                    NPC.velocity += -NPC.Center.DirectionTo(player.Center) * 4;
                    SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);

                }
                NPC.velocity /= 1.05f;
                NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2;
                break;
            case 0:
                AttackTimer = ChainSpamDuration;
                return;
        }
        AttackTimer--;
    }

    const float LightningLancesDuration = 720;
    const float LightningIndicatorTime = 660;
    const float LightningTime = 600;
    const float RainbowArenaTime = 540;
    const float LanceTime = 360;
    void LightningLances()
    {
        switch (AttackTimer)
        {
            case LightningLancesDuration:
                DustEffect();
                if (NotClient)
                {
                    savedPosition = player.Center + Vector2.UnitY.RotatedBy(Main.rand.Next(4) * MathHelper.PiOver2) * 600;
                }
                NPC.netUpdate = true;
                NPC.Center = savedPosition;
                DustEffect();
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                NPC.velocity = NPC.DirectionTo(player.Center) * 3;
                NPC.rotation = 0;
                break;
            case LightningIndicatorTime:
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item29, NPC.Center);
                savedPosition = player.Center;
                for (int i = -12; i <= 12; i++)
                {
                    Vector2 pos = player.Center + new Vector2(i * 300, 0);
                    LemonUtils.DustCircle(pos, 16, 2f, DustID.GemDiamond, 2f);
                }
                for (int i = 0; i < 8; i++)
                {
                    Vector2 pos = NPC.Center + Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * i) * 200;
                    LemonUtils.DustCircle(pos, 16, 2f, DustID.GemDiamond, 2f);
                }
                break;
            case LightningTime:
                SoundEngine.PlaySound(TerrorMod.Thunder);
                SoundEngine.PlaySound(SoundID.Thunder);
                SoundEngine.PlaySound(SoundID.Thunder);
                if (NotClient)
                {
                    for (int i = -12; i <= 12; i++)
                    {
                        Vector2 pos = savedPosition + new Vector2(i * 300, -1000);
                        NewProj(pos, Vector2.UnitY * 15, ProjectileID.CultistBossLightningOrbArc, ai0: MathHelper.PiOver2);
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 dirPos = NPC.Center + Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * i) * 200;
                        NewProj(NPC.Center, NPC.Center.DirectionTo(dirPos) * 10, ProjectileID.CultistBossLightningOrbArc, ai0: NPC.Center.DirectionTo(dirPos).ToRotation());
                    }
                }
                break;
            case RainbowArenaTime:
                for (int j = 1; j < 4; j++)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (NotClient)
                        {
                            savedPosition = player.Center;
                            Vector2 pos = player.Center + Vector2.UnitY.RotatedBy(i * MathHelper.PiOver4 + j * (MathHelper.PiOver4 / 2)) * 300 * j;
                            NewProj(pos, new Vector2(10, 10) * j, ProjectileID.HallowBossLastingRainbow, ai0: 6.6f);
                        }
                        NPC.netUpdate = true;
                    }
                }
                break;
            case > LanceTime and < 480:
                NPC.MoveToPos(savedPosition, 0.05f, 0.1f, 0.02f, 0.01f);
                foreach (var ply in Main.ActivePlayers)
                {
                    if (ply.Distance(savedPosition) > 400)
                    {
                        ply.velocity += ply.DirectionTo(savedPosition) * 2f;
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    Vector2 pos = savedPosition + Vector2.UnitY.RotatedBy(((2 * MathHelper.Pi) / 16) + MathHelper.ToRadians(AttackTimer) * i) * 400;
                    Vector2 directionToCenter = pos.DirectionTo(savedPosition);
                    Dust.NewDustDirect(pos, 2, 2, DustID.GemDiamond, directionToCenter.X * 5, directionToCenter.Y * 5, Scale: Main.rand.NextFloat(0.8f, 2f)).noGravity = true;
                }
                if (AttackTimer % 30 == 0)
                {
                    for (int i = -3; i <= 3; i++)
                    {
                        if (NotClient)
                        {
                            int dir = Main.rand.NextBool().ToDirectionInt();
                            int gap = hard ? 100 : 150;
                            Vector2 pos = player.Center + new Vector2(800 * dir, i * gap);
                            NewProj(pos, Vector2.Zero, ProjectileID.FairyQueenLance, ai0: dir == 1 ? MathHelper.Pi : 0);
                        }
                    }
                }
                break;
            case > 0 and < 300:
                NPC.MoveToPos(savedPosition, 0.05f, 0.1f, 0.02f, 0.01f);
                if (AttackTimer % 20 == 0 && AttackTimer < 270)
                {
                    if (NotClient)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 pos = savedPosition + Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * i) * 900;
                            NewProj(pos, Vector2.Zero, LightBomb, ai0: player.Center.X, ai1: player.Center.Y, ai2: 60);
                        }
                    }
                }
                LemonUtils.DustCircle(NPC.Center, 8, 8f, DustID.GemDiamond, 1.25f);
                break;
            case 0:
                AttackTimer = LightningLancesDuration;
                return;
        }
        AttackTimer--;
    }

    const float LaserFlingingDuration = 600;
    const float LaserFlingingSpinTime = 420;
    const float LaserFlingingTime = 60;
    void LaserFlinging()
    {
        switch (AttackTimer)
        {
            case LaserFlingingDuration:
                NPC.velocity = Vector2.Zero;
                DustEffect();
                if (NotClient)
                {
                    savedPosition = player.Center + Vector2.UnitY.RotatedBy(Main.rand.Next(4) * MathHelper.PiOver2) * 600;
                }
                NPC.netUpdate = true;
                NPC.Center = savedPosition;
                DustEffect();
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);

                if (NotClient)
                {
                    for (int i = -8; i <= 8; i++)
                    {
                        Vector2 pos = player.Center + new Vector2(i * 200, -400);
                        NewProj(pos, Vector2.Zero, LaserSkull, ai0: 60, ai1: MathHelper.PiOver2);
                    }
                }
                break;
            case > LaserFlingingSpinTime:
                if (AttackCount < 10) AttackCount += 0.1f;
                NPC.rotation += MathHelper.ToRadians(AttackCount);
                break;
            case > LaserFlingingTime:
                NPC.rotation += MathHelper.ToRadians(AttackCount);
                NPC.MoveToPos(player.Center, 0.04f, 0.04f, 0.5f, 0.5f);
                if (AttackTimer % 60 == 0)
                {
                    if (NotClient)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 dir = Vector2.UnitY.RotatedBy(i * MathHelper.PiOver4);
                            int speed = 3;
                            if (hard) speed = 6;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, dir * speed, ProjectileID.CultistBossFireBallClone, NPC.damage / 4, 1f);
                        }
                    }
                }
                break;
            case > 0:
                NPC.rotation += MathHelper.ToRadians(AttackCount);
                if (AttackCount > 0) AttackCount -= 0.1f;
                NPC.velocity /= 1.05f;
                break;
            case 0:
                AttackTimer = LaserFlingingDuration;
                return;
        }

        AttackTimer--;
    }

    const float HungryMazeDuration = 780;
    const float HungryMazeSpawnTime = 720;
    const float LaserTime = 240;
    const float LaserTime2 = 120;
    void HungryMaze()
    {
        switch (AttackTimer)
        {
            case HungryMazeDuration:
                DeleteProjectiles();
                AttackCount = 1;
                NPC.velocity = Vector2.Zero;
                DustEffect();
                if (NotClient)
                {
                    savedPosition = player.Center + Vector2.UnitX * 2000;
                }
                NPC.netUpdate = true;
                NPC.Center = savedPosition;
                DustEffect();
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() - MathHelper.PiOver2;
                foreach (var ply in Main.ActivePlayers)
                {
                    ply.Teleport(player.Center);
                }
                SoundEngine.PlaySound(SoundID.Item37);
                break;
            case > HungryMazeSpawnTime:
                if (AttackTimer % 10 == 0)
                {
                    if (NotClient)
                    {
                        for (int i = 0; i < 4 * AttackCount; i++)
                        {
                            float angle = MathHelper.Pi / ((4 * AttackCount) / 2);
                            Vector2 pos = player.Center + Vector2.UnitY.RotatedBy(angle * i + MathHelper.PiOver4) * 200 * AttackCount;
                            float rotation = Main.rand.Next(4) * MathHelper.PiOver2;
                            NewProj(pos, Vector2.Zero, HungryCannon, ai1: rotation);
                        }
                    }
                    AttackCount++;
                }
                foreach (var ply in Main.ActivePlayers)
                {
                    ply.velocity = Vector2.Zero;
                }
                break;
            case > LaserTime:
                int cd = hard ? 30 : 60;
                if (AttackTimer % cd == 0)
                {
                    if (NotClient)
                    {
                        Vector2 pos = player.Center + Vector2.UnitY.RotatedBy(Main.rand.Next(4) * MathHelper.PiOver2) * 400;
                        NewProj(pos, Vector2.Zero, LaserSkull, ai0: 90, ai1: Main.rand.Next(4) * MathHelper.PiOver2);
                    }
                    SoundEngine.PlaySound(SoundID.Roar);
                }
                break;
            case LaserTime:
                if (NotClient)
                {
                    NewProj(NPC.Center, Vector2.Zero, DoomSphere, ai0: 6f, ai1: MathHelper.PiOver2);
                }
                break;
            case LaserTime2:
                if (NotClient)
                {
                    NewProj(NPC.Center, Vector2.Zero, DoomSphere, ai0: 10f, ai1: MathHelper.PiOver2);
                }
                break;
            case 0:
                AttackTimer = HungryMazeDuration;
                return;
        }
        AttackTimer--;
    }

    const float TearsOfFireDuration = 660;
    void TearsOfFire()
    {
        switch (AttackTimer)
        {
            case TearsOfFireDuration:
                DustEffect();
                if (NotClient)
                {
                    savedPosition = player.Center + Vector2.UnitY.RotatedBy(Main.rand.Next(4) * MathHelper.PiOver2) * 500;
                }
                NPC.netUpdate = true;
                NPC.Center = savedPosition;
                DustEffect();
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                NPC.rotation = 0;
                NPC.velocity = Vector2.Zero;
                for (int i = 0; i < 32; i++)
                {
                    Vector2 pos = NPC.Center + Vector2.UnitY.RotatedBy(((2 * MathHelper.Pi) / 32) * i) * 700;
                    Vector2 directionToCenter = pos.DirectionTo(NPC.Center);
                    if (NotClient)
                    {
                        Projectile proj = NewProj(pos, Vector2.Zero, ProjectileID.PhantasmalSphere, ai0: 30);
                        proj.timeLeft = 600;
                        NetMessage.SendData(MessageID.SyncProjectile, number: proj.whoAmI);
                    }
                }
                break;
            case > 60:
                if (AttackTimer % 10 == 0)
                {
                    foreach (var ply in Main.ActivePlayers)
                    {
                        if (NPC.Center.Distance(ply.Center) > 700)
                        {
                            ply.AddBuff(BuffType<UltimateTerror>(), 12);
                        }
                    }
                }
                if (AttackTimer < 600)
                {
                    if (AttackTimer % 25 == 0)
                    {
                        if (NotClient)
                        {
                            Vector2 eyeOffset = AttackCount % 2 == 0 ? new Vector2(30, 0) : new Vector2(-30, 0);
                            NewProj(NPC.Center + eyeOffset, Vector2.UnitY.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-15, 15))) * 5, ProjectileID.Fireball);

                        }
                        SoundEngine.PlaySound(SoundID.NPCDeath10 with { PitchRange = (0.3f, 0.6f) }, NPC.Center);
                    }
                    int cd = hard ? 25 : 30;
                    if (AttackTimer % cd == 0)
                    {
                        if (NotClient)
                        {
                            Vector2 eyeOffset = AttackCount % 2 == 0 ? new Vector2(30, 0) : new Vector2(-30, 0);
                            Vector2 pos = NPC.Center + eyeOffset;
                            NewProj(pos, pos.DirectionTo(player.Center) * 10, ProjectileID.InfernoHostileBolt, ai0: player.Center.X, ai1: player.Center.Y);
                        }
                        AttackCount++;
                    }
                }
                break;
            case 0:
                AttackTimer = TearsOfFireDuration;
                return;
        }
        AttackTimer--;
    }

    const float PhantasmalBarrageCooldown = 600;
    void PhantasmalBarrage()
    {
        switch (AttackTimer)
        {
            case PhantasmalBarrageCooldown:
                DeleteProjectiles();
                DustEffect();
                if (NotClient)
                {
                    savedPosition = player.Center - Vector2.UnitY * 600;
                }
                NPC.netUpdate = true;
                NPC.Center = savedPosition;
                DustEffect();
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                NPC.rotation = 0;
                NPC.velocity = Vector2.Zero;

                for (int i = 0; i < 16; i++)
                {
                    if (NotClient)
                    {
                        savedPosition = player.Center;
                        Vector2 pos = player.Center + Vector2.UnitY.RotatedBy(i * (MathHelper.PiOver4 / 2)) * 300;
                        NewProj(pos, Vector2.Zero, ProjectileID.PhantasmalSphere, ai0: 30f);
                    }
                    NPC.netUpdate = true;
                }
                break;
            case > 0:
                NPC.Center = savedPosition - Vector2.UnitY.RotatedBy(MathHelper.ToRadians(AttackCount)) * 600;
                AttackCount += 3;
                foreach (var ply in Main.ActivePlayers)
                {
                    if (ply.Distance(savedPosition) > 300)
                    {
                        ply.velocity += ply.DirectionTo(savedPosition) * 2f;
                    }
                }

                if (AttackTimer % 30 == 0)
                {
                    if (NotClient)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            NewProj(NPC.Center, Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * i) * 4, ProjectileID.PhantasmalBolt);
                        }
                        if (hard)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                NewProj(NPC.Center, Vector2.UnitY.RotatedBy(MathHelper.PiOver4 * i) * 2f, ProjectileID.PhantasmalBolt);
                            }
                        }
                        Vector2 pos = NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height));
                        NewNPC(pos, NPCType<GiantServant>(), ai3: -0.2f);
                    }
                    LemonUtils.DustCircle(NPC.Center, 8, 10, DustID.GemDiamond, 2);
                }
                break;
            case 0:
                AttackTimer = PhantasmalBarrageCooldown;
                return;
        }
        NPC.rotation += MathHelper.ToRadians(4);
        AttackTimer--;
    }

    const float LaserSpiralTime = 900;
    void LaserSpiral()
    {
        switch (AttackTimer)
        {
            case LaserSpiralTime:
                DeleteProjectiles();
                DustEffect();
                NPC.Center = player.Center - Vector2.UnitY * 700;
                DustEffect();
                SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
                NPC.rotation = 0;
                NPC.velocity = Vector2.Zero;
                savedPosition = player.Center;
                break;
            case > 160:
                foreach (var ply in Main.ActivePlayers)
                {
                    if (ply.Distance(savedPosition) > 600)
                    {
                        ply.velocity += ply.DirectionTo(savedPosition) * 2f;
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    Vector2 pos = savedPosition + Vector2.UnitY.RotatedBy(((2 * MathHelper.Pi) / 16) + MathHelper.ToRadians(AttackTimer) * i) * 600;
                    Vector2 directionToCenter = pos.DirectionTo(savedPosition);
                    Dust.NewDustDirect(pos, 2, 2, DustID.GemDiamond, directionToCenter.X * 5, directionToCenter.Y * 5, Scale: Main.rand.NextFloat(0.8f, 2f)).noGravity = true;
                }

                if (AttackTimer % 25 == 0)
                {
                    if (NotClient)
                    {
                        Vector2 pos = savedPosition - Vector2.UnitY.RotatedBy(MathHelper.ToRadians(AttackCount)) * 600;
                        NewProj(pos, Vector2.Zero, LaserSkull, ai0: 60, ai1: pos.DirectionTo(savedPosition).ToRotation());
                    }
                    AttackCount += 31;
                }
                break;
            case 0:
                AttackTimer = LaserSpiralTime;
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
        if (!Main.expertMode && !Main.masterMode) damage = NPC.damage / 2;
        else if (Main.expertMode && !Main.masterMode) damage = NPC.damage / 4;
        else if (Main.masterMode) damage = NPC.damage / 6;
        return Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), position, velocity, type, damage, knockback, -1, ai0, ai1, ai2);
    }

    NPC NewNPC(Vector2 position, int type, float ai0 = 0, float ai1 = 1, float ai2 = 0, float ai3 = 0)
    {
        return NPC.NewNPCDirect(NPC.GetSource_FromAI(), (int)position.X, (int)position.Y, type, 0, ai0, ai1, ai2, ai3);
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
        if (Main.expertMode) modifiers.FinalDamage *= 0.8f;
    }

    public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        if (projectile.type == ProjectileID.FinalFractal)
        {
            modifiers.FinalDamage *= 0.4f;
        }
    }

    void Visuals()
    {
        Lighting.AddLight(NPC.Center, 10, 10, 10);
        if (!Terraria.Graphics.Effects.Filters.Scene["TerrorMod:DesaturateShader"].IsActive() && Main.netMode != NetmodeID.Server)
        {
            Terraria.Graphics.Effects.Filters.Scene.Activate("TerrorMod:DesaturateShader");
        }
        if (SkyManager.Instance["TerrorMod:TerrorSky"] != null && !SkyManager.Instance["TerrorMod:TerrorSky"].IsActive() && Main.netMode != NetmodeID.Server)
        {
            SkyManager.Instance.Activate("TerrorMod:TerrorSky");
        }

        //foreach (var allPlayer in Main.ActivePlayers)
        //{
        //    allPlayer.moonLordMonolithShader = true;
        //}
    }

    int IntroDuration = 120;
    void Intro()
    {
        PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 15f, 6f, 20, 1000f, FullName);
        Main.instance.CameraModifiers.Add(modifier);
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
            if (Attack > 8) Attack = 0;
            attackDuration = attackDurations[(int)Attack];

            AttackCount = 0;
            AttackTimer = 0;
        }
        NPC.Opacity = 1f;

        NPC.netUpdate = true;
    }

    void DeleteProjectiles()
    {
        foreach (var proj in Main.ActiveProjectiles)
        {
            if (proj.hostile && proj.damage > 0 && !proj.netImportant)
            {
                proj.Kill();
            }
        }
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
        if (!canDie)
        {
            NPC.dontTakeDamage = true;
            NPC.life = 1;
            doDeath = true;
            return false;
        }
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
        LeadingConditionRule classicRule = new LeadingConditionRule(new Conditions.NotExpert());
        LeadingConditionRule expertRule = new LeadingConditionRule(new Conditions.IsExpert());
        classicRule.OnSuccess(ItemDropRule.OneFromOptions(1, ItemType<BlueSkull>(), ItemType<GreenSkull>(), ItemType<RedSkull>()));
        expertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ItemType<BlueSkull>(), ItemType<GreenSkull>(), ItemType<RedSkull>()));
        expertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ItemType<BlueSkull>(), ItemType<GreenSkull>(), ItemType<RedSkull>()));
        npcLoot.Add(classicRule);
        npcLoot.Add(expertRule);
    }

    public override void BossLoot(ref int potionType)
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
        SoundEngine.PlaySound(SoundID.Roar with { PitchRange = (-0.7f, -0.4f) });
        SoundEngine.PlaySound(SoundID.Roar with { PitchRange = (-0.7f, -0.4f) });
        SoundEngine.PlaySound(SoundID.NPCDeath10 with { PitchRange = (-0.7f, -0.4f) });
    }
}