using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class BlockObjectDataEditor : EditorWindow
{
    Camera camera ;
    RenderTexture renderTexture;
    [MenuItem("GameFramework/图块物体编辑器")]
    static void OpenEditorWindow()
    {
        var win = EditorWindow.CreateInstance<BlockObjectDataEditor>();
        win.title = "图块物体编辑器";
        win.autoRepaintOnSceneChange = true;
        win.Show();
    }
    public void Awake()
    {
//        renderTexture = new RenderTexture((int)position.width,
//            (int)position.height,
//            (int)RenderTextureFormat.ARGB32);
    }
//    private void OnEnable()
//    {
//        camera = Camera.main;
//    }
//    public void Update()
//    {
//        if (renderTexture == null)
//        {
//            renderTexture = new RenderTexture((int)position.width,
//    (int)position.height,
//    (int)RenderTextureFormat.ARGB32);
//        }
//        if (camera != null)
//        {
//            camera.targetTexture = renderTexture;
//            camera.Render();
//            camera.targetTexture = null;
//        }
//        if (renderTexture.width != position.width ||
//            renderTexture.height != position.height)
//            renderTexture = new RenderTexture((int)position.width,
//                (int)position.height,
//                (int)RenderTextureFormat.ARGB32);
//    }
//
//    void OnGUI()
//    {
//        if (renderTexture != null)
//        {
//            GUI.DrawTexture(new Rect(0.0f, 0.0f, position.width, position.height), renderTexture);
//        }
//
//    }
}
