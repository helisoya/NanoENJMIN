using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SectionsManager))]
    public class SectionsManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Separator();
            if (GUILayout.Button("Separate"))
            {
                SectionsManager manager = (SectionsManager)target;
                float sectionSize = manager.GetSectionSize();

                for (int i = 0; i < manager.transform.childCount; i++)
                {
                    manager.transform.GetChild(i).position = new Vector3((i - 1) * sectionSize, 0, 0);
                }

            }
        }
    }
}

