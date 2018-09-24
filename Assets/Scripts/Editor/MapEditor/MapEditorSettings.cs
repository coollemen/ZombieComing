using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorSettings : ScriptableObject
{

    public string resPath;
    public List<TileBrushGroup> brushGroups = new List<TileBrushGroup>();
}

