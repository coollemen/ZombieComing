using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions{

	[Category("Movement/Direct")]
	[Description("Rotate the agent towards the target per frame")]
	public class RotateTowards : ActionTask<Transform> {

		[RequiredField]
		public BBParameter<GameObject> target;
		public BBParameter<float> speed = 2;
		[SliderField(1, 180)]
		public BBParameter<float> angleDifference = 5;
		public BBParameter<Vector3> upVector = Vector3.up;
		public bool waitActionFinish;

		protected override void OnUpdate(){
			if (Vector3.Angle(target.value.transform.position - agent.position, agent.forward) <= angleDifference.value){
				EndAction();
				return;
			}

			var dir = target.value.transform.position - agent.position;
			agent.rotation = Quaternion.LookRotation(Vector3.RotateTowards(agent.forward, dir, speed.value * Time.deltaTime, 0), upVector.value);
			if (!waitActionFinish){
				EndAction();
			}
		}
	}
	
	[Category("Movement/Direct")]
	[Description("Rotate the agent towards the target per frame")]
	public class RotateVector3 : ActionTask<Transform> {

		[RequiredField]
		public BBParameter<Vector3> rotateSpeed;
		public BBParameter<float> time = 1;

		private float countTime=0;
		protected override void OnUpdate(){
			
			
			if (countTime<time.value){
				countTime+=Time.deltaTime;
				agent.Rotate(rotateSpeed.value*Time.deltaTime);
			}else
			{	countTime=0;
				EndAction();
				return;
			}
		}
	}
}