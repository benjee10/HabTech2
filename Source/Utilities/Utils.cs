using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HabTech2.Utilities
{
    public static class Utils
    {
        public static ConfigNode FindModule(this StoredPart storedPart, string name)
        {
            return storedPart.snapshot.partInfo.partConfig.GetNodes("MODULE").Where(pm => pm.GetValue("name").Equals(name)).FirstOrDefault();
        }
        public static Sprite ToSprite(this Texture2D texture2D)
        {
            return Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        }

        public static T DeepCopy<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }
    }
}
