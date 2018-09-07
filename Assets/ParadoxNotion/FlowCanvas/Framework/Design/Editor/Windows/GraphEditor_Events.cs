using System;
using System.Collections.Generic;
using System.Linq;
using FlowCanvas;
using FlowCanvas.Macros;
using FlowCanvas.Nodes;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEditor;
using UnityEngine;

namespace NodeCanvas.Editor {

    //Events
    partial class GraphEditor {

		private static bool mouse2Down;

		///Graph events BEFORE nodes
		static void HandlePreNodesGraphEvents(Graph graph, Vector2 canvasMousePos){

			if (e.button == 2 && (e.type == EventType.MouseDown || e.type == EventType.MouseUp) ){
	    		Undo.RecordObject(graph, "Graph Pan");
	    	}

			if (e.type == EventType.MouseUp || e.type == EventType.KeyUp){
				SnapNodesToGridAndPixel(graph);
			}

			if (e.type == EventType.KeyDown && e.keyCode == KeyCode.F && GUIUtility.keyboardControl == 0){
				FocusSelection();
			}

			if (e.type == EventType.MouseDown && e.button == 2 && e.clickCount == 2){
				FocusPosition( ViewToCanvas(e.mousePosition) );
			}

			if (e.type == EventType.ScrollWheel && GraphEditorUtility.allowClick){
				if (canvasRect.Contains(e.mousePosition)){
					var zoomDelta = e.shift? 0.1f : 0.25f;
					ZoomAt(e.mousePosition, -e.delta.y > 0? zoomDelta : -zoomDelta );
				}
			}

			if ( (e.button == 2 && e.type == EventType.MouseDrag && canvasRect.Contains(e.mousePosition))
				|| ( (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.alt && e.isMouse) )
			{
				pan += e.delta;
				smoothPan = null;
				smoothZoomFactor = null;
				e.Use();
			}

			if (e.type == EventType.MouseDown && e.button == 2){ mouse2Down = true; }
			if (e.type == EventType.MouseUp && e.button == 2){ mouse2Down = false; }
			if (e.alt || mouse2Down){
				EditorGUIUtility.AddCursorRect(new Rect(0,0,screenWidth,screenHeight), MouseCursor.Pan);
			}
		}

		///----------------------------------------------------------------------------------------------

		///Graph events AFTER nodes
		static void HandlePostNodesGraphEvents(Graph graph, Vector2 canvasMousePos){

			//Shortcuts
			if (GUIUtility.keyboardControl == 0){

				//Copy/Cut/Paste
				if (e.type == EventType.ValidateCommand || e.type == EventType.Used){
					if (e.commandName == "Copy" || e.commandName == "Cut"){
						List<Node> selection = null;
						if (GraphEditorUtility.activeNode != null){
							selection = new List<Node>{GraphEditorUtility.activeNode};
						}
						if (GraphEditorUtility.activeElements != null && GraphEditorUtility.activeElements.Count > 0){
							selection = GraphEditorUtility.activeElements.Cast<Node>().ToList();
						}
						if (selection != null){
							CopyBuffer.Set<Node[]>( Graph.CloneNodes(selection).ToArray() );
							if (e.commandName == "Cut"){
								foreach (Node node in selection){ graph.RemoveNode(node); }
							}
						}
						e.Use();
					}
					if (e.commandName == "Paste"){
						if (CopyBuffer.Has<Node[]>()){
							TryPasteNodesInGraph(graph, CopyBuffer.Get<Node[]>(), canvasMousePos + new Vector2(500,500) / graph.zoomFactor );
						}
						e.Use();
					}
				}

				if (e.type == EventType.KeyUp){
					
					//Delete
					if (e.keyCode == KeyCode.Delete || e.keyCode == KeyCode.Backspace){

						if (GraphEditorUtility.activeElements != null && GraphEditorUtility.activeElements.Count > 0){
							foreach (var obj in GraphEditorUtility.activeElements.ToArray()){
								if (obj is Node){
									graph.RemoveNode(obj as Node);
								}
								if (obj is Connection){
									graph.RemoveConnection(obj as Connection);
								}
							}
							GraphEditorUtility.activeElements = null;
						}

						if (GraphEditorUtility.activeNode != null){
							graph.RemoveNode(GraphEditorUtility.activeNode);
							GraphEditorUtility.activeElement = null;
						}

						if (GraphEditorUtility.activeConnection != null){
							graph.RemoveConnection(GraphEditorUtility.activeConnection);
							GraphEditorUtility.activeElement = null;
						}
						e.Use();
					}

					//Duplicate
					if (e.keyCode == KeyCode.D && e.control&&!e.alt){
						if (GraphEditorUtility.activeElements != null && GraphEditorUtility.activeElements.Count > 0){
							TryPasteNodesInGraph(graph, GraphEditorUtility.activeElements.OfType<Node>().ToArray(), default(Vector2));
						}
						if (GraphEditorUtility.activeNode != null){
							GraphEditorUtility.activeElement = GraphEditorUtility.activeNode.Duplicate(graph);
						}
						//Connections can't be duplicated by themselves. They do so as part of multiple node duplication.
						e.Use();
					}
				}
			}

			//No panel is obscuring
			if (GraphEditorUtility.allowClick){
                #region 自定义快捷键

                if ((currentGraph.GetType() == typeof(FlowGraph) || currentGraph.GetType().IsSubclassOf(typeof(FlowGraph))) && GUIUtility.keyboardControl == 0)
                {
                    if (e.keyCode == KeyCode.F && e.control && !e.alt)
                    {
                        //Debug.Log("ctrl +f ");

                        FindNode toolNode = currentGraph.GetAllNodesOfType<FindNode>().FirstOrDefault();
                        if (toolNode != null)
                        {
                            NodeCanvas.Editor.GraphEditorUtility.activeElement = toolNode;
                        }
                        else
                        {

                            FindNode newNode = currentGraph.AddNode<FindNode>(ViewToCanvas(e.mousePosition));

                            NodeCanvas.Editor.GraphEditorUtility.activeElement = newNode;
                            //Debug.Log("There is no Find Node,Create a new node!");
                        }
                        //e.Use();
                    }


                    if (e.keyCode == KeyCode.A && e.control && !e.alt && !e.shift)
                    {

                        NodeCanvas.Editor.GraphEditorUtility.activeElements = null;
                        NodeCanvas.Editor.GraphEditorUtility.activeElement = null;

                        if (currentGraph.allNodes == null)
                            return;

                        if (currentGraph.allNodes.Count > 1)
                        {
                            NodeCanvas.Editor.GraphEditorUtility.activeElements = currentGraph.allNodes.Cast<object>().ToList();
                            //e.Use();
                            return;
                        }
                        else
                        {
                            NodeCanvas.Editor.GraphEditorUtility.activeElement = currentGraph.allNodes[0];
                            //e.Use();
                        }
                    }
                    if (e.keyCode == KeyCode.C && e.type == EventType.KeyUp && !e.control && !e.alt && !e.shift && GUIUtility.keyboardControl == 0)
                    {
                        if (GraphEditorUtility.activeElement != null ||
                            GraphEditorUtility.activeElements != null && GraphEditorUtility.activeElements.Count > 0)
                        {   
                            List<Node> node=new List<Node>();
                            if (GraphEditorUtility.activeElement != null)
                            {
                                node.Add((Node)GraphEditorUtility.activeElement);
                            }
                            else
                            {
                                node = GraphEditorUtility.activeElements.Cast<Node>().ToList();
                            }

                            Undo.RegisterCompleteObjectUndo(currentGraph, "Create Group");
                            if (currentGraph.canvasGroups == null)
                            {
                                currentGraph.canvasGroups = new List<Framework.CanvasGroup>();
                            }
	                        Framework.CanvasGroup group = new Framework.CanvasGroup(GetNodeBounds(node).ExpandBy(100f), "New Canvas Group");
                            currentGraph.canvasGroups.Add(group);
                            group.isRenaming = true;
                        }
                    }
                    if (e.keyCode == KeyCode.G && !e.control && !e.alt && e.shift && GUIUtility.keyboardControl == 0)
                    {
                        if (NodeCanvas.Editor.GraphEditorUtility.activeElements == null ||
                            NodeCanvas.Editor.GraphEditorUtility.activeElements.Count < 2 && NodeCanvas.Editor.GraphEditorUtility.activeElement != null &&
                            NodeCanvas.Editor.GraphEditorUtility.activeElement.GetType() == typeof(MacroNodeWrapper))
                        {
                            //Debug.Log("current select is group node");
                            MacroNodeWrapper n = (MacroNodeWrapper)NodeCanvas.Editor.GraphEditorUtility.activeElement;

                            if (n.macro.allNodes.Count <= 2)
                                return;
                            List<Node> childNode = new List<Node>();

                            foreach (var c in n.macro.allNodes)
                            {
                                if (c.GetType() != typeof(MacroInputNode) && c.GetType() != typeof(MacroOutputNode))
                                {
                                    childNode.Add(c);
                                }
                            }

                            var newNodes = Graph.CopyNodesToGraph(childNode, currentGraph);

                            currentGraph.RemoveNode(n);
                            NodeCanvas.Editor.GraphEditorUtility.activeElement = null;

                            if (NodeCanvas.Editor.GraphEditorUtility.activeElements != null)
                                NodeCanvas.Editor.GraphEditorUtility.activeElements = newNodes.Cast<object>().ToList();

                        }
                    }

                    if (e.keyCode == KeyCode.G && e.control && !e.alt && !e.shift && GUIUtility.keyboardControl == 0)
                    {
                        //Debug.Log("ctrl +g ");
                        if (NodeCanvas.Editor.GraphEditorUtility.activeElements == null || NodeCanvas.Editor.GraphEditorUtility.activeElements.Count < 1)
                        {
                            //e.Use();
                            return;
                        }

                        List<Node> ns = NodeCanvas.Editor.GraphEditorUtility.activeElements.Cast<Node>().ToList();
                        Rect groupNodeRect = GetNodeBounds(ns, viewRect, false);
                        var wrapper = currentGraph.AddNode<MacroNodeWrapper>(groupNodeRect.center);
                        wrapper.macro= NestedUtility.CreateBoundNested<GroupMacro>(wrapper, currentGraph);
                        wrapper.macro.name = "NewGroup";
                        wrapper.macro.CategoryPath = (currentGraph.agent != null ? currentGraph.agent.name : currentGraph.name) + "/";
                        wrapper.macro.agent = currentGraph.agent;

                        wrapper.macro.entry.position = new Vector2(groupNodeRect.x - 200f,
                            groupNodeRect.y + 0.5f * groupNodeRect.height);
                        wrapper.macro.exit.position = new Vector2(groupNodeRect.x + groupNodeRect.width + 100f,
                            groupNodeRect.y + 0.5f * groupNodeRect.height);

                        wrapper.macro.translation = -groupNodeRect.center + new Vector2(500f, 500f);

                        Graph.CopyNodesToGraph(NodeCanvas.Editor.GraphEditorUtility.activeElements.OfType<Node>().ToList(), wrapper.macro);

                        foreach (var c in ns)
                        {
                            currentGraph.RemoveNode(c);
                        }
                        NodeCanvas.Editor.GraphEditorUtility.activeElements.Clear();
                        NodeCanvas.Editor.GraphEditorUtility.activeElement = wrapper;
                    }
                    if (e.type == EventType.KeyDown && e.keyCode == KeyCode.I && GUIUtility.keyboardControl == 0)
                    {
                        List<Node> allNodes = currentGraph.GetAllNodesOfType<Node>();
                        if (allNodes != null && allNodes.Count < 2)
                            return;

                        if (NodeCanvas.Editor.GraphEditorUtility.activeElement != null)
                        {
                            if (allNodes.Contains((Node)NodeCanvas.Editor.GraphEditorUtility.activeElement))
                            {
                                allNodes.Remove((Node)NodeCanvas.Editor.GraphEditorUtility.activeElement);
                                NodeCanvas.Editor.GraphEditorUtility.activeElement = null;
                                NodeCanvas.Editor.GraphEditorUtility.activeElements.Clear();
                                NodeCanvas.Editor.GraphEditorUtility.activeElements = allNodes.Cast<object>().ToList();
                            }
                            return;
                        }

                        if (NodeCanvas.Editor.GraphEditorUtility.activeElements != null && NodeCanvas.Editor.GraphEditorUtility.activeElements.Count > 1)
                        {
                            List<object> invertNodeList = NodeCanvas.Editor.GraphEditorUtility.activeElements;
                            invertNodeList.ForEach(x =>
                            {
                                if (allNodes.Contains((Node)x))
                                    allNodes.Remove((Node)x);
                            });
                            NodeCanvas.Editor.GraphEditorUtility.activeElements.Clear();
                            if (allNodes.Count == 0)
                            {
                                NodeCanvas.Editor.GraphEditorUtility.activeElement = null;
                            }
                            else if (allNodes.Count == 1)
                            {
                                NodeCanvas.Editor.GraphEditorUtility.activeElement = allNodes[0];
                            }
                            else
                            {
                                NodeCanvas.Editor.GraphEditorUtility.activeElements = allNodes.Cast<object>().ToList();
                            }

                            return;
                        }
                        e.Use();
                    }
                    if (e.type == EventType.KeyUp && e.alt && e.control)
                    {
                        //Type genericType;

                        UnityEditor.GenericMenu.MenuFunction2 S = (obj) =>
                        {
                            NodeCanvas.Editor.GraphEditorUtility.activeElement = GraphEditor.currentGraph.AddNode(obj.GetType(),
                                ViewToCanvas(e.mousePosition));
                        };

                        UnityEditor.GenericMenu.MenuFunction2 D = (obj) =>
                        {
                            var genericT = typeof(SimplexNodeWrapper<>).MakeGenericType(obj.GetType());
                            NodeCanvas.Editor.GraphEditorUtility.activeElement = GraphEditor.currentGraph.AddNode(genericT, ViewToCanvas(e.mousePosition));
                        };


                        if (e.keyCode == KeyCode.S)
                        {

                            var menu = new UnityEditor.GenericMenu();

                            menu.AddItem(new GUIContent("Split"), false, S, new Split());
                            menu.AddItem(new GUIContent("Sequence"), false, S, new Sequence());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("SetActive"), false, D, new S_Active());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("SendEvent"), false, D, new SendEvent());
                            menu.AddItem(new GUIContent("SendEvent<T>"), false, D, new SendEvent<object>());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("Self"), false, S, new OwnerVariable());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("SwitchByIndex"), false, S, new Switch<object>());
                            menu.AddItem(new GUIContent("Switch Value"), false, D, new SwitchValue<object>());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("SwitchFlow/Switch Bool"), false, S, new SwitchBool());
                            menu.AddItem(new GUIContent("SwitchFlow/Switch String"), false, S, new SwitchString());
                            menu.AddItem(new GUIContent("SwitchFlow/Switch Int"), false, S, new SwitchInt());
                            menu.AddItem(new GUIContent("SwitchFlow/Switch Enum"), false, S, new SwitchEnum());
                            menu.AddItem(new GUIContent("SwitchFlow/Switch Tag"), false, S, new SwitchTag());
                            menu.AddItem(new GUIContent("SwitchFlow/Switch Comparison"), false, S, new SwitchComparison());

                            menu.ShowAsContext();
                            //e.Use();
                            return;
                        }

