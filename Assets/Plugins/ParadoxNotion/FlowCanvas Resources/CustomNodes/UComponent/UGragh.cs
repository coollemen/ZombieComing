using ParadoxNotion.Design;
using NodeCanvas.Framework;


namespace FlowCanvas.Nodes
{

    #region Graph

    [Name("loadGraph")]
    [Category("UnityEngine/Graph")]
    [Description("载入新的graph Asset,消除黑板上的数据")]
    public class S_LoadGraph : CallableFunctionNode<GraphOwner,GraphOwner, FlowScript, bool>
    {
        public override GraphOwner Invoke(GraphOwner targetGraphOwner, FlowScript asset, bool playAfterLoad)
        {
            if (targetGraphOwner.blackboard == null)
            {
                targetGraphOwner.gameObject.AddComponent<Blackboard>();
            }
            targetGraphOwner.blackboard.variables.Clear();
            targetGraphOwner.StopBehaviour();

            targetGraphOwner.graph = asset;

            if (playAfterLoad)
                targetGraphOwner.StartBehaviour();

            return targetGraphOwner;
        }
    }

    [Name("pauseGraph")]
    [Category("UnityEngine/Graph")]
    [Description("暂停graph运行")]
    public class S_PauseGraph : CallableFunctionNode<GraphOwner, GraphOwner>
    {
        public override GraphOwner Invoke(GraphOwner targetGraphOwner)
        {
            targetGraphOwner.PauseBehaviour();
            return targetGraphOwner;
        }
    }

    [Name("stopGraph")]
    [Category("UnityEngine/Graph")]
    [Description("停止graph运行")]
    public class S_StopGraph : CallableFunctionNode<GraphOwner, GraphOwner>
    {
        public override GraphOwner Invoke(GraphOwner targetGraphOwner)
        {
            targetGraphOwner.StopBehaviour();
            return targetGraphOwner;
        }
    }

    [Name("startGraph")]
    [Category("UnityEngine/Graph")]
    [Description("graph开始运行")]
    public class S_StartGraph : CallableFunctionNode<GraphOwner, GraphOwner>
    {
        public override GraphOwner Invoke(GraphOwner targetGraphOwner)
        {
            targetGraphOwner.StartBehaviour();
            return targetGraphOwner;
        }
    }

    [Name("updateGraph")]
    [Category("UnityEngine/Graph")]
    [Description("graph 手动更新")]
    public class S_UpdateGraph : CallableFunctionNode<GraphOwner, GraphOwner> {
        public override GraphOwner Invoke(GraphOwner targetGraphOwner)
        {
            targetGraphOwner.UpdateBehaviour();
            return targetGraphOwner;
        }
    }

    #endregion
}