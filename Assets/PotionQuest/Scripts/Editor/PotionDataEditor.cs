using UnityEditor;
using UnityEngine;
using PotionQuest.Potions;

namespace PotionQuest.EditorScripts
{
    [CustomEditor(typeof(PotionData))]
    public class PotionDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var data = (PotionData)target;

            data.PotionName = EditorGUILayout.TextField("Potion Name", data.PotionName);
            data.Potency = EditorGUILayout.IntSlider("Potency", data.Potency, 1, 100);
            data.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", data.Icon, typeof(Sprite), false);
            data.Description = EditorGUILayout.TextArea(data.Description, GUILayout.Height(60));
            data.AddressableKey = EditorGUILayout.TextField("Addressable Key", data.AddressableKey);

            if (GUI.changed)
                EditorUtility.SetDirty(data);
        }
    }
}