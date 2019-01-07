using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameFramework
{
//    [CustomEditor(typeof(BlockObjectData))]
    public class BlockObjectDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("打开编辑器"))
            {
                BlockObjectSceneEditor.Instance.OpenEditorScene(target as BlockObjectData);
                GUIUtility.ExitGUI();
            }
        }
    }
}