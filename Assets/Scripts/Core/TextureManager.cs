using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Linq;

namespace LOT.Core
{
    public class TextureManager : MonoBehaviour
    {
        static TextureManager _instance;
        static public TextureManager Instance {
            get {
                if (_instance == null)
                    InitInstance ();
                return _instance;
            }
        }

        static void InitInstance ()
        {
            var go = new GameObject ("TextureManager");
            go.AddComponent <DontDestroy> ();
            _instance = go.AddComponent<TextureManager> ();
        }

        private TextureManager ()
        {
        }

        const string LANG = "lang/";
        JSONNode textureJSON;
        Dictionary<string, Sprite> AllSprites = new Dictionary<string, Sprite> ();
        Dictionary<string, Dictionary<string, Sprite>> AllSheets = new Dictionary<string, Dictionary<string, Sprite>> ();

        public string [] spriteNames;
        public string [] sheetNames;

        public void RegisterJSON (JSONNode textureJSON)
        {
            this.textureJSON = textureJSON;
        }

        /**
         * options
         *   path
         */
        public Sprite LoadTexture (string path)
        {
            if (AllSprites.ContainsKey (path)) {
                return AllSprites [path];
            }

            return LoadNewTexture (path);
        }

        Sprite LoadNewTexture (string path)
        {
            var sprite = Resources.Load <Sprite> (LANG + path);

            if (sprite == null) {
                // check in spritesheet json
                foreach (var sheetKey in textureJSON.Keys) {
                    var sheetDef = textureJSON [sheetKey];

                    foreach (var spriteKey in sheetDef.Keys) {
                        var spritePath = string.Format ("{0}/{1}", sheetKey, BaseUtils.GetFilenameWithoutExtension (spriteKey));
                        if (spritePath == path) {
                            return LoadSpriteFromSpriteSheet (path, string.Format ("{0}/{1}", sheetKey, BaseUtils.GetFilenameWithoutExtension (sheetDef [spriteKey] ["src"])));
                        }
                    }
                }
            } else {
                AllSprites [path] = sprite;
                UpdateCount ();
                return sprite;
            }

            Debug.LogError (GetType ().Name + "::LoadNewTexture Cannot load texture at " + path);

            return null;
        }

        Sprite LoadSpriteFromSpriteSheet (string path, string sheetPath)
        {
            Dictionary<string, Sprite> sheet;
            if (!AllSheets.TryGetValue (sheetPath, out sheet)) {
                sheet = new Dictionary<string, Sprite> ();
                AllSheets [sheetPath] = sheet;

                Sprite [] sprites = Resources.LoadAll<Sprite> (LANG + sheetPath);
                foreach (var sprite in sprites) {
                    sheet [sprite.name] = sprite;
                }
                UpdateCount ();
            }

            var shortPath = BaseUtils.GetFilenameWithoutExtension (path);
            if (sheet.ContainsKey (shortPath)) {
                return sheet [shortPath];
            }

            Debug.LogError (GetType ().Name + "::LoadSpriteFromSpriteSheet Cannot load texture at " + path);
            return null;
        }

        void UpdateCount ()
        {
            spriteNames = AllSprites.Keys.ToArray ();
            sheetNames = AllSheets.Keys.ToArray ();
        }


        public void UnloadTexture (string path)
        {
            if (AllSprites.ContainsKey (path)) {
                var sprite = AllSprites [path];
                AllSprites.Remove (path);
                Resources.UnloadAsset (sprite);
                UpdateCount ();
            } else {
                // unload 1 sprite in a sheet will unload all sprites in the sheet
                // this is to keep sheet loading consistent
                string dirName;
                foreach (var sheetKey in AllSheets.Keys) {
                    dirName = BaseUtils.GetDirname (sheetKey) + "/";
                    if (!path.StartsWith (dirName)) {
                        continue;
                    }
                    var sheetCol = AllSheets [sheetKey];
                    bool found = false;
                    foreach (var spriteKey in sheetCol.Keys) {
                        if ((dirName + spriteKey) == path) {
                            found = true;
                            break;
                        }
                    }
                    if (found) {
                        UnloadSheet (sheetKey);
                        break;
                    }
                }
            }
        }

        public void UnloadSheet (string sheetName)
        {
            Dictionary<string, Sprite> sheet;
            if (AllSheets.TryGetValue (sheetName, out sheet)) {
                foreach (var spriteKey in sheet.Keys) {
                    Resources.UnloadAsset (sheet [spriteKey]);
                }
                AllSheets.Remove (sheetName);
            }

            UpdateCount ();
        }

        public void UnloadAllTextures ()
        {
            string [] sheetNames = AllSheets.Keys.ToArray ();
            foreach (var sheetName in sheetNames) {
                UnloadSheet (sheetName);
            }

            string [] spriteNames = AllSprites.Keys.ToArray ();
            foreach (var spriteName in spriteNames) {
                UnloadTexture (spriteName);
            }
        }

    }
}
