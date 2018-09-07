using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlowCanvas.Macros;

using NodeCanvas.Framework;
using UnityEngine;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
#if UNITY_EDITOR
using NodeCanvas.Editor;
using UnityEditor;
#endif



namespace FlowCanvas.Nodes
{

    [Description("find node in graph.")]
    [Category("Functions")]
    [Color("FFE400")]
    public class FindNode : FlowControlNode
    {
        protected override void RegisterPorts()
        {
            // base.RegisterPorts();
        }
#if UNITY_EDITOR
        protected override UnityEngine.GUIStyle nodeGUIType()
        {
            return NodeCanvas.Editor.CanvasStyles.window_variable;
        }
#endif
#if UNITY_EDITOR
        [SerializeField]
        private string _search;
        string search {
            get { return _search; }
            set
            {   
                if(_search==value)
                    return;

                _search = value;
#if UNITY_EDITOR
                RefreshListenereck();
#endif
                if (_search.Length > 1)
                {
                    if (searchType)
                    {
                        SearchNodeWithType();
                    }
                    else
                    {
                        SearchNode();
                    }
                }
            }
        }
        [SerializeField]
        private bool showEventNode = true;
        [SerializeField]
        private bool showCustomFunctionEvent = true;
        [SerializeField]
        private bool showMacroNode = true;
        [SerializeField]
        private bool showNestedFCNode = true;
        [SerializeField]
        private bool showNestedFSMNode = true;
        [SerializeField]
        private bool showNestedBTNode = true;
        [SerializeField]
        private bool nestedSearch = false;
        [SerializeField]
        private bool searchType = false;
        [SerializeField]
        private bool showEventAndNestedNode=false;

        List<FlowNode> searchNodeList = new List<FlowNode>();

        Dictionary<Graph, List<FlowNode>> globalSearchNodeDict = new Dictionary<Graph, List<FlowNode>>();

        List<EventNode> eventNodeList = new List<EventNode>();
        Dictionary<Graph, List<FlowNode>> globalSearchEventNodeDict = new Dictionary<Graph, List<FlowNode>>();

        List<CustomFunctionEvent> CustomFunctionEventList = new List<CustomFunctionEvent>();
        Dictionary<Graph, List<FlowNode>> globalFunctionSearchNodeDict = new Dictionary<Graph, List<FlowNode>>();

        List<MacroNodeWrapper> macroNodeList = new List<MacroNodeWrapper>();
        Dictionary<Graph, List<FlowNode>> globalSearchMacroNodeDict = new Dictionary<Graph, List<FlowNode>>();

        List<NestedFCNode> nestedFCNodeList = new List<NestedFCNode>();
        Dictionary<Graph, List<FlowNode>> globalnestedFCSearchNodeDict = new Dictionary<Graph, List<FlowNode>>();

        List<NestedFSMNode> nestedFSMNodeList = new List<NestedFSMNode>();
        Dictionary<Graph, List<FlowNode>> globalnestedFSMSearchNodeDict = new Dictionary<Graph, List<FlowNode>>();

        List<NestedBTNode> nestedBTNodeList = new List<NestedBTNode>();
        Dictionary<Graph, List<FlowNode>> globalnestedBTSearchNodeDict = new Dictionary<Graph, List<FlowNode>>();


        string GetDetailType(string s)
        {
            string[] ss = s.Split('.');

            return ss.Last();
        }



        void searchNode()
        {
            searchNodeList = graph.GetAllNodesOfType<FlowNode>().FindAll(i=>
            {   
                if (i!=null&&!string.IsNullOrEmpty(i.name))
                {
                    string n = i.name.Replace(" ","").ToString().ToLowerInvariant();
                    return n.Contains(search.ToLowerInvariant());
                }return false;
            });
        }
        void searchNodeByType()
        {
            searchNodeList = graph.GetAllNodesOfType<FlowNode>().FindAll(i =>
            {
                string n = GetDetailType(i.GetType().ToString().Replace(']', ' ')).ToLowerInvariant();
                return n.Contains(search.ToLowerInvariant());
            });
        }

