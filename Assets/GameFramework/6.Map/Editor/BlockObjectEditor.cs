using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace GameFramework
{
    [CustomEditor(typeof(BlockObject))]
    public class BlockObjectEditor : Editor
    {
        public string msg = "";
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (BlockObjectSceneEditor.isOpen ==false)
            {
                //如果没有打开编辑器窗口，显示进入编辑器按键
                if (GUILayout.Button("进入编辑器"))
                {
                    var bo = target as BlockObject;
                    if (bo.data != null)
                    {
                        BlockObjectSceneEditor.ShowWindow(bo.data);
                        msg = "";
                    }
                    else
                    {
                        msg = "图块数据为空，无法进行编辑!!!";
                    }
                }
            }
            else
            {
                //如果已经打开编辑器窗口，显示返回主场景按键
                if (GUILayout.Button("返回主场景"))
                {
                    var win = EditorWindow.GetWindow<BlockObjectSceneEditor>();
                    win.ReturnMainScene();
                }
            }

            if (!string.IsNullOrEmpty(msg))
            {
                EditorGUILayout.HelpBox(msg, UnityEditor.MessageType.Error);
            }
        }
    }
}