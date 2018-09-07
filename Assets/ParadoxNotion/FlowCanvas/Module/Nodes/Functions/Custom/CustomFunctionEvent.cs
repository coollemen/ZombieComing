using System.Collections.Generic;
using System.Linq;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using NodeCanvas.Framework;

namespace FlowCanvas.Nodes{

    [Name("New Custom Function", 10)]
    [Description("A custom function, defined by any number of parameters and an optional return value. It can be called using the 'Call Custom Function' node. To return a value, the 'Return' node should be used.")]
    [Category("Functions/Custom")]
    [ParadoxNotion.Serialization.DeserializeFrom("FlowCanvas.Nodes.RelayFlowOutput")]
    public class CustomFunctionEvent : EventNode, IInvokable, IEditorMenuCallbackReceiver {

        [Tooltip("The identifier name of the function")] [DelayedField]
        public BBParameter<string> identifier = "MyFunction";
        [SerializeField]
        private List<DynamicPortDefinition> _parameters = new List<DynamicPortDefinition>();
        [SerializeField]
        private DynamicPortDefinition _returns = new DynamicPortDefinition("Value", null);

        private object[] args;
        private object returnValue;
        private FlowOutput onInvoke;
#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_nested;
        }
#endif
        ///the parameters port definition
        public List<DynamicPortDefinition> parameters{
            get {return _parameters;}
            private set {_parameters = value;}
        }

        ///the return port definition
        public DynamicPortDefinition returns{
            get {return _returns;}
            private set {_returns = value;}
        }

        //shortcut
        private System.Type returnType{
            get {return returns.type; }
        }

        //shortcut
        private System.Type[] parameterTypes{
            get {return parameters.Select(p => p.type).ToArray();}
        }

        public override string name{
            get { return string.Format("➥ {0}{1}", identifier.value, returns.type == null ? "" : " ："+returns.type.FriendlyName()); }
        }

        protected override void RegisterPorts(){
            onInvoke = AddFlowOutput(" ");
            for (var _i = 0; _i < parameters.Count; _i++){
                var i = _i;
                var parameter = parameters[i];
                AddValueOutput(parameter.name, parameter.ID, parameter.type, ()=>{ return args[i]; });
            }
        }

        string IInvokable.GetInvocationID(){ return identifier.value; }
        object IInvokable.Invoke(params object[] args){ return Invoke(new Flow(), args); }

        ///Invokes the function and return it's return value
        public object Invoke(Flow f, params object[] args){
            this.args = args;
            f.ReturnType = returns.type;
            f.Return = (o)=>{ this.returnValue = o; };
            onInvoke.Call(f);
            return returnValue;
        }

        ///Invokes the function and callbacks when a Return node is hit.
        public void InvokeAsync(Flow f, FlowHandler Callback, params object[] args){
	        this.args = args;
	        Flow functionFlow=f;
	        functionFlow.Return=null;
            functionFlow.ReturnType = returns.type;
	        functionFlow.Return = (o)=>{ this.returnValue = o; Callback(f); };
	        onInvoke.Call(functionFlow);            
        }

        ///Returns the function's last return value
        public object GetReturnValue(){
            return returnValue;
        }

        //Add a parameter to the function
        void AddParameter(System.Type type){
            parameters.Add( new DynamicPortDefinition(type.FriendlyName(), type) );
            GatherPortsUpdateRefs();
        }

        //Helper
        void GatherPortsUpdateRefs(){
            this.GatherPorts();
            foreach(var call in flowGraph.GetAllNodesOfType<CustomFunctionCall>().Where(n => n.sourceFunction == this)){
                call.GatherPorts();
            }
        }

