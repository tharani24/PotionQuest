namespace PotionQuest.Common
{
    public class GlobalConstants
    {
        public struct Event
        {
            public const string GAME_STARTED = "GameStarted";
            public const string POTION_COLLECTED = "PotionCollected";
            public const string SCORE_UPDATED = "ScoreUpdated";
            public const string GAME_ENDED = "GameEnded";
            public const string FIREBASE_SYNC_COMPLETED = "FirebaseSyncCompleted";
            public const string POTION_SPAWNED = "PotionSpawned";
            
            public const string GAME_PAUSED = "GamePaused";
            public const string GAME_RESUMED = "GameResumed";
            public const string LEADERBOARD_LOADED = "LeaderboardLoaded";
            public const string FIREBASE_SYNC_STARTED = "FirebaseSyncStarted";

        }
    }
}