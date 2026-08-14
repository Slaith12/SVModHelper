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

        protected SpriteDescriptor GetStandardSprite(string imageName, float pixelsPerUnit = 100,
            FilterMode filter = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp,
            Rect? rect = null, Vector2? pivot = null,
            bool localName = true)
        {
            return new SpriteDescriptor(GetContentKeyString(imageName, localName), filter, wrapMode, rect, pivot, pixelsPerUnit);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string GetContentKeyString(string fileName, bool localName = true)
        {
            if (localName)
                return GetType().Assembly.GetName().Name + "." + fileName;
            else
                return fileName;
        }
    }
}
