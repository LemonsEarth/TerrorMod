using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria.ModLoader.IO;

namespace TerrorMod.Core.Systems;

public class TerrorTextures : ModSystem
{
    public const string NoisePath = "TerrorMod/Common/Assets/Textures/NoiseTexture";
    public const string GlowBallPath = "TerrorMod/Common/Assets/Textures/GlowBall";
    public const string TrueMagicPixelPath = "TerrorMod/Common/Assets/Textures/TrueMagicPixel";
    public const string Empty100TexPath = "TerrorMod/Common/Assets/Textures/Empty100Tex";
    public static Asset<Texture2D> TrueMagicPixel { get; private set; }
    public static Asset<Texture2D> NoiseTexture { get; private set; }
    public static Asset<Texture2D> GlowBall { get; private set; }
    public static Asset<Texture2D> Empty100Tex { get; private set; }

    public override void Load()
    {
        NoiseTexture = Request<Texture2D>(NoisePath);
        GlowBall = Request<Texture2D>(GlowBallPath);
        TrueMagicPixel = Request<Texture2D>(TrueMagicPixelPath);
        Empty100Tex = Request<Texture2D>(Empty100TexPath);

    }
}
