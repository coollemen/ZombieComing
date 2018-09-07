using System;
using System.Collections;
using System.Collections.Generic;
using ParadoxNotion;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using ParadoxNotion.Services;
using UnityEngine;

namespace FlowCanvas.Nodes
{


    [Category("Variables")]
    [Name("CastTo", 11)]
    [Description("只能转换非数组列表型变量")]
    [ContextDefinedInputs(typeof(Flow), typeof(object))]
    public class CastTypeTo<T> : FlowNode
    {
#if UNITY_EDITOR
        public override string name
        {
            get { return string.Format(" CastTo<{0}> ", typeof(T).Name); }
        }
#endif

        private FlowOutput castSuccessFlow;
        private FlowOutput castFailureFlow;
        private ValueInput<object> castSource;
        private T result;
        protected override void RegisterPorts()
        {
            //base.RegisterPorts();
            AddFlowInput("Cast", (f) =>
            {
                try
                {
                    if (typeof(T).IsPrimitive)
                    {
                        result = (T)Convert.ChangeType(castSource.value, typeof(T));
                    }
                    else
                    {
                        result = (T)(castSource.value);
                    }
                    castSuccessFlow.Call(f);
                }
                catch
                {
                    result = default(T);
                    castFailureFlow.Call(f);
                }
            });
            castSource = AddValueInput<object>("source");

            castSuccessFlow = AddFlowOutput("Success");
            castFailureFlow = AddFlowOutput("Failure");
            AddValueOutput("target", () => result);
        }
    }
}
