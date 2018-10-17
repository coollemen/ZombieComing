using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameEventGroup : List<GameEventDef>
{

    public string name;

    public GameEventGroup(string groupName):base()
    {
        this.name = groupName;
    }
}
