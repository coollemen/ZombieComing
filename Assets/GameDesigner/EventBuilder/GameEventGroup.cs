using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDesigner
{
    [System.Serializable]
    public class GameEventGroup
    {

        public string name;

        public int startNum;

        public int span;

        public List<GameEventDef> events = new List<GameEventDef>();

        public GameEventGroup(string groupName) : base()
        {
            this.name = groupName;
            startNum = 1;
            span = 1;
        }

        public GameEventGroup(string groupName, int setStart, int setSpan) : base()
        {
            this.name = groupName;
            startNum = setStart;
            span = setSpan;
        }

        public int GetNextID()
        {
            return startNum + events.Count * span;
        }
    }
}