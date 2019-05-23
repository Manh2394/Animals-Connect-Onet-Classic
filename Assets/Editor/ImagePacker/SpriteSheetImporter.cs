using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleJSON;
using UnityEditor;
using UnityEngine;
using LOT.Core;

namespace Monster.Editor.ImagePacker
{
    class SpriteSheetImporter : AssetPostprocessor
    {
        const string ImagePrefix = "Assets/Resources/";

        static JSONNode textureJson;

        void OnPreprocessModel ()
        {
            Debug.Log ("OnPreprocessModel " + assetPath);
        }

        void OnPreprocessTexture ()
        {
            Debug.Log ("SpriteSheetImporter::OnPreprocessTexture " + assetPath);

            if (!(assetPath.StartsWith (ImagePrefix) && assetPath.Contains ("sheet"))) {
                return;
            }

            LoadTextureJson ();

            var path = assetPath.Replace (ImagePrefix, "");
            string dirname = BaseUtils.GetDirname (path);

            if (!textureJson.Keys.Contains (dirname)) {
                return;
            }

            var sheetDef = textureJson [dirname];
            var isPacked = false;
            foreach (var key in sheetDef.Keys) {
                var spriteDef = sheetDef [key];
                var src = string.Format ("{0}/{1}", dirname, spriteDef ["src"].Value);
                if (path == src) {
                    isPacked = true;
                    break;
                }
            }

            if (!isPacked) {
                return;
            }

            Debug.Log ("Importing spritesheet..." + assetPath);

            UpdateTextureImporter (sheetDef, (TextureImporter)assetImporter, path, dirname);
        }

        [MenuItem ("ImagePacker/Update All Spritesheets")]
        [MenuItem ("Assets/ImagePacker/Update All Spritesheets")]
        static void UpdateSpritesheetMenuOption ()
        {
            LoadTextureJson ();

            var sheets = new HashSet<string> ();
            foreach (var sheetKey in textureJson.Keys) {
                var sheetDef = textureJson [sheetKey];

                foreach (var key in sheetDef.Keys) {
                    var spriteDef = sheetDef [key];
                    var path = string.Format ("{0}/{1}", sheetKey, spriteDef ["src"].Value);
                    if (sheets.Contains (path)) {
                        continue;
                    }
                    sheets.Add (path);
                }
            }
            foreach (string path in sheets) {
                var assetImporter = AssetImporter.GetAtPath (string.Format ("Assets/Resources/{0}", path));
                if (assetImporter) {
                    assetImporter.SaveAndReimport ();
                }
            }
        }

        [MenuItem ("Assets/ImagePacker/Update Spritesheet", true)]
        static bool UpdateSpritesheetContextMenuOptionValidation ()
        {
            return Selection.activeObject is Texture2D;
        }

        [MenuItem ("Assets/ImagePacker/Update This Spritesheet")]
        static void UpdateSpritesheetContextMenuOption ()
        {
            var path = AssetDatabase.GetAssetPath (Selection.activeObject);

            Debug.Log ("SpriteSheetImporter::UpdateSpritesheetContextMenuOption " + path);

            var assetImporter = AssetImporter.GetAtPath (path);
            if (assetImporter) {
                assetImporter.SaveAndReimport ();
            }
        }

        static void LoadTextureJson ()
        {
            textureJson = JSONNode.Parse (Resources.Load<TextAsset> ("packed_texture").text);
            textureJson = textureJson ["textureJSON"];
        }

        static void UpdateTextureImporter (JSONNode sheetDef, TextureImporter textureImporter, string path, string dirname)
        {
            Debug.Log (path + " " + dirname + " " + sheetDef.ToString ());

            var size = GetImageSize (textureImporter);

            textureImporter.textureType = TextureImporterType.Default;
            textureImporter.normalmap = false;
            textureImporter.lightmap = false;
            textureImporter.maxTextureSize = 2048;
            textureImporter.generateCubemap = TextureImporterGenerateCubemap.None;
            textureImporter.npotScale = TextureImporterNPOTScale.None;
            textureImporter.isReadable = true;
            textureImporter.mipmapEnabled = false;

            // textureImporter.ClearPlatformTextureSettings ("Web");
            // textureImporter.ClearPlatformTextureSettings ("Standalone");
            // textureImporter.ClearPlatformTextureSettings ("iPhone");
            // textureImporter.ClearPlatformTextureSettings ("Android");

            var oldSpriteMetaDatas = textureImporter.spritesheet;
            var spriteMetaDatas = new List<SpriteMetaData> ();

            foreach (var key in sheetDef.Keys) {
                var spriteDef = sheetDef [key];
                var src = string.Format ("{0}/{1}", dirname, spriteDef ["src"].Value);
                if (path == src) {
                    var name = BaseUtils.GetFilenameWithoutExtension (key);

                    SpriteMetaData oldSpriteMetaData = default(SpriteMetaData);
                    bool hasOld = false;
                    if (oldSpriteMetaDatas != null) {
                        oldSpriteMetaData = oldSpriteMetaDatas.FirstOrDefault ((e) => e.name == name);
                        hasOld = true;
                    }

                    var spriteMetaData = new SpriteMetaData ();
                    spriteMetaData.name = name;
                    spriteMetaData.alignment = hasOld ? oldSpriteMetaData.alignment : (int)SpriteAlignment.Center;
                    spriteMetaData.border = hasOld ? oldSpriteMetaData.border : Vector4.zero;
                    spriteMetaData.pivot = hasOld ? oldSpriteMetaData.pivot : new Vector2 (0.5f, 0.5f);
                    spriteMetaData.rect = CalcUV (spriteDef ["uv"], size.x, size.y);

                    spriteMetaDatas.Add (spriteMetaData);
                }
            }

            if (spriteMetaDatas.Count > 0) {
                textureImporter.spriteImportMode = SpriteImportMode.Multiple;
                textureImporter.spritesheet = spriteMetaDatas.ToArray ();
            } else {
                textureImporter.spriteImportMode = SpriteImportMode.Single;
            }
        }

        static Rect CalcUV (JSONNode uv, float textureWidth, float textureHeight)
        {
            float x = uv [0].AsFloat * textureWidth;
            float y = textureHeight * (1f - (uv [1].AsFloat + uv [3].AsFloat));

            float w = uv [2].AsFloat * textureWidth;
            float h = uv [3].AsFloat * textureHeight;

            return new Rect (x, y, w, h);
        }

        static Vector2 GetImageSize (TextureImporter importer)
        {
            Vector2 size = Vector2.zero;

            if (importer != null) {
                object [] args = new object[2] { 0, 0 };
                MethodInfo mi = typeof(TextureImporter).GetMethod ("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke (importer, args);

                size.x = (int)args [0];
                size.y = (int)args [1];
            }

            return size;
        }

    }
}
