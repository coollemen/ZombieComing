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

        [Title("画布")] public Vector3Int canvasSize = new Vector3Int(100, 100, 100);

        public int CanvasMaxX
        {
            get { return canvasSize.x; }
        }

        public int CanvasMaxY
        {
            get { return canvasSize.y; }
        }

        public int CanvasMaxZ
        {
            get { return canvasSize.z; }
        }

        [EnumToggleButtons] public CanvasViewMode canvasViewMode = CanvasViewMode.PanelXZ;

        [ShowIf("canvasViewMode", CanvasViewMode.PanelYZ), PropertyRange(1, "CanvasMaxX")]
        public int viewPanelX = 1;

        [ShowIf("canvasViewMode", CanvasViewMode.PanelXZ), PropertyRange(1, "CanvasMaxY")]
        public int viewPanelY = 1;

        [ShowIf("canvasViewMode", CanvasViewMode.PanelXY), PropertyRange(1, "CanvasMaxZ")]
        public int viewPanelZ = 1;


        //tab 1 画笔

        //tab 2 图块
        [TabGroup("图块定义", false, 0)]
        [ShowInInspector]
//        [TabGroup("图块")]
        public List<BlockDefinition> BlockDefs
        {
            get { return data.blockDefs; }
            set
            {
                data.blockDefs = value;

            }
        }
        [Button("保存数据到Data",ButtonSizes.Large),PropertyOrder(999)]
        public void SaveData()
        {
            Debug.Log("保存图块定义！");
            EditorUtility.SetDirty(data);
        }
        //tab 3 设置
//        [HideInInspector]
//        public string[] panelNames = new string[] {"工具栏", "图块", "设置"};
//        [Title("工具栏")]
//        [CustomValueDrawer("DrawPanelIndex")]
//        public int selectedPanelIndex = 0;
//
//        private int DrawPanelIndex(int value, GUIContent label)
//        {
//            return GUILayout.Toolbar(this.selectedPanelIndex, this.panelNames);
//        }

//        [HideInInspector] public string[] blockDefTypes = new string[] {"单色", "贴图"};
//        [HideInInspector] public int selectedBlockDefTypeIndex = 0;
//        [HideInInspector] public string blockDefName = "block";
//        [HideInInspector] public Color blockDefColor = Color.white;
//        [HideInInspector] public Sprite blockDefSprite;

//        [HideInInspector]
//        public string[] sprBlockCreateModes = new string[] {"One Tex", "Two Tex", "Tree Tex", "Six Tex"};
//
//        [HideInInspector] public int selectedSprBlockCreateModeIndex = 0;
//
//        [HideInInspector] public int selectedBlockDefIndex = 0;
        public List<string> BlockNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (var def in data.blockDefs)
                {
                    names.Add(def.name);
                }

                return names;
            }
        }
        [Title("图块")]
        [CustomValueDrawer("DrawSelectedBlockIndex")]
        public int selectedBlockIndex = 0;
        private int DrawSelectedBlockIndex(int value, GUIContent label)
        {
            return GUILayout.SelectionGrid(this.selectedBlockIndex, this.BlockNames.ToArray(),5);
        }

        [HideInInspector] public string[] toolNames = new string[] {"画笔", "油漆桶", "选择工具", "移动工具", "几何体"};
        [Title("工具栏")]
        [CustomValueDrawer("DrawSelectedToolIndex")]
        public int selectedToolIndex = 0;
        private int DrawSelectedToolIndex(int value, GUIContent label)
        {
            return GUILayout.SelectionGrid(this.selectedToolIndex, this.toolNames, 5);
        }
        [HideInInspector] public string[] geometryNames = new string[] {"正方体", "球", "圆柱体"};
        [HideInInspector] public int selectedGeometeryIndex = 0;
        //tab 设置
        [TabGroup("显示设置",false,1)] public Color faceColor = new Color(1, 1, 1, 0.2f);
        [TabGroup("显示设置", false, 2)] public Color hitFaceColor = new Color(0, 1, 0, 0.2f);
        [TabGroup("显示设置", false, 3)] public Color lineColor = new Color(1, 0.38f, 0, 1f);
        [TabGroup("显示设置", false, 4)] public Color hitLineColor = new Color(1, 1, 1, 0.5f);

        public Dictionary<string, EditorTool> tools = new Dictionary<string, EditorTool>();

        /// <summary>
        /// 是否图块定义需要更新
        /// </summary>
        [HideInInspector]
        public bool isDefDirty = false;
        [HideInInspector]
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