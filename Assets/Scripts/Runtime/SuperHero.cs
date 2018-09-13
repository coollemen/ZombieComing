using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperHero : MonoBehaviour {

    public Animator ainm;
	// Use this for initialization
	void Start () {
        this.ainm = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.ainm.SetFloat("Speed_f", 0.3f);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            this.ainm.SetFloat("Speed_f", 0f);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.ainm.SetInteger("Animation_int", 5);
        }
    }
}
