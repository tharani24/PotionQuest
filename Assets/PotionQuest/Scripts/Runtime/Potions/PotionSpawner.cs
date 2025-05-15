using System.Collections;
using PotionQuest.Common;
using UnityEngine;
using PotionQuest.Events;
using PotionQuest.Utilities;

namespace PotionQuest.Potions
{
    public class PotionSpawner : MonoBehaviour
    {
        [SerializeField] private PotionData[] _potions;
        [SerializeField] private float _spawnInterval = 3f;
        
        private bool _poolReady = false;

        private void Start()
        {
            StartCoroutine(InitPoolsAndSpawn());
        }

        private IEnumerator InitPoolsAndSpawn()
        {
            int remaining = _potions.Length;

            foreach (var data in _potions)
            {
                PoolManager.Instance.CreatePoolAsync(data, 5, () => { remaining--; });
            }

            while (remaining > 0)
                yield return null;

            _poolReady = true;
            StartCoroutine(SpawnPotions());
        }

        private IEnumerator SpawnPotions()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                var data = _potions[Random.Range(0, _potions.Length)];
                Vector3 pos = new(Random.Range(-5, 5), 1, Random.Range(-5, 5));

                GameObject obj = PoolManager.Instance.Spawn(data.PotionName, pos, Quaternion.identity);

                if (obj != null)
                {
                    Potion potion = obj.GetComponent<Potion>();
                    potion.SetData(data);
                    EventManager.Fire(GlobalConstants.Event.POTION_SPAWNED, new { potionName = data.PotionName, pos });
                }
            }
        }
    }
}