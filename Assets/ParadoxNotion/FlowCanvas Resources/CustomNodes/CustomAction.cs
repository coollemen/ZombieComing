using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using FlowCanvas;
using System.Collections.Generic;

namespace NodeCanvas.Tasks.Actions{
	[Category("FlowScript")]
	public class CallFunction<T> : ActionTask<FlowScriptController> {
		
		public BBParameter<string> FunctionName;
		public BBParameter<object> parameter1;
		public BBParameter<object> parameter2;
		public BBParameter<object> parameter3;
		public BBParameter<object> parameter4;
		public BBParameter<object> parameter5;

		public BBParameter<T> saveAs;
		
		protected override string info{
			get {return "CallFuntion:<b><color=yellow>"+(string.IsNullOrEmpty(FunctionName.value)?"NULL":FunctionName.value) +"</color></b> <<b><color=yellow> " + typeof(T).Name +" </color></b>> SaveAs "+saveAs ; }
		}
		protected override void OnExecute(){
			saveAs.value= agent.CallFunction<T>(FunctionName.value,true,parameter1.value,parameter2.value,parameter3.value,parameter4.value,parameter5.value);
			EndAction();
		}
	}
	
	[Category("FlowScript")]
	public class CallFunctionAction : ActionTask<FlowScriptController> {
	
		public BBParameter<string> FunctionName;
		public BBParameter<object> parameter1;
		public BBParameter<object> parameter2;
		public BBParameter<object> parameter3;
		public BBParameter<object> parameter4;
		public BBParameter<object> parameter5;

		
		protected override string info{
			get {return "CallFuntion: <b><color=yellow>"+FunctionName+"</color></b>" ; }
		}
		protected override void OnExecute(){
			agent.CallFunction(FunctionName.value,true,parameter1.value,parameter2.value,parameter3.value,parameter4.value,parameter5.value);
			EndAction();
		}
	}
	
	[Category("FlowScript")]
	public class CallFunctionCondition : ConditionTask<FlowScriptController> {
	
		public BBParameter<string> FunctionName;
		public BBParameter<object> parameter1;
		public BBParameter<object> parameter2;
		public BBParameter<object> parameter3;
		public BBParameter<object> parameter4;
		public BBParameter<object> parameter5;

		
		protected override string info{
			get {return "CallFuntion: <b><color=yellow>"+FunctionName+"</color></b>" ; }
		}
		
		protected override bool OnCheck(){
			
			return agent.CallFunction<bool>(FunctionName.value,true,parameter1.value,parameter2.value,parameter3.value,parameter4.value,parameter5.value);			
		}
	}
}