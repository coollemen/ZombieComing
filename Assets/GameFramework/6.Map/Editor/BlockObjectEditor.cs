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
        //编辑模式下的字段
        /// <summary>
        /// 画布大小
        /// </summary>
        public Vector3Int canvasSize;

        public string[] panelNames = new string[] {"图块","笔刷","设置"};
        public int selectedPanelIndex = 0;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (BlockObjectOwnSceneEditor.Instance.isOpen ==false)
            {
                this.OnNormalModeGUI();
                //如果没有打开编辑器窗口，显示进入编辑器按键
                if (GUILayout.Button("进入编辑器"))
                {
                    var bo = target as BlockObject;
                    if (bo.data != null)
                    {
//                        BlockObjectSceneEditor.Instance.OpenEditorScene(bo.data);
                        BlockObjectOwnSceneEditor.Instance.EnterEditorScene(bo.data);
                        GUIUtility.ExitGUI();
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
                this.OnEditModeGUI();
                //如果已经打开编辑器窗口，显示返回主场景按键
                if (GUILayout.Button("返回主场景"))
                {
//                    BlockObjectSceneEditor.Instance.ReturnMainScene();
                    BlockObjectOwnSceneEditor.Instance.LeaveEditorScene();
                    GUIUtility.ExitGUI();
                }
            }

            if (!string.IsNullOrEmpty(msg))
            {
                EditorGUILayout.HelpBox(msg, UnityEditor.MessageType.Error);
            }
        }
        /// <summary>
        /// 在普通模式下绘制的GUI
        /// </summary>
        public void OnNormalModeGUI()
        {
            
        }
        /// <summary>
        /// 在编辑模式下绘制的GUI
        /// </summary>
        public void OnEditModeGUI()
        {
            this.DrawCanvasConfig();
        }
        #region 编辑模式Inspector绘制函数

        public void DrawCanvasConfig()
        {
            this.canvasSize = EditorGUILayout.Vector3IntField("Canvas Size", canvasSize);
            selectedPanelIndex = GUILayout.Toolbar(selectedPanelIndex, panelNames);
        }
        #endregion
        public void OnSceneGUI()
        {
            
        }
        #region 场景视图绘制函数

        public void Draw()
        {
            
        }
        #endregion
    }
}