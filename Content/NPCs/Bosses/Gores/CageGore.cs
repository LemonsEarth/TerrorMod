using Terraria.DataStructures;
using Terraria.GameContent;

namespace TerrorMod.Content.NPCs.Bosses.Gores;

public class CageGore : ModGore
{
    public override void SetStaticDefaults()
    {
        ChildSafety.SafeGore[Type] = true;
    }

    public override void OnSpawn(Gore gore, IEntitySource source)
    {
        ChildSafety.SafeGore[Type] = true;
        gore.numFrames = 3;
        gore.frame = (byte)Main.rand.Next(0, 3);
        gore.sticky = false;
        UpdateType = 0;
    }
}
