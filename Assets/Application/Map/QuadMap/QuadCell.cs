using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class QuadCell : MonoBehaviour
{
    private MeshRenderer renderer;
    public QuadCoordinates coordinate;
     void Awake()
     {
         this.renderer = GetComponent<MeshRenderer>();
     }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
