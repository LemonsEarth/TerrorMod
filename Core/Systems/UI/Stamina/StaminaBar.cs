using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using TerrorMod.Core.Players;

namespace TerrorMod.Core.Systems.UI.Stamina;

public class StaminaBar : UIState
{
    // For this bar we'll be using a frame texture and then a gradient inside bar, as it's one of the more simpler approaches while still looking decent.
    // Once this is all set up make sure to go and do the required stuff for most UI's in the ModSystem class.
    private UIElement area;
    private UIImage barFrame;
    private Color gradientA;
    private Color gradientB;

    public override void OnInitialize()
    {
        // Create a UIElement for all the elements to sit on top of, this simplifies the numbers as nested elements can be positioned relative to the top left corner of this element. 
        // UIElement is invisible and has no padding.
        area = new UIElement();
        area.Left.Set(0, 0.5f); // Place the resource bar to the left of the hearts.
        area.Top.Set(0, 0.6f); // Placing it just a bit below the top of the screen.
        area.Width.Set(182, 0f); // We will be placing the following 2 UIElements within this 182x60 area.
        area.Height.Set(60, 0f);

        barFrame = new UIImage(ModContent.Request<Texture2D>("TerrorMod/Core/Systems/UI/Stamina/StaminaBarFrame")); // Frame of our resource bar
        barFrame.Left.Set(22, 0f);
        barFrame.Top.Set(0, 0f);
        barFrame.Width.Set(136, 0f);
        barFrame.Height.Set(34, 0f);

        gradientA = Color.Green;
        gradientB = Color.White;

        //area.Append(text);
        area.Append(barFrame);
        Append(area);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    // Here we draw our UI
    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
        area.Left.Set(0 - area.Width.Pixels * 0.5f, 0.5f); // Place the resource bar to the left of the hearts.
        area.Top.Set(0, 0.55f); // Placing it just a bit below the top of the screen.
        area.Recalculate();
        base.DrawSelf(spriteBatch);

        // Here we get the screen dimensions of the barFrame element, then tweak the resulting rectangle to arrive at a rectangle within the barFrame texture that we will draw the gradient. These values were measured in a drawing program.
        Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
        hitbox.X += 8;
        hitbox.Width -= 16;
        hitbox.Y += 4;
        hitbox.Height -= 8;

        // Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
        int left = hitbox.Left;
        int right = hitbox.Right;

        var modPlayer = Main.LocalPlayer.GetModPlayer<StaminaPlayer>();
        float quotient = (float)modPlayer.Stamina / modPlayer.MaxStamina;
        quotient = Utils.Clamp(quotient, 0f, 1f);
        int mainStaminaWidth = (int)(hitbox.Width * quotient);

        // Background
        spriteBatch.Draw(
            TextureAssets.MagicPixel.Value,
            new Rectangle(left, hitbox.Y, hitbox.Width, hitbox.Height),
            Color.DarkGreen);

        int yellowBarWidth = (int)(hitbox.Width * ((modPlayer.Stamina + modPlayer.LastConsumedStaminaAmount) / modPlayer.MaxStamina));
        yellowBarWidth = Utils.Clamp(yellowBarWidth, 0, hitbox.Width);

        // Slower yellow bar
        spriteBatch.Draw(
            TextureAssets.MagicPixel.Value,
            new Rectangle(left, hitbox.Y, (int)yellowBarWidth, hitbox.Height),
            Color.Yellow * 0.5f);

        // Main stamina bar
        Color stColor = modPlayer.Stamina != modPlayer.MaxStamina ?
            Color.PaleGreen :
            Color.Lerp(Color.PaleGreen, Color.White, MathF.Sin((float)Main.timeForVisualEffects / 10));
        spriteBatch.Draw(
            TextureAssets.MagicPixel.Value,
            new Rectangle(left, hitbox.Y, mainStaminaWidth, hitbox.Height),
            stColor);
    }

    public override void Update(GameTime gameTime)
    {
        var modPlayer = Main.LocalPlayer.GetModPlayer<StaminaPlayer>();
        // Setting the text per tick to update and show our resource values.
        //text.SetText(StaminaUISystem.ExampleResourceText.Format(modPlayer.Stamina, modPlayer.MaxStamina));
        base.Update(gameTime);
    }
}

// This class will only be autoloaded/registered if we're not loading on a server
[Autoload(Side = ModSide.Client)]
internal class StaminaUISystem : ModSystem
{
    private UserInterface StaminaBarUserInterface;

    internal StaminaBar StaminaBar;

    public override void Load()
    {
        StaminaBar = new();
        StaminaBarUserInterface = new();
        StaminaBarUserInterface.SetState(StaminaBar);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        StaminaBarUserInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
        if (resourceBarIndex != -1)
        {
            layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                "TerrorMod: Stamina Bar",
                delegate
                {
                    StaminaBarUserInterface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}
