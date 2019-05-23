using System;
using System.Collections.Generic;
using Lean;
using UnityEngine;

// Modified from LeanPool
namespace LOT.Core
{
    public class SimplePool : MonoBehaviour
    {
        public class DelayedDestruction
        {
            public GameObject Clone;
            public float Life;
        }

        public enum NotificationType
        {
            None,
            SendMessage,
            BroadcastMessage
        }

        [Tooltip ("The prefab the clones will be based on")]
        public GameObject prefab;
        public string prefabPath;

        [Tooltip ("Should this pool preload some clones?")]
        public int preload;

        [Tooltip ("Should this pool have a maximum amount of spawnable clones?")]
        public int capacity;

        [Tooltip ("Should this pool send messages to the clones when they're spawned/despawned?")]
        public NotificationType notification = NotificationType.SendMessage;

        // All the currently cached prefab instances
        private List<GameObject> cache = new List<GameObject> ();

        // All the currently spawned prefab instances
        private List<GameObject> spawned = new List<GameObject> ();

        // All the delayed destruction objects
        private List<DelayedDestruction> delayedDestructions = new List<DelayedDestruction> ();

        // The total amount of created prefabs
        private int total;

        // The reference between a spawned GameObject and its pool
        public static Dictionary<string, SimplePool> AllPools = new Dictionary<string, SimplePool> ();

        public static SimplePool GetPool (string prefabPath, int preload = 0, bool persistent = false)
        {
            if (!AllPools.ContainsKey (prefabPath)) {
                var prefab = BaseUtils.LoadPrefab (prefabPath);
                if (prefab == null) {
                    Debug.LogError ("SimplePool::GetPool cannot load prefab " + prefabPath);
                    return null;
                }
                var go = new GameObject (prefabPath + " Pool");
                go.SetActive (false);
                var pool = go.AddComponent<SimplePool> ();
                pool.prefab = prefab;
                pool.preload = preload;
                pool.prefabPath = prefabPath;
                go.SetActive (true);

                if (persistent) {
                    pool.gameObject.AddComponent<DontDestroy> ();
                }

                AllPools [prefabPath] = pool;
                return pool;
            }

            return AllPools [prefabPath];
        }

        public static void DestroyPool (string prefabPath)
        {
            if (AllPools.ContainsKey (prefabPath)) {
                var pool = AllPools [prefabPath];
                AllPools.Remove (prefabPath);
                UnityEngine.Object.Destroy (pool.gameObject);
            }
        }

        public static GameObject Spawn (string prefabPath)
        {
            return Spawn (prefabPath, Vector3.zero, Quaternion.identity, null);
        }

        public static GameObject Spawn (string prefabPath, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var pool = GetPool (prefabPath);
            return pool.FastSpawn (position, rotation, parent);
        }

        public static void Despawn (GameObject clone, float delay = 0.0f)
        {
            if (clone != null) {
                foreach (var pool in AllPools.Values) {
                    if (pool.HasSpawned (clone)) {
                        pool.FastDespawn (clone, delay);
                        return;
                    }
                }

                Debug.LogError ("Attempting to despawn " + clone.name + ", but failed to find pool for it!");
            }
        }

        // Returns the total amount of spawned clones
        public int Total {
            get {
                return total;
            }
        }

        // Returns the amount of cached clones
        public int Cached {
            get {
                return cache.Count;
            }
        }

        public bool HasSpawned (GameObject clone)
        {
            return spawned.Contains (clone);
        }

        // This will return a clone from the cache, or create a new instance
        public GameObject FastSpawn (Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (prefab != null) {
                // Attempt to spawn from the cache
                while (cache.Count > 0) {
                    // Get last cache entry
                    var index = cache.Count - 1;
                    var clone = cache [index];

                    // Remove cache entry
                    cache.RemoveAt (index);

                    if (clone != null) {
                        spawned.Add (clone);

                        // Update transform of clone
                        var cloneTransform = clone.transform;
                        cloneTransform.localPosition = position;
                        cloneTransform.localRotation = rotation;
                        cloneTransform.SetParent (parent, false);

                        // Activate clone
                        clone.SetActive (true);

                        // Messages?
                        SendNotification (clone, "OnSpawn");

                        return clone;
                    } else {
                        Debug.LogError ("The " + name + " pool contained a null cache entry");
                    }
                }

                // Make a new clone?
                if (capacity <= 0 || total < capacity) {
                    var clone = FastClone (position, rotation, parent);
                    spawned.Add (clone);

                    // Messages?
                    SendNotification (clone, "OnSpawn");

                    return clone;
                }
            } else {
                Debug.LogError ("Attempting to spawn null");
            }

            return null;
        }

