using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDesigner
{
    [System.Serializable]
    public class BlockBrush
    {
        [System.NonSerialized] public static BlockBrush activeBrush;
        public string name;
        public string prefab;
        [System.NonSerialized] public Texture2D preview;
        public int rotation = 0;

        public BlockBrush(string setName)
        {
            this.name = setName;
        }

        public void Rotate()
        {
            rotation += 90;
            if (rotation >= 360)
            {
                rotation = 0;
            }
        }
    }
}