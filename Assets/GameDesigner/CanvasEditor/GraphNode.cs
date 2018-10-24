using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDesigner
{
    public class GraphNode :IGraphNode
    {
        public virtual string ID {
            get
            {
                return "GraphNode";
            }
                }
        public string Title { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
    }
}