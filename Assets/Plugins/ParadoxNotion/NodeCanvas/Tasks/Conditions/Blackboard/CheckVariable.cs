using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Blackboard")]
	[Description("It's best to use the respective Condition for a type if existant since they support operations as well")]
	public class CheckVariable<T> : ConditionTask {

		[BlackboardOnly]
		public BBParameter<T> valueA;
		public BBParameter<T> valueB;
		
		protected override string info{
			get {return valueA + " == " + valueB  + (!checkResultChange? "": "When value is Changed");}
		}

		protected override bool OnCheck(){
			bool compareResult=	Equals(valueA.value, valueB.value);
			bool tempCheckResult=result; result=false;
			return 
				!checkResultChange?
				compareResult: invert? !(tempCheckResult&&!compareResult): tempCheckResult&&compareResult;
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