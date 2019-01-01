using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace GameFramework
{

    /// <summary>
    /// 打开编辑器会进入新场景，在新场景中进行相关编辑
    /// </summary>
    public class OwnSceneEditorSingleton<T>: Singleton<T> where T:class
    {
        public string mainScenePath;
        public Object data;
        public bool isOpen;

        protected OwnSceneEditorSingleton()
        {
            mainScenePath = "";
            isOpen = false;
        }
        /// <summary>
        /// 打开编辑器场景，保存原场景
        /// </summary>
        public void EnterEditorScene(Object setData)
        {
            this.data = setData;
            EditorSceneManager.SaveOpenScenes();
            var path = EditorSceneManager.GetActiveScene().path;
            if (!string.IsNullOrEmpty(path))
            {
                mainScenePath = path;
            }
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            this.OnEnterEditorScene();
            SceneView.onSceneGUIDelegate += this.OnSceneViewGUI;
            isOpen = true;
        }

        /// <summary>
        /// 离开编辑器场景，回到原先的场景
        /// </summary>
        public void LeaveEditorScene()
        {
            this.OnLeaveEditorScene();
            EditorSceneManager.OpenScene(mainScenePath, OpenSceneMode.Single);
            this.mainScenePath = "";
            SceneView.onSceneGUIDelegate -= this.OnSceneViewGUI;
            isOpen = false;

        }
        /// <summary>
        /// 当进入编辑器场景时
        /// </summary>
        public virtual void OnEnterEditorScene()
        {
            
        }
        /// <summary>
        /// 当离开编辑器创景时
        /// </summary>
        public virtual void OnLeaveEditorScene()
        {
            
        }
        public virtual void OnSceneViewGUI(SceneView view)
        {

        }
    }
}