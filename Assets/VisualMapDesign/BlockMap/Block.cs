using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
/// <summary>
/// 地图块
/// </summary>
public class Block : MonoBehaviour
{
    public int index;

    public int X
    {
        get { return index; }
    }

    public int Y
    {
        get { return index; }
    }

    public int Z
    {
        get { return index; }
    }

    public bool empty=true;
    public GameObject prefab;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddBlock(GameObject go)
    {
        //获取当前block的绘制组件
        var meshFilter = GetComponent<MeshFilter>();
        var meshRenderer = GetComponent<MeshRenderer>();
        //获取prefab的绘制组件
        var targetMeshFilter = go.GetComponent<MeshFilter>();
        var targetMeshRenderer = go.GetComponent<MeshRenderer>();
        if (meshFilter == null)
        {
            meshFilter = this.gameObject.AddComponent<MeshFilter>();
        }
        if (meshRenderer == null)
        {
            meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        }
        if (targetMeshFilter != null)
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(targetMeshFilter);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(meshFilter);
        }
        if (targetMeshRenderer != null)
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(targetMeshRenderer);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(meshRenderer);
        }
        transform.rotation = go.transform.rotation;
        empty = false;
    }

    public void RemoveBlock()
    {
        var meshFilter = GetComponent<MeshFilter>();
        var meshRenderer = GetComponent<MeshRenderer>();
        if (meshFilter != null)
        {
            DestroyImmediate(meshFilter);
        }
        if (meshRenderer != null)
        {
            DestroyImmediate(meshRenderer);
        }
        empty = true;
    }
    [Button("Add Block")]
    public void AddPrefabToBlock()
    {
        this.AddBlock(this.prefab);
    }
    [Button("Remove Data")]
    public void RemoveData()
    {
        this.RemoveBlock();
    }
}