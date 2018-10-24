using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
namespace GameDesigner
{
    public interface IGraphNode
    {
        string ID { get;  }
        string Title { get; set; }
        Vector2 Position { get; set; }
        Vector2 Size { get; set; }
    }
}
