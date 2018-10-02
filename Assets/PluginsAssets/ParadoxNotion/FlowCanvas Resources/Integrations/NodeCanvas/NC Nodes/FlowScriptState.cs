using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

using FlowCanvas;
using Object = UnityEngine.Object;
using FlowCanvas.Nodes;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace NodeCanvas.StateMachines
{

    [Name("FlowScript")]
    [Category("Nested")]
    [Description(
         "Adds a FlowCanvas FlowScript as a nested State of the FSM. The FlowScript State is never finished by itself, unless a 'Finish' node is used in the FlowScript. Success/Failure events can optionlay be used alongside with a CheckEvent on state transitions to catch whether the FlowScript Finished in success or failure."
     )]
    public class FlowScriptState : FSMState, IGraphAssignable
    {
        public BBParameter<Transform> NestedGraphOwner;
        [SerializeField] private BBParameter<FlowScript> _flowScript = null;
        private Dictionary<FlowScript, FlowScript> instances = new Dictionary<FlowScript, FlowScript>();

        public string successEvent;
        public string failureEvent;

        public FlowScript flowScript
        {
            get { return _flowScript.value; }
            set { _flowScript.value = value; }
        }
        //-----------
#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_nested;
        }
#endif
        Graph IGraphAssignable.nestedGraph
        {
            get { return flowScript; }
            set { flowScript = (FlowScript) value; }
        }

        Graph[] IGraphAssignable.GetInstances()
        {
            return instances.Values.ToArray();
        }


        protected override void OnEnter()
        {

            if (flowScript == null)
            {
                Finish(false);
                return;
            }

            CheckInstance();
            flowScript.StartGraph(graphAgent, graphBlackboard, false, OnFlowScriptFinished);
        }

        protected override void OnUpdate()
        {
            flowScript.UpdateGraph();
        }

        void OnFlowScriptFinished(bool success)
        {
            if (this.status == Status.Running)
            {
                if (!string.IsNullOrEmpty(successEvent) && success)
                {
                    SendEvent(new EventData(successEvent));
                }

                if (!string.IsNullOrEmpty(failureEvent) && !success)
                {
                    SendEvent(new EventData(failureEvent));
                }

                Finish(success);
            }
        }

        protected override void OnExit()
        {
            if (IsInstance(flowScript) && (flowScript.isRunning || flowScript.isPaused))
            {
                flowScript.Stop();
            }
        }

        protected override void OnPause()
        {
            if (IsInstance(flowScript) && flowScript.isRunning)
            {
                flowScript.Pause();
            }
        }

        bool IsInstance(FlowScript fs)
        {
            return instances.Values.Contains(fs);
        }

        void CheckInstance()
        {

            if (IsInstance(flowScript))
            {
                return;
            }
            if (NestedGraphOwner.value != null)
            {
                graph.agent = NestedGraphOwner.value;
            }
            else
            {
                NestedGraphOwner.value = graph.agent.transform;
            }
            FlowScript instance = null;
            if (!instances.TryGetValue(flowScript, out instance))
            {
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

        protected override void OnNodeGUI()
        {

            GUILayout.Label(_flowScript.ToString());

            if (flowScript != null){

			} else {
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
            flowScript.AddNode<Finish>(new Vector2(-flowScript.translation.x +700, -flowScript.translation.y + 200));
            flowScript.AddNode<UpdateEvent>(new Vector2(-flowScript.translation.x + 300, -flowScript.translation.y + 400));
            flowScript.AddNode<DisableEvent>(new Vector2(-flowScript.translation.x + 300, -flowScript.translation.y + 600));
        }

		protected override void OnNodeInspectorGUI(){

			ShowBaseFSMInspectorGUI();
            NodeCanvas.Editor.BBParameterEditor.ParameterField("NestedGraphOwner", NestedGraphOwner);
            NodeCanvas.Editor.BBParameterEditor.ParameterField("FlowScript", _flowScript);
            if (flowScript == null)
            {
                return;
            }
            var alpha1 = string.IsNullOrEmpty(successEvent)? 0.5f : 1;
			var alpha2 = string.IsNullOrEmpty(failureEvent)? 0.5f : 1;
			GUILayout.BeginVertical("box");
			GUI.color = new Color(1,1,1,alpha1);
			successEvent = UnityEditor.EditorGUILayout.TextField("Success Event", successEvent);
			GUI.color = new Color(1,1,1,alpha2);
			failureEvent = UnityEditor.EditorGUILayout.TextField("Failure Event", failureEvent);
			GUILayout.EndVertical();
			GUI.color = Color.white;

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