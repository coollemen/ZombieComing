using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;

namespace GameFramework
{
//    [CustomEditor(typeof(BlockObjectRTE))]
    public class BlockObjectRTEEditor : Editor
    {
     
        public BlockObjectRTE rte;

        public void Awake()
        {
//            Debug.Log("Black Object Editor Awake");
        }

        private void OnEnable()
        {
            Debug.Log("Black Object Editor Enable");
            if (rte == null)
            {
                rte = target as BlockObjectRTE;
            }
            rte.canvasSize = new Vector3Int(rte.data.Width, rte.data.Height, rte.data.Depth);
        }

        private void OnDisable()
        {
            EditorUtility.SetDirty(rte.data);
        }

        private void OnDestroy()
        {
            EditorUtility.SetDirty(rte.data);
        }

        public override void OnInspectorGUI()
        {
            if (rte == null)
            {
                rte = target as BlockObjectRTE;
                if (rte.data.blocks.Count == 0)
                {
                    rte.data.ResizeBlocksArray(rte.canvasSize.x, rte.canvasSize.y, rte.canvasSize.z);
                }
            }
            base.OnInspectorGUI();
            //            this.rte.data = EditorGUILayout.ObjectField(rte.data, typeof(BlockObjectData)) as BlockObjectData;
            EditorGUILayout.LabelField("Canvas");
            EditorGUI.indentLevel++;
            rte.canvasSize = EditorGUILayout.Vector3IntField("Size", rte.canvasSize);
            rte.canvasViewMode = (BlockObjectRTE.CanvasViewMode) EditorGUILayout.EnumPopup("View Mode", rte.canvasViewMode);
            if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.Free)
            {
                rte.viewPanelX = EditorGUILayout.IntSlider("Panel X", rte.viewPanelX, 1, rte.canvasSize.x);
                rte.viewPanelY = EditorGUILayout.IntSlider("Panel Y", rte.viewPanelY, 1, rte.canvasSize.y);
                rte.viewPanelZ = EditorGUILayout.IntSlider("Panel Z", rte.viewPanelZ, 1, rte.canvasSize.z);
            }
            else if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.PanelXY)
            {
                rte.viewPanelZ = EditorGUILayout.IntSlider("Panel Z", rte.viewPanelZ, 1, rte.canvasSize.z);
            }
            else if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.PanelYZ)
            {
                rte.viewPanelX = EditorGUILayout.IntSlider("Panel X", rte.viewPanelX, 1, rte.canvasSize.x);
            }
            else if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.PanelXZ)
            {
                rte.viewPanelY = EditorGUILayout.IntSlider("Panel Y", rte.viewPanelY, 1, rte.canvasSize.y);
            }
            EditorGUI.indentLevel--;
            if (GUILayout.Button("创建空画布"))
            {
                this.rte.data.ResizeBlocksArray(rte.canvasSize.x, rte.canvasSize.y, rte.canvasSize.z);
                EditorCoroutineUtility.StartCoroutine(rte.CreateMeshAsyn(), this);
            }
            rte.selectedPanelIndex = GUILayout.Toolbar(rte.selectedPanelIndex, rte.panelNames);
            if (rte.selectedPanelIndex == 0)
            {
                this.DrawBrushPanel();
            }
            else if (rte.selectedPanelIndex == 1)
            {
                this.DrawBlockPanel();
            }
        }

        private void DrawBlockPanel()
        {
            EditorGUILayout.LabelField("创建新图块");
            EditorGUI.indentLevel++;

            rte.selectedBlockDefTypeIndex = EditorGUILayout.Popup("图块类型", rte.selectedBlockDefTypeIndex, rte.blockDefTypes);
            if (rte.selectedBlockDefTypeIndex == 0)
            {
                rte.blockDefName = EditorGUILayout.TextField("名称", rte.blockDefName);
                rte.blockDefColor = EditorGUILayout.ColorField("颜色", rte.blockDefColor);
            }
            else if (rte.selectedBlockDefTypeIndex == 1)
            {
                rte.blockDefName = EditorGUILayout.TextField("名称", rte.blockDefName);
                rte.blockDefSprite = EditorGUILayout.ObjectField("精灵", rte.blockDefSprite, typeof(Sprite)) as Sprite;
            }
            EditorGUI.indentLevel--;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("添加", GUILayout.Width(80)))
            {
                if (rte.selectedBlockDefTypeIndex == 0)
                {
                    var def = new ColorBlockDefinition((byte) (rte.data.blockDefs.Count + 1), rte.blockDefName,
                        rte.blockDefColor);
                    rte.data.blockDefs.Add(def);
                }
                else if (rte.selectedBlockDefTypeIndex == 1)
                {
                    var def = new SpriteBlockDefinition((byte) (rte.data.blockDefs.Count + 1), rte.blockDefName,
                        rte.blockDefSprite);
                    rte.data.blockDefs.Add(def);
                }
            }
            if (GUILayout.Button("清空", GUILayout.Width(40)))
            {
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            //显示当前存在的图块定义
            EditorGUILayout.LabelField("图块列表");
            EditorGUI.indentLevel++;
            for (int i = 0; i < rte.data.blockDefs.Count; i++)
            {
                var def = rte.data.blockDefs[i];
                if (i == rte.selectedBlockDefIndex)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical((GUIStyle) "MeTransitionSelect", GUILayout.Height(200));
                    GUILayout.Toggle(true, string.Format("ID:{0},Name:{1}", def.id, def.name),
                        (GUIStyle) "MeTransitionSelectHead", GUILayout.Height(30));
                    def = rte.data.blockDefs[i];
                    if (def is SpriteBlockDefinition)
                    {
                        this.DrawSpriteBlockDefItem(def as SpriteBlockDefinition);
                    }
                    else if (def is ColorBlockDefinition)
                    {
                        this.DrawColorBlockDefItem(def as ColorBlockDefinition);
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    MyGUITools.SetBackgroundColor(Color.red);
                    if (GUILayout.Button("Del", GUILayout.MinWidth(80)))
                    {
                        rte.data.blockDefs.RemoveAt(rte.selectedBlockDefIndex);
                    }
                    MyGUITools.RestoreBackgroundColor();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
                else
                {
                    var flag = GUILayout.Toggle(false, string.Format("ID:{0},Name:{1}", def.id, def.name),
                        (GUIStyle) "OL Title");
                    if (flag == true)
                    {
                        rte.selectedBlockDefIndex = i;
                    }
                }
            }
            EditorGUI.indentLevel--;
        }

        private void DrawSpriteBlockDefItem(SpriteBlockDefinition sprDef)
        {
            sprDef.id = EditorGUILayout.IntField("ID", sprDef.id);
            sprDef.name = EditorGUILayout.TextField("Name", sprDef.name);
            //贴图设置
            EditorGUILayout.PrefixLabel("CreateMode");
            rte.selectedSprBlockCreateModeIndex = GUILayout.Toolbar(rte.selectedSprBlockCreateModeIndex, rte.sprBlockCreateModes);
            if (rte.selectedSprBlockCreateModeIndex == 0)
            {
                var temp = EditorGUILayout.ObjectField("All Face", sprDef.top, typeof(Sprite), false) as Sprite;
                if (temp != null && temp != sprDef.top)
                {
                    sprDef.top = temp;
                    sprDef.bottom = sprDef.front = sprDef.back = sprDef.left = sprDef.right = sprDef.top;
                }
            }
            else if (rte.selectedSprBlockCreateModeIndex == 1)
            {
                sprDef.top = EditorGUILayout.ObjectField("Top Face", sprDef.top, typeof(Sprite), false) as Sprite;
                var temp =
                    EditorGUILayout.ObjectField("Other Face", sprDef.bottom, typeof(Sprite), false) as Sprite;
                if (temp != null && temp != sprDef.bottom)
                {
                    sprDef.bottom = temp;
                    sprDef.front = sprDef.back = sprDef.left = sprDef.right = sprDef.bottom;
                }
            }
            else if (rte.selectedSprBlockCreateModeIndex == 2)
            {
                sprDef.top = EditorGUILayout.ObjectField("Top Face", sprDef.top, typeof(Sprite), false) as Sprite;
                sprDef.bottom =
                    EditorGUILayout.ObjectField("Bottom Face", sprDef.bottom, typeof(Sprite), false) as Sprite;
                var temp =
                    EditorGUILayout.ObjectField("Other Face", sprDef.front, typeof(Sprite), false) as Sprite;
                if (temp != null && temp != sprDef.front)
                {
                    sprDef.front = temp;
                    sprDef.back = sprDef.left = sprDef.right = sprDef.front;
                }
            }
            else if (rte.selectedSprBlockCreateModeIndex == 3)
            {
                sprDef.top = EditorGUILayout.ObjectField("Top Face", sprDef.top, typeof(Sprite), false) as Sprite;
                sprDef.bottom =
                    EditorGUILayout.ObjectField("Bottom Face", sprDef.bottom, typeof(Sprite), false) as Sprite;
                sprDef.front =
                    EditorGUILayout.ObjectField("Front Face", sprDef.front, typeof(Sprite), false) as Sprite;
                sprDef.back =
                    EditorGUILayout.ObjectField("Back Face", sprDef.back, typeof(Sprite), false) as Sprite;
                sprDef.left =
                    EditorGUILayout.ObjectField("Left Face", sprDef.left, typeof(Sprite), false) as Sprite;
                sprDef.right =
                    EditorGUILayout.ObjectField("Right Face", sprDef.right, typeof(Sprite), false) as Sprite;
            }
        }

        private void DrawBlockCreateMode()
        {
        }

        private void DrawColorBlockDefItem(ColorBlockDefinition colorDef)
        {
            colorDef.id = EditorGUILayout.IntField("ID", colorDef.id);
            colorDef.name = EditorGUILayout.TextField("Name", colorDef.name);
            colorDef.color = EditorGUILayout.ColorField("Color", colorDef.color);
        }

        private void DrawBrushPanel()
        {
            rte.blockNames.Clear();
            for (int i = 0; i < rte.data.blockDefs.Count; i++)
            {
                rte.blockNames.Add(rte.data.blockDefs[i].name);
            }
            EditorGUILayout.LabelField("图块列表");
            rte.selectedBlockIndex = GUILayout.SelectionGrid(rte.selectedBlockIndex, rte.blockNames.ToArray(), 5);
            EditorGUILayout.LabelField("笔刷工具");
            rte.selectedToolIndex = GUILayout.Toolbar(rte.selectedToolIndex, rte.toolNames);
            if (rte.toolNames[rte.selectedToolIndex] == "画笔")
            {
                if (!rte.tools.ContainsKey("画笔"))
                {
                    var brushTool = new BrushEditorTool();
                    rte.tools.Add(brushTool.name, brushTool);
                }

                rte.tools["画笔"].OnGUI();
            }
            else if (rte.toolNames[rte.selectedToolIndex] == "油漆桶")
            {
            }
            else if (rte.toolNames[rte.selectedToolIndex] == "选择工具")
            {
            }
            else if (rte.toolNames[rte.selectedToolIndex] == "移动工具")
            {
            }
            else if (rte.toolNames[rte.selectedToolIndex] == "几何体")
            {
                if (!rte.tools.ContainsKey("几何体"))
                {
                    var geometryTool = new GeometryEditorTool();
                    rte.tools.Add(geometryTool.name, geometryTool);
                }

                rte.tools["几何体"].OnGUI();
            }
        }

        private void DrawConfigPanel()
        {
        }

        public void OnSceneGUI()
        {
            if (rte == null)
            {
                rte = target as BlockObjectRTE;
            }
            //            this.DrawBackgroundGrid(canvasSize.x, canvasSize.z, faceColor, lineColor);
            List<Bounds> bounds = new List<Bounds>();
            if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.PanelXY)
            {
                var realPanelZ = rte.viewPanelZ - 1;
                this.DrawBgGridXY(realPanelZ, rte.canvasSize.x, rte.canvasSize.y);
                bounds = CreateHitBoundsXY(realPanelZ, rte.canvasSize.x, rte.canvasSize.y);
            }
            else if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.PanelYZ)
            {
                var realPanelX = rte.viewPanelX - 1;
                this.DrawBgGridYZ(realPanelX, rte.canvasSize.y, rte.canvasSize.z);
                bounds = CreateHitBoundsYZ(realPanelX, rte.canvasSize.y, rte.canvasSize.z);
            }
            else if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.PanelXZ)
            {
                var realPanelY = rte.viewPanelY - 1;
                this.DrawBgGridXZ(realPanelY, rte.canvasSize.x, rte.canvasSize.z);
                bounds = CreateHitBoundsXZ(realPanelY, rte.canvasSize.x, rte.canvasSize.z);
            }
            else if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.Free)
            {
            }
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            var hitBounds = CheckHitBounds(mouseRay, bounds);
            var oldColor = Handles.color;
            Handles.color = Color.green;
            foreach (var hb in hitBounds)
            {
//                Handles.DrawWireCube(hb.center, hb.size);
                this.DrawHitBoundsRect(hb);
            }
            //process event
            var e = Event.current;
            if (hitBounds.Count > 0)
            {
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    var point = GetPointFromBounds(hitBounds[0]);
                    rte.SetBlock(point.x, point.y, point.z, (byte) (rte.selectedBlockIndex + 1));
                    e.Use();
                }
                else if (e.type == EventType.MouseDown && e.button == 1)
                {
                }
            }

            if (rte.isInit == false)
            {
                rte.Init();
            }
            if (rte.blocks == null || rte.blocks.Length == 0)
            {
                rte.InitBlocks();
            }
            //如果图块定义有变化，重新创建图块池
            if (rte.isDefDirty)
            {
                rte.CreateBlockPool();
            }
            //如果图块数据有变化，重新创建mesh
            if (rte.isDirty)
            {
                rte.isDirty = false;
                EditorCoroutineUtility.StartCoroutine(rte.CreateMeshAsyn(), this);
            }
            //ongui
            Handles.color = oldColor;

            Handles.BeginGUI();
            foreach (var hb in hitBounds)
            {
                Vector3 p = new Vector3(hb.center.x - 0.5f, hb.center.y - 0.5f, hb.center.z - 0.5f);
                var rect = new Rect(0, 0, 300, 30);
                rect.y += hitBounds.IndexOf(hb) * 30;
                GUI.Label(rect, "当前选择图块坐标：" + p.ToString());
            }
            Handles.EndGUI();

            SceneView.RepaintAll();
        }

        #region 场景视图绘制函数

        public Vector3Int GetPointFromBounds(Bounds b)
        {
            return new Vector3Int((int) (b.center.x - 0.5f), (int) (b.center.y - 0.5f), (int) (b.center.z - 0.5f));
        }

        public void DrawHitBoundsRect(Bounds b)
        {
            if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.PanelXY)
            {
                //绘制z轴面的矩形
                var center = b.center;
                var extend = b.extents;
                Vector3 v1 = new Vector3(center.x - extend.x, center.y - extend.y, center.z - extend.z);
                Vector3 v2 = new Vector3(center.x + extend.x, center.y - extend.y, center.z - extend.z);
                Vector3 v3 = new Vector3(center.x + extend.x, center.y + extend.y, center.z - extend.z);
                Vector3 v4 = new Vector3(center.x - extend.x, center.y + extend.y, center.z - extend.z);
                this.DrawBackgroundGrid(v1, v2, v3, v4, rte.hitFaceColor, rte.hitLineColor);
            }
            else if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.PanelYZ)
            {
                //绘制x轴面的矩形
                var center = b.center;
                var extend = b.extents;
                Vector3 v1 = new Vector3(center.x - extend.x, center.y - extend.y, center.z - extend.z);
                Vector3 v2 = new Vector3(center.x - extend.x, center.y + extend.y, center.z - extend.z);
                Vector3 v3 = new Vector3(center.x - extend.x, center.y + extend.y, center.z + extend.z);
                Vector3 v4 = new Vector3(center.x - extend.x, center.y - extend.y, center.z + extend.z);
                this.DrawBackgroundGrid(v1, v2, v3, v4, rte.hitFaceColor, rte.hitLineColor);
            }
            else if (rte.canvasViewMode == BlockObjectRTE.CanvasViewMode.PanelXZ)
            {
                //绘制y轴面的矩形
                var center = b.center;
                var extend = b.extents;
                Vector3 v1 = new Vector3(center.x - extend.x, center.y - extend.y, center.z - extend.z);
                Vector3 v2 = new Vector3(center.x + extend.x, center.y - extend.y, center.z - extend.z);
                Vector3 v3 = new Vector3(center.x + extend.x, center.y - extend.y, center.z + extend.z);
                Vector3 v4 = new Vector3(center.x - extend.x, center.y - extend.y, center.z + extend.z);
                this.DrawBackgroundGrid(v1, v2, v3, v4, rte.hitFaceColor, rte.hitLineColor);
            }
            else
            {
            }
        }

        public void DrawBackgroundGrid(int width, int depth, Color faceColor, Color lineColor)
        {
            Vector3[] vectors = new Vector3[4]
            {
                new Vector3(0, 0, 0),
                new Vector3(width, 0, 0),
                new Vector3(width, 0, depth),
                new Vector3(0, 0, depth)
            };
            Handles.DrawSolidRectangleWithOutline(vectors, faceColor, lineColor);
        }

        public void DrawBackgroundGrid(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Color faceColor, Color lineColor)
        {
            Vector3[] vectors = new Vector3[4]
            {
                v1, v2, v3, v4
            };
            Handles.DrawSolidRectangleWithOutline(vectors, faceColor, lineColor);
        }

        public void DrawBgGridXY(int z, int width, int height)
        {
            Vector3 v1 = new Vector3(0, 0, z);
            Vector3 v2 = new Vector3(width, 0, z);
            Vector3 v3 = new Vector3(width, height, z);
            Vector3 v4 = new Vector3(0, height, z);
            this.DrawBackgroundGrid(v1, v2, v3, v4, rte.faceColor, rte.lineColor);
        }

        public void DrawBgGridXZ(int y, int width, int depth)
        {
            Vector3 v1 = new Vector3(0, y, 0);
            Vector3 v2 = new Vector3(width, y, 0);
            Vector3 v3 = new Vector3(width, y, depth);
            Vector3 v4 = new Vector3(0, y, depth);
            this.DrawBackgroundGrid(v1, v2, v3, v4, rte.faceColor, rte.lineColor);
        }

        public void DrawBgGridYZ(int x, int height, int depth)
        {
            Vector3 v1 = new Vector3(x, 0, 0);
            Vector3 v2 = new Vector3(x, height, 0);
            Vector3 v3 = new Vector3(x, height, depth);
            Vector3 v4 = new Vector3(x, 0, depth);
            this.DrawBackgroundGrid(v1, v2, v3, v4, rte.faceColor, rte.lineColor);
        }

        public List<Bounds> CreateHitBoundsXZ(int y, int width, int depth)
        {
            List<Bounds> bounds = new List<Bounds>();
            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    var b = new Bounds(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), new Vector3(1, 1, 1));
                    bounds.Add(b);
                }
            }
            return bounds;
        }

        public List<Bounds> CreateHitBoundsXY(int z, int width, int height)
        {
            List<Bounds> bounds = new List<Bounds>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var b = new Bounds(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), new Vector3(1, 1, 1));
                    bounds.Add(b);
                }
            }
            return bounds;
        }

        public List<Bounds> CreateHitBoundsYZ(int x, int height, int depth)
        {
            List<Bounds> bounds = new List<Bounds>();
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    var b = new Bounds(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), new Vector3(1, 1, 1));
                    bounds.Add(b);
                }
            }
            return bounds;
        }

        public List<Bounds> CheckHitBounds(Ray mouseRay, List<Bounds> bounds)
        {
            List<Bounds> hitBounds = new List<Bounds>();
            foreach (var b in bounds)
            {
                if (b.IntersectRay(mouseRay))
                {
                    hitBounds.Add(b);
                }
            }
            return hitBounds;
        }

        #endregion
    }
}