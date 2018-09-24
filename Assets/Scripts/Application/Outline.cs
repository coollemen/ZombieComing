using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;
public class Outline : MonoBehaviour
{
    private Highlighter h;
    // Use this for initialization
    void Start ()
    {
        h = GetComponent<Highlighter>();
    }

   public  void OnTap()
    {
        // Fade in constant highlighting  
        h.ConstantOn(Color.red);

        // Turn off constant highlighting  
        //h.ConstantOffImmediate();

        // Start flashing from blue to cyan color and frequency = 2f  
        //h.FlashingOn(Color.blue, Color.cyan, 2f);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
