using UnityEngine;

namespace SVModHelper
{
    internal static class SpriteHelper
    {
		private static Sprite _transparentSprite;

	    public static Sprite GetTransparentSprite()
	    {
			if (_transparentSprite != null)
				return _transparentSprite;

			// Create a 1x1 transparent texture
			Texture2D transparentTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
			transparentTexture.SetPixel(0, 0, new Color(0, 0, 0, 0)); // Fully transparent
			transparentTexture.Apply();

			// Create sprite from the transparent texture
			_transparentSprite = Sprite.Create(transparentTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100);

			return _transparentSprite;
	    }
            
    }
}
