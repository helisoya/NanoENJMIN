
using System;

namespace Editor
{
    using UnityEditor;
    using UnityEngine;
    
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EnemyTypeSO))]
    public class EnemyTypeCustomEditor : Editor
    {
        private SerializedProperty _hasShieldProperty;
        private SerializedProperty _shieldMaterialProperty;
        private SerializedProperty _shieldLifePointsProperty;
        private SerializedProperty _canFireProperty;
        private SerializedProperty _fireModeProperty;
        private SerializedProperty _projectileTypeSoProperty;
        private SerializedProperty _nbBurstProjectilesProperty;
        private SerializedProperty _burstProjectileAngleSpacingProperty;
        private SerializedProperty _targetingModeProperty;
        private SerializedProperty _fireAngleRangeProperty;
        private SerializedProperty _fireRateProperty;
        
        private void OnEnable()
        {
            _hasShieldProperty = serializedObject.FindProperty("hasShield");
            _shieldMaterialProperty = serializedObject.FindProperty("shieldMaterial");
            _shieldLifePointsProperty = serializedObject.FindProperty("shieldLifePoints");
            _canFireProperty = serializedObject.FindProperty("canFire");
            _fireModeProperty = serializedObject.FindProperty("fireMode");
            _projectileTypeSoProperty = serializedObject.FindProperty("projectileTypeSo");
            _nbBurstProjectilesProperty = serializedObject.FindProperty("nbBurstProjectiles");
            _burstProjectileAngleSpacingProperty = serializedObject.FindProperty("burstProjectileAngleSpacing");
            _targetingModeProperty = serializedObject.FindProperty("targetingMode");
            _fireAngleRangeProperty = serializedObject.FindProperty("fireAngleRange");
            _fireRateProperty = serializedObject.FindProperty("fireRate");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();
            
            var enemyType = (EnemyTypeSO)target;
            
            DrawShield(enemyType);
            
            DrawProjectile(enemyType);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawShield(EnemyTypeSO enemyTypeSo)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_hasShieldProperty, new GUIContent("Has Shield"));

            if (enemyTypeSo.hasShield)
            {
                EditorGUILayout.PropertyField(_shieldMaterialProperty, new GUIContent("Shield Material"));
                
                EditorGUILayout.PropertyField(_shieldLifePointsProperty, new GUIContent("Shield Life Points"));
            }
        }

        private void DrawProjectile(EnemyTypeSO enemyTypeSo)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(_canFireProperty, new GUIContent("Can Fire"));

            if (enemyTypeSo.canFire)
            {
                EditorGUILayout.PropertyField(_fireModeProperty, new GUIContent("Fire Mode"));

                EditorGUILayout.PropertyField(_projectileTypeSoProperty, new GUIContent("Projectile Type"));
                    
                if (enemyTypeSo.fireMode == FireMode.Homing)
                    return;
                
                if (enemyTypeSo.fireMode == FireMode.Burst)
                { 
                    EditorGUILayout.PropertyField(_nbBurstProjectilesProperty, new GUIContent("Nb Projectiles"));
                    EditorGUILayout.PropertyField(_burstProjectileAngleSpacingProperty, new GUIContent("Angle Spacing"));
                }

                EditorGUILayout.PropertyField(_targetingModeProperty, new GUIContent("Targeting Type"));

                EditorGUILayout.PropertyField(_fireAngleRangeProperty, new GUIContent("Fire Angle Range"));

                EditorGUILayout.PropertyField(_fireRateProperty, new GUIContent("Fire Rate"));
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