        // This will despawn a clone and add it to the cache
        public void FastDespawn (GameObject clone, float delay = 0.0f)
        {
            if (clone != null) {
                // Delay the despawn?
                if (delay > 0.0f) {
                    // Make sure we only add it to the marked object list once
                    if (delayedDestructions.Exists (m => m.Clone == clone) == false) {
                        var delayedDestruction = LeanClassPool<DelayedDestruction>.Spawn () ?? new DelayedDestruction ();

                        delayedDestruction.Clone = clone;
                        delayedDestruction.Life = delay;

                        delayedDestructions.Add (delayedDestruction);
                    }
                }
				// Despawn now?
				else {
                    // Add it to the cache
                    cache.Add (clone);
                    spawned.Remove (clone);

                    // Messages?
                    SendNotification (clone, "OnDespawn");

                    // Deactivate it
                    clone.SetActive (false);
                    clone.transform.SetParent (transform, false);
                }
            } else {
                Debug.LogWarning ("Attempting to despawn a null clone");
            }
        }

        // This allows you to make another clone and add it to the cache
        public void FastPreload ()
        {
            if (prefab != null) {
                // Create clone
                var clone = FastClone (Vector3.zero, Quaternion.identity, null);

                // Add it to the cache
                cache.Add (clone);

                // Deactivate it
                clone.SetActive (false);

                // Move it under this GO
                clone.transform.SetParent (transform, false);
            }
        }

        // Adds pool to list
        protected virtual void OnAwake ()
        {
            AllPools [prefabPath] = this;
        }

        // Remove pool from list
        protected virtual void OnDestroy ()
        {
            try {
                AllPools.Remove (prefabPath);
            } catch (Exception e) {
            }
        }

        // Update marked objects
        protected virtual void Update ()
        {
            // Go through all marked objects
            for (var i = delayedDestructions.Count - 1; i >= 0; i--) {
                var markedObject = delayedDestructions [i];

                // Is it still valid?
                if (markedObject.Clone != null) {
                    // Age it
                    markedObject.Life -= Time.deltaTime;

                    // Dead?
                    if (markedObject.Life <= 0.0f) {
                        RemoveDelayedDestruction (i);

                        // Despawn it
                        FastDespawn (markedObject.Clone);
                    }
                } else {
                    RemoveDelayedDestruction (i);
                }
            }
        }

        private void RemoveDelayedDestruction (int index)
        {
            var delayedDestruction = delayedDestructions [index];
            delayedDestructions.RemoveAt (index);
            LeanClassPool<DelayedDestruction>.Despawn (delayedDestruction);
        }

        // Makes sure the right amount of prefabs have been preloaded
        private void UpdatePreload ()
        {
            if (prefab != null) {
                for (var i = total; i < preload; i++) {
                    FastPreload ();
                }
            }
        }

        // Returns a clone of the prefab and increments the total
        // NOTE: Prefab is assumed to exist
        private GameObject FastClone (Vector3 position, Quaternion rotation, Transform parent)
        {
            var clone = (GameObject)Instantiate (prefab, position, rotation);
            total += 1;
            clone.name = prefab.name + " " + total;
            clone.transform.SetParent (parent, false);
            return clone;
        }

        // Sends messages to clones
        // NOTE: clone is assumed to exist
        private void SendNotification (GameObject clone, string messageName)
        {
            switch (notification) {
            case NotificationType.SendMessage:
                {
                    clone.SendMessage (messageName, SendMessageOptions.DontRequireReceiver);
                }
                break;

            case NotificationType.BroadcastMessage:
                {
                    clone.BroadcastMessage (messageName, SendMessageOptions.DontRequireReceiver);
                }
                break;
            }
        }
    }
}
