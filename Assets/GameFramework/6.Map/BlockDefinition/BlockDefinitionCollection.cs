using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [System.Serializable]
    public class BlockDefinitionCollection : ISerializationCallbackReceiver
    {
        [NonSerialized]
        public List<BlockDefinition> blockDefs = new List<BlockDefinition>();
        [SerializeField]
        public List<ColorBlockDefinition> colorDefs ;
        [SerializeField]
        public List<SpriteBlockDefinition> spriteDefs ;
        [SerializeField]
        public List<int> indexList;

        public void Add(BlockDefinition def)
        {
            this.blockDefs.Add(def);
        }

        public void Remove(BlockDefinition def)
        {
            this.blockDefs.Remove(def);
        }

        public void RemoveAt(int idx)
        {
            this.blockDefs.RemoveAt(idx);
        }

        public int Count
        {
            get { return this.blockDefs.Count; }
        }

        public BlockDefinition this[int index]
        {
            get { return this.blockDefs[index]; }
            set { this.blockDefs[index] = value; }
        }
        public void OnBeforeSerialize()
        {
            colorDefs = new List<ColorBlockDefinition>();
            spriteDefs = new List<SpriteBlockDefinition>();
            indexList = new List<int>();
            for (int i = 0; i < blockDefs.Count; i++)
            {
                var type = blockDefs[i].GetType();
                if (type == typeof(ColorBlockDefinition))
                {
                    //添加类别标识，0代码颜色图块定义
                    indexList.Add(0);
                    //添加在颜色图块定义列表中的索引
                    indexList.Add(colorDefs.Count);
                    //在颜色图块定义列表中添加数据
                    colorDefs.Add(blockDefs[i] as ColorBlockDefinition);
                }else if (type == typeof(SpriteBlockDefinition))
                {
                    //添加类别标识，0代码颜色图块定义
                    indexList.Add(1);
                    //添加在颜色图块定义列表中的索引
                    indexList.Add(spriteDefs.Count);
                    //在颜色图块定义列表中添加数据
                    spriteDefs.Add(blockDefs[i] as SpriteBlockDefinition);
                }
            }
        }

        public void OnAfterDeserialize()
        {
            blockDefs.Clear();
            for (int i = 0; i < indexList.Count; i += 2)
            {
                switch (indexList[i])
                {
                    case 0:
                        blockDefs.Add(colorDefs[indexList[i + 1]]);
                        break;
                    case 1:
                        blockDefs.Add(spriteDefs[indexList[i + 1]]);
                        break;
                }
            }
            indexList = null;
            colorDefs = null;
            spriteDefs = null;
        }
    }
}