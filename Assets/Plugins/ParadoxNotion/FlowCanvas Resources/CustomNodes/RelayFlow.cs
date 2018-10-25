using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using NodeCanvas.Editor;
#endif
using NodeCanvas.Framework;
using UnityEngine;
using ParadoxNotion.Design;


namespace FlowCanvas.Nodes{

    [Name("RelayFlow_In",99)]
	[Description("Relay Flow In.")]
	[Category("Flow Controllers")]
    [ContextDefinedInputs(typeof(Flow))]
    public class RelayFlowInput : FlowControlNode
	{
		
		[Tooltip("The identifier name of the relay")] public string identifier = "RelayFlow";
		
		[HideInInspector]
		public FlowInput port { get; private set; }

        [SerializeField]
        List<RelayFlowOutput> sourceOutput = new List<RelayFlowOutput>();

        [Description("Use When Call From OnAwake node")]
        public bool lateFrame = false;
		
		public void RegisterOutput(RelayFlowOutput output)
		{
			if (!sourceOutput.Contains(output))
			{
                //Debug.Log("add new output:" + output.UID);
				sourceOutput.Add(output);
			}
		}
		
		public void UnRegisterOutput(RelayFlowOutput output)
		{
			if (sourceOutput.Contains(output))
			{
                //Debug.Log("remove new output:" + output.UID);
				sourceOutput.Remove(output);
			}
		}
		
		public override string name
		{
			get { return string.Format("@ {0}", identifier); }
		}
		
		
		protected override void RegisterPorts()
		{
			
			AddFlowInput("In", (f) =>
			{
				if (!lateFrame)
				{
					for (int i = 0; i < sourceOutput.Count; i++)
					{
						if (sourceOutput[i] != null)
							sourceOutput[i].port.Call(f);
					}
				}
				else
				{
					StartCoroutine(LateFrame(f));
				}
			});
		}
		
		IEnumerator LateFrame(Flow f)
		{
			yield return new WaitForEndOfFrame();
			
            //Debug.Log("delay callf");
			for (int i = 0; i < sourceOutput.Count; i++)
			{
				if (sourceOutput[i] != null)
					sourceOutput[i].port.Call(f);
			}
		}


        public override void OnGraphStarted()
        {
            //base.OnGraphPaused();
            RefreshListenereck();
        }
        void RefreshListenereck()
        {
            //sourceOutput.Clear();
            sourceOutput = graph.GetAllNodesOfType<RelayFlowOutput>().FindAll(i => i.sourceInputUID == this.UID);
        }
        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnNodeInspectorGUI()
		{   
			base.OnNodeInspectorGUI(); 
			
			GUILayout.Label("Current Output Listener Count: " + sourceOutput.Count);
			
			for (int i = 0; i < sourceOutput.Count; i++)
			{
				if (sourceOutput[i] != null)
				{
					if (GUILayout.Button("Node ID: " + sourceOutput[i].ID.ToString()))
					{
						GraphEditorUtility.activeElement = sourceOutput[i];
					}
				}
			}
			
			if (GUILayout.Button("Refresh Listener"))
			{
				RefreshListenereck();
			}
		}


#endif


    }
    [Name("RelayFlow_Out", 98)]
    [Description("Relay Flow Out.")]
	[Category("Flow Controllers")]
    [ContextDefinedOutputs(typeof(Flow))]
    public class RelayFlowOutput : FlowControlNode {
		
		[SerializeField]
		private string _sourceInputUID;
		public string sourceInputUID{
			get {return _sourceInputUID;}
			set {_sourceInputUID = value;}
		}
		
		[HideInInspector]
		public FlowOutput port { get; private set; }
        [SerializeField]
        private RelayFlowInput _sourceInput;
		private RelayFlowInput sourceInput
		{
			get
			{
				if (_sourceInput == null)
				{
					_sourceInput = graph.GetAllNodesOfType<RelayFlowInput>().FirstOrDefault(i => i.UID == sourceInputUID);
					if (_sourceInput == null)
					{
						//_sourceInput = new object();
					    return null;
					}
				}
				return _sourceInput as RelayFlowInput;
			}
			set { _sourceInput = value; }
		}
		
		public override string name{
			get {return string.Format("{0}", sourceInput != null? sourceInput.name.ToString() : "@ NONE");}
		}
		
		protected override void RegisterPorts()
		{
			port = AddFlowOutput("Out");
		}
		
		public override void OnDestroy(bool isReplace)
		{
			var relayInputs = graph.GetAllNodesOfType<RelayFlowInput>();
			var currentInput = relayInputs.FirstOrDefault(i => i.UID == sourceInputUID);
			if (currentInput!=null)
			{
				currentInput.UnRegisterOutput(this);
			}
		}
		
        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR
		
		protected override void OnNodeInspectorGUI(){
			var relayInputs = graph.GetAllNodesOfType<RelayFlowInput>();
			var currentInput = relayInputs.FirstOrDefault(i => i.UID == sourceInputUID);
			var newInput = EditorUtils.Popup<RelayFlowInput>("Relay Input Source", currentInput, relayInputs);
			if (newInput != currentInput){
                if(sourceInput!=null)sourceInput.UnRegisterOutput(this);

				sourceInputUID = newInput != null? newInput.UID : null;
				sourceInput = newInput != null? newInput : null;

                if (sourceInput != null) sourceInput.RegisterOutput(this);
            }
			
			if (currentInput != null)
			{
				if (GUILayout.Button("source"))
				{
					GraphEditorUtility.activeElement = currentInput;
				}
			}
		}
		
		#endif
	}
}