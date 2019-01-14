using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
namespace GameFramework
{
    /// <summary>
    /// 图块物体实时编辑组件
    /// </summary>
    public class BlockObjectRTE : BlockObject
    {
        #region CanvasViewMode定义

        public enum CanvasViewMode
        {
            PanelXY,
            PanelYZ,
            PanelXZ,
            Free
        }

        #endregion
        [Title("画布")]
        public Vector3Int canvasSize = new Vector3Int(100, 100, 100);

        public int CanvasMaxX { get { return canvasSize.x ; } }
        public int CanvasMaxY { get { return canvasSize.y; } }
        public int CanvasMaxZ { get { return canvasSize.z; } }

        [EnumToggleButtons]
        public CanvasViewMode canvasViewMode = CanvasViewMode.PanelXZ;
        [ShowIf("canvasViewMode",CanvasViewMode.PanelYZ),PropertyRange(1, "CanvasMaxX")]
        public int viewPanelX = 1;
        [ShowIf("canvasViewMode", CanvasViewMode.PanelXZ), PropertyRange(1, "CanvasMaxY")]
        public int viewPanelY = 1;
        [ShowIf("canvasViewMode", CanvasViewMode.PanelXY), PropertyRange(1, "CanvasMaxZ")]
        public int viewPanelZ = 1;

        [Title("工具栏")]
        //tab 1 画笔

        //tab 2 图块
        //tab 3 设置
        [HideInInspector] public string[] panelNames = new string[] {"工具栏", "图块", "设置"};
        [CustomValueDrawer("DrawPanelIndex")]
        public int selectedPanelIndex = 0;

        private   int DrawPanelIndex(int value, GUIContent label)
        {
            return GUILayout.Toolbar(this.selectedPanelIndex, this.panelNames);
        }
            
        [HideInInspector] public string[] blockDefTypes = new string[] {"单色", "贴图"};
        [HideInInspector] public int selectedBlockDefTypeIndex = 0;
        [HideInInspector] public string blockDefName = "block";
        [HideInInspector] public Color blockDefColor = Color.white;
        [HideInInspector] public Sprite blockDefSprite;
        [HideInInspector] public string[] sprBlockCreateModes = new string[] {"One Tex", "Two Tex", "Tree Tex", "Six Tex"};
        [HideInInspector] public int selectedSprBlockCreateModeIndex = 0;

        [HideInInspector] public int selectedBlockDefIndex = 0;

        [HideInInspector] public List<string> blockNames = new List<string>();
        [HideInInspector] public int selectedBlockIndex = 0;

        [HideInInspector] public string[] toolNames = new string[] {"画笔", "油漆桶", "选择工具", "移动工具", "几何体"};
        [HideInInspector] public int selectedToolIndex = 0;

        [HideInInspector] public string[] geometryNames = new string[] {"正方体", "球", "圆柱体"};
        [HideInInspector] public int selectedGeometeryIndex = 0;

        [HideInInspector] public Color faceColor = new Color(1, 1, 1, 0.2f);
        [HideInInspector] public Color hitFaceColor = new Color(0, 1, 0, 0.2f);
        [HideInInspector] public Color lineColor = new Color(1, 0.38f, 0, 1f);
        [HideInInspector] public Color hitLineColor = new Color(1, 1, 1, 0.5f);

        public Dictionary<string, EditorTool> tools = new Dictionary<string, EditorTool>();

        /// <summary>
        /// 是否图块定义需要更新
        /// </summary>
        public bool isDefDirty = false;

        public bool isInit = false;

        public void Init()
        {
            this.LoadFromData();
            this.CreateBlockPool();

            isInit = true;
        }

        //        public void SetMeshToMeshFilter()
        //        {
        //            this.GetComponent<MeshFilter>().mesh = this.mesh;
        //        }
        [OnInspectorGUI]
        public void OnInspectorGUI()
        {
            
        }
    }
}