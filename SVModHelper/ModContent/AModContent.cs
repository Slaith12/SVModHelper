using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVModHelper.ModContent
{
    public abstract class AModContent
    {
        internal static Sprite shadowSprite;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetContentData(string fileName, out byte[] data)
        {
            return ModContentManager.contentData.TryGetValue(GetContentKeyString(fileName), out data);
        }

        //TODO: Update this function to cache textures for future calls
        protected Texture2D GetTexture(string imageName, FilterMode filter = FilterMode.Bilinear)
        {
            if (!TryGetContentData(imageName, out byte[] data))
                return null;
            Texture2D texture = new Texture2D(2, 2) { filterMode = filter };
            texture.LoadImage(data);
            return texture;
        }

        //TODO: Update this function to cache sprites for future calls
        protected Sprite GetStandardSprite(string imageName, float pixelsPerUnit = 100, FilterMode filter = FilterMode.Bilinear)
        {
            Texture2D texture = GetTexture(imageName, filter);
            if (texture == null)
                return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }

        public static Sprite GetDefaultShadowSprite()
        {
            return shadowSprite;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetContentKeyString(string fileName)
        {
            return GetType().Assembly.GetName().Name + "." + fileName;
        }
    }
}
