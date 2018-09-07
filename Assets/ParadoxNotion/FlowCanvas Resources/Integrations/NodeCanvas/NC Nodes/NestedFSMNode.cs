using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion;
using ParadoxNotion.Design;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace FlowCanvas.Nodes
{

	[Name("FSM")]
	[Category("Nested")]
    [ContextDefinedInputs(typeof(Flow))]
    [ContextDefinedOutputs(typeof(Flow))]
    [Description("Execute a nested FSM OnEnter and Stop that FSM OnExit. This state is Finished when the nested FSM is finished as well")]
	public class NestedFSMNode : FlowNode, IGraphAssignable{

		[SerializeField]
		protected BBParameter<FSM> _nestedFSM = null; //protected so that derived user types can be reflected correctly
		private Dictionary<FSM, FSM> instances = new Dictionary<FSM, FSM>();
		private FSM currentInstance = null;

        public string successEvent;
        public string failureEvent;

        ValueInput<Transform> nestedGraphOwner;
        public FSM nestedFSM{
			get {return _nestedFSM.value;}
			set {_nestedFSM.value = value;}
		}
#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_nested;
        }
#endif
        Graph IGraphAssignable.nestedGraph{
			get {return nestedFSM;}
			set {nestedFSM = (FSM)value;}
		}

		Graph[] IGraphAssignable.GetInstances(){ return instances.Values.ToArray(); }

        FlowOutput o;
        FlowOutput paused;
        FlowOutput stoped;
        FlowOutput resumed;
        ////
        protected override void RegisterPorts()
	    {
            nestedGraphOwner = AddValueInput<Transform>("NestedGraphOwner");
            AddFlowInput("Start",(f)=>
	        {
	            OnEnter();
                o.Call(f);}
            );
	        o = AddFlowOutput("Out");

	        AddFlowInput("Pause", (f) => {
                OnPause(); 
                paused.Call(f); });
	        paused = AddFlowOutput("Paused");

            AddFlowInput("Resume", (f) =>
            {
                //OnUnPause();
                //RegisterUpdate();
                if (currentInstance.isPaused)
                {
                    OnEnter();
                    resumed.Call(f);
                }
            });
            resumed = AddFlowOutput("Resumed");

            AddFlowInput("Stop", (f) =>
            {
                OnExit();
                stoped.Call(f); });
            stoped = AddFlowOutput("Stoped");

            AddValueOutput("isRunning",()=>currentInstance.isRunning);
            AddValueOutput("isPaused", () => currentInstance.isPaused);
        }

	    void OnEnter(){
			if (nestedFSM == null){
				return;
			}

			currentInstance = CheckInstance();
			currentInstance.StartGraph(graphAgent, graphBlackboard, true, OnFinish);
	        //RegisterUpdate();
	    }

		void OnUpdate(){
			currentInstance.UpdateGraph();
		}

		void OnExit(){
			if (currentInstance != null && (currentInstance.isRunning || currentInstance.isPaused) )
			{
			    //UnRegisterUpdate();
                currentInstance.Stop();
			}
		}

        void OnFinish(bool success)
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

                //Finish(success);
            }
        }

	    //private bool isRegisted = false;
	    //void RegisterUpdate()
	    //{   
     //       //if(isRegisted)
     //       //    return;; 

     //       // isRegisted = false;
     //       //MonoManager.current.onUpdate += OnUpdate;
     //   }
     //   void UnRegisterUpdate()
     //   {   
     //       //if(!isRegisted)
     //       //    return;
     //       //isRegisted = true;
     //       //MonoManager.current.onUpdate += OnUpdate;
     //   }

        void OnPause(){
			if (currentInstance != null){
				currentInstance.Pause();
			    //UnRegisterUpdate();
			}
		}

	    void OnUnPause()
	    {
            if (currentInstance != null&& currentInstance.isPaused)
            {
                if (nestedFSM == null)
                {
                    return;
                }
                currentInstance.StartGraph(graphAgent, graphBlackboard, true);
                //RegisterUpdate();
            }
        }


		FSM CheckInstance(){

			if (nestedFSM == currentInstance){
				return currentInstance;
			}

            //------------------------custom nested node graph owner
		    if (nestedGraphOwner != null && nestedGraphOwner.value != null)
                graph.agent = nestedGraphOwner.value;

                FSM instance = null;
			if (!instances.TryGetValue(nestedFSM, out instance)){
				instance = Graph.Clone<FSM>(nestedFSM);
				instances[nestedFSM] = instance;
			}

            instance.agent = graphAgent;
		    instance.blackboard = graphBlackboard;
			nestedFSM = instance;
			return instance;
		}

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR
        protected override void OnNodeGUI(){
			GUILayout.Label(string.Format("Sub FSM\n{0}", _nestedFSM) );
            if (nestedFSM != null)
            {
            }
            else
            {
                if (!Application.isPlaying && GUILayout.Button("CREATE BOUND FSM"))
                {
                    NestedUtility.CreateBoundNested<FSM>(this, graph);
                    GatherPorts();
                    nestedFSM.name = name;
                }
                if (!Application.isPlaying && GUILayout.Button("CREATE ASSET FSM"))
                {   
                    Node.CreateNested<FSM>(this);
                    GatherPorts();
                }
            }

            base.OnNodeGUI();
        }

		protected override void OnNodeInspectorGUI(){
            name = EditorGUILayout.TextField("FSM Name", name);
            NodeCanvas.Editor.BBParameterEditor.ParameterField("FSM", _nestedFSM);
            if (nestedFSM == null){
				return;
			}
		    //nestedFSM.name = name;
            var alpha1 = string.IsNullOrEmpty(successEvent) ? 0.5f : 1;
            var alpha2 = string.IsNullOrEmpty(failureEvent) ? 0.5f : 1;
            GUILayout.BeginVertical("box");
            GUI.color = new Color(1, 1, 1, alpha1);
            successEvent = EditorGUILayout.TextField("Success Status Event", successEvent);
            GUI.color = new Color(1, 1, 1, alpha2);
            failureEvent = EditorGUILayout.TextField("Failure Status Event", failureEvent);
            GUILayout.EndVertical();
            GUI.color = Color.white;

            var defParams = nestedFSM.GetDefinedParameters();
	    	if (defParams.Length != 0){

		    	EditorUtils.TitledSeparator("Defined Nested BT Parameters");
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
                if (GUILayout.Button("Check/Create Blackboard Variables"))
                {
                    nestedFSM.PromoteDefinedParametersToVariables(graphBlackboard);
                }
            }
            //---------------------更新Asset资源名称

            if (!Application.isPlaying && GUILayout.Button("RefreshAssetName"))
            {
                nestedFSM.name = name;
                if (AssetDatabase.IsMainAsset(nestedFSM) || AssetDatabase.IsSubAsset(nestedFSM))
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(nestedFSM));
            }
        }
#endif
    }
}