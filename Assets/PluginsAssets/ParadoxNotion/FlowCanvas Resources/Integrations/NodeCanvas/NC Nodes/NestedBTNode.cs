using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;


namespace FlowCanvas.Nodes{

	[Name("Behavior Tree")]
	[Category("Nested")]
    [ContextDefinedInputs(typeof(Flow))]
    [ContextDefinedOutputs(typeof(Flow))]
    [Description("Execute a nested BehaviourTree OnEnter and Stop that BehaviourTree OnExit. This state is Finished when the nested BehaviourTree is finished as well")]
	public class NestedBTNode : FlowNode, IGraphAssignable{

		[SerializeField]
		protected BBParameter<BehaviourTree> _nestedBT = null; //protected so that derived user types can be reflected correctly
		private Dictionary<BehaviourTree, BehaviourTree> instances = new Dictionary<BehaviourTree, BehaviourTree>();
		private BehaviourTree currentInstance = null;

        public string successEvent;
        public string failureEvent;
        //[SerializeField]
        ValueInput<Transform> nestedGraphOwner;
        public BehaviourTree nestedBT
        {
			get {return _nestedBT.value;}
			set {_nestedBT.value = value;}
		}

		Graph IGraphAssignable.nestedGraph{
			get {return nestedBT;}
			set {nestedBT = (BehaviourTree)value;}
		}
#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_nested;
        }
#endif
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
			if (nestedBT == null){
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
                if (nestedBT == null)
                {
                    return;
                }
                currentInstance.StartGraph(graphAgent, graphBlackboard, true);
                //RegisterUpdate();
            }
        }


        BehaviourTree CheckInstance(){

			if (nestedBT == currentInstance){
				return currentInstance;
			}
            //------------------------custom nested node graph owner
            if (nestedGraphOwner != null && nestedGraphOwner.value != null)
                graph.agent = nestedGraphOwner.value;

            BehaviourTree instance = null;
			if (!instances.TryGetValue(nestedBT, out instance)){
				instance = Graph.Clone<BehaviourTree>(nestedBT);
				instances[nestedBT] = instance;
			}

            instance.agent = graphAgent;
		    instance.blackboard = graphBlackboard;
			nestedBT = instance;
			return instance;
		}

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR
      

        protected override void OnNodeGUI(){
			GUILayout.Label(string.Format("Sub BehaviourTree\n{0}", _nestedBT) );
            if (nestedBT != null)
            {

            }
            else
            {
                if (!Application.isPlaying && GUILayout.Button("CREATE BOUND BehaviourTree"))
                {
                    NestedUtility.CreateBoundNested<BehaviourTree>(this, graph);
                    GatherPorts();
                    nestedBT.name = name;
                }
                if (!Application.isPlaying && GUILayout.Button("CREATE ASSET BehaviourTree"))
                {
                    Node.CreateNested<BehaviourTree>(this);
                    GatherPorts();
                }
            }

            base.OnNodeGUI();
        }

		protected override void OnNodeInspectorGUI(){
            name = EditorGUILayout.TextField("Behavior Name", name);
            NodeCanvas.Editor.BBParameterEditor.ParameterField("BehaviourTree", _nestedBT);
            //nestedBT = (BehaviourTree)EditorGUILayout.ObjectField(nestedBT, typeof(BehaviourTree),true);
            if (nestedBT == null){
				return;
			}
            //nestedBT.name = this.name;
            var alpha1 = string.IsNullOrEmpty(successEvent) ? 0.5f : 1;
            var alpha2 = string.IsNullOrEmpty(failureEvent) ? 0.5f : 1;
            GUILayout.BeginVertical("box");
            GUI.color = new Color(1, 1, 1, alpha1);
            successEvent = EditorGUILayout.TextField("Success Status Event", successEvent);
            GUI.color = new Color(1, 1, 1, alpha2);
            failureEvent = EditorGUILayout.TextField("Failure Status Event", failureEvent);
            GUILayout.EndVertical();
            GUI.color = Color.white;

            var defParams = nestedBT.GetDefinedParameters();
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
                    nestedBT.PromoteDefinedParametersToVariables(graphBlackboard);
                }
            }
            //---------------------更新Asset资源名称

            if (!Application.isPlaying && GUILayout.Button("RefreshAssetName"))
            {
                nestedBT.name = name;
                if (AssetDatabase.IsMainAsset(nestedBT) || AssetDatabase.IsSubAsset(nestedBT))
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(nestedBT));
            }
        }
#endif
    }
}