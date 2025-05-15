using UnityEngine;

namespace PotionQuest.Potions
{
    [CreateAssetMenu(fileName = "Potion", menuName = "Data/PotionData")]
    public class PotionData : ScriptableObject
    {
        public string PotionName;
        public int Potency;
        public Sprite Icon;
        public string Description;

        [Header("Addressable Settings")]
        public string AddressableKey;
    }
}