using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEngine;
using Logger = ParadoxNotion.Services.Logger;
using FlowCanvas.Nodes;

namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Utility")]
	[Description("Check if a Value On Set or Result Change")]
	public class CheckValue<T> : ConditionTask<GraphOwner> {

		protected override string info{ get {return string.Format("[{0}].value is Changed", targetVariable.name);} }
		protected override bool OnCheck(){ bool tempResult=result; result=false; return tempResult; }
		
		[Name("Compare Value To")]
		[BlackboardOnly]
		public BBParameter<T> targetVariable;

		protected override string OnInit()
		{	
			if (targetVariable.varRef != null){
				targetVariable.varRef.onValueChanged += OnValueChanged;
			}			
			return base.OnInit();
		}
		
		bool result=false;
		void OnValueChanged(string name, object value){
			result=true;						
		}
	}
}