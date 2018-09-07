using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Blackboard")]
	public class CheckFloat : ConditionTask{
		[Name("Compare Value To")]
		[BlackboardOnly]
		public BBParameter<float> valueA;
		public CompareMethod checkType = CompareMethod.EqualTo;
		public BBParameter<float> valueB;
		
		public bool checkResultChange=false;

		[SliderField(0,0.1f)]
		public float differenceThreshold = 0.05f;

		protected override string info{
			get	{return valueA + OperationTools.GetCompareString(checkType) + valueB + (!checkResultChange? "": "When value is Changed");}
		}

		protected override bool OnCheck(){
			bool compareResult=	OperationTools.Compare((float)valueA.value, (float)valueB.value, checkType, differenceThreshold);
			bool tempCheckResult=result; result=false;
			return 
				!checkResultChange?
				compareResult:invert? !(tempCheckResult&&!compareResult): tempCheckResult&&compareResult;
				
		}

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