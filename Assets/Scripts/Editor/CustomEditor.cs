using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(EnemyType))]
    public class EnemyTypeCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var enemyType = (EnemyType)target;
            enemyType.hasShield = EditorGUILayout.Toggle("Has Shield", enemyType.hasShield);

            if (enemyType.hasShield)
                enemyType.shieldLifePoints =
                    EditorGUILayout.IntField("Shield Life Points", enemyType.shieldLifePoints);
        }
    }
}