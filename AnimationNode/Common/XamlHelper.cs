using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace AnimationNode.Common
{
    public static class ExtensionMethods
    {
        public static T XamlClone<T>(this T original)            where T : class
        {
            if (original == null)
                return null;

            object clone;
            using (var stream = new System.IO.MemoryStream())
            {
                XamlWriter.Save(original, stream);
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                clone = XamlReader.Load(stream);
            }

            if (clone is T)
                return (T)clone;
            else
                return null;
        }
    }
}
