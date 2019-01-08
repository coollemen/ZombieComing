using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    public class BlockObjectSceneEditor :Singleton<BlockObjectSceneEditor>
    {
        public string mainScenePath;
        public BlockObjectData data;
        public GameObject go;
        public bool isOpen ;

        private BlockObjectSceneEditor()
        {
            mainScenePath = "";
            isOpen = false;
        }
        /// <summary>
        /// 打开编辑器的场景
        /// </summary>
        public void OpenEditorScene(BlockObjectData bod)
        {
            this.data = bod;
            EditorSceneManager.SaveOpenScenes();
            var path = EditorSceneManager.GetActiveScene().path;
            if (!string.IsNullOrEmpty(path))
            {
                mainScenePath = path;
            }
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            go = new GameObject("Block Object");
            var filter= go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            var bo= go.AddComponent<BlockObject>();
            filter.mesh = bo.mesh;
            bo.data = this.data;
            Selection.activeGameObject = go;
            SceneView.onSceneGUIDelegate += this.OnSceneViewGUI;
            isOpen = true;
        }

        /// <summary>
        /// 回到原先的场景
        /// </summary>
        public void ReturnMainScene()
        {
           
            GameObject.DestroyImmediate(go);
            EditorSceneManager.OpenScene(mainScenePath, OpenSceneMode.Single);
            this.mainScenePath = "";
            SceneView.onSceneGUIDelegate -= this.OnSceneViewGUI;
            isOpen = false;

        }

        public void OnSceneViewGUI(SceneView view)
        {
            Handles.BeginGUI();
            var  hrect= EditorGUILayout.BeginHorizontal();
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