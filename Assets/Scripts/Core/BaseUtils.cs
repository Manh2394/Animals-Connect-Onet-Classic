using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using Assets.Phunk.Core;
using System;
using LOT.Extensions;
using LOT.Core;

namespace LOT.Core
{
    public class BaseUtils
    {
        private static Dictionary<string, GameObject> preloadedPrefabs = new Dictionary<string, GameObject> ();

        public static void PreloadPrefabs ()
        {
            PreloadPrefabsByPath (BasePath.GetBattleEffectPath (""));
            PreloadPrefabsByPath (BasePath.GetProjectilePath (""));
        }

        public static void UnloadPrefabs ()
        {
            preloadedPrefabs.Clear ();
        }

        private static void PreloadPrefabsByPath (string path)
        {
            var prefabs = Resources.LoadAll<GameObject> (path);
            foreach (var prefab in prefabs) {
                Debug.Log (path + prefab.name);
                preloadedPrefabs [path + prefab.name] = prefab;
            }
        }

        public static GameObject Instantiate (string path, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
        {
            return UnityEngine.Object.Instantiate (BaseUtils.LoadPrefab (path), position, rotation) as GameObject;
        }

        public static GameObject LoadPrefab (string path)
        {
            return preloadedPrefabs.ContainsKey (path) ? preloadedPrefabs [path] : LoadResource (path) as GameObject;
        }

        public static GameObject FastLoadPrefab (string path)
        {
            return LoadResource ("Prefabs/" + path) as GameObject;
        }

        public static GameObject LoadCommonPrefab (string path)
        {
            return LoadResource ("lang/common/Prefabs/" + path) as GameObject;
        }

        public static GameObject SpawnPrefabByName (string name)
        {
            var go = SimplePool.Spawn (name);
            if (go == null) {
                Log.ErrorFormat ("BaseUtils::SpawnPrefabByName prefab does not exist: {0}", name);
                return null;
            }
            return go;
        }

        public static UnityEngine.Object LoadResource (string path)
        {
            return Resources.Load (path);
        }

        public static JSONNode LoadJSONResource (string path)
        {
            TextAsset text = BaseUtils.LoadResource (path) as TextAsset;
            var data = JSONNode.Parse (text.text);
            return data;
        }

        public static JSONNode LoadMockJSON (string path)
        {
            TextAsset text = BaseUtils.LoadResource (BasePath.GetJSONPath (path)) as TextAsset;
            var data = JSONNode.Parse (text.text);
            return data;
        }

        public static IEnumerator AsyncPrint (string m)
        {
            Log.Verbose (m);
            yield return null;
        }

        public static IEnumerator WaitForSeconds (float seconds)
        {
            yield return new WaitForSeconds (seconds);
        }

        public static bool IsAnimStateStartWith (AnimatorStateInfo stateInfo, string name)
        {
            for (int i = 0; i < 1000; i++) {
                if (stateInfo.IsName (name + i)) {
                    return true;
                }
            }
            return false;
        }

        public static JSONNode GetFakeEnemyData (string enemyId)
        {
            return LoadMockJSON (enemyId);
        }

        public static JSONNode GetFakePlayerCombatData (string cardId)
        {
            return LoadMockJSON (cardId);
        }

        public static JSONNode GetFakeBattleData ()
        {
            return LoadMockJSON ("mock_battle");
        }

        public static GameObject GetChildRecursive (GameObject go, string name)
        {
            Component [] transforms = go.GetComponentsInChildren (typeof(Transform), true);

            foreach (Transform transform in transforms) {
                if (transform.gameObject.name == name) {
                    return transform.gameObject;
                }
            }

            return null;
        }

        public static bool IsHitBox (Collider2D col)
        {
            return col.tag.Contains ("HitBox");
        }

        public static bool IsHurtBox (Collider2D col)
        {
            return col.tag.Contains ("HurtBox");
        }

        public static List<JSONNode> JSONArrayToList (JSONNode data)
        {
            var res = new List<JSONNode> ();
            foreach (JSONNode node in data.AsArray) {
                res.Add (node);
            }
            return res;
        }


        public static string PascalToCamel (string str)
        {
            if (str == null)
                return str;

            return str.Substring (0, 1).ToLower () + str.Substring (1);
        }

        public struct LineSegment2D
        {
            public Vector2 p1, p2;
        }

        /// <summary>
        /// Test whether two line segments intersect. If so, calculate the intersection point.
        /// <see cref="http://stackoverflow.com/a/14143738/292237"/>
        /// </summary>
        /// <param name="p">Vector to the start point of p.</param>
        /// <param name="p2">Vector to the end point of p.</param>
        /// <param name="q">Vector to the start point of q.</param>
        /// <param name="q2">Vector to the end point of q.</param>
        /// <param name="intersection">The point of intersection, if any.</param>
        /// <param name="considerOverlapAsIntersect">Do we consider overlapping lines as intersecting?
        /// </param>
        /// <returns>True if an intersection point was found.</returns>
        public static bool LineSegmentIntersect (Vector2 p, Vector2 p2, Vector2 q, Vector2 q2, out Vector2 intersection, bool considerCollinearOverlapAsIntersect = false)
        {
            intersection = Vector2.zero;

            var r = p2 - p;
            var s = q2 - q;
            var rxs = r.Cross (s);
            var qpxr = (q - p).Cross (r);

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero () && qpxr.IsZero ()) {
                // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
                // then the two lines are overlapping,
                if (considerCollinearOverlapAsIntersect)
                if ((0 <= (q - p).Dot (r) && (q - p).Dot (r) <= r.Dot (r))
                    || (0 <= (p - q).Dot (s) && (p - q).Dot (s) <= s.Dot (s)))
                    return false;

                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
                // then the two lines are collinear but disjoint.
                // No need to implement this expression, as it follows from the expression above.
                return false;
            }

            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (rxs.IsZero () && !qpxr.IsZero ()) {
                return false;
            }

            // t = (q - p) x s / (r x s)
            var t = (q - p).Cross (s) / rxs;
            // u = (q - p) x r / (r x s)
            var u = (q - p).Cross (r) / rxs;

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
            // the two line segments meet at the point p + t r = q + u s.
            if (!rxs.IsZero () && (0 <= t && t <= 1) && (0 <= u && u <= 1)) {
                // We can calculate the intersection point using either t or u.
                intersection = p + t * r;
                // An intersection was found.
                return true;
            }

            return false;
        }

