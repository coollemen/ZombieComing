using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
namespace GameFramework
{
    public enum TextureCreateMode
    {
        OneTex,TwoTex,TreeTex,SixTex
    }
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
        /// <summary>
        /// 创建模式
        /// </summary>
        [EnumToggleButtons]
        public MeshCreateMode meshCreateMode;
        /// <summary>
        /// 颜色
        /// </summary>
        [ShowIf("meshCreateMode", MeshCreateMode.Color)]
        public Color color;

        [EnumToggleButtons, ShowIf("meshCreateMode", MeshCreateMode.Texture)]
        public TextureCreateMode texCreateMode;


        [ShowIf("IsShowTop"),OnValueChanged("OnTopChanged")]
        [PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite top;
        private bool IsShowTop()
        {
            return this.meshCreateMode == MeshCreateMode.Texture;
        }

        private void OnTopChanged()
        {
            switch (texCreateMode)
            {
                case TextureCreateMode.OneTex:
                    bottom = front = back = left = right = top;
                    break;
                case TextureCreateMode.TwoTex:
                    break;
                case TextureCreateMode.TreeTex:
                    break;
                case TextureCreateMode.SixTex:
                    break;
                default:
                    break;
            }
        }
        [ShowIf("IsShowBottom"), OnValueChanged("OnBottomChanged")]
        [PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite bottom;
        private bool IsShowBottom()
        {
            return this.meshCreateMode == MeshCreateMode.Texture&&
                (texCreateMode!=TextureCreateMode.OneTex);
        }
        private void OnBottomChanged()
        {
            switch (texCreateMode)
            {
                case TextureCreateMode.OneTex:
                    break;
                case TextureCreateMode.TwoTex:
                    front = back = left = right = bottom;
                    break;
                case TextureCreateMode.TreeTex:
                    break;
                case TextureCreateMode.SixTex:
                    break;
                default:
                    break;
            }
        }
        [ShowIf("IsShowLeft")]
        [PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite left;
        private bool IsShowLeft()
        {
            return this.meshCreateMode == MeshCreateMode.Texture &&
                (texCreateMode == TextureCreateMode.SixTex); 
        }
        private void OnLeftChanged()
        {

        }
        [ShowIf("IsShowRight")]
        [PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite right;
        private bool IsShowRight()
        {
            return this.meshCreateMode == MeshCreateMode.Texture &&
                (texCreateMode == TextureCreateMode.SixTex);
        }
        private void OnRightChanged()
        {

        }
        [ShowIf("IsShowFront"), OnValueChanged("OnFrontChanged")]
        [PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite front;

        private bool IsShowFront()
        {
            return this.meshCreateMode == MeshCreateMode.Texture &&
                (texCreateMode == TextureCreateMode.TreeTex|| texCreateMode == TextureCreateMode.SixTex);
        }
        private void OnFrontChanged()
        {
            switch (texCreateMode)
            {
                case TextureCreateMode.OneTex:
                    break;
                case TextureCreateMode.TwoTex:

                    break;
                case TextureCreateMode.TreeTex:
                     back = left = right = front;
                    break;
                case TextureCreateMode.SixTex:
                    break;
                default:
                    break;
            }
        }
        [ShowIf("IsShowBack")]
        [PreviewField(50, ObjectFieldAlignment.Right)]
        public Sprite back;
        private bool IsShowBack()
        {
            return this.meshCreateMode == MeshCreateMode.Texture &&
                (texCreateMode == TextureCreateMode.SixTex);
        }
        private void OnBackChanged()
        {

        }
        public BlockDefinition()
        {
            
        }
        public BlockDefinition(byte setID, string setName)
        {
            this.id = setID;
            this.name = setName;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="setID">id</param>
        /// <param name="setName">名称</param>
        /// <param name="setColor">颜色</param>
        public BlockDefinition(byte setID, string setName, Color setColor) 
        {
            this.id = setID;
            this.name = setName;
            this.color = setColor;
            this.meshCreateMode = MeshCreateMode.Color;
        }
        //都是A面的方块
        public BlockDefinition(byte id, string name, Sprite face)
            : this(id, name, face, face, face, face, face, face)
        {
        }

        //上面是A，其他面是B的方块
        public BlockDefinition(byte id, string name, Sprite setTop, Sprite setOther)
            : this(id, name, setTop, setOther, setOther, setOther, setOther, setOther)
        {
        }

        //上面是A，下面是B，其他面是C的方块
        public BlockDefinition(byte id, string name, Sprite setTop, Sprite setBottom, Sprite setOther)
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
        public BlockDefinition(byte id, string name, Sprite setTop, Sprite setBottom, Sprite setFront,
            Sprite setBack, Sprite setLeft, Sprite setRight) 
        {
            this.id = id;
            this.name = name;
            this.meshCreateMode = MeshCreateMode.Texture;
            this.texCreateMode = TextureCreateMode.OneTex;
            this.top = setTop;
            this.bottom = setBottom;
            this.front = setFront;
            this.back = setBack;
            this.left = setLeft;
            this.right = setRight;
        }
    }
}
