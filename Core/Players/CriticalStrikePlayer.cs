using System.Collections.Generic;
using Terraria.Audio;
using TerrorMod.Content.Buffs.Debuffs.Movement;

namespace TerrorMod.Core.Players;

public class CriticalStrikePlayer : ModPlayer
{
    public int CriticalStrikeChanceDenominator { get; set; } = 10;
    public bool WasJustCriticallyStriked { get; private set; } = false;
    public override void ResetEffects()
    {
        WasJustCriticallyStriked = false;
        CriticalStrikeChanceDenominator = 10;
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (Main.rand.NextBool(CriticalStrikeChanceDenominator))
        {
            WasJustCriticallyStriked = true;
            modifiers.FinalDamage.Base += Player.statLifeMax2 * 0.2f;
        }
    }

    public override void OnHurt(Player.HurtInfo info)
    {
        if (WasJustCriticallyStriked)
        {
            BodyPart[] bodyParts = Enum.GetValues<BodyPart>();
            List<BodyPart> bodyPartList = new List<BodyPart>(bodyParts);
            BodyPart randomBodyPart = Main.rand.NextFromCollection(bodyPartList);

            AdvancedPopupRequest apr = new AdvancedPopupRequest
            {
                Text = "Critically Striked!",
                DurationInFrames = 120,
                Color = Color.Red,
                Velocity = -Vector2.UnitY * 20, 
            };
            PopupText.NewText(apr, Player.Center);
            SoundEngine.PlaySound(SoundID.NPCHit2 with { PitchRange = (0.2f, 0.3f)}, Player.Center);
            SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt with { PitchRange = (-0.2f, 0f)}, Player.Center);

            switch (randomBodyPart)
            {
                case BodyPart.Head:
                    Player.AddBuff(BuffID.Obstructed, 300);
                    Player.AddBuff(BuffID.Confused, 300);
                    break;
                case BodyPart.Body:
                    Player.AddBuff(BuffID.WitheredArmor, 600);
                    Player.AddBuff(BuffID.BrokenArmor, 600);
                    break;
                case BodyPart.LeftArm or BodyPart.RightArm:
                    Player.AddBuff(BuffID.WitheredWeapon, 480);
                    Player.AddBuff(BuffID.NoBuilding, 480);
                    break;
                case BodyPart.LeftLeg or BodyPart.RightLeg:
                    Player.AddBuff(BuffID.Slow, 480);
                    Player.AddBuff(BuffID.Bleeding, 600);
                    break;
            }
        }
    }
}

public enum BodyPart
{
    Head,
    Body,
    LeftArm,
    RightArm,
    LeftLeg,
    RightLeg
}