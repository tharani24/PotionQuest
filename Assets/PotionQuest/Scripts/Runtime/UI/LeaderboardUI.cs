using System.Collections.Generic;
using PotionQuest.Common;
using PotionQuest.Data;
using PotionQuest.Events;
using PotionQuest.FirebaseSetup;
using TMPro;
using UnityEngine;

namespace PotionQuest.UI
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private TMP_Text _leaderboardText;

        private void OnEnable()
        {
            EventManager.Subscribe(GlobalConstants.Event.LEADERBOARD_LOADED, UpdateLeaderboard);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe(GlobalConstants.Event.LEADERBOARD_LOADED, UpdateLeaderboard);
        }

        #region UICallback
        public void OnLeaderboardButtonClicked()
        {
            _root.SetActive(true);
            FirebaseManager.Instance.LoadLeaderboard();
        }

        public void OnCloseButtonClicked()
        {
            _root.SetActive(false);
        }
        #endregion
        
        private void UpdateLeaderboard(object data)
        {
            var entries = (List<ScoreData>)data;
            _leaderboardText.text = "";

            foreach (var entry in entries)
            {
                _leaderboardText.text += $"{entry.Score}\n";
            }
        }
    }
}