using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace GameFramework
{
    /// <summary>
    /// 画笔工具
    /// </summary>
    public class BrushEditorTool:EditorTool
    {
        public int size = 1;
        public BrushEditorTool()
        {
            this.name = "画笔";  
        }
        public override void OnGUI()
        {
            size = EditorGUILayout.IntField("Size", size);

        }
    }
    /// <summary>
    /// 几何体工具
    /// </summary>
    public class GeometryEditorTool : EditorTool
    {
        public enum GeometryType
        {
            Cube,Sphere,Cylinder
        }

        public GeometryType type = GeometryType.Cube;
        public Vector3Int size = new Vector3Int(1, 1, 1);
        public int radius = 5;
       
        public GeometryEditorTool()
        {
            this.name = "几何体";
        }
        public override void OnGUI()
        {
            type = (GeometryType) EditorGUILayout.EnumPopup("几何体", type)  ;
            if (type == GeometryType.Cube)
            {
                size = EditorGUILayout.Vector3IntField("大小", size);
            }
            else if (type == GeometryType.Sphere)
            {
                radius = EditorGUILayout.IntField("半径", radius);
            }
            else if (type == GeometryType.Cylinder)
            {
                size = EditorGUILayout.Vector3IntField("大小", size);
                radius = EditorGUILayout.IntField("半径", radius);
            }
        }
    }
}