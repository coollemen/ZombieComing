using UnityEngine;
using System.Collections.Generic;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes{

	[Category("Functions/Utility")]
    //[Name("Switch")]
    [ContextDefinedInputs(typeof(int))]
    public class Switch<T> : FlowNode{

		[SerializeField]
		private int _portCount = 4;
		public int portCount{
			get {return _portCount;}
		    set
		    {
		        _portCount = value;
		        GatherPorts();
		    }
		}

		protected override void RegisterPorts(){
			var ins = new List<ValueInput<T>>();
			for (var i = 0; i < portCount; i++){
				ins.Add( AddValueInput<T>(i.ToString()) );
			}
			var index = AddValueInput<int>("Index");
			AddValueOutput<T>("Value", ()=>{ return ins[index.value].value; });
		}

#if UNITY_EDITOR
        protected override void OnNodeGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Space(10f);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                portCount++;
            }
            GUILayout.Space(10f);
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                portCount--;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
            GUILayout.EndVertical();
            base.OnNodeGUI();
        }

#endif
    }
}