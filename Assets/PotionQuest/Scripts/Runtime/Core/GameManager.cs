using System.Collections.Generic;
using PotionQuest.Common;
using UnityEngine;
using PotionQuest.Events;
using PotionQuest.FirebaseSetup;
using PotionQuest.Potions;

namespace PotionQuest.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        private int _score;
        private Dictionary<string, int> _potionCounts = new();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            EventManager.Fire(GlobalConstants.Event.GAME_STARTED, System.DateTime.Now);
        }

        public void CollectPotion(PotionData data)
        {
            _score += data.Potency;
            EventManager.Fire(GlobalConstants.Event.POTION_COLLECTED, new {
                potionType = data.PotionName,
                value = data.Potency,
                timestamp = System.DateTime.Now
            });
            EventManager.Fire(GlobalConstants.Event.SCORE_UPDATED, _score);
            FirebaseManager.Instance.SyncScore(_score);
        }
        
        public void PauseGame()
        {
            Time.timeScale = 0f;
            EventManager.Fire(GlobalConstants.Event.GAME_PAUSED, System.DateTime.Now);
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            EventManager.Fire(GlobalConstants.Event.GAME_RESUMED, System.DateTime.Now);
        }

        public void EndGame()
        {
            EventManager.Fire(GlobalConstants.Event.GAME_ENDED, new { timestamp = System.DateTime.Now, totalScore = _score });
        }
    }
}