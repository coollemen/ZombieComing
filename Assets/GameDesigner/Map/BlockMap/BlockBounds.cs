using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDesigner
{
    [System.Serializable]
    public class BlockBounds
    {
        public int id;
        public Bounds bounds;

        public BlockBounds(int setID, Bounds setBounds)
        {
            this.id = setID;
            this.bounds = setBounds;
        }
    }
}