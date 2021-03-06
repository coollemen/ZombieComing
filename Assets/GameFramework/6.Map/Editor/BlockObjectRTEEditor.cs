﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
namespace GameFramework
{
   [CustomEditor(typeof(BlockObjectRTE))]
    public class BlockObjectRTEEditor :OdinEditor
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