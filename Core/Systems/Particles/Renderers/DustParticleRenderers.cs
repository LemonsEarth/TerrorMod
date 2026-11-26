using Microsoft.Xna.Framework.Graphics;

namespace TerrorMod.Core.Systems.Particles.Renderers;

public class BeforeDustParticleRenderer : ParticleRenderer
{
    public override void Load()
    {
        On_Main.DrawDust += On_Main_DrawDust;
    }

    private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
    {
        DrawParticles();
        orig(self);
    }
}

public class AfterDustParticleRenderer : ParticleRenderer
{
    public override void Load()
    {
        On_Main.DrawDust += On_Main_DrawDust;
    }

    private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
    {
        orig(self);
        DrawParticles();
    }
}

public class AfterDustParticleRendererGlowy : ParticleRenderer
{
    public override void Load()
    {
        On_Main.DrawDust += On_Main_DrawDust;
    }

    private void On_Main_DrawDust(On_Main.orig_DrawDust orig, Main self)
    {
        orig(self);
        DrawParticles();
    }

    public override void DrawParticles()
    {
        //var shader = GameShaders.Misc["TerrorMod:FireballShader"];
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        for (int i = 0; i < ActiveParticleCount; i++)
        {
            ParticleSystem.TypesByID[Particles[i].type].Draw(Particles[i]);
        }
        Main.spriteBatch.End();
    }
}
