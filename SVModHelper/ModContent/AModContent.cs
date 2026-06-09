using MelonLoader;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SVModHelper.ModContent
{
    public abstract class AModContent
    {
        //TODO: Consolidate content functions here and in SVMod in a separate helper class
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetContentData(string fileName, out byte[] data, bool localName = true, bool warnOnFail = true)
        {
            bool success = ModContentManager.contentData.TryGetValue(GetContentKeyString(fileName, localName), out data);
            if (!success && warnOnFail)
            {
                Melon<Core>.Logger.Error($"Failed to load file - {fileName}. Make sure the file exists, it has been added as an Embedded Resource, and that the path is specified relative to the .csproj file.");
            }
            return success;
        }

        //TODO: Update this function to cache textures for future calls
        protected Texture2D oldGetTexture(string imageName,
            FilterMode filter = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp,
            bool localName = true, bool warnOnFail = true)
        {
            if (!TryGetContentData(imageName, out byte[] data, localName, warnOnFail))
                return null;
            Texture2D texture = new Texture2D(2, 2) { filterMode = filter, wrapMode = wrapMode};
            texture.LoadImage(data);
            return texture;
        }

        //TODO: Update this function to cache sprites for future calls
        protected Sprite oldGetStandardSprite(string imageName, float pixelsPerUnit = 100,
            FilterMode filter = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp,
            bool localName = true, bool warnOnFail = true)
        {
            Texture2D texture = oldGetTexture(imageName, filter, wrapMode, localName, warnOnFail);
            if (texture == null)
                return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }

        protected SpriteDescriptor GetStandardSprite(string imageName, float pixelsPerUnit = 100,
            FilterMode filter = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp,
            Rect? rect = null, Vector2? pivot = null,
            bool localName = true)
        {
            return new SpriteDescriptor(GetContentKeyString(imageName, localName), filter, wrapMode, rect, pivot, pixelsPerUnit);
        }

        protected Sprite GetDefaultEntitySprite()
        {
            return oldGetStandardSprite("SVModHelper.DefaultEntity.png", localName: false);
        }

        protected Sprite GetDefaultShadowSprite()
        {
            return oldGetStandardSprite("SVModHelper.DefaultShadow.png", localName: false);
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
