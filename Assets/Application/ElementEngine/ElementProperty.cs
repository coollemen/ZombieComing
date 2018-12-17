using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementProperty  {
    public string name;
    [Range(0,100)]
    public float value;
    [Range(0, 100)]
    public float defense;
    public bool enable;

    public ElementProperty()
    {
        this.name = "water";
        this.value = 100;
        this.enable = true;
    }
}
