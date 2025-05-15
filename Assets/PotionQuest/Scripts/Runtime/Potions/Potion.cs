    using UnityEngine;
    using PotionQuest.Core;
    using PotionQuest.Utilities;

    namespace PotionQuest.Potions
{
    public class Potion : MonoBehaviour
    {
        private PotionData _data;
        private float _lifetime = 5f;

        private void OnEnable()
        {
            Invoke(nameof(Deactivate), _lifetime);
        }

        private void OnMouseDown()
        {
            GameManager.Instance.CollectPotion(_data);
            Deactivate();
        }

        private void Deactivate()
        {
            if(_data == null)
                return;
            
            CancelInvoke();
            PoolManager.Instance.ReturnToPool(_data.PotionName, gameObject);
        }
        
        public void SetData(PotionData potionData) => _data = potionData;
    }
}