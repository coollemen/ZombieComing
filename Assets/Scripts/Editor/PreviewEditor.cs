using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetPreviewWindow : EditorWindow
{
    [MenuItem("Example/AssetPreviewWindow")]
    static void Init()
    {
        EditorWindow window = EditorWindow.CreateInstance<AssetPreviewWindow>();
        window.Show();
    }

    void OnGUI()
    {
        Texture2D previewTex = AssetPreview.GetAssetPreview(Selection.activeObject);
        EditorGUILayout.BeginVertical();
        GUILayout.SelectionGrid(0, new Texture2D[] { previewTex }, 1);//显示所选对象的预览图
        EditorGUILayout.EndVertical();
    }
}
