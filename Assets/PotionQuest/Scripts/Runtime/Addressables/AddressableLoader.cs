using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

namespace PotionQuest.AddressablesHelper
{
    public static class AddressableLoader
    {
        public static void LoadAsset<T>(string key, Action<T> onLoaded) where T : UnityEngine.Object
        {
            Addressables.LoadAssetAsync<T>(key).Completed += (AsyncOperationHandle<T> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    onLoaded?.Invoke(handle.Result);
                }
                else
                {
                    Debug.LogError($"Failed to load Addressable asset: {key}");
                }
            };
        }

        public static void Instantiate(string key, Vector3 position, Quaternion rotation, Transform parent = null, Action<GameObject> onComplete = null)
        {
            Addressables.InstantiateAsync(key, position, rotation, parent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    onComplete?.Invoke(handle.Result);
                }
                else
                {
                    Debug.LogError($"Failed to instantiate Addressable asset: {key}");
                }
            };
        }
        
        public static void Release<T>(T obj) where T : UnityEngine.Object
        {
            Addressables.Release(obj);
        }
    }
}