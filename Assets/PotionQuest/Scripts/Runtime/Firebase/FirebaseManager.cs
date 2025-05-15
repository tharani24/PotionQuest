using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Database;
using PotionQuest.Common;
using PotionQuest.Data;
using PotionQuest.Events;

namespace PotionQuest.FirebaseSetup
{
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseManager Instance;

        private FirebaseAuth _auth;
        private FirebaseUser _user;
        private DatabaseReference _dbReference;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            SetUserProperties();

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                if (task.Result != DependencyStatus.Available)
                {
                    Debug.LogError("Firebase initialization failed: " + task.Result);
                }
                else
                {
                    FirebaseApp app = FirebaseApp.DefaultInstance;
                    _auth = FirebaseAuth.DefaultInstance;

                    SignInAnonymously(); 
                }
            });
        }

        private void SignInAnonymously()
        {
            _auth.SignInAnonymouslyAsync().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully && !task.IsFaulted && !task.IsCanceled)
                {
                    Firebase.Auth.AuthResult result = task.Result;
                    _user = result.User;

                    _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
                    Debug.Log("Signed in anonymously as: " + _user.UserId);
                    Debug.Log("Firebase initialized successfully");
                }
                else
                {
                    Debug.LogError("Firebase anonymous sign-in failed: " + task.Exception);
                }
            });
        }
        
        private void SetUserProperties()
        {
            FirebaseAnalytics.SetUserProperty("player_level", "1");
            FirebaseAnalytics.SetUserProperty("session_count", "1");
            FirebaseAnalytics.SetUserProperty("last_active_date", DateTime.UtcNow.ToString("yyyy-MM-dd"));
            FirebaseAnalytics.SetUserProperty("preferred_potion_type", "Healing");
        }

        public void SyncScore(int currentScore)
        {
            if (_dbReference == null || _user == null)
            {
                Debug.LogError("Database reference or user is null.");
                return;
            }

            string sessionId = Guid.NewGuid().ToString();
            ScoreData data = new ScoreData(currentScore);
            string json = JsonUtility.ToJson(data);

            Debug.Log("Uploading JSON: " + json);

            EventManager.Fire(GlobalConstants.Event.FIREBASE_SYNC_STARTED, new { operationType = "Upload" });

            _dbReference.Child("users").Child(_user.UserId).Child("sessions").Child(sessionId)
                .SetRawJsonValueAsync(json)
                .ContinueWith(task =>
                {
                    bool success = task.IsCompletedSuccessfully && !task.IsFaulted;
                    Debug.Log(success ? "Score synced successfully." : "Failed to sync score: " + task.Exception);
                    EventManager.Fire(GlobalConstants.Event.FIREBASE_SYNC_COMPLETED, new { operationType = "Upload", success });
                });
        }

        public void SyncScore(int currentScore, DateTime startTime, DateTime endTime)
        {
            if (_dbReference == null || _user == null)
            {
                Debug.LogError("Database reference or user is null.");
                return;
            }

            string sessionId = Guid.NewGuid().ToString();

            _dbReference.Child("users").Child(_user.UserId).Child("sessions").Child(sessionId)
                .SetRawJsonValueAsync(JsonUtility.ToJson(new
                {
                    score = currentScore,
                    startTime = startTime.ToString("o"),
                    endTime = endTime.ToString("o")
                }));
        }

        public void LoadLeaderboard()
        {
            _dbReference.Child("scores").OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    List<int> topScores = new();
                    foreach (DataSnapshot child in snapshot.Children)
                    {
                        topScores.Add(int.Parse(child.Child("score").Value.ToString()));
                    }
                    EventManager.Fire(GlobalConstants.Event.LEADERBOARD_LOADED, topScores);
                }
            });
        }
    }
}
