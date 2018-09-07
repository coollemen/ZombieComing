//using ParadoxNotion.Design;


//namespace FlowCanvas.Nodes
//{

//    #region EasyButton

//    [Name("CameraAnimatorAction")]
//    [Category("Functions/CameraPath")]
//    [Description(".")]
//    [ContextDefinedInputs(typeof(Flow))]
//    public class CameraAnimatorAction : FlowNode
//    {
//        private FlowOutput outPut;
//        protected override void RegisterPorts()
//        {
//            outPut = AddFlowOutput("Out");
//            var cAnimtor = AddValueInput<CameraPathAnimator>("CameraAnimator");
//            AddFlowInput("Play", (f) => { cAnimtor.value.Play(); outPut.Call(f); });
//            AddFlowInput("Stop", (f) => { cAnimtor.value.Stop(); outPut.Call(f); });
//            AddFlowInput("Pause", (f) => { cAnimtor.value.Pause(); outPut.Call(f); });
//            AddFlowInput("Reverse", (f) => { cAnimtor.value.Reverse(); outPut.Call(f); });

//            var seekPos = AddValueInput<float>("seekPercent");
//            AddFlowInput("SeekPercent", (f) => { cAnimtor.value.Seek(seekPos.value); outPut.Call(f); });

//            var speed = AddValueInput<float>("speed");
//            AddFlowInput("SetSpeed", (f) => { cAnimtor.value.Seek(speed.value); outPut.Call(f); });
//        }
//    }

//    [Name("CameraAnimatorParameter")]
//    [Category("Functions/CameraPath")]
//    [Description(".")]
//    public class CameraAnimatorParameter : FlowNode
//    {
//        protected override void RegisterPorts()
//        {
//            var cAnimtor = AddValueInput<CameraPathAnimator>("CameraAnimator");
//            AddValueOutput("isPlaying", () => { return cAnimtor.value.isPlaying; });
//            AddValueOutput("percent", () => { return cAnimtor.value.percentage; });
//            AddValueOutput("pathSpeed", () => { return cAnimtor.value.pathSpeed; });
//            AddValueOutput("currentTime", () => { return cAnimtor.value.currentTime; });
//            AddValueOutput("nextPath", () => { return cAnimtor.value.cameraPath; });
//        }
//    }

//    #endregion
//}
