using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

/// <summary>
/// 基本化学元素类型基类
/// </summary>
public class ElementObject : MonoBehaviour {
    ///// <summary>
    ///// 水元素值
    ///// </summary>
    //public int water;
    ///// <summary>
    ///// 水元素抗性
    ///// </summary>
    //public int waterDefense;
    ///// <summary>
    ///// 火元素值
    ///// </summary>
    //public int fire;
    ///// <summary>
    ///// 火元素抗性
    ///// </summary>
    //public int fireDefense;
    ///// <summary>
    ///// 电元素
    ///// </summary>
    //public int electric;
    ///// <summary>
    ///// 电元素抗性
    ///// </summary>
    //public int electricDefense;
    ///// <summary>
    ///// 病毒值
    ///// </summary>
    //public int virus;
    ///// <summary>
    ///// 病毒抗性
    ///// </summary>
    //public int virusDefense;
    ///// <summary>
    ///// 鲜血值
    ///// </summary>
    //public int blood;
    ///// <summary>
    ///// 鲜血抗性
    ///// </summary>
    //public int bloodDefense;

    [OdinSerialize]
    [InfoBox("添加新的元素特性")]
    public ElementProperty[] elementProperties;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
