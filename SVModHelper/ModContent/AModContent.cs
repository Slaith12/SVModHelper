using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVModHelper.ModContent
{
    public abstract class AModContent
    {
        //TODO: Consolidate content functions here and in SVMod in a separate helper class
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetContentData(string fileName, out byte[] data, bool localName = true)
        {
            return ModContentManager.contentData.TryGetValue(GetContentKeyString(fileName, localName), out data);
        }

        //TODO: Update this function to cache textures for future calls
        protected Texture2D GetTexture(string imageName, FilterMode filter = FilterMode.Bilinear, bool localName = true)
        {
	        if (!TryGetContentData(imageName, out byte[] data, localName))
	        {
                MelonLoader.MelonLogger.Error($"Failed to load image - {imageName}. Make sure the file exists has been added as an Embedded Resource, and that the path is specified relative to the .csproj.");
		            return null;
          }
            Texture2D texture = new Texture2D(2, 2) { filterMode = filter };
            texture.LoadImage(data);
            return texture;
        }

        //TODO: Update this function to cache sprites for future calls
        protected Sprite GetStandardSprite(string imageName, float pixelsPerUnit = 100, FilterMode filter = FilterMode.Bilinear, bool localName = true)
        {
            Texture2D texture = GetTexture(imageName, filter, localName);
            if (texture == null)
                return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }

        protected Sprite GetDefaultEntitySprite()
        {
            return GetStandardSprite("SVModHelper.DefaultEntity.png", localName: false);
        }

        protected Sprite GetDefaultShadowSprite()
        {
            return GetStandardSprite("SVModHelper.DefaultShadow.png", localName: false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetContentKeyString(string fileName, bool localName = true)
        {
            if (localName)
                return GetType().Assembly.GetName().Name + "." + fileName;
            else
                return fileName;
        }
    }
}
