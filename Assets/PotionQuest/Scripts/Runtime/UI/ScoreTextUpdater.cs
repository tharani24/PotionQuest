using PotionQuest.Common;
using UnityEngine;
using TMPro;
using PotionQuest.Events;

namespace PotionQuest.UI
{
    public class ScoreTextUpdater : MonoBehaviour
    {
        public TMP_Text scoreText;

        private void OnEnable()
        {
            EventManager.Subscribe(GlobalConstants.Event.SCORE_UPDATED, UpdateScore);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe(GlobalConstants.Event.SCORE_UPDATED, UpdateScore);
        }

        private void UpdateScore(object score)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}