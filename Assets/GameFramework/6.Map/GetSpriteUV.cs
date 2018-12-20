using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GetSpriteUV : MonoBehaviour
{
    public Sprite spr;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmosSelected()
    {
        for (int i=0;i<spr.uv.Length;i++)
        {
            Debug.Log(i.ToString() + "=" + spr.uv[i].ToString());
        }
    }
}
