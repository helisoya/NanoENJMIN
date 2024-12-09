using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(EnemyTypeSO))]
    public class EnemyTypeCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var enemyType = (EnemyTypeSO)target;
            
            DrawShield(enemyType);
            
            DrawProjectile(enemyType);
        }

        private void DrawShield(EnemyTypeSO enemyTypeSo)
        {
            EditorGUILayout.Separator();
            enemyTypeSo.hasShield = EditorGUILayout.Toggle("Has Shield", enemyTypeSo.hasShield);

            if (enemyTypeSo.hasShield)
            {
                enemyTypeSo.shieldMaterial = (Material)EditorGUILayout.ObjectField("Shield Material", enemyTypeSo.shieldMaterial, typeof(Material), false);
                
                enemyTypeSo.shieldLifePoints =
                    EditorGUILayout.IntField("Shield Life Points", enemyTypeSo.shieldLifePoints);
            }
        }

        private void DrawProjectile(EnemyTypeSO enemyTypeSo)
        {
            EditorGUILayout.Separator();
            enemyTypeSo.canFire = EditorGUILayout.Toggle("Can Fire", enemyTypeSo.canFire);

            if (enemyTypeSo.canFire)
            {
                enemyTypeSo.projectileTypeSo =
                    (ProjectileTypeSO)EditorGUILayout.ObjectField("Projectile Type", enemyTypeSo.projectileTypeSo, typeof(ProjectileTypeSO), false);
                enemyTypeSo.fireRate =
                    EditorGUILayout.FloatField("Fire Rate", enemyTypeSo.fireRate);
            }
        }
    }
}