using NodeCanvas.Framework;
using System.Collections.Generic;

namespace FlowCanvas
{

    ///Add this component on a game object to be controlled by a FlowScript
    [UnityEngine.AddComponentMenu("FlowCanvas/FlowScript Controller")]
	public class FlowScriptController : GraphOwner<FlowScript> {
		
		public object CallFunction2(string name,params object[] args)
		{ 
			return CallFunction(name,true,args);
		}
		///Calls and returns a value of a custom function in the flowgraph
		public object CallFunction(string name, bool IncludeNestedGraph = false, params object[] args){
            if (IncludeNestedGraph)
            {
                if (!searchOnce)
                {
                    searchOnce = true;
                    nestedGraph = behaviour.GetAllNestedGraphs<Graph>(true);
                    nestedGraph.Add(behaviour);
                    foreach (var g in nestedGraph)
                    {
                        if (g.GetType() == typeof(FlowGraph) || g.GetType().IsSubclassOf(typeof(FlowGraph)))
                        {
                            var dict = ((FlowGraph)g).GetFunctions();
                            if (dict != null && dict.Count > 0)
                            {
                                foreach (var d in dict)
                                {
                                    FunctionNode.Add(d.Key, d.Value);
                                }
                            }
                        }
                    }
                }
            }
            if (IncludeNestedGraph)
            {
                if (FunctionNode.TryGetValue(name, out func))
                {
                    return func.Invoke(args);
                }
                return null;
            }
            else
            {
                return behaviour.CallFunction(name, args);
            }
        }

        bool searchOnce = false;
        List<Graph> nestedGraph=new List<Graph>(); IInvokable func = null;
		Dictionary<string, IInvokable> FunctionNode=new Dictionary<string, IInvokable>();
        
        
        public T CallFunction<T>(string name, bool IncludeNestedGraph = false,params object[] args)
        {   	
            if(IncludeNestedGraph)
            {
                if(!searchOnce)
                {
                    searchOnce = true;
                    nestedGraph = behaviour.GetAllNestedGraphs<Graph>(true);
                    nestedGraph.Add(behaviour);
                    foreach (var g in nestedGraph)
                    {
                        if (g.GetType()== typeof(FlowGraph)||g.GetType().IsSubclassOf(typeof(FlowGraph)))
                        {
                            var dict= ((FlowGraph)g).GetFunctions();
                            if (dict != null && dict.Count > 0)
                            {
                                foreach (var d in dict)
                                {
                                    FunctionNode.Add(d.Key, d.Value);
                                }
                            }
                        }
                    }
                }
            }
            if (IncludeNestedGraph)
            {
                if (FunctionNode.TryGetValue(name, out func))
                {
                    return (T)func.Invoke(args);
                }
                return default(T);
            }else
            {
                return behaviour.CallFunction<T>(name, args);
            }
        }
    }
}