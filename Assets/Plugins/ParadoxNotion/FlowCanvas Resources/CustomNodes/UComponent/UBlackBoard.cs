using System;
using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;



namespace FlowCanvas.Nodes
{

    #region BlackBoard
    [Name("ContainValueNameInBlackBoard")]
    [Category("UnityEngine/BlackBoard")]
    [Description("黑板中是否有此变量名")]
    public class G_ContainValueNameInBlackBoard : PureFunctionNode<bool, string, Blackboard>
    {
        public override bool Invoke(string valueName, Blackboard a)
        {
            return a.variables.ContainsKey(valueName);
        }
    }

    [Name("SaveBlackBoard")]
    [Category("UnityEngine/BlackBoard")]
    [Description("保存黑板数据,返回json")]
    public class S_SaveBlackBoard : CallableFunctionNode<string,Blackboard,string>
    {
        public override string Invoke(Blackboard a, string saveKey)
        {
           return a.Save(saveKey);
        }
    }

    [Name("LoadBlackBoard")]
    [Category("UnityEngine/BlackBoard")]
    [Description("保存黑板数据,载入成功返回true")]
    public class G_LoadBlackBoard : CallableFunctionNode<bool,Blackboard, string>
    {
        public override bool Invoke(Blackboard a, string saveKey)
        {
           return a.Load(saveKey);
        }
    }
    #endregion
}
