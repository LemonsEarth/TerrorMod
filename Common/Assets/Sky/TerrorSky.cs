using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace TerrorMod.Common.Assets.Sky
{
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
            Texture2D sky = ModContent.Request<Texture2D>("TerrorMod/Common/Assets/Sky/TerrorSky").Value;
            //spriteBatch.Draw(sky, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), null, Color.White * opacity, 0f, sky.Size() * 0.5f, SpriteEffects.None, 0);
            Vector2 pos = Vector2.Zero;
            for (int i = 0; i < 16; i++)
            {
                pos = i * (Vector2.UnitX * sky.Width);
                spriteBatch.Draw(sky, pos, null, Color.Black, 0f, sky.Size() * 0.5f, 2f, SpriteEffects.None, 0);
            }
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
}
