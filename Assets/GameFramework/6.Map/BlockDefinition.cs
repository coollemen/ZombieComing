using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
namespace GameFramework
{
    
    /// <summary>
    /// 地图块的定义，用于创建地图块
    /// </summary>
    [System.Serializable]
    public class BlockDefinition
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id;
        /// <summary>
        /// 名字
        /// </summary>
        public string name;
        /// <summary>
        /// 图标名字
        /// </summary>
        public string iconName;
        //方向（指的是前面所面朝的方向）
        public BlockDirection direction = BlockDirection.Front;

        public Sprite front;
        public Sprite back;
        public Sprite top;
        public Sprite bottom;
        public Sprite left;
        public Sprite right;

        public BlockDefinition()
        {
            this.id = 0;
            this.name = "block";
        }
        //都是A面的方块
        public BlockDefinition(byte id, string name, Sprite face):this(id,name,face,face,face,face,face,face)
        {
        }

        //上面是A，其他面是B的方块
        public BlockDefinition(byte id, string name,Sprite setTop,Sprite setOther)
            : this(id, name, setTop,setOther,setOther,setOther,setOther,setOther)
        {
        }

        //上面是A，下面是B，其他面是C的方块
        public BlockDefinition(byte id, string name, Sprite setTop,Sprite setBottom, Sprite setOther)
            : this(id, name, setTop, setBottom, setOther, setOther, setOther, setOther)
        {
        }

        //上面是A，下面是B，前面是C，其他面是D的方块
//        public BlockDefinition(byte id, string name, byte textureFrontX, byte textureFrontY, byte textureX, byte textureY,
//            byte textureTopX, byte textureTopY, byte textureBottomX, byte textureBottomY)
//            : this(id, name, textureFrontX, textureFrontY, textureX, textureY, textureX, textureY, textureX, textureY,
//                textureTopX, textureTopY, textureBottomX, textureBottomY)
//        {
//        }

        //上下左右前后面都不一样的方块
        public BlockDefinition(byte id, string name,Sprite setTop,Sprite setBottom,Sprite setFront,Sprite setBack,Sprite setLeft,Sprite setRight)
        {
            this.id = id;
            this.name = name;
            top = setTop;
            bottom = setBottom;
            front = setFront;
            back = setBack;
            left = setLeft;
            right = setRight;
        }
    }
}
