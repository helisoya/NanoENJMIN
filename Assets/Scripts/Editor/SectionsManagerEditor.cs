using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SectionsManager))]
    public class SectionsManagerEditor : UnityEditor.Editor
    {
        private float sectionSize = 100.12f;
        private Transform objsRoot;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Separator();

            objsRoot = (Transform)EditorGUILayout.ObjectField("Objs root", objsRoot, typeof(Transform), true);
            sectionSize = EditorGUILayout.FloatField("Section size", sectionSize);

            EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Separate"))
            {
                SectionsManager manager = (SectionsManager)target;

                for (int i = 0; i < manager.transform.childCount; i++)
                {
                    manager.transform.GetChild(i).position = new Vector3((i - 1) * sectionSize, 0, 0);
                }
            }

            if (GUILayout.Button("Link objs"))
            {
                SectionsManager manager = (SectionsManager)target;

                List<Transform> childs = new List<Transform>();
                if (objsRoot)
                {
                    for (int i = 0; i < objsRoot.childCount; i++)
                    {
                        childs.Add(objsRoot.GetChild(i));
                    }
                }

                foreach (Transform child in childs)
                {
                    int index = Mathf.CeilToInt(child.transform.position.x / sectionSize);
                    Transform parent = manager.transform.GetChild(index);
                    child.parent = parent;
                }
            }

            if (GUILayout.Button("Generate LOD"))
            {
                SectionsManager manager = (SectionsManager)target;

                foreach (Transform child in manager.transform)
                {
                    LODGroup group = child.GetComponent<LODGroup>();
                    group.GetLODs()[0].renderers = child.GetComponentsInChildren<Renderer>();
                }
            }

            if (GUILayout.Button("Reset LOD"))
            {
                SectionsManager manager = (SectionsManager)target;

                foreach (Transform child in manager.transform)
                {
                    LODGroup group = child.GetComponent<LODGroup>();
                    group.GetLODs()[0].renderers = new Renderer[0];
                }
            }
        }
    }
}

