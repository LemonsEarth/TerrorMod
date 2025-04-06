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
            Main.NewText("Briar: " + SkullSystem.briarSkullActive);
            Main.NewText("Gluttony: " + SkullSystem.gluttonySkullActive);
            Main.NewText("Greed: " + SkullSystem.greedSkullActive);
        }
    }

    public class BlindSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "blind";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (ModLoader.TryGetMod("DragonLens", out Mod dragonLens))
            {
                SkullSystem.blindSkullActive = !SkullSystem.blindSkullActive;
                Main.NewText("Blind: " + SkullSystem.blindSkullActive);
            }
        }
    }

    public class VagrantSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "vagrant";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (ModLoader.TryGetMod("DragonLens", out Mod dragonLens))
            {
                SkullSystem.vagrantSkullActive = !SkullSystem.vagrantSkullActive;
                Main.NewText("Vagrant: " + SkullSystem.vagrantSkullActive);
            }
        }
    }

    public class SavageSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "savage";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (ModLoader.TryGetMod("DragonLens", out Mod dragonLens))
            {
                SkullSystem.savageSkullActive = !SkullSystem.savageSkullActive;
                Main.NewText("Savage: " + SkullSystem.savageSkullActive);
            }
        }
    }

    public class ToughLuckSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "toughluck";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (ModLoader.TryGetMod("DragonLens", out Mod dragonLens))
            {
                SkullSystem.toughLuckSkullActive = !SkullSystem.toughLuckSkullActive;
                Main.NewText("Tough Luck: " + SkullSystem.toughLuckSkullActive);
            }
        }
    }

    public class BriarSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "briar";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (ModLoader.TryGetMod("DragonLens", out Mod dragonLens))
            {
                SkullSystem.briarSkullActive = !SkullSystem.briarSkullActive;
                Main.NewText("Briar: " + SkullSystem.briarSkullActive);
            }
        }
    }

    public class GluttonySkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "gluttony";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (ModLoader.TryGetMod("DragonLens", out Mod dragonLens))
            {
                SkullSystem.gluttonySkullActive = !SkullSystem.gluttonySkullActive;
                Main.NewText("Gluttony: " + SkullSystem.gluttonySkullActive);
            }
        }
    }

    public class GreedSkullCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "greed";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (ModLoader.TryGetMod("DragonLens", out Mod dragonLens))
            {
                SkullSystem.greedSkullActive = !SkullSystem.greedSkullActive;
                Main.NewText("Greed: " + SkullSystem.greedSkullActive);
            }
        }
    }
}
