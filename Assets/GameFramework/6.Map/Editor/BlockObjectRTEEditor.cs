using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
namespace GameFramework
{
    [CustomEditor(typeof(BlockObjectRTE))]
    public class BlockObjectRTEEditor : Editor
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

        public Vector3Int canvasSize = new Vector3Int(100, 100, 100);
        public CanvasViewMode canvasViewMode = CanvasViewMode.PanelXZ;
        public int viewPanelX = 1;
        public int viewPanelY = 1;
        public int viewPanelZ = 1;
        public string[] panelNames = new string[] {"工具栏", "图块", "设置"};
        public int selectedPanelIndex = 0;

        public string[] blockDefTypes = new string[] {"单色", "贴图"};
        public int selectedBlockDefTypeIndex = 0;
        public string blockDefName = "block";
        public Color blockDefColor = Color.white;
        public Sprite blockDefSprite;
        public string[] sprBlockCreateModes = new string[] {"One Tex", "Two Tex", "Tree Tex", "Six Tex"};
        public int selectedSprBlockCreateModeIndex = 0;

        public int selectedBlockDefIndex = 0;

        public List<string> blockNames = new List<string>();
        public int selectedBlockIndex = 0;

        public string[] toolNames = new string[] {"画笔", "油漆桶", "选择工具", "移动工具", "几何体"};
        public int selectedToolIndex = 0;

        public string[] geometryNames = new string[] {"正方体", "球", "圆柱体"};
        public int selectedGeometeryIndex = 0;

        Color faceColor = new Color(1, 1, 1, 0.2f);
        Color hitFaceColor = new Color(0, 1, 0, 0.2f);
        Color lineColor = new Color(1, 0.38f, 0, 1f);
        Color hitLineColor = new Color(1, 1, 1, 0.5f);

