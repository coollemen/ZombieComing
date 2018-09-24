using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 僵尸类的基类
/// </summary>
public class ZombieBase : MonoBehaviour {
    /// <summary>
    /// 生命值
    /// </summary>
    public GameProperty<int> hp;
    /// <summary>
    /// 攻击力
    /// </summary>
    public GameProperty<int> attack;
    /// <summary>
    /// 防御力
    /// </summary>
    public GameProperty<int> defense;
    /// <summary>
    /// 移动速度
    /// </summary>
    public GameProperty<int> speed;
    /// <summary>
    /// 攻击速度
    /// </summary>
    public GameProperty<int> attackSpeed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