                        if (e.keyCode == KeyCode.A)
                        {

                            var menu = new UnityEditor.GenericMenu();

                            menu.AddItem(new GUIContent("Awake"), false, S, new ConstructionEvent());
                            menu.AddItem(new GUIContent("OnEnable"), false, S, new EnableEvent());
                            menu.AddItem(new GUIContent("Start"), false, S, new StartEvent());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("Update"), false, S, new UpdateEvent());
                            menu.AddItem(new GUIContent("FixedUpdate"), false, S, new FixedUpdateEvent());
                            menu.AddItem(new GUIContent("LateUpdate"), false, S, new LateUpdateEvent());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("OnDisable"), false, S, new DisableEvent());
                            menu.ShowAsContext();
                            return;
                        }
                        if (e.keyCode == KeyCode.C)
                        {

                            var menu = new UnityEditor.GenericMenu();
                            menu.AddItem(new GUIContent("Cache"), false, D, new Cache<object>());

                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("Coroutine"), false, S, new CoroutineState());
                            menu.AddItem(new GUIContent("Cooldown"), false, S, new Cooldown());
                            menu.AddItem(new GUIContent("Chance"), false, S, new Chance());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("CustomEvent"), false, S, new CustomEvent());
                            menu.AddItem(new GUIContent("CustomEvent<T>"), false, S, new CustomEvent<object>());

                            menu.ShowAsContext();
                            return;
                        }

                        if (e.keyCode == KeyCode.E)
                        {
                            var menu = new UnityEditor.GenericMenu();
                            menu.AddItem(new GUIContent("KeyBoard"), false, S, new KeyboardEvents());
                            menu.AddItem(new GUIContent("UnityEventAutoCallbackEvent"), false, S, new UnityEventAutoCallbackEvent());
                            menu.AddItem(new GUIContent("CSharpAutoCallbackEvent"), false, S, new CSharpAutoCallbackEvent());
                            menu.AddItem(new GUIContent("DelegateCallbackEvent"), false, S, new DelegateCallbackEvent());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("UnityEventCallbackEvent"), false, S, new UnityEventCallbackEvent());
                            menu.AddItem(new GUIContent("CSharpEventCallback"), false, S, new CSharpEventCallback());
                            menu.ShowAsContext();
                            return;
                        }
                        if (e.keyCode == KeyCode.D)
                        {

                            var menu = new UnityEditor.GenericMenu();

                            menu.AddItem(new GUIContent("Debug log OnScreen"), false, S, new G_LogOnScreen());
                            menu.AddItem(new GUIContent("Debug log"), false, D, new LogText());
                            menu.AddItem(new GUIContent("Debug Event"), false, D, new DebugEvent());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("DummyFlow"), false, S, new Dummy());
                            menu.AddItem(new GUIContent("DummyValue"), false, D, new Identity<object>());

                            menu.AddItem(new GUIContent("DoOnce"), false, S, new DoOnce());
                            menu.ShowAsContext();
                            e.Use();
                        }

                        if (e.keyCode == KeyCode.R)
                        {
                            var menu = new UnityEditor.GenericMenu();

                            menu.AddItem(new GUIContent("RelayFlowInput"), false, S, new RelayFlowInput());
                            menu.AddItem(new GUIContent("RelayFlowOutput"), false, S, new RelayFlowOutput());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("RelayValueInput"), false, S, new RelayValueInput<object>());
                            menu.AddItem(new GUIContent("RelayValueOutput"), false, S, new RelayValueOutput<object>());

                            menu.ShowAsContext();
                            e.Use();
                        }

                        if (e.keyCode == KeyCode.W)
                        {
                            var menu = new UnityEditor.GenericMenu();

                            menu.AddItem(new GUIContent("Wait"), false, D, new Wait());
                            menu.AddItem(new GUIContent("While"), false, S, new While());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("WaitForOneFrame"), false, D, new WaitForOneFrame());
                            menu.AddItem(new GUIContent("WaitForEndOfFrame"), false, D, new FlowCanvas.Nodes.WaitForEndOfFrame());
                            menu.AddItem(new GUIContent("WaitForFixedUpdate"), false, D, new WaitForFixedUpdate());
                            menu.AddItem(new GUIContent("WaitForPhysicsFrame"), false, D, new WaitForPhysicsFrame());
                            menu.ShowAsContext();
                            e.Use();
                        }

                        if (e.keyCode == KeyCode.F)
                        {   
                            Selection.activeGameObject=null;  //防止快捷键冲突
                            var menu = new UnityEditor.GenericMenu();
                            menu.AddItem(new GUIContent("Find"), false, D, new G_FindGameObject());
                            menu.AddItem(new GUIContent("FindChild"), false, D, new G_FindChild());
                            menu.AddItem(new GUIContent("FindGameObjectWithTag"), false, D, new G_FindGameObjectWithTag());
                            menu.AddItem(new GUIContent("FindGameObjectsWithTag"), false, D, new G_FindGameObjectsWithTag());
                            menu.AddItem(new GUIContent("FindObjectOfType"), false, D, new FindObjectOfType());
                            menu.AddItem(new GUIContent("FindObjectsOfType"), false, D, new FindObjectsOfType());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("Finish"), false, S, new Finish());
                            menu.AddItem(new GUIContent("CustomFunction"), false, S, new CustomFunctionEvent());
                            menu.AddItem(new GUIContent("CallCustomFunction"), false, S, new CustomFunctionCall());
                            menu.AddItem(new GUIContent("CallCustomFunction(CustomPort)"), false, S, new FunctionCall());
                            menu.AddItem(new GUIContent("FunctionCustomReturn"), false, S, new Return());

                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("ForLoop"), false, S, new ForLoop());
                            menu.AddItem(new GUIContent("ForEach"), false, S, new ForEach<float>());
                            menu.AddItem(new GUIContent("FlipFlop"), false, S, new FlipFlop());

                            menu.ShowAsContext();
                            e.Use();
                        }

                        if (e.keyCode == KeyCode.G)
                        {
                            var menu = new UnityEditor.GenericMenu();
                            menu.AddItem(new GUIContent("GetName"), false, D, new G_Name());
                            menu.AddItem(new GUIContent("GetAcive"), false, D, new G_Active());
                            menu.AddItem(new GUIContent("GetPosition"), false, D, new G_Position());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("GetChildByIndex"), false, D, new G_Child());
                            menu.AddItem(new GUIContent("GetChildCount"), false, D, new G_ChildCount());
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("GetComponentByType"), false, D, new G_Component());
                            menu.AddItem(new GUIContent("GetComponentsByType"), false, D, new G_Components());
                            menu.AddItem(new GUIContent("GetComponentByName"), false, D, new G_ComponentByTypeName());
                            menu.AddItem(new GUIContent("GetComponentsInChildren"), false, D, new G_ComponentsInChildren());
                            menu.AddItem(new GUIContent("GetComponentInChildren"), false, D, new G_ComponentInChildren());
                            menu.AddItem(new GUIContent("GetComponentInParent"), false, D, new G_ComponentInParent());

                            menu.ShowAsContext();
                            e.Use();
                        }
                    }

                    if (e.type == EventType.KeyUp && e.alt)
                    {
                        Type genericType;

                        switch (e.keyCode)
                        {
                            case KeyCode.Alpha1:
                                genericType = typeof(GetVariable<>).MakeGenericType(typeof(float));
                                break;
                            case KeyCode.Alpha2:
                                genericType = typeof(GetVariable<>).MakeGenericType(typeof(Vector2));
                                break;
                            case KeyCode.Alpha3:
                                genericType = typeof(GetVariable<>).MakeGenericType(typeof(Vector3));
                                break;
                            case KeyCode.Alpha4:
                                genericType = typeof(GetVariable<>).MakeGenericType(typeof(Quaternion));
                                break;
                            case KeyCode.Alpha5:
                                genericType = typeof(GetVariable<>).MakeGenericType(typeof(Color));
                                break;
                            default:
                                //genericType = typeof(GetVariable<>).MakeGenericType(typeof(float));
                                return;
                                //break;
                        }

                        var varN = (FlowNode)GraphEditor.currentGraph.AddNode(genericType);
                        varN.position = ViewToCanvas(e.mousePosition - varN.rect.center * 0.5f);

                        NodeCanvas.Editor.GraphEditorUtility.activeElement = varN;

                        e.Use();
                    }
                }
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.F && GUIUtility.keyboardControl == 0)
                {
                    FocusSelection();
                }

                #endregion


                //'Tilt' or 'Space' keys, opens up the complete context menu browser
                if (e.type == EventType.KeyDown && !e.shift && (e.keyCode == KeyCode.BackQuote || e.keyCode == KeyCode.Space) ){
					GenericMenuBrowser.Show( GetAddNodeMenu(graph, canvasMousePos), e.mousePosition, string.Format("Add {0} Node", graph.GetType().FriendlyName()), graph.baseNodeType );
					e.Use();
				}

				//Right click canvas context menu. Basicaly for adding new nodes.
				if (e.type == EventType.ContextClick){
					var menu = GetAddNodeMenu(graph, canvasMousePos);
					if (CopyBuffer.Has<Node[]>() && CopyBuffer.Peek<Node[]>()[0].GetType().IsSubclassOf(graph.baseNodeType)){
						menu.AddSeparator("/");
						var copiedNodes = CopyBuffer.Get<Node[]>();
						if (copiedNodes.Length == 1){
							menu.AddItem(new GUIContent(string.Format("Paste Node ({0})", copiedNodes[0].GetType().FriendlyName() )), false, ()=> { TryPasteNodesInGraph(graph, copiedNodes, canvasMousePos); });
						} else if (copiedNodes.Length > 1){
							menu.AddItem(new GUIContent(string.Format("Paste Nodes ({0})", copiedNodes.Length.ToString() )), false, ()=> { TryPasteNodesInGraph(graph, copiedNodes, canvasMousePos); });
						}
					}

					if (NCPrefs.useBrowser){
						menu.ShowAsBrowser( e.mousePosition, string.Format("Add {0} Node", graph.GetType().FriendlyName()), graph.baseNodeType );
					} else {
						menu.ShowAsContext();
						
					}
					e.Use();
				}
			}
		}
        ///Gets the bound rect for the nodes
        static Rect GetNodeBounds(List<Node> nodes, Rect container, bool expandToContainer = false)
        {
            if (nodes == null || nodes.Count == 0)
            {
                return container;
            }

            var arr = new Rect[nodes.Count];
            for (var i = 0; i < nodes.Count; i++)
            {
                arr[i] = nodes[i].rect;
            }
            var result = RectUtils.GetBoundRect(arr);
            if (expandToContainer)
            {
                result = RectUtils.GetBoundRect(result, container);
            }
            return result;
        }
        //SL-------------
        ///----------------------------------------------------------------------------------------------

        //Paste nodes in this graph
        static void TryPasteNodesInGraph(Graph graph, Node[] nodes, Vector2 originPosition){
			var newNodes = Graph.CloneNodes(nodes.ToList(), graph, originPosition);
			GraphEditorUtility.activeElements = newNodes.Cast<object>().ToList();
		}

		///The final generic menu used for adding nodes in the canvas
		static GenericMenu GetAddNodeMenu(Graph graph, Vector2 canvasMousePos){
			System.Action<System.Type> Selected = (type) =>	{ GraphEditorUtility.activeElement = graph.AddNode(type, canvasMousePos); };
			var menu = EditorUtils.GetTypeSelectionMenu(graph.baseNodeType, Selected);
			menu = graph.CallbackOnCanvasContextMenu(menu, canvasMousePos);
			return menu;
		}		
	}
}