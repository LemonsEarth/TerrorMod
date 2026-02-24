using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria.ModLoader.IO;

namespace TerrorMod.Core.Systems;

public class TerrorTextures : ModSystem
{
    const string NoisePath = "TerrorMod/Common/Assets/Textures/NoiseTexture";
    public static Asset<Texture2D> NoiseTexture { get; private set; }

    public override void Load()
    {
        NoiseTexture = Request<Texture2D>(NoisePath);
    }
}
