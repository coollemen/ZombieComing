using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuadGrid : MonoBehaviour
{
    public int width=50;
    public int height=50;

    public int cellSize = 1;

    public QuadCell cellPrefab;
    public List<QuadCell> cells = new List<QuadCell>();

    public Text cellLabelPrefab;
    public Canvas gridCanvas;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        for (int i = 0; i < cells.Count; i++)
        {
            
        }
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawSphere(transform.position, 1);
    }
    private void CreateMapGrid()
    {
        ClearMapGrid();
        //计算行数，列数
        int rowCount = height / cellSize;
        int colCount = width / cellSize;
        //先绘制行
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                QuadCell cell = Instantiate<QuadCell>(cellPrefab);
                cell.transform.SetParent(transform, false);
                Vector3 position = new Vector3();
                position.x = cellSize / 2 + j * cellSize;
                position.y = 0;
                position.z = cellSize / 2 + i * cellSize;
                cell.transform.localPosition = position;
                cell.transform.localScale = new Vector3(cellSize, 0.001f, cellSize);
                QuadCoordinates coordinate = new QuadCoordinates(j, i);
                cell.coordinate = coordinate;
                this.cells.Add(cell);
                //添加标签
//                Text label = Instantiate<Text>(cellLabelPrefab);
//                label.transform.SetParent(gridCanvas.transform);
//                label.transform.localPosition = new Vector3(position.x,position.z,1);
//                label.text = coordinate.ToString();
            }
        }
    }
    private void ClearMapGrid()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            DestroyImmediate(cells[i].gameObject);
        }
        cells.Clear();
//        List<GameObject> labels = new List<GameObject>();
//        foreach(Transform child in gridCanvas.transform)
//        {
//            labels.Add(child.gameObject);
//        }
//        for (int i = 0; i < labels.Count; i++)
//        {
//            DestroyImmediate(labels[i]);
//        }
//        labels.Clear();
    }
}
