using System;
using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region GameObject

    [Name("FindGameObject")]
    [Category("UnityEngine/GameObject")]
    [Description("查找输入名称的游戏物体")]
    public class G_FindGameObject : PureFunctionNode<GameObject, string>
    {
        public override GameObject Invoke(string targetName)
        {
            return GameObject.Find(targetName);
        }
    }

    [Name("set_name")]
    [Category("UnityEngine/GameObject")]
    [Description("设置该Object的名称")]
    public class Set_Name : CallableFunctionNode<GameObject,GameObject, string>
    {
        public override GameObject Invoke(GameObject obj, string name)
        {
            obj.name = name;
            return obj;
        }
    }
	
	[Name("get_name")]
	[Category("UnityEngine/GameObject")]
	[Description("获得Object的名称")]
	public class Get_GOName : PureFunctionNode<string,GameObject>
	{
		public override string Invoke(GameObject obj)
		{
			return obj.name;
		}
	}

    [Name("set_tag")]
    [Category("UnityEngine/GameObject")]
    [Description("设置该Object的tag")]
    public class Set_Tag : CallableFunctionNode<GameObject,GameObject, string>
    {
        public override GameObject Invoke(GameObject obj, string tag)
        {
            obj.tag = tag;
            return obj;
        }
    }

    [Name("FindGameObjectWithTag")]
    [Category("UnityEngine/GameObject")]
    [Description("查找Tag为输入值的游戏物体")]
    public class G_FindGameObjectWithTag : PureFunctionNode<GameObject, string>
    {
        public override GameObject Invoke(string tag)
        {
            return GameObject.FindGameObjectWithTag(tag);
        }
    }

    [Name("FindGameObjectsWithTag")]
    [Category("UnityEngine/GameObject")]
    [Description("查找Tag为输入值的游戏物体集合")]
    public class G_FindGameObjectsWithTag : PureFunctionNode<GameObject[], string>
    {
        public override GameObject[] Invoke(string tag)
        {
            return GameObject.FindGameObjectsWithTag(tag);
        }
    }

    [Name("newGameObject")]
    [Category("UnityEngine/GameObject")]
    [Description("新建一个游戏物体")]
    public class G_NeWGameObject : PureFunctionNode<GameObject, string>
    {
        public override GameObject Invoke(string name)
        {
            return new GameObject(name);
        }
    }

    [Name("getActive")]
    [Category("UnityEngine/GameObject")]
    [Description("游戏物体的激活状态")]
    public class G_Active : PureFunctionNode<bool, GameObject>
    {
        public override bool Invoke(GameObject gameObject)
        {
            return gameObject.activeSelf;
        }
    }

    [Name("getLayer")]
    [Category("UnityEngine/GameObject")]
    [Description("游戏物体的层")]
    public class G_Layer : PureFunctionNode<int, GameObject>
    {
        public override int Invoke(GameObject gameObject)
        {
            return gameObject.layer;
        }
    }

    [Name("getTransform")]
    [Category("UnityEngine/GameObject")]
    [Description("游戏物体的transform组件")]
    public class Go_Transform : PureFunctionNode<Transform, GameObject>
    {
        public override Transform Invoke(GameObject gameObject)
        {
            return gameObject.transform;
        }
    }

    [Name("setActive")]
    [Category("UnityEngine/GameObject")]
    [Description("新建一个游戏物体")]
    public class S_Active : CallableFunctionNode<GameObject,GameObject, bool>
    {
        public override GameObject Invoke(GameObject gameObject, bool activeState)
        {
            gameObject.SetActive(activeState);
            return gameObject;
        }
    }

    [Name("setLayer")]
    [Category("UnityEngine/GameObject")]
    [Description("设置游戏物体层")]
    public class S_Layer : CallableFunctionNode<GameObject,GameObject, int, string>
    {
        public override GameObject Invoke(GameObject gameObject, int layerIndex, string layerName)
        {
            if (layerIndex>-1)
            {
                gameObject.layer = layerIndex;
            }
            else if (!string.IsNullOrEmpty(layerName))
            {
                gameObject.layer = LayerMask.NameToLayer(layerName);
            }
            return gameObject;
        }
    }

    [Name("addComponent")]
    [Category("UnityEngine/GameObject")]
    [Description("附加组件")]
    public class S_AddComponent : CallableFunctionNode<Component, GameObject, Type>
    {
        public override Component Invoke(GameObject gameObject, Type type)
        {
            return gameObject.AddComponent(type);
        }
    }

    #endregion
}