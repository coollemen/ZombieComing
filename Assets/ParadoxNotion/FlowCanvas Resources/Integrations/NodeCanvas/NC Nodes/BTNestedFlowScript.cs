using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

using FlowCanvas;
using NodeCanvas.StateMachines;
using FlowCanvas.Nodes;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Object = UnityEngine.Object;

namespace NodeCanvas.BehaviourTrees{

	[Name("FlowScript")]
	[Category("Nested")]
	[Description("Executes a nested FlowScript. Returns Running while the FlowScript is active. You can Finish the FlowScript with the 'Finish' node and return Success or Failure")]
	public class BTNestedFlowScript : BTNode, IGraphAssignable{
        public BBParameter<Transform> NestedGraphOwner;
        [SerializeField]
		private BBParameter<FlowScript> _flowScript = null;
		private Dictionary<FlowScript, FlowScript> instances = new Dictionary<FlowScript, FlowScript>();

		public FlowScript flowScript{
			get {return _flowScript.value;}
			set {_flowScript.value = value;}
		}

		Graph IGraphAssignable.nestedGraph{
			get {return flowScript;}
			set {flowScript = (FlowScript)value;}
		}

		Graph[] IGraphAssignable.GetInstances(){ return instances.Values.ToArray(); }
        //-----------
#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_nested;
        }
#endif

        protected override Status OnExecute(Component agent, IBlackboard blackboard){
			
			if (flowScript == null){
				return Status.Failure;
			}

			if (status == Status.Resting){
				CheckInstance();
				status = Status.Running;
				flowScript.StartGraph(agent, blackboard, false, OnFlowScriptFinished);
			}

			if (status == Status.Running){
				flowScript.UpdateGraph();
			}

			return status;
		}

		void OnFlowScriptFinished(bool success){
			if (status == Status.Running){
				status = success? Status.Success : Status.Failure;
			}
		}

		protected override void OnReset(){
			if (IsInstance(flowScript)){
				flowScript.Stop();
			}
		}

		public override void OnGraphPaused(){
			if (IsInstance(flowScript)){
				flowScript.Pause();
			}
		}

		public override void OnGraphStoped(){
			if (IsInstance(flowScript)){
				flowScript.Stop();
			}
		}

		bool IsInstance(FlowScript fs){
			return instances.Values.Contains(fs);
		}

		void CheckInstance(){

			if (IsInstance(flowScript)){
				return;
			}
            //-----------------------Custom NestedGraphOwner
            if (NestedGraphOwner.value != null)
            {
                graph.agent = NestedGraphOwner.value;
            }
            else
            {
                NestedGraphOwner.value = graph.agent.transform;
            }

            FlowScript instance = null;
			if (!instances.TryGetValue(flowScript, out instance)){
				instance = Graph.Clone<FlowScript>(flowScript);
				instances[flowScript] = instance;
			}

            instance.agent = graphAgent;
		    instance.blackboard = graphBlackboard;
			flowScript = instance;
		}

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR
        protected override void OnNodeGUI(){
			
			GUILayout.Label(_flowScript.ToString());

            if (flowScript != null)
            {

            }
            else
            {
                if (!Application.isPlaying && GUILayout.Button("CREATE BOUND FlowScirpt"))
                {
                    NestedUtility.CreateBoundNested<FlowScript>(this, graph);
                    flowScript.name = name;
                    CreateDefaultNode();
                }
                if (!Application.isPlaying && GUILayout.Button("CREATE ASSET FlowScirpt"))
                {
                    Node.CreateNested<FlowScript>(this);
                    CreateDefaultNode();
                }
            }
        }
        void CreateDefaultNode()
        {
            if (flowScript == null)
                return;
            flowScript.AddNode<ConstructionEvent>(new Vector2(-flowScript.translation.x + 300, -flowScript.translation.y + 0));
            flowScript.AddNode<EnableEvent>(new Vector2(-flowScript.translation.x + 300, -flowScript.translation.y + 200));
            flowScript.AddNode<Finish>(new Vector2(-flowScript.translation.x + 700, -flowScript.translation.y + 200));
            flowScript.AddNode<UpdateEvent>(new Vector2(-flowScript.translation.x + 300, -flowScript.translation.y + 400));
            flowScript.AddNode<DisableEvent>(new Vector2(-flowScript.translation.x + 300, -flowScript.translation.y + 600));
        }
        protected override void OnNodeInspectorGUI(){
            NodeCanvas.Editor.BBParameterEditor.ParameterField("NestedGraphOwner", NestedGraphOwner);
            NodeCanvas.Editor.BBParameterEditor.ParameterField("FlowScript", _flowScript);

			if (flowScript == null){
				return;
			}
            //flowScript.name = this.name;
            var defParams = flowScript.GetDefinedParameters();
	    	if (defParams.Length != 0){

		    	EditorUtils.TitledSeparator("Defined Nested FlowScript Parameters");
		    	GUI.color = Color.yellow;
		    	UnityEditor.EditorGUILayout.LabelField("Name", "Type");
				GUI.color = Color.white;
		    	var added = new List<string>();
		    	foreach(var bbVar in defParams){
		    		if (!added.Contains(bbVar.name)){
			    		UnityEditor.EditorGUILayout.LabelField(bbVar.name, bbVar.varType.FriendlyName());
			    		added.Add(bbVar.name);
			    	}
		    	}
		    	if (GUILayout.Button("Check/Create Blackboard Variables")){
		    		flowScript.PromoteDefinedParametersToVariables(graphBlackboard);
		    	}
		    }
            //---------------------更新Asset资源名称

            if (!Application.isPlaying && GUILayout.Button("RefreshAssetName"))
            {
                flowScript.name = name;
                if (AssetDatabase.IsMainAsset(flowScript) || AssetDatabase.IsSubAsset(flowScript))
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(flowScript));
            }
        }
		#endif
	}
}