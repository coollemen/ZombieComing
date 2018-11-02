using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///地图单元格
/// </summary>
public class MapCell : MonoBehaviour
{
    /// <summary>
    /// 行索引
    /// </summary>
    public int row;
    /// <summary>
    /// 列索引
    /// </summary>
    public int column;
    /// <summary>
    /// 地面物体，原则上只有一个地面
    /// </summary>
    public GameObject ground;
    /// <summary>
    /// 附着物，可以有多个
    /// </summary>
    public List<GameObject> attachments = new List<GameObject>();
    /// <summary>
    /// 是否为空
    /// </summary>
    public bool isEmpty = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// 添加地面，如果已经存在，则替换
    /// </summary>
    /// <param name="go"></param>
    public void AddGround(GameObject go)
    {
        if (isEmpty)
        {
            this.ground = go;
            this.ground.transform.SetParent(transform);
            this.ground.transform.localPosition = Vector3.zero;
            this.isEmpty = false;
        }
        else
        {
            DestroyImmediate(this.ground);
            this.ground = go;
            this.ground.transform.SetParent(transform);
            this.ground.transform.localPosition = Vector3.zero;
        }

    }
    /// <summary>
    /// 移除地面
    /// </summary>
    public void RemoveGround()
    {
        DestroyImmediate(this.ground);
        this.isEmpty = true;
    }

    public void RemoveAttachments()
    {
        for (int i = 0; i < this.attachments.Count; i++)
        {
            DestroyImmediate(this.attachments[i]);
        }
        this.attachments.Clear();
    }

    public void Clear()
    {
        this.RemoveGround();
        this.RemoveAttachments();
    }
}
