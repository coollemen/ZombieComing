using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Blackboard")]
	public class CheckVectorDistance : ConditionTask{

		[BlackboardOnly]
		public BBParameter<Vector3> vectorA;
		[BlackboardOnly]
		public BBParameter<Vector3> vectorB;
		public CompareMethod comparison = CompareMethod.EqualTo;
		public BBParameter<float> distance;
				
		protected override string info{
			get {return string.Format("Distance ({0}, {1}) {2} {3}", vectorA, vectorB, OperationTools.GetCompareString(comparison), distance)  + (!checkResultChange? "": "When value is Changed");}
		}

		protected override bool OnCheck(){
			var d = Vector3.Distance(vectorA.value, vectorB.value);
			bool compareResult=	OperationTools.Compare((float)d, (float)distance.value, comparison, 0f);
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
				
			if (vectorA.varRef != null){
				vectorA.varRef.onValueChanged += OnValueChanged;
			}			
			return base.OnInit();
		}
		
		bool result=false;
		void OnValueChanged(string name, object value){
			result=true;						
		}
	}
}