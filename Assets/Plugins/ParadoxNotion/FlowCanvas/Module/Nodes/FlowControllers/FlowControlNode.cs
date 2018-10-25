using ParadoxNotion.Design;

namespace FlowCanvas.Nodes {

    [Category("Flow Controllers")]
    [Color("EE9A00")]
    [ContextDefinedInputs(typeof(Flow))]
    [ContextDefinedOutputs(typeof(Flow))]
    abstract public class FlowControlNode : FlowNode
    {
#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_flowcontrol;
        }
#endif 
    }
}