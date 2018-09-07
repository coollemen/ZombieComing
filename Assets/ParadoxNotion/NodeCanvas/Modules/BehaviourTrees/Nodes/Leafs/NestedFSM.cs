using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NodeCanvas.BehaviourTrees
{

    [Name("FSM")]
    [Category("Nested")]
    [Description("NestedFSM can be assigned an entire FSM. This node will return Running for as long as the FSM is Running. If a Success or Failure State is selected, then it will return Success or Failure as soon as the Nested FSM enters that state at which point the FSM will also be stoped. If the Nested FSM ends otherwise, this node will return Success.")]
    [Icon("FSM")]
    public class NestedFSM : BTNode, IGraphAssignable
    {
        public BBParameter<Transform> NestedGraphOwner;
        [SerializeField]
        private BBParameter<FSM> _nestedFSM = null;
        private Dictionary<FSM, FSM> instances = new Dictionary<FSM, FSM>();
        private FSM currentInstance = null;

        public string successState;
        public string failureState;

        public override string name
        {
            get { return base.name.ToUpper(); }
        }

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

        /////////
        //-----------
#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_nested;
        }
#endif
        protected override Status OnExecute(Component agent, IBlackboard blackboard)
        {

            if (nestedFSM == null || nestedFSM.primeNode == null)
            {
                return Status.Failure;
            }

            if (status == Status.Resting)
            {
                currentInstance = CheckInstance();
            }

            if (status == Status.Resting || currentInstance.isPaused)
            {
                status = Status.Running;
                currentInstance.StartGraph(agent, blackboard, false, OnFSMFinish);
            }

            if (status == Status.Running)
            {
                currentInstance.UpdateGraph();
            }

            if (!string.IsNullOrEmpty(successState) && currentInstance.currentStateName == successState)
            {
                currentInstance.Stop(true);
                return Status.Success;
            }

            if (!string.IsNullOrEmpty(failureState) && currentInstance.currentStateName == failureState)
            {
                currentInstance.Stop(false);
                return Status.Failure;
            }

            return status;
        }

        void OnFSMFinish(bool success)
        {
            if (status == Status.Running)
            {
                status = success ? Status.Success : Status.Failure;
            }
        }

        protected override void OnReset()
        {
            if (currentInstance != null)
            {
                currentInstance.Stop();
            }
        }

        public override void OnGraphPaused()
        {
            if (currentInstance != null)
            {
                currentInstance.Pause();
            }
        }

        public override void OnGraphStoped()
        {
            if (currentInstance != null)
            {
                currentInstance.Stop();
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

        ////////////////////////////
        //////EDITOR AND GUI////////
        ////////////////////////////
#if UNITY_EDITOR
        protected override void OnNodeGUI()
        {

            GUILayout.Label(string.Format("SubFSM\n{0}", _nestedFSM));
            if (nestedFSM != null)
            {

            }
            else
            {
                if (!Application.isPlaying && GUILayout.Button("CREATE BOUND FSM"))
                {
                    NestedUtility.CreateBoundNested<FSM>(this, graph);
                    nestedFSM.name = name;
                }
                if (!Application.isPlaying && GUILayout.Button("CREATE ASSET FSM"))
                {
                    Node.CreateNested<FSM>(this);
                }
            }
        }

        protected override void OnNodeInspectorGUI()
        {
            name = EditorGUILayout.TextField("FSM Name", name);
            NodeCanvas.Editor.BBParameterEditor.ParameterField("NestedGraphOwner", NestedGraphOwner);
            NodeCanvas.Editor.BBParameterEditor.ParameterField("Nested FSM", _nestedFSM);
            if (nestedFSM == null)
            {
                return;
            }

            //nestedFSM.name = this.name;
            successState = EditorUtils.StringPopup("Success State", successState, nestedFSM.GetStateNames().ToList(), false, true);
            failureState = EditorUtils.StringPopup("Failure State", failureState, nestedFSM.GetStateNames().ToList(), false, true);

            var defParams = nestedFSM.GetDefinedParameters();
            if (defParams.Length != 0)
            {

                EditorUtils.TitledSeparator("Defined SubFSM Parameters");
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