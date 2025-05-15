namespace PotionQuest.Data
{
    [System.Serializable]
    public class ScoreData
    {
        public int Score;
        public string Timestamp;

        public ScoreData(int score)
        {
            this.Score = score;
            this.Timestamp = System.DateTime.UtcNow.ToString("o");
        }
    }

}