using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Blackboard")]
	public class CheckEnum : ConditionTask {

		[BlackboardOnly]
		public BBObjectParameter valueA = new BBObjectParameter(typeof(System.Enum));
		public BBObjectParameter valueB = new BBObjectParameter(typeof(System.Enum));

		protected override string info{
			get {return valueA + " == " + valueB  + (!checkResultChange? "": "When value is Changed");}
		}

		protected override bool OnCheck(){
			bool compareResult=	Equals(valueA.value, valueB.value);
			bool tempCheckResult=result; result=false;
			return 
				!checkResultChange?
				compareResult:invert? !(tempCheckResult&&!compareResult): tempCheckResult&&compareResult;
		}
		
		public bool checkResultChange=false;
		protected override string OnInit()
		{	
			if (!checkResultChange)
				return base.OnInit();
				
			if (valueA.varRef != null){
				valueA.varRef.onValueChanged += OnValueChanged;
			}			
			return base.OnInit();
		}
		
		bool result=false;
		void OnValueChanged(string name, object value){
			result=true;						
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnTaskInspectorGUI(){
			DrawDefaultInspector();
			if (GUI.changed && valueB.varType != valueA.refType){
				valueB.SetType( valueA.refType );
			}
		}

		#endif
	}
}