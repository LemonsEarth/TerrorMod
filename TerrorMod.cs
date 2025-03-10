using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorMod.Core.Players;

namespace TerrorMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class TerrorMod : Mod
	{
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
            Asset<Effect> laserShader = Assets.Request<Effect>("Common/Assets/Shaders/LaserShader");
            GameShaders.Misc["TerrorMod:LaserShader"] = new MiscShaderData(laserShader, "LaserShader");

            Asset<Effect> sphereShader = Assets.Request<Effect>("Common/Assets/Shaders/SphereShader");
            GameShaders.Misc["TerrorMod:SphereShader"] = new MiscShaderData(sphereShader, "SphereShader");

            Asset<Effect> desaturateShader = Assets.Request<Effect>("Common/Assets/Shaders/DesaturateShader");
            Filters.Scene["TerrorMod:DesaturateShader"] = new Filter(new ScreenShaderData(desaturateShader, "DesaturateShader"), EffectPriority.VeryHigh);
        }
    }
}