        ///Adds a new input port definition to the Macro
        public bool AddInputDefinition(DynamicPortDefinition def)
        {
            if (parameters.Find(d => d.ID == def.ID) == null)
            {
                parameters.Add(def);
                return true;
            }
            return false;
        }
        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        protected override UnityEditor.GenericMenu OnDragAndDropPortContextMenu(UnityEditor.GenericMenu menu, Port port)
        {
            if (port is ValueInput)
            {
                menu.AddItem(new GUIContent(string.Format("Promote to new Input '{0}'", port.name)), false, () =>
                {
                    var def = new DynamicPortDefinition(port.name, port.type);
                    if (AddInputDefinition(def))
                    {
                        GatherPorts();
                        BinderConnection.Create(GetOutputPort(def.ID), port);
                    }
                });
            }
            return menu;
        }
        void IEditorMenuCallbackReceiver.OnMenu(UnityEditor.GenericMenu menu, Vector2 pos, Port sourcePort, object dropInstance){
            if (sourcePort != null){
                if (sourcePort is ValueInput && !sourcePort.type.IsAssignableFrom(returnType)){ return; }
                if (sourcePort is ValueOutput && !parameterTypes.Any(t => t.IsAssignableFrom(sourcePort.type))){ return; }
            }
			menu.AddItem(new GUIContent(string.Format("Functions/Custom/Call '{0}()'", identifier.value) ), false, ()=>
            {
                var node = flowGraph.AddNode<CustomFunctionCall>(pos);
                node.SetFunction(this);
                FlowGraphExtensions.Finalize(node, sourcePort, null);
            });
        }


        bool reName=false;
        string renameString;
	    Dictionary<Graph,List<FunctionCall>> callFunctionNodeDict=new Dictionary<Graph,List<FunctionCall>>();
	    Dictionary<Graph,List<CustomFunctionCall>> callCustomFunctionNodeDict=new Dictionary<Graph,List<CustomFunctionCall>>();
        protected override void OnNodeInspectorGUI(){
            base.OnNodeInspectorGUI();

            if (GUILayout.Button("Add Parameter")){
                EditorUtils.GetPreferedTypesSelectionMenu(typeof(object), (t)=>{ AddParameter(t); } ).ShowAsContext();
            }

            UnityEditor.EditorGUI.BeginChangeCheck();

			var options = new EditorUtils.ReorderableListOptions();
			options.allowRemove = true;
			EditorUtils.ReorderableList(parameters, options, (i, r)=>
            {
                var parameter = parameters[i];
                GUILayout.BeginHorizontal();
                parameter.name = UnityEditor.EditorGUILayout.DelayedTextField(parameter.name, GUILayout.Width(150), GUILayout.ExpandWidth(true));
                EditorUtils.ButtonTypePopup("", parameter.type, (t)=>{ parameter.type = t; GatherPortsUpdateRefs(); });
                GUILayout.EndHorizontal();
			});

            EditorUtils.Separator();

            EditorUtils.ButtonTypePopup("Return Type", returns.type, (t)=>{ returns.type = t; GatherPortsUpdateRefs(); } );
            if (GUILayout.Button("ResetType"))
            {
                returns.type = null; GatherPortsUpdateRefs();
            }
            if (UnityEditor.EditorGUI.EndChangeCheck()){
                GatherPortsUpdateRefs();
            }
            GUILayout.Space(20f);

            if (!reName&&GUILayout.Button("Rename"))
            {
                reName=true;
                renameString=identifier.value;
            }

            if (reName)
            {   
                GUILayout.BeginHorizontal();
                renameString= GUILayout.TextField(renameString);            
                if (GUILayout.Button("Done"))
                {   
                    identifier.value= renameString;
                    callFunctionNodeDict.ForEach(x=>x.Value.ForEach(y=>y.functionName=identifier.value));
                    reName=false;
                }
                GUILayout.EndHorizontal();
            }

	        if (!string.IsNullOrEmpty(identifier.value)&&GUILayout.Button("Find Call FunctionNode"))
	        {	
	        	FindAllRefNode();
	        }
	        
	        if (callFunctionNodeDict.Count>0)
	        {	
	        	GUILayout.BeginVertical();
	        	foreach (var item in callFunctionNodeDict)
	        	{	
		        	GUILayout.BeginVertical();
		        	GUILayout.Label(item.Key.name);
		        	
		        	item.Value.ForEach(z=>{
			        	if (GUILayout.Button(string.Format("callFunctionNode:{0}",z.functionName)))
			        	{   

                            if (item.Key.agent.gameObject!=null)
				        	{
					        	UnityEditor.Selection.activeGameObject=item.Key.agent.gameObject;
				        	}
				        	UnityEditor.Selection.selectionChanged();
				        	
				        	if (ParadoxNotion.Services.MonoManager.current == null)
				        	{
					        	var _current = UnityEngine.Object.FindObjectOfType<ParadoxNotion.Services.MonoManager>();
					        	if (_current != null)
					        	{
						        	UnityEngine.Object.DestroyImmediate(_current.gameObject);
					        	}

					        	var current = new GameObject("_MonoManager").AddComponent<ParadoxNotion.Services.MonoManager>();
					        	current.StartCoroutine(waitForGraphChange(z));
				        	}
				        	else
				        	{
					        	ParadoxNotion.Services.MonoManager.current.StartCoroutine(waitForGraphChange(z));
				        	}
				        	
			        	}
		        	});
	        		
	        		GUILayout.EndVertical();
	        	}
	        	
	        	foreach (var item in callCustomFunctionNodeDict)
	        	{	
		        	GUILayout.BeginVertical();
		        	GUILayout.Label(item.Key.name);
		        	
		        	item.Value.ForEach(z=>{
			        	if (GUILayout.Button(string.Format("callFunctionNode:{0}",z.FunctionName)))
			        	{
				        	UnityEditor.Selection.activeObject=item.Key;
				        	UnityEditor.Selection.selectionChanged();
				        	
				        	if (ParadoxNotion.Services.MonoManager.current == null)
				        	{
					        	var _current = UnityEngine.Object.FindObjectOfType<ParadoxNotion.Services.MonoManager>();
					        	if (_current != null)
					        	{
						        	UnityEngine.Object.DestroyImmediate(_current.gameObject);
					        	}

					        	var current = new GameObject("_MonoManager").AddComponent<ParadoxNotion.Services.MonoManager>();
					        	current.StartCoroutine(waitForGraphChange(z));
				        	}
				        	else
				        	{
					        	ParadoxNotion.Services.MonoManager.current.StartCoroutine(waitForGraphChange(z));
				        	}

				        	NodeCanvas.Editor.GraphEditorUtility.activeElement =item.Value;
			        	}
		        	});
	        		
	        		GUILayout.EndVertical();
	        	}
	        	
	        	GUILayout.EndVertical();
	        }
        }
	    System.Collections.IEnumerator waitForGraphChange(FlowNode n)
	    {
		    //Debug.Log("double press F to focus:" + n
		    yield return new WaitForSecondsRealtime(0.15f);
		    NodeCanvas.Editor.GraphEditorUtility.activeElement = n;
	    }

