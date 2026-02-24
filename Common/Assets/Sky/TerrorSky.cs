using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using TerrorMod.Core.Systems;

namespace TerrorMod.Common.Assets.Sky;

public class TerrorSky : CustomSky
{
    public bool _isActive = false;
    float opacity = 0f;

    public override void Update(GameTime gameTime)
    {
        opacity = MathHelper.Lerp(opacity, 1f, 1f / 60f);
    }

    public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
    {
        Texture2D sky = Request<Texture2D>("TerrorMod/Common/Assets/Sky/BlackBackground").Value;

        var shader = GameShaders.Misc["TerrorMod:BlackSunShader"];
        shader.Shader.Parameters["moveSpeed"].SetValue(-2f);
        shader.Shader.Parameters["opacity"].SetValue(opacity);
        shader.Shader.Parameters["uScreenResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, shader.Shader, Main.GameViewMatrix.TransformationMatrix);
        Main.instance.GraphicsDevice.Textures[1] = TerrorTextures.NoiseTexture.Value;
        shader.Apply();
        spriteBatch.Draw(sky, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
    }

    public override bool IsActive()
    {
        return _isActive;
    }

    public override void Activate(Vector2 position, params object[] args)
    {
        _isActive = true;
        opacity = 0;
    }

    public override void Deactivate(params object[] args)
    {
        _isActive = false;
        opacity = 0;
    }

    public override void Reset()
    {
        _isActive = false;
        opacity = 0;
    }
}
