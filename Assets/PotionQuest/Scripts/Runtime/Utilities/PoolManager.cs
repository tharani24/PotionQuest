using System;
using System.Collections.Generic;
using UnityEngine;
using PotionQuest.AddressablesHelper;
using PotionQuest.Potions;

namespace PotionQuest.Utilities
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;

        private Dictionary<string, Queue<GameObject>> poolDict = new();
        private Dictionary<string, GameObject> prefabCache = new();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void CreatePoolAsync(PotionData data, int count, Action onComplete = null)
        {
            if (poolDict.ContainsKey(data.PotionName))
            {
                onComplete?.Invoke();
                return;
            }

            poolDict[data.PotionName] = new Queue<GameObject>();

            AddressableLoader.LoadAsset<GameObject>(data.AddressableKey, (prefab) =>
            {
                prefabCache[data.PotionName] = prefab;

                for (int i = 0; i < count; i++)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);
                    poolDict[data.PotionName].Enqueue(obj);
                }

                onComplete?.Invoke();
            });
        }

        public GameObject Spawn(string potionName, Vector3 pos, Quaternion rot)
        {
            if (!poolDict.ContainsKey(potionName) || poolDict[potionName].Count == 0)
            {
                Debug.LogWarning($"Pool for {potionName} is empty or not created.");
                return null;
            }

            GameObject obj = poolDict[potionName].Dequeue();
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.SetActive(true);
            return obj;
        }

        public void ReturnToPool(string potionName, GameObject obj)
        {
            obj.SetActive(false);
            poolDict[potionName].Enqueue(obj);
        }
        
        public void UpdatePrefab(string potionName, GameObject newPrefab)
        {
            if (!prefabCache.ContainsKey(potionName)) return;
                prefabCache[potionName] = newPrefab;
        }
    }
}
