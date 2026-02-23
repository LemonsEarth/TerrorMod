using TerrorMod.Core.Systems;

namespace TerrorMod.Core.SceneEffects;

public class HellbreachScene : ModSceneEffect
{
    public override int Music => MusicID.OtherworldlyUnderworld;
    public override SceneEffectPriority Priority => SceneEffectPriority.Event;

    public override bool IsSceneEffectActive(Player player)
    {
        return EventSystem.hellbreachActive && player.ZoneOverworldHeight;
    }
}
