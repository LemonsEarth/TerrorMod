using Terraria;
using Terraria.Chat;
using Terraria.ModLoader;
using TerrorMod.Core.Systems;

namespace TerrorMod.Core.Commands
{
    public class AllSkullsCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "allskulls";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Main.NewText("Blind: " + SkullSystem.blindSkullActive);
            Main.NewText("Vagrant: " + SkullSystem.vagrantSkullActive);
            Main.NewText("Savage: " + SkullSystem.savageSkullActive);
            Main.NewText("Tough Luck: " + SkullSystem.toughLuckSkullActive);
            Main.NewText("briar: " + SkullSystem.briarSkullActive);
        }
    }

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

    public class SavageSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "savageskull";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            SkullSystem.savageSkullActive = !SkullSystem.savageSkullActive;
            Main.NewText("Savage: " + SkullSystem.savageSkullActive);
        }
    }

    public class ToughLuckSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "toughluckskull";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            SkullSystem.toughLuckSkullActive = !SkullSystem.toughLuckSkullActive;
            Main.NewText("Tough Luck: " + SkullSystem.toughLuckSkullActive);
        }
    }

    public class BriarSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "briarskull";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            SkullSystem.briarSkullActive = !SkullSystem.briarSkullActive;
            Main.NewText("briar: " + SkullSystem.briarSkullActive);
        }
    }

    public class GluttonySkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "gluttonyskull";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            SkullSystem.gluttonySkullActive = !SkullSystem.gluttonySkullActive;
            Main.NewText("gluttony: " + SkullSystem.gluttonySkullActive);
        }
    }
}
