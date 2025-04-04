using Terraria;
using Terraria.Chat;
using Terraria.ModLoader;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Commands
{
    public class BlindSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "blindskull";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            SkullSystem.blindSkullActive = !SkullSystem.blindSkullActive;
            Main.NewText("Blind: " + SkullSystem.blindSkullActive);
        }
    }

    public class VagrantSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "vagrantskull";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            SkullSystem.vagrantSkullActive = !SkullSystem.vagrantSkullActive;
            Main.NewText("Vagrant: " + SkullSystem.vagrantSkullActive);
        }
    }
}
