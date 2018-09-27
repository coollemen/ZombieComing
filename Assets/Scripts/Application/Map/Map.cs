using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
public class Map : MonoBehaviour
{
    public string name;
    public int version=1;
    [Title("Count")]
    public int countX=16;
    public int countY=10;
    public int countZ=10;
    [Title("Tile")]
    public float tileLength;
    public float tileWidth;
    public float tileHeight;
    [Title("Map Size")]
    public float mapLength=16f;
    public float mapWidth = 10f;
    public float mapHeight=10f;

    public MapLayer activeLayer;
    public Dictionary<string, MapLayer> layers = new Dictionary<string, MapLayer>();
    private void Awake()
    {
       
        this.CalculateSize();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //计算地图大小，格子大小
    public void CalculateSize()
    {
        tileLength = mapLength / countX;
        tileWidth = mapWidth / countZ;
        tileHeight = mapHeight / countY;
    }

    #region layer

    public void AddLayer(string layerName)
    {
        if (!layers.ContainsKey(layerName))
        {
            var layer = new MapLayer(layerName);
            this.layers.Add(layerName, layer);
        }   
    }

    public bool ContainsLayer(string layerName)
    {
        return layers.ContainsKey(layerName);

    }

    public void RemoveLayer(string layerName)
    {
        if (layers.ContainsKey(layerName))
        {
            layers.Remove(layerName);
        }
    }

    public bool IsActiveLayer(string layerName)
    {
        if (this.activeLayer != null && this.activeLayer.name==layerName)
        {
            return true;
        }
        return false;
    }

    public void SetActiveLayer(string layerName)
    {
        if (!IsActiveLayer(layerName) && this.layers.ContainsKey(layerName))
        {
            this.activeLayer = this.layers[layerName];
        }
    }
    #endregion

    //获取鼠标所在位置的世界坐标
    Vector3 GetWorldPosition()
    {
        Vector3 viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        return worldPos;
    }
    private void OnDrawGizmos()
    {
        this.CalculateSize();
//        Gizmos.DrawCube(transform.position, new Vector3(mapLength, 0, mapWidth));
        Gizmos.color = Color.green;
        //绘制行
        for (int row = 0; row <= countZ; row++)
        {
            Vector3 from = new Vector3(transform.position.x-mapLength / 2, transform.position.y,transform.position.z-mapWidth / 2 + row * tileWidth);
            Vector3 to = new Vector3(transform.position.x+mapLength / 2, transform.position.y, transform.position.z - mapWidth / 2 + row * tileWidth) ;
            Gizmos.DrawLine(from, to);
        }

        //绘制列
        for (int col = 0; col <= countX; col++)
        {
            Vector3 from = new Vector3(transform.position.x - mapLength / 2 + col * tileLength, transform.position.y, transform.position.z+mapHeight / 2);
            Vector3 to = new Vector3(transform.position.x - mapLength / 2 + col * tileLength, transform.position.y, transform.position.z - mapHeight / 2);
            Gizmos.DrawLine(from, to);
        }
    }
    private void OnDrawGizmosSelected()
    {
        
    }
# if UNITY_EDITOR
    [Button("Open Map Editor")]
    private void OpenEditorWindow()
    {
        EditorApplication.ExecuteMenuItem("GameDesign/Map Editor");
    }
#endif
}
