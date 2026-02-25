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

        LoadMiscShader("LaserShader", "Common/Assets/Shaders/LaserShader");
        LoadMiscShader("SphereShader", "Common/Assets/Shaders/SphereShader");
        LoadMiscShader("BlackSunShader", "Common/Assets/Shaders/BlackSunShader");
        LoadMiscShader("ProjectileLightShader", "Common/Assets/Shaders/ProjectileLightShader");
        LoadMiscShader("ProjectileLightShader", "Common/Assets/Shaders/ShieldPulseShader");

        LoadFilterShader("DesaturateShader", "Common/Assets/Shaders/DesaturateShader", EffectPriority.VeryHigh);
        LoadFilterShader("WavyShader", "Common/Assets/Shaders/WavyShader", EffectPriority.VeryHigh);
        LoadFilterShader("VignetteShader", "Common/Assets/Shaders/VignetteShader", EffectPriority.VeryHigh);

        SkyManager.Instance["TerrorMod:TerrorSky"] = new TerrorSky();
    }

    void LoadFilterShader(string name, string path, EffectPriority priority)
    {
        Asset<Effect> filter = Assets.Request<Effect>(path);
        Filters.Scene[$"TerrorMod:{name}"] = new Filter(new ScreenShaderData(filter, name), priority);
    }

    void LoadMiscShader(string name, string path)
    {
        Asset<Effect> shader = Assets.Request<Effect>(path);
        GameShaders.Misc[$"TerrorMod:{name}"] = new MiscShaderData(shader, name);
    }
}
