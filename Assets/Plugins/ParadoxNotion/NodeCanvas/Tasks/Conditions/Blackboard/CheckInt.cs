using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Blackboard")]
	public class CheckInt : ConditionTask{

		[BlackboardOnly]
		public BBParameter<int> valueA;
		public CompareMethod checkType = CompareMethod.EqualTo;
		public BBParameter<int> valueB;
		
		protected override string info{
			get {return valueA + OperationTools.GetCompareString(checkType) + valueB  + (!checkResultChange? "": "When value is Changed");}
		}

		protected override bool OnCheck(){
			bool compareResult=	OperationTools.Compare((int)valueA.value, (int)valueB.value, checkType);
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
	}
}