        void searchNestedNode()
        {
            foreach (var _o in graph.GetAllNestedGraphs<Graph>(true))
            {
                var o = _o;
                if (o != graph)
                {
                    List<FlowNode> l = o.GetAllNodesOfType<FlowNode>().FindAll(i =>
                    {
                        string n = (i.name.Replace(" ", "").Replace(']', ' ')).ToLowerInvariant();
                        return n.Contains(search.ToLowerInvariant());
                    });

                    if (l != null)
                    {
                        globalSearchNodeDict.Add(o, l);
                    }
                }
            }
        }

        void searchNestedNodeByType()
        {
            foreach (var _o in graph.GetAllNestedGraphs<Graph>(true))
            {
                var o = _o;
                if (o != graph)
                {
                    List<FlowNode> l = o.GetAllNodesOfType<FlowNode>().FindAll(i =>
                    {
                        string n = GetDetailType(i.GetType().ToString().Replace(']', ' ')).ToLowerInvariant();
                        return n.Contains(search.ToLowerInvariant());
                    });

                    if (l != null)
                    {
                        globalSearchNodeDict.Add(o, l);
                    }
                }
            }
        }

        void searchNestedNodeInToDict(Dictionary<Graph, List<FlowNode>> owner,string type)
        {
            foreach (var _o in graph.GetAllNestedGraphs<Graph>(true))
            {
                var o = _o;
                if (o != graph)
                {
                    List<FlowNode> l = o.GetAllNodesOfType<FlowNode>().FindAll(i =>
                    {
                        string n = GetDetailType(i.GetType().ToString().Replace(']', ' ')).ToLowerInvariant();
                        return n.Contains(type.ToLowerInvariant());
                    });

                    if (l != null)
                    {
                        owner.Add(o, l);
                    }
                }
            }
        }