        void FindAllRefNode()
        {
                callFunctionNodeDict=new Dictionary<Graph, List<FunctionCall>>();
	        	var allGraphOwner= UnityEngine.Object.FindObjectsOfType<GraphOwner>();
		        allGraphOwner.ForEach(x=>{
		        	if (x.graph!=null)
		        	{
		        		var rootGraph=x.graph;
		        		List<Graph> allGraph=new List<Graph>();
		        		allGraph.Add(rootGraph);
		        		var childGraph=rootGraph.GetAllNestedGraphs<Graph>(true);
		        		if (childGraph.Count>0)
		        		{
			        		allGraph.AddRange(childGraph);
		        		}
		        		
		        		allGraph.ForEach(y=>{
			        		var targetFunctionNode= y.GetAllNodesOfType<FunctionCall>().FindAll(z=>z.functionName==identifier.value);
			        		if (targetFunctionNode!=null&&targetFunctionNode.Count>0)
			        		{	
				        		callFunctionNodeDict.Add(y,targetFunctionNode);
			        		}	
			        		var targetCustomFunctionNode= y.GetAllNodesOfType<CustomFunctionCall>().FindAll(z=>z.FunctionName==identifier.value);
			        		if (targetCustomFunctionNode!=null&&targetCustomFunctionNode.Count>0)
			        		{	
				        		callCustomFunctionNodeDict.Add(y,targetCustomFunctionNode);
			        		}	
		        		});		        		
		        	}
		        });
        }
        #endif
        ///----------------------------------------------------------------------------------------------
    }
}