using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class Map : MonoBehaviour
{
    public string name;
    public int version = 1;
    [Title("Grid Size")] public int gridWidth = 20;
    public int gridHeight = 20;
    [Title("Cell")] public float cellSize = 1;
    [Title("BlockTerrain Size")] public float mapWidth;
    public float mapHeight;
    public MapLayer activeLayer;
    public Dictionary<string, MapLayer> layers = new Dictionary<string, MapLayer>();
    [Title("Grid Gizmos Style")]
    public Color gridActiveColor = new Color(0, 1f, 0, 0.35f);
    public Color gridColor1 = new Color(0.5f, 0.5f, 0.5f, 0.8f);
    public Color gridColor2 = new Color(0.6f, 0.6f, 0.6f, 0.8f);
//    public List<Vector3> cells = new List<Vector3>();
    public List<MapCell> cells = new List<MapCell>();
    public MapCell activeCell;
    private void Awake()
    {
        this.CalculateSize();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 计算地图大小
    /// </summary>
    public void CalculateSize()
    {
        this.mapWidth = this.gridWidth * this.cellSize;
        this.mapHeight = this.gridHeight * this.cellSize;
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
        if (this.activeLayer != null && this.activeLayer.name == layerName)
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
        Color oldColor = Gizmos.color;
        DrawMapGrid();
        Gizmos.color = oldColor;
        //绘制行
//        for (int row = 0; row <= countZ; row++)
//        {
//            Vector3 from = new Vector3(transform.position.x-mapLength / 2, transform.position.y,transform.position.z-mapWidth / 2 + row * tileWidth);
//            Vector3 to = new Vector3(transform.position.x+mapLength / 2, transform.position.y, transform.position.z - mapWidth / 2 + row * tileWidth) ;
//            Gizmos.DrawLine(from, to);
//        }
//
//        //绘制列
//        for (int col = 0; col <= countX; col++)
//        {
//            Vector3 from = new Vector3(transform.position.x - mapLength / 2 + col * tileLength, transform.position.y, transform.position.z+mapHeight / 2);
//            Vector3 to = new Vector3(transform.position.x - mapLength / 2 + col * tileLength, transform.position.y, transform.position.z - mapHeight / 2);
//            Gizmos.DrawLine(from, to);
//        }
    }

    private void OnDrawGizmosSelected()
    {
    }

    private void CreateCells()
    {
        for (int i = 0; i < this.cells.Count; i++)
        {
            DestroyImmediate(this.cells[i].gameObject);
        }
        this.cells.Clear();
        var originPos = new Vector3();
        originPos.x = transform.position.x - mapWidth / 2;
        originPos.y = transform.position.y;
        originPos.z = transform.position.z - mapHeight / 2;
        for (int j = 0; j < gridHeight; j++)
        {
            for (int i = 0; i < gridWidth; i++)
            {
                var cellPosition = new Vector3();
                cellPosition.x = originPos.x + cellSize / 2 + i * cellSize;
                cellPosition.y = originPos.y;
                cellPosition.z = originPos.z + cellSize / 2 + j * cellSize;
//                cells.Add(cellPosition);
                //添加map cell
                var cellObj = new GameObject("Cell" + i.ToString() + "," + j.ToString());
                cellObj.transform.SetParent(transform);
                cellObj.transform.localPosition = cellPosition;
                var cell = cellObj.AddComponent<MapCell>();
                cell.row = i;
                cell.column = j;
                cells.Add(cell);
            }
        }
    }

    public void AddGameObjectToActiveCell(GameObject go)
    {
        go.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
        activeCell.AddGround(go);
    }

    public void RemoveGameObjectFromActiveCell()
    {
        activeCell.Clear();
    }
    /// <summary>
    /// 绘制地图网格
    /// </summary>
    private void DrawMapGrid()
    {
        //绘制基础网格
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].isEmpty == false) continue;
            if (Mathf.FloorToInt(i / gridWidth) % 2 != 0)
            {
                if (i % 2 != 0)
                {
                    Gizmos.color = gridColor1;
                    Gizmos.DrawCube(cells[i].transform.position, new Vector3(cellSize, 0, cellSize));
                }
                else
                {
                    Gizmos.color = gridColor2;
                    Gizmos.DrawCube(cells[i].transform.position, new Vector3(cellSize, 0, cellSize));
                }
            }
            else
            {
                if (i % 2 != 0)
                {
                    Gizmos.color = gridColor2;
                    Gizmos.DrawCube(cells[i].transform.position, new Vector3(cellSize, 0, cellSize));
                }
                else
                {
                    Gizmos.color = gridColor1;
                    Gizmos.DrawCube(cells[i].transform.position, new Vector3(cellSize, 0, cellSize));
                }
            }

        }
        //绘制active cell 的网格
        if (activeCell)
        {
            Gizmos.color = gridActiveColor;
            Gizmos.DrawCube(activeCell.transform.position, new Vector3(cellSize, 0, cellSize));
        }
    }

    public bool IsEnterCell(Vector3 mousePos,out int row,out int col)
    {
        bool flag = false;
        row = -1;
        col = -1;
        foreach (var c in cells)
        {
            Rect rect = new Rect();
            rect.x = c.transform.position.x - cellSize / 2;
            rect.y = c.transform.position.z - cellSize / 2;
            rect.width = cellSize;
            rect.height = cellSize;
            if (rect.Contains(new Vector2(mousePos.x,mousePos.z)))
            {
                flag = true;
                activeCell = c;
                row = c.row;
                col = c.column;
                break;
            }
        }
        return flag;
    }

# if UNITY_EDITOR
    [Button("Open BlockTerrain Editor")]
    private void OpenEditorWindow()
    {
        EditorApplication.ExecuteMenuItem("GameDesign/BlockTerrain Editor");
    }

    [Button("Create BlockTerrain Grid")]
    private void CreateMapGrid()
    {
        this.CreateCells();
    }
    [Button("Clear BlockTerrain Data")]
    private void ClearMapData()
    {
        for (int i = 0; i < this.cells.Count; i++)
        {
            this.cells[i].Clear();
        }
    }
#endif
}