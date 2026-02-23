using System.Collections.Generic;

namespace TerrorMod.Common.Utils;

public class NPCLists : NPCID
{
    public static HashSet<int> SafeNPCs { get; private set; } = new HashSet<int>()
    {
        Creeper, ServantofCthulhu, EaterofWorldsHead, EaterofWorldsBody, EaterofWorldsTail, SkeletronHand, CultistArcherBlue, CultistDevote,
        OldMan, TheHungry, TheHungryII, LeechHead, LeechBody, LeechTail, TheDestroyerBody, TheDestroyerTail, PrimeCannon, PrimeLaser, PrimeCannon, PrimeSaw,
        PlanterasHook, PlanterasTentacle, GolemFistLeft, GolemFistRight, GolemHead, GolemHeadFree, CultistDragonBody1, CultistDragonBody2, CultistDragonBody3, CultistDragonBody4, CultistDragonHead, CultistDragonTail, MoonLordCore, MoonLordFreeEye, MoonLordHand, MoonLordHead, MoonLordLeechBlob,
        WallofFleshEye
    };
}
