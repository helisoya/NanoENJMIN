using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(EnemyType))]
    public class EnemyTypeCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var enemyType = (EnemyType)target;
            
            DrawShield(enemyType);
            
            DrawProjectile(enemyType);
        }

        private void DrawShield(EnemyType enemyType)
        {
            enemyType.hasShield = EditorGUILayout.Toggle("Has Shield", enemyType.hasShield);

            if (enemyType.hasShield)
            {
                enemyType.shieldColour = (ColorTarget)EditorGUILayout.EnumPopup("Shield Colour", enemyType.shieldColour);
                
                enemyType.shieldLifePoints =
                    EditorGUILayout.IntField("Shield Life Points", enemyType.shieldLifePoints);
            }
        }

        private void DrawProjectile(EnemyType enemyType)
        {
            EditorGUILayout.Separator();
            enemyType.projectileType =
                (ProjectileType)EditorGUILayout.ObjectField("Projectile Type", enemyType.projectileType, typeof(ProjectileType), true);

            if (enemyType.projectileType != null)
            {
                enemyType.projectileColour = (ColorTarget)EditorGUILayout.EnumPopup("Projectile Colour", enemyType.projectileColour);
                
                enemyType.fireRate =
                    EditorGUILayout.FloatField("Fire Rate", enemyType.fireRate);
            }
        }
    }
}