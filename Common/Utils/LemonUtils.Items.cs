using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using TerrorMod.Core.Players;

namespace TerrorMod.Common.Utils;

/// <summary>
/// Contains a lot of utillities and global usings
/// </summary>
public static partial class LemonUtils
{
    public static void DrawAscendedWeaponGlowInWorld(Item item, int originalItemID, float rotation, float scale, SpriteBatch spriteBatch, Color color)
    {
        Main.instance.LoadItem(originalItemID);
        Texture2D origTexture = TextureAssets.Item[originalItemID].Value;
        Texture2D glowTexture = TextureAssets.Item[item.type].Value;
        Vector2 drawPos = item.Center - Main.screenPosition;
        spriteBatch.Draw(glowTexture, drawPos, null, color, rotation, glowTexture.Size() * 0.5f, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(origTexture, drawPos, null, Color.White, rotation, origTexture.Size() * 0.5f, scale, SpriteEffects.None, 0);
    }

    public static void DrawAscendedWeaponGlowInInventory(Item item, int originalItemID, Vector2 position, float scale, Rectangle frame, SpriteBatch spriteBatch, Color color)
    {
        Main.instance.LoadItem(originalItemID);
        Texture2D origTexture = TextureAssets.Item[originalItemID].Value;
        Texture2D glowTexture = TextureAssets.Item[item.type].Value;
        spriteBatch.Draw(glowTexture, position, null, color, 0f, glowTexture.Size() * 0.5f, scale, SpriteEffects.None, 0);
        spriteBatch.Draw(origTexture, position, null, Color.White, 0f, origTexture.Size() * 0.5f, scale, SpriteEffects.None, 0);
    }

    public static int GetStaminaCost(Item item)
    {
        return StaminaConsumingItem.GetStaminaCost(item);
    }
}
