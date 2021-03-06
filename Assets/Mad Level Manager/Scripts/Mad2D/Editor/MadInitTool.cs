/*
* Mad Level Manager by Mad Pixel Machine
* http://www.madpixelmachine.com
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using MadLevelManager;

#if !UNITY_3_5
namespace MadLevelManager {
#endif

public class MadInitTool : EditorWindow {
    public const int CameraNearClip = -10;
    public const int CameraFarClip = 5;

    // ===========================================================
    // Constants
    // ===========================================================

    // ===========================================================
    // Fields
    // ===========================================================
    
    string rootObjectName = "Mad Level Root";
    int layer = 0;
    
    protected MadRootNode root;

    protected bool hideInvisibleSprites;
    protected bool depthBasedRenderMode;

    // ===========================================================
    // Methods for/from SuperClass/Interfaces
    // ===========================================================
    
    protected virtual void OnFormGUI() {}
    protected virtual void AfterCreate(MadRootNode root) {}

    // ===========================================================
    // Methods
    // ===========================================================
    
    void OnGUI() {
        MadGUI.Info("This tool initialized your scene for GUI drawing. Please choose root object name and layer "
            + "on which GUI will be painted.");
    
        rootObjectName = EditorGUILayout.TextField("Root TypeID", rootObjectName);
        layer = EditorGUILayout.LayerField("Layer", layer);
        
        OnFormGUI();
        
        if (GUILayout.Button("Create")) {
            var panel = MadPanel.UniqueOrNull();
            bool doInit = true;
            if (panel != null) {
                doInit = EditorUtility.DisplayDialog("Scene initialized", "Scene looks like it is already initialized. "
                    + "Are you sure that you want to continue?", "Yes", "No");
            }
        
            if (doInit) {
                root = Init(rootObjectName, layer, hideInvisibleSprites, depthBasedRenderMode);
                AfterCreate(root);
            }
        }
    }
    
    public static MadRootNode Init(string rootObjectName, int layer, bool hideInvisibleSprites = false, bool depthBasedRenderMode = false) {
        CheckEmptyScene();

        var go = new GameObject();
        go.name = rootObjectName;
        var root = go.AddComponent<MadRootNode>();

        Camera[] otherCameras = GameObject.FindObjectsOfType(typeof(Camera)) as Camera[];
        bool hasOtherCamera = otherCameras.Length > 0;

        float maxDepth = 0;
        for (int i = 0; i < otherCameras.Length; ++i) {
            if (otherCameras[i].depth > maxDepth) {
                maxDepth = otherCameras[i].depth;
            }
        }
        
        var camera = MadTransform.CreateChild<MadNode>(go.transform, "Camera 2D");
        var cam = camera.gameObject.AddComponent<Camera>();
        cam.backgroundColor = Color.gray;
        cam.orthographic = true;
        cam.orthographicSize = 1;
        cam.nearClipPlane = CameraNearClip;
        cam.farClipPlane = CameraFarClip;
        //cam.transform.localScale = new Vector3(1, 1, 0.01f);
        cam.depth = maxDepth + 1;

        if (hasOtherCamera) {
            cam.clearFlags = CameraClearFlags.Depth;
        } else {
            cam.tag = "MainCamera";
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.gameObject.AddComponent<AudioListener>();
        }

        var panel = MadTransform.CreateChild<MadPanel>(go.transform, "Panel");
        panel.hideInvisibleSprites = hideInvisibleSprites;
        if (depthBasedRenderMode) {
            panel.renderMode = MadPanel.RenderMode.DepthBased;
        }
        
        // setup layers
        cam.cullingMask = 1 << layer;
        panel.gameObject.layer = layer;
        
        return root;
    }

    // if a scene has only one Main Camera object, then init tool proposes to remove it
    private static void CheckEmptyScene() {
        if (SceneIsNewlyCreated()) {
            bool remove = EditorUtility.DisplayDialog(
                "Remove Unused Objects?",
                "It seems that this is a newly created scene. Level select screen do not need the Main Camera or Directional Light objects. If you're not planning to use it, it's recommended to remove it.",
                "Remove", "Leave");
            if (remove) {
                var mainCamera = GameObject.Find("/Main Camera");
                if (mainCamera != null) {
                    MadGameObject.SafeDestroy(mainCamera);
                }

                var light = GameObject.Find("/Directional Light");
                if (light != null) {
                    MadGameObject.SafeDestroy(light);
                }
            }
        }
    }

    private static bool SceneIsNewlyCreated() {
        var allObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (var obj in allObjects) {
            if (obj.name != "Main Camera" && obj.name != "Directional Light") {
                return false;
            }
        }

        return true;
    }
    
    // ===========================================================
    // Static Methods
    // ===========================================================
    
    public static void ShowWindow() {
        EditorWindow.GetWindow<MadInitTool>(false, "Init Tool", true);
    }
    
    // ===========================================================
    // Inner and Anonymous Classes
    // ===========================================================

}

#if !UNITY_3_5
} // namespace
#endif