        public Dictionary<string, EditorTool> tools = new Dictionary<string, EditorTool>();
        public BlockObjectRTE boEditor;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            EditorUtility.SetDirty(boEditor.data);
        }

        private void OnDestroy()
        {
            EditorUtility.SetDirty(boEditor.data);
        }

        public override void OnInspectorGUI()
        {
            if (boEditor == null)
            {
                boEditor = target as BlockObjectRTE;
            }
            base.OnInspectorGUI();
            //            this.boEditor.data = EditorGUILayout.ObjectField(boEditor.data, typeof(BlockObjectData)) as BlockObjectData;
            EditorGUILayout.LabelField("Canvas");
            EditorGUI.indentLevel++;
            this.canvasSize = EditorGUILayout.Vector3IntField("Size", canvasSize);
            this.canvasViewMode = (CanvasViewMode) EditorGUILayout.EnumPopup("View Mode", canvasViewMode);
            if (canvasViewMode == CanvasViewMode.Free)
            {
                viewPanelX = EditorGUILayout.IntSlider("Panel X", viewPanelX, 1, canvasSize.x);
                viewPanelY = EditorGUILayout.IntSlider("Panel Y", viewPanelY, 1, canvasSize.y);
                viewPanelZ = EditorGUILayout.IntSlider("Panel Z", viewPanelZ, 1, canvasSize.z);
            }
            else if (canvasViewMode == CanvasViewMode.PanelXY)
            {
                viewPanelZ = EditorGUILayout.IntSlider("Panel Z", viewPanelZ, 1, canvasSize.z);
            }
            else if (canvasViewMode == CanvasViewMode.PanelYZ)
            {
                viewPanelX = EditorGUILayout.IntSlider("Panel X", viewPanelX, 1, canvasSize.x);
            }
            else if (canvasViewMode == CanvasViewMode.PanelXZ)
            {
                viewPanelY = EditorGUILayout.IntSlider("Panel Y", viewPanelY, 1, canvasSize.y);
            }
            EditorGUI.indentLevel--;
            selectedPanelIndex = GUILayout.Toolbar(selectedPanelIndex, panelNames);
            if (selectedPanelIndex == 0)
            {
                this.DrawBrushPanel();
            }
            else if (selectedPanelIndex == 1)
            {
                this.DrawBlockPanel();
            }
        }

        private void DrawBlockPanel()
        {
            EditorGUILayout.LabelField("创建新图块");
            EditorGUI.indentLevel++;

            selectedBlockDefTypeIndex = EditorGUILayout.Popup("图块类型", selectedBlockDefTypeIndex, blockDefTypes);
            if (selectedBlockDefTypeIndex == 0)
            {
                blockDefName = EditorGUILayout.TextField("名称", blockDefName);
                blockDefColor = EditorGUILayout.ColorField("颜色", blockDefColor);
            }
            else if (selectedBlockDefTypeIndex == 1)
            {
                blockDefName = EditorGUILayout.TextField("名称", blockDefName);
                blockDefSprite = EditorGUILayout.ObjectField("精灵", blockDefSprite, typeof(Sprite)) as Sprite;
            }
            EditorGUI.indentLevel--;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("添加", GUILayout.Width(80)))
            {
                if (selectedBlockDefTypeIndex == 0)
                {
                    var def = new ColorBlockDefinition((byte) boEditor.data.blockDefs.Count, blockDefName,
                        blockDefColor);
                    boEditor.data.blockDefs.Add(def);
                }
                else if (selectedBlockDefTypeIndex == 1)
                {
                    var def = new SpriteBlockDefinition((byte) boEditor.data.blockDefs.Count, blockDefName,
                        blockDefSprite);
                    boEditor.data.blockDefs.Add(def);
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
            for (int i = 0; i < boEditor.data.blockDefs.Count; i++)
            {
                var def = boEditor.data.blockDefs[i];
                if (i == selectedBlockDefIndex)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical((GUIStyle) "MeTransitionSelect", GUILayout.Height(200));
                    GUILayout.Toggle(true, string.Format("ID:{0},Name:{1}", def.id, def.name),
                        (GUIStyle) "MeTransitionSelectHead", GUILayout.Height(30));
                    def = boEditor.data.blockDefs[i];
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
                        boEditor.data.blockDefs.RemoveAt(selectedBlockDefIndex);
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
                        selectedBlockDefIndex = i;
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
            selectedSprBlockCreateModeIndex = GUILayout.Toolbar(selectedSprBlockCreateModeIndex, sprBlockCreateModes);
            if (selectedSprBlockCreateModeIndex == 0)
            {
                var temp = EditorGUILayout.ObjectField("All Face", sprDef.top, typeof(Sprite), false) as Sprite;
                if (temp != null && temp != sprDef.top)
                {
                    sprDef.top = temp;
                    sprDef.bottom = sprDef.front = sprDef.back = sprDef.left = sprDef.right = sprDef.top;
                }
            }
            else if (selectedSprBlockCreateModeIndex == 1)
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
            else if (selectedSprBlockCreateModeIndex == 2)
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
            else if (selectedSprBlockCreateModeIndex == 3)
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
            this.blockNames.Clear();
            for (int i = 0; i < boEditor.data.blockDefs.Count; i++)
            {
                this.blockNames.Add(boEditor.data.blockDefs[i].name);
            }
            EditorGUILayout.LabelField("图块列表");
            selectedBlockIndex = GUILayout.SelectionGrid(selectedBlockIndex, blockNames.ToArray(), 5);
            EditorGUILayout.LabelField("笔刷工具");
            selectedToolIndex = GUILayout.Toolbar(selectedToolIndex, toolNames);
            if (toolNames[selectedToolIndex] == "画笔")
            {
                if (!tools.ContainsKey("画笔"))
                {
                    var brushTool = new BrushEditorTool();
                    this.tools.Add(brushTool.name, brushTool);
                }

                this.tools["画笔"].OnGUI();
            }
            else if (toolNames[selectedToolIndex] == "油漆桶")
            {
            }
            else if (toolNames[selectedToolIndex] == "选择工具")
            {
            }
            else if (toolNames[selectedToolIndex] == "移动工具")
            {
            }
            else if (toolNames[selectedToolIndex] == "几何体")
            {
                if (!tools.ContainsKey("几何体"))
                {
                    var geometryTool = new GeometryEditorTool();
                    this.tools.Add(geometryTool.name, geometryTool);
                }

                tools["几何体"].OnGUI();
            }
        }

        private void DrawConfigPanel()
        {
        }

        public void OnSceneGUI()
        {
//            this.DrawBackgroundGrid(canvasSize.x, canvasSize.z, faceColor, lineColor);
            List<Bounds> bounds = new List<Bounds>();
            if (canvasViewMode == CanvasViewMode.PanelXY)
            {
                this.DrawBgGridXY(viewPanelZ, canvasSize.x, canvasSize.y);
                bounds = CreateHitBoundsXY(viewPanelZ, canvasSize.x, canvasSize.y);
            }
            else if (canvasViewMode == CanvasViewMode.PanelYZ)
            {
                this.DrawBgGridYZ(viewPanelX, canvasSize.y, canvasSize.z);
                bounds = CreateHitBoundsYZ(viewPanelX, canvasSize.y, canvasSize.z);
            }
            else if (canvasViewMode == CanvasViewMode.PanelXZ)
            {
                this.DrawBgGridXZ(viewPanelY, canvasSize.x, canvasSize.z);
                bounds = CreateHitBoundsXZ(viewPanelY, canvasSize.x, canvasSize.z);
            }
            else if (canvasViewMode == CanvasViewMode.Free)
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
                if (e.type == EventType.MouseDown && e.button==0)
                {
                    var point = GetPointFromBounds(hitBounds[0]);
                    boEditor.SetBlock(point.x, point.y, point.z, (byte)selectedBlockIndex);
                    e.Use();
                }
                else if (e.type == EventType.MouseDown && e.button==1)
                {
                }
            }

            if (boEditor.isInit == false)
            {
                boEditor.Init();
            }
            //如果图块定义有变化，重新创建图块池
            if (boEditor.isDefDirty)
            {
                boEditor.CreateBlockPool();
            }
            //如果图块数据有变化，重新创建mesh
            if (boEditor.isDirty)
            {
                EditorCoroutineUtility.StartCoroutine(boEditor.CreateMeshAsyn(), this);
            }
            //ongui
            Handles.color = oldColor;

            Handles.BeginGUI();
            foreach (var hb in hitBounds)
            {
                Vector3 p = new Vector3(hb.center.x - 0.5f, hb.center.y - 0.5f, hb.center.z - 0.5f);
                GUILayout.Label("当前选择图块坐标：" + p.ToString());
            }
            Handles.EndGUI();

            SceneView.RepaintAll();
        }

        #region 场景视图绘制函数

        public Vector3Int GetPointFromBounds(Bounds b)
        {
            return new Vector3Int((int)(b.center.x - 0.5f), (int)(b.center.y - 0.5f), (int)(b.center.z - 0.5f));
        }
        public void DrawHitBoundsRect(Bounds b)
        {
            if (canvasViewMode == CanvasViewMode.PanelXY)
            {
                //绘制z轴面的矩形
                var center = b.center;
                var extend = b.extents;
                Vector3 v1 = new Vector3(center.x - extend.x, center.y - extend.y, center.z - extend.z);
                Vector3 v2 = new Vector3(center.x + extend.x, center.y - extend.y, center.z - extend.z);
                Vector3 v3 = new Vector3(center.x + extend.x, center.y + extend.y, center.z - extend.z);
                Vector3 v4 = new Vector3(center.x - extend.x, center.y + extend.y, center.z - extend.z);
                this.DrawBackgroundGrid(v1, v2, v3, v4, hitFaceColor, hitLineColor);
            }
            else if (canvasViewMode == CanvasViewMode.PanelYZ)
            {
                //绘制x轴面的矩形
                var center = b.center;
                var extend = b.extents;
                Vector3 v1 = new Vector3(center.x - extend.x, center.y - extend.y, center.z - extend.z);
                Vector3 v2 = new Vector3(center.x - extend.x, center.y + extend.y, center.z - extend.z);
                Vector3 v3 = new Vector3(center.x - extend.x, center.y + extend.y, center.z + extend.z);
                Vector3 v4 = new Vector3(center.x - extend.x, center.y - extend.y, center.z + extend.z);
                this.DrawBackgroundGrid(v1, v2, v3, v4, hitFaceColor, hitLineColor);
            }
            else if (canvasViewMode == CanvasViewMode.PanelXZ)
            {
                //绘制y轴面的矩形
                var center = b.center;
                var extend = b.extents;
                Vector3 v1 = new Vector3(center.x - extend.x, center.y - extend.y, center.z - extend.z);
                Vector3 v2 = new Vector3(center.x + extend.x, center.y - extend.y, center.z - extend.z);
                Vector3 v3 = new Vector3(center.x + extend.x, center.y - extend.y, center.z + extend.z);
                Vector3 v4 = new Vector3(center.x - extend.x, center.y - extend.y, center.z + extend.z);
                this.DrawBackgroundGrid(v1, v2, v3, v4, hitFaceColor, hitLineColor);

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
            Vector3 v1 = new Vector3(0, 0, z - 1);
            Vector3 v2 = new Vector3(width, 0, z - 1);
            Vector3 v3 = new Vector3(width, height, z - 1);
            Vector3 v4 = new Vector3(0, height, z - 1);
            this.DrawBackgroundGrid(v1, v2, v3, v4, faceColor, lineColor);
        }

        public void DrawBgGridXZ(int y, int width, int depth)
        {
            Vector3 v1 = new Vector3(0, y - 1, 0);
            Vector3 v2 = new Vector3(width, y - 1, 0);
            Vector3 v3 = new Vector3(width, y - 1, depth);
            Vector3 v4 = new Vector3(0, y - 1, depth);
            this.DrawBackgroundGrid(v1, v2, v3, v4, faceColor, lineColor);
        }

        public void DrawBgGridYZ(int x, int height, int depth)
        {
            Vector3 v1 = new Vector3(x - 1, 0, 0);
            Vector3 v2 = new Vector3(x - 1, height, 0);
            Vector3 v3 = new Vector3(x - 1, height, depth);
            Vector3 v4 = new Vector3(x - 1, 0, depth);
            this.DrawBackgroundGrid(v1, v2, v3, v4, faceColor, lineColor);
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