using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace NodeCanvas.StateMachines
{

    [Name("FSM")]
    [Category("Nested")]
    [Description("Execute a nested FSM OnEnter and Stop that FSM OnExit. This state is Finished when the nested FSM is finished as well")]
    public class NestedFSMState : FSMState, IGraphAssignable
    {
        public BBParameter<Transform> NestedGraphOwner;
        [SerializeField]
        protected BBParameter<FSM> _nestedFSM = null; //protected so that derived user types can be reflected correctly
        private Dictionary<FSM, FSM> instances = new Dictionary<FSM, FSM>();
        private FSM currentInstance = null;

        public string successEvent;
        public string failureEvent;
        [HideInInspector]
        public FSM nestedFSM
        {
            get { return _nestedFSM.value; }
            set { _nestedFSM.value = value; }
        }

        Graph IGraphAssignable.nestedGraph
        {
            get { return nestedFSM; }
            set { nestedFSM = (FSM)value; }
        }

        Graph[] IGraphAssignable.GetInstances() { return instances.Values.ToArray(); }

        ////
        //-----------
#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_nested;
        }
#endif
        protected override void OnEnter()
        {
            if (nestedFSM == null)
            {
                Finish(false);
                return;
            }

            currentInstance = CheckInstance();
            currentInstance.StartGraph(graphAgent, graphBlackboard, false, OnFinish);
        }

        protected override void OnUpdate()
        {
            currentInstance.UpdateGraph();
        }

        protected override void OnExit()
        {
            if (currentInstance != null && (currentInstance.isRunning || currentInstance.isPaused))
            {
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

                Finish(success);
            }
        }

        protected override void OnPause()
        {
            if (currentInstance != null)
            {
                currentInstance.Pause();
            }
        }

        FSM CheckInstance()
        {

            if (nestedFSM == currentInstance)
            {
                return currentInstance;
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
            FSM instance = null;
            if (!instances.TryGetValue(nestedFSM, out instance))
            {
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
        protected override void OnNodeGUI()
        {
            GUILayout.Label(string.Format("Sub FSM\n{0}", _nestedFSM));
            if (nestedFSM != null)
            {

            }
            else
            {
                if (!Application.isPlaying && GUILayout.Button("CREATE BOUND FSM"))
                {
                    NestedUtility.CreateBoundNested<FSM>(this, graph);
                    nestedFSM.name = name;
                    //Debug.Log("node name:"+name);
                }
                if (!Application.isPlaying && GUILayout.Button("CREATE ASSET FSM"))
                {
                    Node.CreateNested<FSM>(this);
                }
            }
        }

        protected override void OnNodeInspectorGUI()
        {
            //base.OnNodeInspectorGUI();
            ShowBaseFSMInspectorGUI();
            NodeCanvas.Editor.BBParameterEditor.ParameterField("NestedGraphOwner", NestedGraphOwner);
            NodeCanvas.Editor.BBParameterEditor.ParameterField("FSM", _nestedFSM);

            if (nestedFSM == this.FSM)
            {
                Debug.LogWarning("Nested FSM can't be itself!");
                nestedFSM = null;
            }
            if (nestedFSM == null)
            {
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
            if (defParams.Length != 0)
            {

                EditorUtils.TitledSeparator("Defined Nested BT Parameters");
                GUI.color = Color.yellow;
                UnityEditor.EditorGUILayout.LabelField("Name", "Type");
                GUI.color = Color.white;
                var added = new List<string>();
                foreach (var bbVar in defParams)
                {
                    if (!added.Contains(bbVar.name))
                    {
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