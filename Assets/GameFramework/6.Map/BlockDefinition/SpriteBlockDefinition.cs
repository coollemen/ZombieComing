using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [System.Serializable]
    public class SpriteBlockDefinition : BlockDefinition
    {
        public Sprite front;
        public Sprite back;
        public Sprite top;
        public Sprite bottom;
        public Sprite left;
        public Sprite right;

        public SpriteBlockDefinition(byte id, string name)
            : base(id, name)
        {
        }

        //都是A面的方块
        public SpriteBlockDefinition(byte id, string name, Sprite face)
            : this(id, name, face, face, face, face, face, face)
        {
        }

        //上面是A，其他面是B的方块
        public SpriteBlockDefinition(byte id, string name, Sprite setTop, Sprite setOther)
            : this(id, name, setTop, setOther, setOther, setOther, setOther, setOther)
        {
        }

        //上面是A，下面是B，其他面是C的方块
        public SpriteBlockDefinition(byte id, string name, Sprite setTop, Sprite setBottom, Sprite setOther)
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
        public SpriteBlockDefinition(byte id, string name, Sprite setTop, Sprite setBottom, Sprite setFront,
            Sprite setBack, Sprite setLeft, Sprite setRight) : base(id, name)
        {
            top = setTop;
            bottom = setBottom;
            front = setFront;
            back = setBack;
            left = setLeft;
            right = setRight;
        }
    }
}