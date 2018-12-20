using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 方块的方向
    /// </summary>
    public enum BlockDirection : byte
    {
        Front = 0,
        Back = 1,
        Left = 2,
        Right = 3,
        Top = 4,
        Bottom = 5
    }

    /// <summary>
    /// 方块对象，存储方块的所有信息
    /// </summary>
    public class Block
    {
        //方块的ID
        public byte id;

        //方块的名字
        public string name;

        //方块的图标，并不会采用在游戏中动态生成的做法
        public Texture icon;

        //方向（指的是前面所面朝的方向）
        public BlockDirection direction = BlockDirection.Front;

        //前面贴图的坐标
        public Vector2[] uvFront;
        //后面贴图的坐标
        public Vector2[] uvBack;

        //右面贴图的坐标
        public Vector2[] uvRight;

        //左面贴图的坐标
        public Vector2[] uvLeft;

        //上面贴图的坐标
        public Vector2[] uvTop;

        //下面贴图的坐标
        public Vector2[] uvBottom;

        //都是A面的方块
        public Block(byte id, string name, Vector2[] face)
            : this(id, name, face, face, face, face, face, face)
        {
        }

        //上面是A，其他面是B的方块
        public Block(byte id, string name, Vector2[] top,  Vector2[] other)
            : this(id, name, top, other, other, other, other, other)
        {
        }

        //上面是A，下面是B，其他面是C的方块
        public Block(byte id, string name, Vector2[] top, Vector2[] bottom,Vector2[] other)
            : this(id, name, top, bottom, other, other, other, other)
        {
        }

        //上面是A，下面是B，前面是C，其他面是D的方块
        public Block(byte id, string name,Vector2[] top,Vector2[] bottom,Vector2[] front,Vector2[] other)
            : this(id, name, top,bottom,other,other,front,other)
        {
        }

        //上下左右前后面都不一样的方块
        public Block(byte id, string name, Vector2[] top,Vector2[] bottom,Vector2[] left,Vector2[] right,Vector2[] front,Vector2[] back)
        {
            this.id = id;
            this.name = name;

            this.uvTop = top;
            this.uvBottom = bottom;

            this.uvLeft = left;
            this.uvRight = right;

            this.uvFront = front;
            this.uvBack = back;
        }
    }
}