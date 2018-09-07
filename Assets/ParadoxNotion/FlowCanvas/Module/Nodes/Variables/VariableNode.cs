using ParadoxNotion.Design;

namespace FlowCanvas.Nodes{

	[Category("Variables")]
	abstract public class VariableNode : FlowNode {
#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_variable;
        }
#endif
        ///For setting the default variable
        abstract public void SetVariable(object o);
	}
}