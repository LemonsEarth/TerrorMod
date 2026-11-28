namespace TerrorMod.Core.Players;

public class StaminaPlayer : ModPlayer
{
    public float Stamina { get; set; } = 200;
    public int MaxStamina { get; set; } = 200;

    public int StaminaRegenTimer { get; set; } = 0;
    public int StaminaRegenDelay { get; set; } = 30;
    public float StaminaRegenAmount { get; set; } = 1f;
    public int TimeSinceStaminaRegened { get; set; } = 0;

    public float LastConsumedStaminaAmount { get; set; } = 0;

    public float RunningStaminaCost = 0.1f;
    public float JumpingStaminaCost = 10f;
    public float DashingStaminaCost = 20f;

    public void ConsumeStamina(float amount)
    {
        Stamina -= amount;
        LastConsumedStaminaAmount = amount;
        if (Stamina < 0)
        {
            Stamina = 0;
        }
        StaminaRegenTimer = StaminaRegenDelay;
    }

    public override void OnRespawn()
    {
        Stamina = MaxStamina;
    }

    public override void PreUpdateMovement()
    {

    }

    public override void UpdateEquips()
    {
        //Stamina = MaxStamina;
    }

    public override void PostUpdateRunSpeeds()
    {

    }

    public override void PostUpdateMiscEffects()
    {
        if (StaminaRegenTimer > 0)
        {
            StaminaRegenTimer--;
        }

        RunningStaminaConsumption();
        //JumpingStaminaConsumption();
        DashingStaminaConsumption();

        RegenStamina();

        Stamina = MathHelper.Clamp(Stamina, 0, MaxStamina);
    }

    public void RunningStaminaConsumption()
    {
        if (MathF.Abs(Player.velocity.X) > Player.accRunSpeed - 1)
        {
            ConsumeStamina(RunningStaminaCost);
        }
    }

    public void JumpingStaminaConsumption()
    {
        if (Player.justJumped)
        {
            ConsumeStamina(JumpingStaminaCost);
        }
    }

    public int dashDelayFrameCount = 0;
    public bool DashJustStarted => dashDelayFrameCount == 1;

    public void DashDelayFrameCounter()
    {
        if (Player.dashDelay == -1)
        {
            dashDelayFrameCount++;
        }
        else
        {
            dashDelayFrameCount = 0;
        }
    }

    public void DashingStaminaConsumption()
    {
        DashDelayFrameCounter();
        if (DashJustStarted)
        {
            DashingStaminaCost = 50;
            ConsumeStamina(DashingStaminaCost);
        }
    }

    public void RegenStamina()
    {
        TimeSinceStaminaRegened++;
        if (StaminaRegenTimer == 0)
        {
            if (Stamina < MaxStamina)
            {
                Stamina += StaminaRegenAmount;
            }
            TimeSinceStaminaRegened = 0;
        }
    }
}

public class StaminaConsumingItem : GlobalItem
{
    public override bool InstancePerEntity => true;

    public static int GetStaminaCost(Item item)
    {
        if (item.useStyle != ItemUseStyleID.Swing &&
            item.useStyle != ItemUseStyleID.Thrust &&
            item.useStyle != ItemUseStyleID.Rapier)
        {
            return 0;
        }

        if (item.damage == -1)
        {
            return 0;
        }

        int cost = 0;

        cost += item.useTime;

        return cost;
    }

    public override bool CanUseItem(Item item, Player player)
    {
        return player.GetStamina() > GetStaminaCost(item);
    }

    public override bool? UseItem(Item item, Player player)
    {
        if (player.ItemTimeIsZero)
        {
            player.GetModPlayer<StaminaPlayer>().ConsumeStamina(GetStaminaCost(item));
            return true;
        }
        return null;
    }
}
