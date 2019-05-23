using System;
using System.Collections.Generic;
using UnityEngine;

namespace LOT.Core
{
    public class SpriteCollection : MonoBehaviour
    {
        public List<Sprite> sprites = new List<Sprite> ();
        public string collectionName;
        public string folderName;

        public static Dictionary<string, SpriteCollection> AllCollections = new Dictionary<string, SpriteCollection> ();

        public static SpriteCollection GetCollection (string collectionName)
        {
            SpriteCollection collection = null;
            AllCollections.TryGetValue (collectionName,out collection);
            if (collection == null) {
                collection = BaseUtils.Instantiate ("Prefabs/SpriteCollections/" + collectionName).GetComponent<SpriteCollection> ();
                collection.collectionName = collectionName;
                AllCollections [collectionName] = collection;
            }

            return collection;
        }

        public Sprite GetSprite (int index)
        {
            if (index >= 0 && index < sprites.Count)
                return sprites [index];

            return null;
        }

        public Sprite GetSprite (string spriteName)
        {
            foreach (Sprite sprite in sprites) {
                if (sprite != null && sprite.name == spriteName)
                    return sprite;
            }
            return null;
        }
    }
}
