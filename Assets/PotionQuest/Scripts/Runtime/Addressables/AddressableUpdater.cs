using PotionQuest.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PotionQuest.AddressablesHelper
{
    public class AddressableUpdater : MonoBehaviour 
    {
        public void ReloadPotion(string key, string potionName)
        {
            Addressables.LoadAssetAsync<GameObject>(key).Completed += handle => {
                if (handle.Status == AsyncOperationStatus.Succeeded) {
                    PoolManager.Instance.UpdatePrefab(potionName, handle.Result);
                }
            };
        }
    }
}