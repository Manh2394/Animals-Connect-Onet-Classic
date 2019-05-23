using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Enum = System.Enum;
using System.Linq;
using SimpleJSON;

namespace LOT.Extensions
{
    public static class ClassExtensions
    {
        private const string INDENT_STRING = "    ";
        private const double Epsilon = 1e-10;

        private static Dictionary<object, Guid> objectIds = new Dictionary<object, Guid>();

        public static string CamelCase2Underscore(this string str)
        {
            string result = Regex.Replace(str, "(?<=.)([A-Z])", "_$0", RegexOptions.Singleline);
            // Debug.Log(str + " -> " + result.ToLower());
            return result.ToLower();
        }

        public static string UnderscoreToCamel(this string str)
        {
            string result = "";
            str.Split('_').ToList().ForEach(substr =>
            {
                result += substr.First().ToString().ToUpper() + substr.Substring(1);
            });
            return result.First().ToString().ToLower() + result.Substring(1); ;
        }

        public static int ToAnimatorHash(this string str)
        {
            return Animator.StringToHash(str);
        }

        public static Guid GetUniqueId(this object obj)
        {
            if (objectIds.ContainsKey(obj))
            {
                return objectIds[obj];
            }
            Guid id = Guid.NewGuid();
            objectIds[obj] = id;
            return id;
        }

        // public static string ToString (this BaseUnitStates state)
        // {
        //     return Enum.GetName (typeof(BaseUnitStates), state);
        // }

        public static GameObject FindChildWithTag(this GameObject gameObject, string tag)
        {
            GameObject result = null;
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.CompareTag(tag))
                {
                    result = child.gameObject;
                }
                else
                {
                    result = child.gameObject.FindChildWithTag(tag);
                }
                if (result != null)
                {
                    return result;
                }
            }
            return result;
        }

        public static List<GameObject> FindChildrenWithTag(this GameObject gameObject, string tag)
        {
            List<GameObject> result = new List<GameObject>();
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.CompareTag(tag))
                {
                    result.Add(child.gameObject);
                }
                result.AddRange(child.gameObject.FindChildrenWithTag(tag));
            }
            return result;
        }

        public static MemoryStream ToStream(this string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static MemoryStream ToStreamUTF8(this string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str ?? ""));
        }

        public static string GetFileNameOnly(this FileInfo file)
        {
            return Path.GetFileNameWithoutExtension(file.Name);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> func)
        {
            foreach (var item in collection)
            {
                func(item);
            }
        }

        public static string Concat(this IEnumerable<string> collection)
        {
            string result = "";
            foreach (var item in collection)
            {
                result += item;
            }
            return result;
        }

        public static string ToStringFormat(this JSONNode node)
        {
            string json = "";
            if (node is JSONClass)
            {
                json = node.AsObject.ToString();
            }
            else if (node is JSONArray)
            {
                json = node.AsArray.ToString();
            }

            int quoteCount = 0;
            int indentation = 0;
            int separator = 0;
            var result = from ch in json
                         let quotes = ch == '"' ? quoteCount++ : quoteCount
                         let lineBreak = ch == ',' && quotes % 2 == 0 ? ch + Environment.NewLine + Enumerable.Repeat(INDENT_STRING, indentation).Concat() : null
                         let propChar = ch == ':' ? separator++ : separator > 0 ? separator-- : separator
                         let openChar = ch == '{' || ch == '[' ? (propChar != 0 ? Environment.NewLine + Enumerable.Repeat(INDENT_STRING, indentation).Concat() : "") + ch + Environment.NewLine + Enumerable.Repeat(INDENT_STRING, ++indentation).Concat() : ch.ToString()
                         let closeChar = ch == '}' || ch == ']' ? Environment.NewLine + Enumerable.Repeat(INDENT_STRING, --indentation).Concat() + ch : (propChar != 0 ? " " : "") + ch.ToString()
                         select lineBreak == null
                                 ? openChar.Length > 1
                                     ? openChar
                                     : closeChar
                                 : lineBreak;

            return result.Concat();
        }

        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < Epsilon;
        }

        public static bool IsZero(this float d)
        {
            return Math.Abs(d) < Epsilon;
        }

        #region Vectors

        public static Vector2 Clamp(this Vector2 vec, Vector2 min, Vector2 max)
        {
            vec.x = Mathf.Clamp(vec.x, min.x, max.x);
            vec.y = Mathf.Clamp(vec.y, min.y, max.y);
            return vec;
        }

        public static Vector3 Clamp(this Vector3 vec, Vector3 min, Vector3 max)
        {
            vec.x = Mathf.Clamp(vec.x, min.x, max.x);
            vec.y = Mathf.Clamp(vec.y, min.y, max.y);
            vec.z = Mathf.Clamp(vec.z, min.z, max.z);
            return vec;
        }

        public static Vector3 ToVector3(this Vector2 vec, float z = 0)
        {
            return new Vector3(vec.x, vec.y, z);
        }

        public static float Cross(this Vector2 u, Vector2 v)
        {
            return u.x * v.y - u.y * v.x;
        }

        public static float Dot(this Vector2 u, Vector2 v)
        {
            return Vector2.Dot(u, v);
        }

        #endregion
    }
}
