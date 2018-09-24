using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class ElementProperty  {
    [ValueDropdown("ElementTypes")]
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
    private static IEnumerable ElementTypes = new ValueDropdownList<string>()
        {
            { "水", "water" },
            { "火", "fire" },
            { "电", "electric" },
            { "毒", "virus" },
            { "风", "wind" },
            { "血", "blood" },
        };
}