        void SearchNode()
        {
            if (!nestedSearch)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    searchNode();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(search))
                {
                    searchNode();

                    searchNestedNode();
                }
            }
        }

        void SearchNodeWithType()
        {
            if (!nestedSearch)
            {
                if (!string.IsNullOrEmpty(search))
                    searchNodeByType();
            }
            else
            {
                if (!string.IsNullOrEmpty(search))
                {
                    searchNestedNodeByType();
                }
            }
        }

        void ShowNestedDict(Dictionary<Graph, List<FlowNode>> targetDict)
        {
            foreach (var v in targetDict)
            {
                List<FlowNode> t = v.Value;
                Graph o = v.Key;

                for (int i = 0; i < t.Count; i++)
                {
                    if (t[i] != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(
                            string.Format(t[i].ToString() + " in: "+ o.name),
                            GUILayout.MaxWidth(280));
                        if (GUILayout.Button("select"))
                        {
                            Selection.activeObject = o;
                            Selection.selectionChanged();
                        
                        //if (graph.agent != null && graph.agent == o.agent)
                            //    graph.currentChildGraph = o;

                            //Selection.activeObject = o;
                            //Selection.selectionChanged();

                            if (MonoManager.current == null)
                            {
                                var _current = UnityEngine.Object.FindObjectOfType<MonoManager>();
                                if (_current != null)
                                {
                                    UnityEngine.Object.DestroyImmediate(_current.gameObject);
                                }

                                var current = new GameObject("_MonoManager").AddComponent<MonoManager>();
                                current.StartCoroutine(waitForGraphChange(t[i]));
                            }
                            else
                            {
                                MonoManager.current.StartCoroutine(waitForGraphChange(t[i]));
                            }

                            GraphEditorUtility.activeElement = t[i];
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnNodeInspectorGUI()
        {   
            //base.OnNodeInspectorGUI();

           GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("<b>Search Input:</b>"));

            nestedSearch = GUILayout.Toggle(nestedSearch, "searchAllNested");
            searchType = GUILayout.Toggle(searchType, "NodeType");
            GUILayout.EndHorizontal();

            search = EditorGUILayout.TextField(search);

            if (GUILayout.Button("Search Node"))
            {
                RefreshListenereck();


                if (searchType)
                {
                    SearchNodeWithType();
                }
                else
                {
                    SearchNode();
                }
            }

            GUILayout.Label(string.Format("<b>Search Result:</b>"));



            for (int i = 0; i < searchNodeList.Count; i++)
            {
                if (searchNodeList[i] != null)
                {   
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(string.Format(searchNodeList[i].ToString() + "   :  " + GetDetailType(searchNodeList[i].GetType().ToString().Replace(']', ' '))), GUILayout.MaxWidth(280));
                    if (GUILayout.Button("select"))
                    {
                        GraphEditorUtility.activeElement = searchNodeList[i];
                    }
                    GUILayout.EndHorizontal();
                }
            }


            if (nestedSearch)
            {
                GUILayout.Label(string.Format("<b>Nested Search Result:</b>"));

                ShowNestedDict(globalSearchNodeDict);
            }

            GUILayout.Space(10f);

            if (!(showEventAndNestedNode = GUILayout.Toggle(showEventAndNestedNode, "ShowEventAndNestedNode")))
                return;


            GUILayout.Label("---------------------------");

            if (showEventNode = GUILayout.Toggle(showEventNode, "ShowEventNode"))
            {
                GUILayout.Label(string.Format("<b>EventNodeList:</b>"));
                GUILayout.Space(3f);
                for (int i = 0; i < eventNodeList.Count; i++)
                {
                    if (eventNodeList[i] != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(
                            string.Format(eventNodeList[i].ToString()),
                            GUILayout.MaxWidth(280));
                        if (GUILayout.Button("select"))
                        {
                            GraphEditorUtility.activeElement = eventNodeList[i];
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                if (nestedSearch)
                {
                    GUILayout.Label("---nested:---");
                    ShowNestedDict(globalSearchEventNodeDict);
                }

                GUILayout.Space(10f);
            }

            if (showCustomFunctionEvent = GUILayout.Toggle(showCustomFunctionEvent, "ShowCustomFunctionEvent"))
            {
                GUILayout.Label(string.Format("<b>CustomFunctionEventList:</b>"));
                GUILayout.Space(3f);
                for (int i = 0; i < CustomFunctionEventList.Count; i++)
                {
                    if (CustomFunctionEventList[i] != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(
                            string.Format(CustomFunctionEventList[i].ToString()),
                            GUILayout.MaxWidth(280));
                        if (GUILayout.Button("select"))
                        {
                            GraphEditorUtility.activeElement = CustomFunctionEventList[i];
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                if (nestedSearch)
                {
                    GUILayout.Label("---nested:---");
                    ShowNestedDict(globalFunctionSearchNodeDict);
                }
                GUILayout.Space(10f);
            }

            if (showMacroNode = GUILayout.Toggle(showMacroNode, "ShowMacroNode"))
            {
                GUILayout.Label(string.Format("<b>MacroNode:</b>"));
                GUILayout.Space(3f);
                for (int i = 0; i < macroNodeList.Count; i++)
                {
                    if (macroNodeList[i] != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(
                            string.Format(macroNodeList[i].ToString()),
                            GUILayout.MaxWidth(280));
                        if (GUILayout.Button("select"))
                        {
                            GraphEditorUtility.activeElement = macroNodeList[i];
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                if (nestedSearch)
                {
                    GUILayout.Label("---nested:---");
                    ShowNestedDict(globalSearchMacroNodeDict);
                }
                GUILayout.Space(10f);
            }

            if (showNestedFCNode = GUILayout.Toggle(showNestedFCNode, "ShowNestedFCNode"))
            {
                GUILayout.Label(string.Format("<b>NestedFCNode:</b>"));
                GUILayout.Space(3f);
                for (int i = 0; i < nestedFCNodeList.Count; i++)
                {
                    if (nestedFCNodeList[i] != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(
                            string.Format(nestedFCNodeList[i].ToString()),
                            GUILayout.MaxWidth(280));
                        if (GUILayout.Button("select"))
                        {
                            GraphEditorUtility.activeElement = nestedFCNodeList[i];
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                if (nestedSearch)
                {
                    GUILayout.Label("---nested:---");
                    ShowNestedDict(globalnestedFCSearchNodeDict);
                }
                GUILayout.Space(10f);
            }

            if (showNestedFSMNode = GUILayout.Toggle(showNestedFSMNode, "ShowNestedFSMNode"))
            {
                GUILayout.Label(string.Format("<b>NestedFSMNode:</b>"));
                GUILayout.Space(3f);
                for (int i = 0; i < nestedFSMNodeList.Count; i++)
                {
                    if (nestedFSMNodeList[i] != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(
                            string.Format(nestedFSMNodeList[i].ToString()),
                            GUILayout.MaxWidth(280));
                        if (GUILayout.Button("select"))
                        {
                            GraphEditorUtility.activeElement = nestedFSMNodeList[i];
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                if (nestedSearch)
                {
                    GUILayout.Label("---nested:---");
                    ShowNestedDict(globalnestedFSMSearchNodeDict);
                }
                GUILayout.Space(10f);
            }

            if (showNestedBTNode = GUILayout.Toggle(showNestedBTNode, "ShowNestedBTMNode"))
            {
                GUILayout.Label(string.Format("<b>NestedBTMNode:</b>"));
                GUILayout.Space(3f);
                for (int i = 0; i < nestedBTNodeList.Count; i++)
                {
                    if (nestedBTNodeList[i] != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(
                            string.Format(nestedBTNodeList[i].ToString()),
                            GUILayout.MaxWidth(280));
                        if (GUILayout.Button("select"))
                        {
                            GraphEditorUtility.activeElement = nestedBTNodeList[i];
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                if (nestedSearch)
                {
                    GUILayout.Label("---nested:---");
                    ShowNestedDict(globalnestedBTSearchNodeDict);
                }
                GUILayout.Space(10f);
            }

            if (GUILayout.Button("Refresh Event And Function List"))
            {
                RefreshListenereck();
            }
        }


        void RefreshListenereck()
        {
            searchNodeList.Clear();
            eventNodeList.Clear();
            CustomFunctionEventList.Clear();
            macroNodeList.Clear();
            nestedFCNodeList.Clear();
            nestedFSMNodeList.Clear();
            nestedBTNodeList.Clear();

            globalSearchNodeDict.Clear();
            globalSearchEventNodeDict.Clear();
            globalFunctionSearchNodeDict.Clear();
            globalSearchMacroNodeDict.Clear();
            globalnestedFCSearchNodeDict.Clear();
            globalnestedFSMSearchNodeDict.Clear();
            globalnestedBTSearchNodeDict.Clear();

            eventNodeList = graph.GetAllNodesOfType<EventNode>();
            CustomFunctionEventList = graph.GetAllNodesOfType<CustomFunctionEvent>();

            macroNodeList = graph.GetAllNodesOfType<MacroNodeWrapper>();
            nestedFCNodeList = graph.GetAllNodesOfType<NestedFCNode>();
            nestedFSMNodeList = graph.GetAllNodesOfType<NestedFSMNode>();
            nestedBTNodeList = graph.GetAllNodesOfType<NestedBTNode>();

            searchNestedNodeInToDict(globalSearchEventNodeDict, "EventNode");
            searchNestedNodeInToDict(globalFunctionSearchNodeDict, "CustomFunctionEvent");
            searchNestedNodeInToDict(globalSearchMacroNodeDict, "MacroNodeWrapper");
            searchNestedNodeInToDict(globalnestedFCSearchNodeDict, "NestedFCNode");
            searchNestedNodeInToDict(globalnestedFSMSearchNodeDict, "NestedFSMNode");
            searchNestedNodeInToDict(globalnestedBTSearchNodeDict, "NestedBTNode");
        }

        IEnumerator waitForGraphChange(FlowNode n)
        {
            //Debug.Log("double press F to focus:" + n
            yield return new WaitForSecondsRealtime(0.15f);
            GraphEditorUtility.activeElement = n;
        }
#endif

#endif
    }
}
