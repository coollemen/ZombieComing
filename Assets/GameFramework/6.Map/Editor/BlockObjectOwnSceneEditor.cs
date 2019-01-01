using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework
{
    public class BlockObjectOwnSceneEditor :OwnSceneEditorSingleton<BlockObjectOwnSceneEditor>
    {
        protected BlockObjectOwnSceneEditor()
        {
            
        }
        public override void OnEnterEditorScene()
        {
            var go = new GameObject("Block Object RTE");
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            var bo = go.AddComponent<BlockObjectRTE>();
            bo.data = (BlockObjectData)this.data;
            Selection.activeGameObject = go;
        }
        public override void OnLeaveEditorScene()
        {
            base.OnLeaveEditorScene();
        }
        public override void OnSceneViewGUI(SceneView view)
        {
            Handles.BeginGUI();
            var hrect = EditorGUILayout.BeginHorizontal();
            GUILayout.Label("图块物体编辑器");
            if (GUILayout.Button("返回"))
            {

            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            Handles.EndGUI();
        }
    }
}