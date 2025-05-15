using System;
using PotionQuest.Common;
using PotionQuest.Events;
using UnityEngine;

namespace PotionQuest.UI
{
    public class UIManager : MonoBehaviour 
    {
        private void OnEnable()
        {
            EventManager.Subscribe(GlobalConstants.Event.GAME_STARTED, OnGameStarted);
            EventManager.Subscribe(GlobalConstants.Event.GAME_ENDED, OnGameEnded);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe(GlobalConstants.Event.GAME_STARTED, OnGameStarted);
            EventManager.Unsubscribe(GlobalConstants.Event.GAME_ENDED, OnGameEnded);
        }

        private void OnGameStarted(object timestamp) => Debug.Log("Game started");
        private void OnGameEnded(object data) => Debug.Log("Game ended");
    }
}