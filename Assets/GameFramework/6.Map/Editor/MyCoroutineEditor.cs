using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
public class MyCoroutineEditor : EditorWindow
{
    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    }
    void Start(){

    }

}
