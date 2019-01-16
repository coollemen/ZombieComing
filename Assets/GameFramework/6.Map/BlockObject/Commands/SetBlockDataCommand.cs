using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class SetBlockDataCommand : ICustomCommand
    {
        public Vector3Int blockIndex;
        public byte data;
        public byte lastData;
        public SetBlockDataCommand(Vector3Int setBlockIndex,byte setData)
        {
            this.blockIndex = setBlockIndex;
            this.data = setData;
        }
        public void Execute(GameObject go)
        {
            this.lastData = data;
            var bo = go.GetComponent<BlockObject>();
            bo.SetBlock(blockIndex.x, blockIndex.y, blockIndex.z, data);
        }

        public void Undo(GameObject go)
        {
            this.data = lastData;
            var bo = go.GetComponent<BlockObject>();
            bo.SetBlock(blockIndex.x, blockIndex.y, blockIndex.z, data);
        }
    }
}