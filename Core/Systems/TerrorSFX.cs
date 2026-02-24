using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria.Audio;
using Terraria.ModLoader.IO;

namespace TerrorMod.Core.Systems;

public class TerrorSFX : ModSystem
{
    public static SoundStyle Jumpscare => new SoundStyle("TerrorMod/Common/Assets/Audio/SFX/Jumpscare_", 3);
    public static SoundStyle Thunder => new SoundStyle("TerrorMod/Common/Assets/Audio/SFX/Thunder");
}
