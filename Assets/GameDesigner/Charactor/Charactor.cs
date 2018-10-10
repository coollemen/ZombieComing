using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色基类
/// </summary>
public class Charactor : MonoBehaviour
{
    public string name;

    public int level;

    public float hp;

    public float mp;

    public float atk;
    public AttackType atkType;
    public float atkSpeed;

    public float def;
    public DefenseType defType;
    public float moveSpeed;

	// Use this for initialization
	void Start ()
	{
	    atkType = AttackType.Normal;
	    defType = DefenseType.Light;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