        public static bool LineSegmentIntersect (LineSegment2D line1, LineSegment2D line2, out Vector2 intersection, bool considerCollinearOverlapAsIntersect = false)
        {
            return LineSegmentIntersect (line1.p1, line1.p2, line2.p1, line2.p2, out intersection, considerCollinearOverlapAsIntersect);
        }

        public static bool DetectCollisionPoint (Collider2D col, Collider2D otherCol, out Vector2 collisionPoint)
        {
            // Assume that the function is only called on collided
            // -> one might be inside the other.
            // So set this case as default
            collisionPoint = otherCol.bounds.center;
            // Debug.DrawLine(Vector2.zero, otherCol.bounds.center, Color.red, 10);

            var colEdges = GetColliderEdges (col);
            var otherColEdges = GetColliderEdges (otherCol);
            if (colEdges != null && otherColEdges != null) {
                foreach (var colEdge in colEdges) {
                    foreach (var otherColEdge in otherColEdges) {
                        // Found collision point on the borders
                        if (LineSegmentIntersect (colEdge, otherColEdge, out collisionPoint, true)) {
                            // Debug.LogFormat("{0}{1} - {2}{3}", colEdge.p1, colEdge.p2, otherColEdge.p1, otherColEdge.p2);
                            // Debug.DrawLine(colEdge.p1, colEdge.p2, Color.red, 10);
                            // Debug.DrawLine(otherColEdge.p1, otherColEdge.p2, Color.red, 10);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // Get all edged of a Collider2D as a list of LineSegment2Ds
        private static List<LineSegment2D> GetColliderEdges (Collider2D col)
        {
            if (col is CircleCollider2D)
                return null;
            var edges = new List<LineSegment2D> ();
            if (col is BoxCollider2D) {
                BoxCollider2D box = (BoxCollider2D)col;
                Transform bcTransform = box.transform;

                // The collider's centre point in the world
                Vector3 worldPosition = bcTransform.TransformPoint (0, 0, 0);

                // The collider's local width and height, accounting for scale, divided by 2
                Vector2 size = new Vector2 (box.size.x * bcTransform.localScale.x * 0.5f, box.size.y * bcTransform.localScale.y * 0.5f);

                // STEP 1: FIND LOCAL, UN-ROTATED CORNERS
                // Find the 4 corners of the BoxCollider2D in LOCAL space, if the BoxCollider2D had never been rotated
                Vector3 corner1 = new Vector2 (-size.x, -size.y);
                Vector3 corner2 = new Vector2 (-size.x, size.y);
                Vector3 corner3 = new Vector2 (size.x, size.y);
                Vector3 corner4 = new Vector2 (size.x, -size.y);

                // STEP 2: ROTATE CORNERS
                // Rotate those 4 corners around the centre of the collider to match its transform.rotation
                corner1 = RotatePointAroundPivot (corner1, Vector3.zero, bcTransform.eulerAngles);
                corner2 = RotatePointAroundPivot (corner2, Vector3.zero, bcTransform.eulerAngles);
                corner3 = RotatePointAroundPivot (corner3, Vector3.zero, bcTransform.eulerAngles);
                corner4 = RotatePointAroundPivot (corner4, Vector3.zero, bcTransform.eulerAngles);

                // STEP 3: FIND WORLD POSITION OF CORNERS
                // Add the 4 rotated corners above to our centre position in WORLD space - and we're done!
                corner1 = worldPosition + corner1;
                corner2 = worldPosition + corner2;
                corner3 = worldPosition + corner3;
                corner4 = worldPosition + corner4;
                edges.Add (new LineSegment2D { p1 = corner1, p2 = corner2 });
                edges.Add (new LineSegment2D { p1 = corner2, p2 = corner3 });
                edges.Add (new LineSegment2D { p1 = corner3, p2 = corner4 });
                edges.Add (new LineSegment2D { p1 = corner4, p2 = corner1 });
            } else {    // PolygonCollider2D or EdgeCollider2D
                var points = ((PolygonCollider2D)col).points;
                var trans = col.transform;
                // if (col is EdgeCollider2D) {
                //     points = ((EdgeCollider2D) col).points;
                // }
                for (var i = 0; i < points.Length - 1; i++) {
                    edges.Add (new LineSegment2D { p1 = trans.TransformPoint (points [i]), p2 = trans.TransformPoint (points [i + 1]) });
                }
                if (col is PolygonCollider2D) {
                    edges.Add (new LineSegment2D { p1 = trans.TransformPoint (points [points.Length - 1]), p2 = trans.TransformPoint (points [0]) });
                }
            }
            return edges;
        }

        private static Vector3 RotatePointAroundPivot (Vector3 point, Vector3 pivot, Vector3 angles)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler (angles) * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }

        // Copy from http://stackoverflow.com/questions/829174/is-there-an-easy-way-to-turn-an-int-into-an-array-of-ints-of-each-digit
        public static int[] NumbersIn (int value)
        {
            var numbers = new Stack<int> ();

            for (; value > 0; value /= 10)
                numbers.Push (value % 10);

            return numbers.ToArray ();
        }

        public static string GetFilenameWithoutExtension (string path)
        {
            int indexStart = path.LastIndexOf ('/') + 1;
            int indexEnd = path.LastIndexOf ('.');
            if (indexEnd < 0) {
                indexEnd = path.Length;
            }

            if (indexStart > 0 && indexEnd >= 0) {
                return path.Substring (indexStart, indexEnd - indexStart);
            } else {
                return path.Substring (0, indexEnd);
            }
        }

        public static string GetDirname (string path)
        {
            int index = path.LastIndexOf ('/');
            if (index >= 0) {
                return path.Substring (0, index);
            } else {
                return path;
            }
        }


        public static string GetDamageColorName (Color color)
        {
            if (color == Color.blue) {
                return "blue";
            } else if (color == Color.yellow) {
                return "yellow";
            } else if (color == Color.green) {
                return "green";
            } else if (color == Color.red) {
                return "red";
            } else if (color == Color.magenta) {
                return "magenta";
            }
            return "blue";
        }

    }

}
