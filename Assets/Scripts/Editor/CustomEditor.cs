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
                enemyType.shieldMaterial = (Material)EditorGUILayout.ObjectField("Shield Material", enemyType.shieldMaterial, typeof(Material), false);
                
                enemyType.shieldLifePoints =
                    EditorGUILayout.IntField("Shield Life Points", enemyType.shieldLifePoints);
            }
        }

        private void DrawProjectile(EnemyType enemyType)
        {
            EditorGUILayout.Separator();
            enemyType.projectileType =
                (ProjectileType)EditorGUILayout.ObjectField("Projectile Type", enemyType.projectileType, typeof(ProjectileType), false);

            if (enemyType.projectileType != null)
            {
                enemyType.fireRate =
                    EditorGUILayout.FloatField("Fire Rate", enemyType.fireRate);
            }
        }
    }
}