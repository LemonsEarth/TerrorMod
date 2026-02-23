global using Microsoft.Xna.Framework;
global using System;
global using Terraria;
global using Terraria.ID;
global using Terraria.ModLoader;
global using TerrorMod.Common.Utils;
global using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using TerrorMod.Common.Assets.Sky;
using TerrorMod.Core.Players;

namespace TerrorMod;

// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
public class TerrorMod : Mod
{
    const string NoisePath = "TerrorMod/Common/Assets/Textures/NoiseTexture";
    public static Asset<Texture2D> noiseTexture;

    public static SoundStyle Jumpscare => new SoundStyle("TerrorMod/Common/Assets/Audio/SFX/Jumpscare_", 3);
    public static SoundStyle Thunder => new SoundStyle("TerrorMod/Common/Assets/Audio/SFX/Thunder");

    public static TerrorMod instance;

    public TerrorMod()
    {
        MusicSkipsVolumeRemap = true;
    }

    internal enum MessageType : byte
    {
        CurseLevelSync
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        MessageType msg = (MessageType)reader.ReadByte();

        switch (msg)
        {
            case MessageType.CurseLevelSync:
                byte playerNumber = reader.ReadByte();
                TerrorPlayer player = Main.player[playerNumber].GetModPlayer<TerrorPlayer>();
                player.ReceivePlayerSync(reader);
                if (Main.netMode == NetmodeID.Server)
                {
                    player.SyncPlayer(-1, whoAmI, false);
                }
                break;
        }
    }

    public override void Load()
    {
        instance = this;

        Asset<Effect> laserShader = Assets.Request<Effect>("Common/Assets/Shaders/LaserShader");
        GameShaders.Misc["TerrorMod:LaserShader"] = new MiscShaderData(laserShader, "LaserShader");

        Asset<Effect> sphereShader = Assets.Request<Effect>("Common/Assets/Shaders/SphereShader");
        GameShaders.Misc["TerrorMod:SphereShader"] = new MiscShaderData(sphereShader, "SphereShader");

        Asset<Effect> blackSunShader = Assets.Request<Effect>("Common/Assets/Shaders/BlackSunShader");
        GameShaders.Misc["TerrorMod:BlackSunShader"] = new MiscShaderData(blackSunShader, "BlackSunShader");

        Asset<Effect> projLightShader = Assets.Request<Effect>("Common/Assets/Shaders/ProjectileLightShader");
        GameShaders.Misc["TerrorMod:ProjectileLightShader"] = new MiscShaderData(projLightShader, "ProjectileLight");

        Asset<Effect> desaturateShader = Assets.Request<Effect>("Common/Assets/Shaders/DesaturateShader");
        Filters.Scene["TerrorMod:DesaturateShader"] = new Filter(new ScreenShaderData(desaturateShader, "DesaturateShader"), EffectPriority.VeryHigh);

        noiseTexture = Request<Texture2D>(NoisePath);
        SkyManager.Instance["TerrorMod:TerrorSky"] = new TerrorSky();
    }
}
