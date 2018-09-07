using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;


namespace FlowCanvas.Nodes
{

	[Name("FlowScript")]
	[Category("Nested")]
    [ContextDefinedInputs(typeof(Flow))]
    [ContextDefinedOutputs(typeof(Flow))]
    [Description("Execute a nested FlowScript OnEnter and Stop that FlowScript OnExit. This state is Finished when the nested FlowScript is finished as well")]
	public class NestedFCNode : FlowNode, IGraphAssignable{

		[SerializeField]
		protected BBParameter<FlowScript> _nestedFC = null; //protected so that derived user types can be reflected correctly

		private Dictionary<FlowScript, FlowScript> instances = new Dictionary<FlowScript, FlowScript>();
		private FlowScript currentInstance = null;

        public string successEvent;
        public string failureEvent;

	    public FlowScript nestedFC
        {
			get {return _nestedFC.value;}
			set {_nestedFC.value = value;}
		}

#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_nested;
        }
#endif

        Graph IGraphAssignable.nestedGraph{
			get {return nestedFC;}
			set {nestedFC = (FlowScript)value;}
		}

		Graph[] IGraphAssignable.GetInstances(){ return instances.Values.ToArray(); }

	    ValueInput<Transform> nestedGraphOwner;
        FlowOutput o;
        FlowOutput paused;
        FlowOutput stoped;
        FlowOutput resumed;
        ////
        /// 
        /// 
        protected override void RegisterPorts()
	    {
            nestedGraphOwner = AddValueInput<Transform>("NestedGraphOwner");

            AddFlowInput("Start",(f)=>
	        {
	            OnEnter();
                o.Call(f);
                }
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
			if (nestedFC == null){
				return;
			}

			currentInstance = CheckInstance();
			currentInstance.StartGraph(graph.agent, graphBlackboard, true, OnFinish);
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
                if (nestedFC == null)
                {
                    return;
                }
                currentInstance.StartGraph(graph.agent, graphBlackboard, true);
                //RegisterUpdate();
            }
        }


        FlowScript CheckInstance(){

			if (nestedFC == currentInstance){
				return currentInstance;
			}

            //------------------------custom nested node graph owner
            if (nestedGraphOwner != null && nestedGraphOwner.value != null)
                graph.agent = nestedGraphOwner.value;

            FlowScript instance = null;
			if (!instances.TryGetValue(nestedFC, out instance)){
				instance = Graph.Clone<FlowScript>(nestedFC);
				instances[nestedFC] = instance;
			}

            instance.agent = graphAgent;
		    instance.blackboard = graphBlackboard;
			nestedFC = instance;
			return instance;
		}

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnNodeGUI(){
			GUILayout.Label(string.Format("Sub FlowScript\n{0}", _nestedFC) );
            if (nestedFC != null)
            {

            }
            else
            {
                if ( !Application.isPlaying && GUILayout.Button("CREATE BOUND FlowScript"))
                {
                    NestedUtility.CreateBoundNested<FlowScript>(this, graph);
                    GatherPorts();
                    nestedFC.name = name;
                }
                if (!Application.isPlaying && GUILayout.Button("CREATE ASSET FlowScript"))
                {
                    GatherPorts();
                    CreateNested<FlowScript>(this);
                }
            }
            base.OnNodeGUI();
        }
        
        protected override void OnNodeInspectorGUI(){
            name = EditorGUILayout.TextField("FlowScript Name", name);
            NodeCanvas.Editor.BBParameterEditor.ParameterField("FlowScript", _nestedFC);
            if (nestedFC == null){
				return;
			}

            //nestedFC.name = this.name;
            var alpha1 = string.IsNullOrEmpty(successEvent) ? 0.5f : 1;
            var alpha2 = string.IsNullOrEmpty(failureEvent) ? 0.5f : 1;
            GUILayout.BeginVertical("box");
            GUI.color = new Color(1, 1, 1, alpha1);
            successEvent = EditorGUILayout.TextField("Success Status Event", successEvent);
            GUI.color = new Color(1, 1, 1, alpha2);
            failureEvent = EditorGUILayout.TextField("Failure Status Event", failureEvent);
            GUILayout.EndVertical();
            GUI.color = Color.white;

            var defParams = nestedFC.GetDefinedParameters();
	    	if (defParams.Length != 0){

		    	EditorUtils.TitledSeparator("Defined Nested Parameters");
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
                    nestedFC.PromoteDefinedParametersToVariables(graphBlackboard);
                }
            }
            //---------------------更新Asset资源名称

            if (!Application.isPlaying && GUILayout.Button("RefreshAssetName"))
            {
                nestedFC.name = name;
                if(AssetDatabase.IsMainAsset(nestedFC)||AssetDatabase.IsSubAsset(nestedFC))
                 AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(nestedFC));
            }
        }
#endif
    }
}