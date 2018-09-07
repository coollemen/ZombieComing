using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Blackboard")]
	public class CheckString : ConditionTask {

		[BlackboardOnly]
		public BBParameter<string> valueA;
		public BBParameter<string> valueB;

		
		protected override string info{
			get {return valueA + " == " + valueB  + (!checkResultChange? "": "When value is Changed");}
		}

		protected override bool OnCheck(){
			bool compareResult=	valueA.value == valueB.value;
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