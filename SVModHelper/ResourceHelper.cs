using Il2CppStarVaders;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper
{
    public static class ResourceHelper
    {
        public static byte[] LoadResource(Assembly assembly, string fileName)
        {
            using (Stream stream = assembly.GetManifestResourceStream(fileName))
            {
                if (stream == null)
                    return null;

                byte[] arr;
                if (stream is MemoryStream memStream)
                {
                    arr = memStream.ToArray();
                }
                else
                {
                    using (memStream = new MemoryStream())
                    {
                        stream.CopyTo(memStream);
                        arr = memStream.ToArray();
                    }
                }
                return arr;
            }
        }
    }
}
