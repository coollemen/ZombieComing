using System.Collections.Generic;
using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes{

	[Description("Split the Flow in multiple directions. Calls all outputs in the same frame but in order")]
	public class Split : FlowControlNode {

		[SerializeField] [ExposeField]
		[GatherPortsCallback] [MinValue(2)] [DelayedField]
		private int _portCount = 2;
        public int portCount
        {
            get { return _portCount; }
            set
            {
                _portCount = value;
                GatherPorts();
            }
        }

        protected override void RegisterPorts(){
			var outs = new List<FlowOutput>();
			for (var i = 0; i < _portCount; i++){
				outs.Add( AddFlowOutput(i.ToString()) );
			}
			AddFlowInput("In", (f)=> {
				for (var i = 0; i < _portCount; i++){
					if (!graph.isRunning){
						break;
					}
					outs[i].Call(f);
				}
			});
		}

//#if UNITY_EDITOR
//        protected override void OnNodeGUI()
//		{	
//			base.OnNodeGUI();
//            GUILayout.BeginVertical();
//            GUILayout.Space(10f);

//            GUILayout.BeginHorizontal();
//            if (GUILayout.Button("+", GUILayout.Width(30)))
//            {
//                portCount++;
//            }
//            GUILayout.Space(10f);
//            if (GUILayout.Button("-", GUILayout.Width(30)))
//            {
//                portCount--;
//            }
//            GUILayout.EndHorizontal();
//            GUILayout.Space(10f);
//            GUILayout.EndVertical();

//        }

//#endif
    }
}