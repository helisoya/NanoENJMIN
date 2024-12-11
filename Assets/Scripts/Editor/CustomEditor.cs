
using System;

namespace Editor
{
    using UnityEditor;
    using UnityEngine;
    
    [CustomEditor(typeof(EnemyTypeSO))]
    public class EnemyTypeCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var enemyType = (EnemyTypeSO)target;
            
            DrawShield(enemyType);
            
            DrawProjectile(enemyType);

            serializedObject.ApplyModifiedProperties();
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
                enemyTypeSo.fireMode = (FireMode)EditorGUILayout.EnumPopup("Fire Mode", enemyTypeSo.fireMode);

                enemyTypeSo.projectileTypeSo = (ProjectileTypeSO)EditorGUILayout.ObjectField("Projectile Type", enemyTypeSo.projectileTypeSo, typeof(ProjectileTypeSO), false);
                    
                enemyTypeSo.fireMode = (FireMode)EditorGUILayout.EnumPopup("Fire Mode", enemyTypeSo.fireMode);

                if (enemyTypeSo.fireMode == FireMode.Homing)
                    return;
                
                if (enemyTypeSo.fireMode == FireMode.Burst)
                { 
                    enemyTypeSo.nbBurstProjectiles = 
                        EditorGUILayout.IntField("Nb Projectiles", enemyTypeSo.nbBurstProjectiles);
                    enemyTypeSo.burstProjectileAngleSpacing =
                        EditorGUILayout.FloatField("Angle Spacing", enemyTypeSo.burstProjectileAngleSpacing);
                }

                enemyTypeSo.targetingMode =
                    (TargetingMode)EditorGUILayout.EnumPopup("Targeting Type", enemyTypeSo.targetingMode);

                enemyTypeSo.fireAngleRange =
                    EditorGUILayout.FloatField("Fire Angle Range", enemyTypeSo.fireAngleRange);

                enemyTypeSo.fireRate =
                    EditorGUILayout.FloatField("Fire Rate", enemyTypeSo.fireRate);
            }
        }
    }

    
#if UNITY_EDITOR
    [CustomEditor(typeof(CustomPatternTest))]
    public class GridHolderEditor : Editor
    {
        SerializedProperty grid;
        SerializedProperty array;
 
        int length;
 
        private void OnEnable()
        {
            grid = serializedObject.FindProperty("grid");
            length = Enum.GetValues(typeof(Elements)).Length;
        }
 
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
 
            CustomPatternTest script = (CustomPatternTest)target;
 
            DrawGrid();
 
            if (GUILayout.Button("Reset"))
                script.ResetGrid();
 
            serializedObject.ApplyModifiedProperties();
        }
 
        private void DrawGrid()
        {
            try
            {
                GUILayout.BeginVertical();
                for (int i = 0; i < CustomPatternTest.size; i++)
                {
                    GUILayout.BeginHorizontal();
                    array = grid.GetArrayElementAtIndex(i).FindPropertyRelative("values");
                    for (int j = 0; j < CustomPatternTest.size; j++)
                    {
                        var value = array.GetArrayElementAtIndex(j);
                        Elements element = (Elements)value.intValue;
                        if (GUILayout.Button(element.ToString(), GUILayout.MaxWidth(100), GUILayout.MaxHeight(50)))
                        {
                            value.intValue = NextIndex(value.intValue);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
 
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }
 
        private int NextIndex(int index)
        {
            int result = ++index % length;
            return result;
        }
    }
#endif
}