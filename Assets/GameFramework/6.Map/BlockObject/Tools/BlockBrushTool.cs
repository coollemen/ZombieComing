using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [System.Serializable]
    public class BlockBrushTool : CustomEditorTool
    {
        public int size = 1;
        public int blockIndex;

        public BlockBrushTool() : base()
        {
            this.name = "画笔";
        }
    }
}
