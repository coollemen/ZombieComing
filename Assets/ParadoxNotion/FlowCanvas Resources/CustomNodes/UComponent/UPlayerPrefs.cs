using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    #region PlayerPrefs

    [Name("setFloat(save)")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("存档float")]
    public class G_SetFloat : CallableActionNode<string, float>
    {
        public override void Invoke(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }
    }

    [Name("setVector3(save)")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("存档vector3")]
    public class S_SetVector3 : CallableActionNode<string, Vector3>
    {
        public override void Invoke(string key, Vector3 value)
        {
            PlayerPrefs.SetFloat(key + ".x", value.x);
            PlayerPrefs.SetFloat(key + ".y", value.y);
            PlayerPrefs.SetFloat(key + ".z", value.z);
        }
    }

    [Name("setInt(save)")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("存档int")]
    public class G_SetInt : CallableActionNode<string, int>
    {
        public override void Invoke(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
    }

    [Name("setString(save)")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("存档String")]
    public class G_SetString : CallableActionNode<string, string>
    {
        public override void Invoke(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }

    [Name("getFloat(load)")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("取档float")]
    public class G_GetFloat : PureFunctionNode<float, string, float>
    {
        public override float Invoke(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }
    }

    [Name("getVector3(load)")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("取档float")]
    public class G_GetVector3 : PureFunctionNode<Vector3, string, Vector3>
    {
        public override Vector3 Invoke(string key, Vector3 defaultValue)
        {
            return new Vector3(PlayerPrefs.GetFloat(key + ".x", defaultValue.x),
                PlayerPrefs.GetFloat(key + ".y", defaultValue.y), PlayerPrefs.GetFloat(key + ".z", defaultValue.z));
        }
    }

    [Name("getInt(load)")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("取档int")]
    public class G_GetInt : PureFunctionNode<int, string, int>
    {
        public override int Invoke(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }
    }

    [Name("getString(load)")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("取档String")]
    public class G_GetString : PureFunctionNode<string, string, string>
    {
        public override string Invoke(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }
    }

    [Name("save")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("保存所有更改")]
    public class G_Save : CallableActionNode
    {
        public override void Invoke()
        {
            PlayerPrefs.Save();
        }
    }

    [Name("deleteKey")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("删除key和对应的值")]
    public class G_DeleteKey : CallableActionNode<string>
    {
        public override void Invoke(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

    [Name("deleteAll")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("删除key和对应的值")]
    public class G_DeleteAll : CallableActionNode
    {
        public override void Invoke()
        {
            PlayerPrefs.DeleteAll();
        }
    }

    [Name("hasKey")]
    [Category("UnityEngine/PlayerPrefs")]
    [Description("是否存在该key值")]
    public class G_HasKey : PureFunctionNode<bool, string>
    {
        public override bool Invoke(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }

    #endregion
}