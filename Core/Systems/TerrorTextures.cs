using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace TerrorMod.Core.Systems;

public class TerrorTextures : ModSystem
{
    public static string NoiseTexturePath { get; private set; } = "TerrorMod/Common/Assets/Textures/NoiseTexture";
    public static string TransparentNoiseTexturePath { get; private set; } = "TerrorMod/Common/Assets/Textures/TransparentNoise";
    public static string GlowBallTexturePath { get; private set; } = "TerrorMod/Common/Assets/Textures/GlowBall";
    public static string TrueMagicPixelPath { get; private set; } = "TerrorMod/Common/Assets/Textures/TrueMagicPixel";
    public static string Empty100TexPath { get; private set; } = "TerrorMod/Common/Assets/Textures/Empty100Tex";
    public static string FadingIndicatorPath { get; private set; } = "TerrorMod/Common/Assets/Textures/FadingIndicator";
    public static string UIArrowPath { get; private set; } = "TerrorMod/Common/Assets/Textures/UIArrow";
    public static string UIEyePath { get; private set; } = "TerrorMod/Common/Assets/Textures/EyeIcon";

    public static Asset<Texture2D> NoiseTexture { get; private set; }
    public static Asset<Texture2D> TransparentNoiseTexture { get; private set; }
    public static Asset<Texture2D> GlowBallTexture { get; private set; }
    public static Asset<Texture2D> TrueMagicPixel { get; private set; }
    public static Asset<Texture2D> Empty100Tex { get; private set; }
    public static Asset<Texture2D> FadingIndicator { get; private set; }
    public static Asset<Texture2D> UIArrow { get; private set; }
    public static Asset<Texture2D> UIEye { get; private set; }
    public override void Load()
    {
        NoiseTexture = Request<Texture2D>(NoiseTexturePath);
        TransparentNoiseTexture = Request<Texture2D>(TransparentNoiseTexturePath);
        GlowBallTexture = Request<Texture2D>(GlowBallTexturePath);
        TrueMagicPixel = Request<Texture2D>(TrueMagicPixelPath);
        Empty100Tex = Request<Texture2D>(Empty100TexPath);
        FadingIndicator = Request<Texture2D>(FadingIndicatorPath);
        UIArrow = Request<Texture2D>(UIArrowPath);
        UIEye = Request<Texture2D>(UIEyePath);
    }
}
