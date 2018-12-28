using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace GameFramework
{
    public class BlockObjectSceneEditor : EditorWindow
    {
        public string mainScenePath;
        public BlockObjectData data;
        public GameObject go;
        public static bool isOpen = false;
        /// <summary>
        /// 打开编辑器的场景
        /// </summary>
        public void OpenEditorScene()
        {
            EditorSceneManager.SaveOpenScenes();
            var path = EditorSceneManager.GetActiveScene().path;
            if (!string.IsNullOrEmpty(path))
            {
                mainScenePath = path;
            }
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            go = new GameObject("Block Object");
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            var bo= go.AddComponent<BlockObject>();
            bo.data = this.data;
            isOpen = true;
        }

        /// <summary>
        /// 回到原先的场景
        /// </summary>
        public void ReturnMainScene()
        {
            EditorSceneManager.OpenScene(mainScenePath, OpenSceneMode.Single);
            this.Close();
            isOpen = false;

        }

        private void OnEnable()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        private void OnDestroy()
        {
            if (!string.IsNullOrEmpty(mainScenePath))
            {
                this.ReturnMainScene();
            }
            isOpen = false;
        }

        private void OnGUI()
        {
//            if (GUILayout.Button("进入编辑场景"))
//            {
//                this.OpenEditorScene();
//            }

            if (GUILayout.Button("返回主场景"))
            {
                this.ReturnMainScene();
            }
        }

        public void OnSceneGUI(SceneView view)
        {
//        Debug.Log("On Scene GUI");
        }

        [MenuItem("Game Framework/图块物体编辑器")]
        public static void ShowWindow()
        {
            var win = EditorWindow.GetWindow<BlockObjectSceneEditor>();
            win.title = "图块物体编辑器";
            win.Show();
        }

        public static void ShowWindow(BlockObjectData data)
        {
            var win = EditorWindow.GetWindow<BlockObjectSceneEditor>();
            win.title = "图块物体编辑器";
            win.data = data;
            win.Show();
            win.OpenEditorScene();
        }
    }
}