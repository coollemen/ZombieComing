//using System.Collections;
//using ParadoxNotion.Design;
//using HedgehogTeam.EasyTouch;
//using NodeCanvas.Framework;
//using UnityEngine;
//using System;

//namespace FlowCanvas.Nodes
//{

//    #region TouchEvent

//    [ViewName("EasyTouchEvent")]
//    [Category("Events/EasyTouch")]
//    [Description(".")]
//    public class EasyTouchEvent : EventNode
//    {
//        public override void OnGraphStarted()
//        {
//            base.OnGraphStarted();
//            Register();
//        }

//        #region Register

//        void Register()
//        {
//            if (TouchToggle)
//            {
//                EasyTouch.On_TouchStart += MyTouchStart; //  开始触摸瞬间          
//                EasyTouch.On_TouchDown += MyTouchDown; //触摸状态                 
//                EasyTouch.On_TouchUp += MyTouchUp; //停止触摸       
//            }
//            if (simpleTapToggle)
//            {
//                EasyTouch.On_SimpleTap += MySimpleTap; //点击
//            }

//            if (LongTapToggle)
//            {
//                EasyTouch.On_LongTapStart += MyLongTapStart; //开始点击瞬间   
//                EasyTouch.On_LongTap += MyLongTap; //长按中
//                EasyTouch.On_LongTapEnd += MyLongTapEnd; //停止点击
//            }

//            if (DoubleTapToggle)
//                EasyTouch.On_DoubleTap += MyDoubleTap; //单指双击屏幕

//            if (Touch2FingersToggle)
//            {
//                EasyTouch.On_TouchStart2Fingers += MyTouchStart2Fingers;
//                //双指触摸瞬间    void MyTouchStart2Fingers(Gesture gesture)  {  …  }
//                EasyTouch.On_TouchDown2Fingers += MyTouchDown2Fingers; //双指触摸时      
//                EasyTouch.On_TouchUp2Fingers += MyTouchUp2Fingers; // 双指离开屏幕时
//                EasyTouch.On_Cancel2Fingers += MyCancle2Fingers; //  取消双指触摸(只要有一只手指离开便生效)  
//            }

//            if (SimpleTap2FingersToggle)
//                EasyTouch.On_SimpleTap2Fingers += MySimpleTap2Fingers; //双指点击

//            if (LongTap2FingersToggle)
//            {
//                EasyTouch.On_LongTapStart2Fingers += MyLongTapStart2Fingers; //双指触碰瞬间
//                EasyTouch.On_LongTap2Fingers += MyLongTap2Finger; //双指触摸时
//                EasyTouch.On_LongTapEnd2Fingers += MyLongTapEnd2Finger;
//            }
//            if (DoubleTap2FingersToggle)
//            {
//                EasyTouch.On_DoubleTap2Fingers += MyDoubleTap2Finger; //双指双击
//            }
//            if (drag2FingersToggle)
//            {
//                EasyTouch.On_DragStart2Fingers += MyDragStart2Fingers; //双指拖动开始
//                EasyTouch.On_Drag2Fingers += MyOnDrag2Fingers;
//                EasyTouch.On_DragEnd2Fingers += MyOnDagEnd2Fingers;
//            }

//            if (pinchToggle)
//            {
//                EasyTouch.On_PinchIn += MyPinchIn; //双指向内挤压
//                EasyTouch.On_PinchOut += MyPinchOut; //双指相反方向移动
//                EasyTouch.On_PinchEnd += MyPinchEnd; //结束捏的动作
//            }

//            if (twistToggle)
//            {
//                EasyTouch.On_Twist += MyTwist; //开始扭转
//                EasyTouch.On_TwistEnd += MyTwistEnd; //扭转结束
//            }

//            if (dragToggle)
//            {
//                EasyTouch.On_DragStart += MyDragStart; //开始滑动瞬间   
//                EasyTouch.On_Drag += MyDrag; //滑动中
//                EasyTouch.On_DragEnd += MyDragEnd; //结束滑动时 
//            }

//            if (swipeToggle)
//                EasyTouch.On_Swipe += MySwipe; //滑动
//        }

//        #endregion

//        #region Unregister

//        void UnRegister()
//        {
//            if (TouchToggle)
//            {
//                EasyTouch.On_TouchStart -= MyTouchStart; //  开始触摸瞬间          
//                EasyTouch.On_TouchDown -= MyTouchDown; //触摸状态                 
//                EasyTouch.On_TouchUp -= MyTouchUp; //停止触摸       
//            }
//            if (simpleTapToggle)
//            {
//                EasyTouch.On_SimpleTap -= MySimpleTap; //点击
//            }

//            if (LongTapToggle)
//            {
//                EasyTouch.On_LongTapStart -= MyLongTapStart; //开始点击瞬间   
//                EasyTouch.On_LongTap -= MyLongTap; //长按中
//                EasyTouch.On_LongTapEnd -= MyLongTapEnd; //停止点击
//            }

//            if (DoubleTapToggle)
//                EasyTouch.On_DoubleTap -= MyDoubleTap; //单指双击屏幕

//            if (Touch2FingersToggle)
//            {
//                EasyTouch.On_TouchStart2Fingers -= MyTouchStart2Fingers;
//                //双指触摸瞬间    void MyTouchStart2Fingers(Gesture gesture)  {  …  }
//                EasyTouch.On_TouchDown2Fingers -= MyTouchDown2Fingers; //双指触摸时      
//                EasyTouch.On_TouchUp2Fingers -= MyTouchUp2Fingers; // 双指离开屏幕时
//                EasyTouch.On_Cancel2Fingers -= MyCancle2Fingers; //  取消双指触摸(只要有一只手指离开便生效)  
//            }

//            if (SimpleTap2FingersToggle)
//                EasyTouch.On_SimpleTap2Fingers -= MySimpleTap2Fingers; //双指点击

//            if (LongTap2FingersToggle)
//            {
//                EasyTouch.On_LongTapStart2Fingers -= MyLongTapStart2Fingers; //双指触碰瞬间
//                EasyTouch.On_LongTap2Fingers -= MyLongTap2Finger; //双指触摸时
//                EasyTouch.On_LongTapEnd2Fingers -= MyLongTapEnd2Finger;
//            }
//            if (DoubleTap2FingersToggle)
//            {
//                EasyTouch.On_DoubleTap2Fingers -= MyDoubleTap2Finger; //双指双击
//            }
//            if (drag2FingersToggle)
//            {
//                EasyTouch.On_DragStart2Fingers -= MyDragStart2Fingers; //双指拖动开始
//                EasyTouch.On_Drag2Fingers -= MyOnDrag2Fingers;
//                EasyTouch.On_DragEnd2Fingers -= MyOnDagEnd2Fingers;
//            }

//            if (pinchToggle)
//            {
//                EasyTouch.On_PinchIn -= MyPinchIn; //双指向内挤压
//                EasyTouch.On_PinchOut -= MyPinchOut; //双指相反方向移动
//                EasyTouch.On_PinchEnd -= MyPinchEnd; //结束捏的动作
//            }

//            if (twistToggle)
//            {
//                EasyTouch.On_Twist -= MyTwist; //开始扭转
//                EasyTouch.On_TwistEnd -= MyTwistEnd; //扭转结束
//            }

//            if (dragToggle)
//            {
//                EasyTouch.On_DragStart -= MyDragStart; //开始滑动瞬间   
//                EasyTouch.On_Drag -= MyDrag; //滑动中
//                EasyTouch.On_DragEnd -= MyDragEnd; //结束滑动时 
//            }

//            if (swipeToggle)
//                EasyTouch.On_Swipe -= MySwipe; //滑动
//        }

//        #endregion

//        public override void OnGraphStoped()
//        {
//            base.OnGraphStoped();
//            UnRegister();
//        }

//        private Gesture ges;

//        private FlowOutput TouchStart2Fingers;
//        private FlowOutput OnCancle2Finger;

//        [SerializeField]
//        private bool swipeToggle = false;
//        private FlowOutput Swipe;

//        private EasyTouch.SwipeDirection swipe;
//        private Vector2 swipeVector2;


//        [SerializeField]
//        private bool dragToggle = false;

//        private float dragAngle;
//        private FlowOutput DragStart;
//        private FlowOutput Draging;
//        private FlowOutput DragEnd;

//        [SerializeField]
//        private bool twistToggle = false;

//        private float twistAngle;

//        private FlowOutput Twisting;
//        private FlowOutput TwistEnd;

//        [SerializeField]
//        private bool pinchToggle = false;

//        private float pinchDelta;
//        private FlowOutput PinchIn;
//        private FlowOutput PinchOut;
//        private FlowOutput PinchEnd;

//        [SerializeField]
//        private bool drag2FingersToggle = false;

//        private FlowOutput Drag2FingersStart;
//        private FlowOutput Drag2Fingersing;
//        private FlowOutput Drag2FingersEnd;

//        [SerializeField]
//        private bool DoubleTap2FingersToggle = false;

//        private FlowOutput DoubleTap2Fingers;

//        [SerializeField]
//        private bool LongTap2FingersToggle = false;

//        private FlowOutput LongTapStart2Fingers;
//        private FlowOutput LongTaping2Fingers;
//        private FlowOutput LongTapEnd2Fingers;

//        [SerializeField]
//        private bool SimpleTap2FingersToggle = false;

//        private FlowOutput SimpleTap2Fingers;


//        [SerializeField]
//        private bool Touch2FingersToggle = false;

//        private FlowOutput TouchUp2Fingers;
//        private FlowOutput TouchDown2Fingers;

//        [SerializeField]
//        private bool DoubleTapToggle = false;

//        private FlowOutput DoubleTap;

//        [SerializeField]
//        private bool TouchToggle = false;

//        private FlowOutput TouchStart;
//        private FlowOutput TouchUp;
//        private FlowOutput TouchDown;

//        [SerializeField]
//        private bool simpleTapToggle = false;

//        private FlowOutput SimpleTap;

//        [SerializeField]
//        private bool LongTapToggle = false;

//        private FlowOutput LongTapStart;
//        private FlowOutput LongTaping;
//        private FlowOutput LongTapEnd;

//        private void MySwipe(Gesture gesture)
//        {
//            ges = gesture;

//            swipe = ges.swipe;
//            swipeVector2 = ges.swipeVector;

//            Swipe.Call(new Flow());
//        }

//        private void MyDragEnd(Gesture gesture)
//        {
//            ges = gesture;
//            dragAngle = gesture.GetSwipeOrDragAngle();
//            DragEnd.Call(new Flow());
//        }

//        private void MyDrag(Gesture gesture)
//        {
//            ges = gesture;

//            dragAngle = gesture.GetSwipeOrDragAngle();
//            Draging.Call(new Flow());
//        }

//        private void MyDragStart(Gesture gesture)
//        {
//            ges = gesture;

//            dragAngle = gesture.GetSwipeOrDragAngle();
//            DragStart.Call(new Flow());
//        }

//        private void MyTwistEnd(Gesture gesture)
//        {
//            ges = gesture;

//            twistAngle = ges.twistAngle;
//            TwistEnd.Call(new Flow());
//        }

//        private void MyTwist(Gesture gesture)
//        {
//            ges = gesture;

//            twistAngle = ges.twistAngle;
//            Twisting.Call(new Flow());
//        }

//        private void MyPinchEnd(Gesture gesture)
//        {
//            ges = gesture;

//            pinchDelta = ges.deltaPinch;
//            PinchEnd.Call(new Flow());
//        }

//        private void MyPinchOut(Gesture gesture)
//        {
//            ges = gesture;

//            pinchDelta = -ges.deltaPinch;
//            PinchOut.Call(new Flow());
//        }

//        private void MyPinchIn(Gesture gesture)
//        {
//            ges = gesture;

//            pinchDelta = ges.deltaPinch;
//            PinchIn.Call(new Flow());
//        }

//        private void MyOnDagEnd2Fingers(Gesture gesture)
//        {
//            ges = gesture;

//            dragAngle = gesture.GetSwipeOrDragAngle();
//            Drag2FingersEnd.Call(new Flow());
//        }

//        private void MyOnDrag2Fingers(Gesture gesture)
//        {
//            ges = gesture;

//            dragAngle = gesture.GetSwipeOrDragAngle();
//            Drag2Fingersing.Call(new Flow());
//        }

//        private void MyDragStart2Fingers(Gesture gesture)
//        {
//            ges = gesture;

//            dragAngle = gesture.GetSwipeOrDragAngle();
//            Drag2FingersStart.Call(new Flow());
//        }

//        private void MyDoubleTap2Finger(Gesture gesture)
//        {
//            ges = gesture;

//            DoubleTap2Fingers.Call(new Flow());
//        }

//        private void MyCancle2Fingers(Gesture gesture)
//        {
//            ges = gesture;
//            OnCancle2Finger.Call(new Flow());
//        }

//        private void MyLongTapEnd2Finger(Gesture gesture)
//        {
//            ges = gesture;
//            LongTapEnd2Fingers.Call(new Flow());
//        }

//        private void MyLongTap2Finger(Gesture gesture)
//        {
//            ges = gesture;
//            LongTaping2Fingers.Call(new Flow());
//        }

//        private void MyLongTapStart2Fingers(Gesture gesture)
//        {
//            ges = gesture;
//            LongTapStart2Fingers.Call(new Flow());
//        }

//        private void MySimpleTap2Fingers(Gesture gesture)
//        {
//            ges = gesture;
//            SimpleTap2Fingers.Call(new Flow());
//        }

//        private void MyTouchUp2Fingers(Gesture gesture)
//        {
//            ges = gesture;
//            TouchUp2Fingers.Call(new Flow());
//        }

//        private void MyTouchDown2Fingers(Gesture gesture)
//        {
//            ges = gesture;
//            TouchDown2Fingers.Call(new Flow());
//        }

//        private void MyTouchStart2Fingers(Gesture gesture)
//        {
//            ges = gesture;
//            TouchStart2Fingers.Call(new Flow());
//        }

//        private void MyDoubleTap(Gesture gesture)
//        {
//            ges = gesture;
//            DoubleTap.Call(new Flow());
//        }

//        private void MyLongTapEnd(Gesture gesture)
//        {
//            ges = gesture;
//            LongTapEnd.Call(new Flow());
//        }

//        private void MyLongTap(Gesture gesture)
//        {
//            ges = gesture;
//            LongTaping.Call(new Flow());
//        }

//        private void MyLongTapStart(Gesture gesture)
//        {
//            ges = gesture;
//            LongTapStart.Call(new Flow());
//        }

//        void MyTouchStart(Gesture gesture) //注册触摸事件的自定义方法, 参数是Gesture
//        {
//            ges = gesture;
//            TouchStart.Call(new Flow());
//        }

//        void MyTouchDown(Gesture gesture)
//        {
//            ges = gesture;
//            TouchDown.Call(new Flow());
//        }

//        void MyTouchUp(Gesture gesture)
//        {
//            ges = gesture;
//            TouchUp.Call(new Flow());
//        }

//        void MySimpleTap(Gesture gesture)
//        {
//            ges = gesture;
//            SimpleTap.Call(new Flow());
//        }

//        protected override void RegisterPorts()
//        {

//            if (LongTapToggle)
//            {
//                LongTapStart = AddFlowOutput("LongTapStart");
//                LongTaping = AddFlowOutput("LongTaping");
//                LongTapEnd = AddFlowOutput("LongTapEnd");
//            }

//            if (simpleTapToggle)
//            {
//                SimpleTap = AddFlowOutput("SimpleTap");
//            }

//            if (TouchToggle)
//            {
//                TouchStart = AddFlowOutput("TouchStart");
//                TouchUp = AddFlowOutput("TouchUp");
//                TouchDown = AddFlowOutput("TouchDown");
//            }

//            if (DoubleTapToggle)
//            {
//                DoubleTap = AddFlowOutput("DoubleTap");
//            }

//            if (DoubleTap2FingersToggle)
//            {
//                DoubleTap2Fingers = AddFlowOutput("DoubleTap");
//            }
//            if (Touch2FingersToggle)
//            {
//                TouchStart2Fingers = AddFlowOutput("TouchStart2Fingers");
//                TouchUp2Fingers = AddFlowOutput("TouchUp2Fingers");
//                TouchDown2Fingers = AddFlowOutput("TouchDown2Fingers");
//                OnCancle2Finger = AddFlowOutput("OnCancle2Finger");
//            }

//            if (LongTap2FingersToggle)
//            {
//                LongTapStart2Fingers = AddFlowOutput("LongTapStart2Fingers");
//                LongTaping2Fingers = AddFlowOutput("LongTaping2Fingers");
//                LongTapEnd2Fingers = AddFlowOutput("LongTapEnd2Fingers");
//            }

//            if (drag2FingersToggle)
//            {
//                Drag2FingersStart = AddFlowOutput("Drag2FingersStart");
//                Drag2Fingersing = AddFlowOutput("Drag2Fingersing");
//                Drag2FingersEnd = AddFlowOutput("Drag2FingersEnd");
//            }

//            if (pinchToggle)
//            {
//                TouchStart2Fingers = AddFlowOutput("TouchStart2Fingers");
//                PinchIn = AddFlowOutput("PinchIn");
//                PinchOut = AddFlowOutput("PinchOut");
//                PinchEnd = AddFlowOutput("PinchEnd");
//                OnCancle2Finger = AddFlowOutput("OnCancle2Finger");

//                AddValueOutput<float>("pinchDelta", () => { return pinchDelta; });
//            }

//            if (twistToggle)
//            {
//                TouchStart2Fingers = AddFlowOutput("TouchStart2Fingers");
//                Twisting = AddFlowOutput("Twisting");
//                TwistEnd = AddFlowOutput("TwistEnd");
//                OnCancle2Finger = AddFlowOutput("OnCancle2Finger");

//                AddValueOutput<float>("twistAngle", () => { return twistAngle; });

//            }

//            if (dragToggle)
//            {
//                DragStart = AddFlowOutput("DragStart");
//                Draging = AddFlowOutput("Draging");
//                DragEnd = AddFlowOutput("DragEnd");

//            }

//            if (swipeToggle)
//            {
//                Swipe = AddFlowOutput("Swipe");
//                AddValueOutput<EasyTouch.SwipeDirection>("SwipeDiection", () => { return swipe; });
//                AddValueOutput<Vector2>("SwipeVector2", () => { return swipeVector2; });
//            }

//            if (drag2FingersToggle || dragToggle)
//                AddValueOutput<float>("dragAngle", () => { return dragAngle; });

//            AddValueOutput<GameObject>("PickGameObject", () => { return ges.pickedObject; });
//            AddValueOutput<Gesture>("Gesture", () => { return ges; });
//            AddValueOutput<int>("FingerIndex", () => { return ges.fingerIndex; });
//        }
//#if UNITY_EDITOR
//        protected override void OnNodeInspectorGUI()
//        {
//            base.OnNodeInspectorGUI();
//            swipeToggle = GUILayout.Toggle(swipeToggle, "swipeToggle");
//            dragToggle = GUILayout.Toggle(dragToggle, "dragToggle");
//            twistToggle = GUILayout.Toggle(twistToggle, "twistToggle");
//            pinchToggle = GUILayout.Toggle(pinchToggle, "pinchToggle");
//            drag2FingersToggle = GUILayout.Toggle(drag2FingersToggle, "drag2FingersToggle");

//            DoubleTap2FingersToggle = GUILayout.Toggle(DoubleTap2FingersToggle, "DoubleTap2FingersToggle");
//            LongTap2FingersToggle = GUILayout.Toggle(LongTap2FingersToggle, "LongTap2FingersToggle");
//            Touch2FingersToggle = GUILayout.Toggle(Touch2FingersToggle, "Touch2FingersToggle");
//            DoubleTapToggle = GUILayout.Toggle(DoubleTapToggle, "DoubleTapToggle");
//            TouchToggle = GUILayout.Toggle(TouchToggle, "TouchToggle");
//            simpleTapToggle = GUILayout.Toggle(simpleTapToggle, "simpleTapToggle");
//            LongTapToggle = GUILayout.Toggle(LongTapToggle, "LongTapToggle");

//            if (GUILayout.Button("Refresh"))
//            {
//                GatherPorts();
//                Register();
//            }
//        }
//#endif
//    }

//    #endregion

//    #region Joystick
//    [ViewName("Joystick")]
//    [Category("Events/EasyTouch")]
//    [Description(".")]
//    public class EasyJoystick : EventNode<ETCJoystick>
//    {
//        public override void OnGraphStarted()
//        {
//            base.OnGraphStarted();
//            Register();
//        }
//        public override void OnGraphStoped()
//        {
//            base.OnGraphStarted();
//            UnRegister();
//        }

//        void Register()
//        {
//            if (target.value == null)
//                return;
//            if (downToggle)
//            {
//                target.value.OnDownDown.AddListener(OnDownDown);
//                target.value.OnDownUp.AddListener(OnDownUp);
//                target.value.OnDownLeft.AddListener(OnDownLeft);
//                target.value.OnDownRight.AddListener(OnDownRight);
//            }
//            if (pressToggle)
//            {
//                target.value.OnPressDown.AddListener(OnPressDown);
//                target.value.OnPressUp.AddListener(OnPressUp);
//                target.value.OnPressLeft.AddListener(OnPressLeft);
//                target.value.OnPressRight.AddListener(OnPressRight);
//            }
//            if (moveToggle)
//            {
//                target.value.onMove.AddListener(OnMove);
//                target.value.onMoveStart.AddListener(OnMoveStart);
//                target.value.onMoveSpeed.AddListener(OnMoveSpeed);
//                target.value.onMoveEnd.AddListener(OnMoveEnd);
//            }

//            if (touchToggle)
//            {
//                target.value.onTouchStart.AddListener(OnTouchStart);
//                target.value.onTouchUp.AddListener(OnTouchUp);
//            }
//        }


//        void UnRegister()
//        {
//            if (downToggle)
//            {
//                target.value.OnDownDown.RemoveListener(OnDownDown);
//                target.value.OnDownUp.RemoveListener(OnDownUp);
//                target.value.OnDownLeft.RemoveListener(OnDownLeft);
//                target.value.OnDownRight.RemoveListener(OnDownRight);
//            }
//            if (pressToggle)
//            {
//                target.value.OnPressDown.RemoveListener(OnPressDown);
//                target.value.OnPressUp.RemoveListener(OnPressUp);
//                target.value.OnPressLeft.RemoveListener(OnPressLeft);
//                target.value.OnPressRight.RemoveListener(OnPressRight);
//            }
//            if (moveToggle)
//            {
//                target.value.onMove.RemoveListener(OnMove);
//                target.value.onMoveStart.RemoveListener(OnMoveStart);
//                target.value.onMoveSpeed.RemoveListener(OnMoveSpeed);
//                target.value.onMoveEnd.RemoveListener(OnMoveEnd);
//            }

//            if (touchToggle)
//            {
//                target.value.onTouchStart.RemoveListener(OnTouchStart);
//                target.value.onTouchUp.RemoveListener(OnTouchUp);
//            }
//        }
//        [SerializeField]
//        private bool downToggle;
//        private FlowOutput _OnDownDown;
//        private FlowOutput _OnDownUp;
//        private FlowOutput _OnDownLeft;
//        private FlowOutput _OnDownRight;


//        void OnDownDown()
//        {
//            _OnDownDown.Call(new Flow());
//        }
//        void OnDownUp()
//        {
//            _OnDownUp.Call(new Flow());
//        }
//        void OnDownLeft()
//        {
//            _OnDownLeft.Call(new Flow());
//        }
//        void OnDownRight()
//        {
//            _OnDownRight.Call(new Flow());
//        }

//        [SerializeField]
//        private bool pressToggle;
//        private FlowOutput _OnPressDown;
//        private FlowOutput _OnPressUp;
//        private FlowOutput _OnPressLeft;
//        private FlowOutput _OnPressRight;


//        void OnPressDown()
//        {
//            _OnPressDown.Call(new Flow());
//        }
//        void OnPressUp()
//        {
//            _OnPressUp.Call(new Flow());
//        }
//        void OnPressLeft()
//        {
//            _OnPressLeft.Call(new Flow());
//        }
//        void OnPressRight()
//        {
//            _OnPressRight.Call(new Flow());
//        }

//        [SerializeField]
//        private bool moveToggle;

//        private Vector2 moveSpeed;
//        private Vector2 moveVector2;
//        private FlowOutput _OnMove;
//        private FlowOutput _OnMoveStart;
//        private FlowOutput _OnMoveEnd;
//        private void OnMoveEnd()
//        {
//            _OnMoveEnd.Call(new Flow());
//        }

//        private void OnMoveSpeed(Vector2 arg0)
//        {
//            moveSpeed = arg0;
//        }

//        private void OnMoveStart()
//        {
//            _OnMoveStart.Call(new Flow());
//        }

//        private void OnMove(Vector2 arg0)
//        {
//            moveVector2 = arg0;
//            _OnMove.Call(new Flow());
//        }


//        [SerializeField]
//        private bool touchToggle;

//        private FlowOutput _OnTouchStart;
//        private FlowOutput _OnTouchUp;

//        private void OnTouchUp()
//        {
//            _OnTouchUp.Call(new Flow());
//        }

//        private void OnTouchStart()
//        {
//            _OnTouchStart.Call(new Flow());
//        }

//        protected override void RegisterPorts()
//        {
//            //base.RegisterPorts();

//            if (downToggle)
//            {
//                _OnDownDown = AddFlowOutput("OnDownDown");
//                _OnDownUp = AddFlowOutput("OnDownUp");
//                _OnDownLeft = AddFlowOutput("OnDownLeft");
//                _OnDownRight = AddFlowOutput("OnDownRight");
//            }

//            if (pressToggle)
//            {
//                _OnPressDown = AddFlowOutput("OnPressDown");
//                _OnPressUp = AddFlowOutput("OnPressUp");
//                _OnPressLeft = AddFlowOutput("OnPressLeft");
//                _OnPressRight = AddFlowOutput("OnPressRight");
//            }

//            if (moveToggle)
//            {
//                _OnMove = AddFlowOutput("OnMove");
//                _OnMoveStart = AddFlowOutput("OnMoveStart");
//                _OnMoveEnd = AddFlowOutput("OnMoveEnd");

//                AddValueOutput("moveSpeed", () => { return moveSpeed; });
//                AddValueOutput("moveVector2", () => { return moveVector2; });
//            }

//            if (touchToggle)
//            {
//                _OnTouchStart = AddFlowOutput("OnTouchStart");
//                _OnTouchUp = AddFlowOutput("OnTouchUp");
//            }
//        }
//#if UNITY_EDITOR
//        protected override void OnNodeInspectorGUI()
//        {
//            base.OnNodeInspectorGUI();
//            downToggle = GUILayout.Toggle(downToggle, "downEvent");
//            pressToggle = GUILayout.Toggle(pressToggle, "pressEvent");
//            moveToggle = GUILayout.Toggle(moveToggle, "moveEvent");
//            touchToggle = GUILayout.Toggle(touchToggle, "touchToggle");

//            if (GUILayout.Button("Refresh"))
//            {
//                GatherPorts();
//                Register();
//            }
//        }
//#endif
//    }
//    #endregion

//    #region EasyTouchPad
//    [ViewName("TouchPad")]
//    [Category("Events/EasyTouch")]
//    [Description(".")]
//    public class EasyTouchPad : EventNode<ETCTouchPad>
//    {
//        public override void OnGraphStarted()
//        {
//            base.OnGraphStarted();
//            Register();
//        }
//        public override void OnGraphStoped()
//        {
//            base.OnGraphStarted();
//            UnRegister();
//        }

//        void Register()
//        {
//            if (target.value == null)
//                return;
//            if (downToggle)
//            {
//                target.value.OnDownDown.AddListener(OnDownDown);
//                target.value.OnDownUp.AddListener(OnDownUp);
//                target.value.OnDownLeft.AddListener(OnDownLeft);
//                target.value.OnDownRight.AddListener(OnDownRight);
//            }
//            if (pressToggle)
//            {
//                target.value.OnPressDown.AddListener(OnPressDown);
//                target.value.OnPressUp.AddListener(OnPressUp);
//                target.value.OnPressLeft.AddListener(OnPressLeft);
//                target.value.OnPressRight.AddListener(OnPressRight);
//            }
//            if (moveToggle)
//            {
//                target.value.onMove.AddListener(OnMove);
//                target.value.onMoveStart.AddListener(OnMoveStart);
//                target.value.onMoveSpeed.AddListener(OnMoveSpeed);
//                target.value.onMoveEnd.AddListener(OnMoveEnd);
//            }

//            if (touchToggle)
//            {
//                target.value.onTouchStart.AddListener(OnTouchStart);
//                target.value.onTouchUp.AddListener(OnTouchUp);
//            }
//        }


//        void UnRegister()
//        {
//            if (downToggle)
//            {
//                target.value.OnDownDown.RemoveListener(OnDownDown);
//                target.value.OnDownUp.RemoveListener(OnDownUp);
//                target.value.OnDownLeft.RemoveListener(OnDownLeft);
//                target.value.OnDownRight.RemoveListener(OnDownRight);
//            }
//            if (pressToggle)
//            {
//                target.value.OnPressDown.RemoveListener(OnPressDown);
//                target.value.OnPressUp.RemoveListener(OnPressUp);
//                target.value.OnPressLeft.RemoveListener(OnPressLeft);
//                target.value.OnPressRight.RemoveListener(OnPressRight);
//            }
//            if (moveToggle)
//            {
//                target.value.onMove.RemoveListener(OnMove);
//                target.value.onMoveStart.RemoveListener(OnMoveStart);
//                target.value.onMoveSpeed.RemoveListener(OnMoveSpeed);
//                target.value.onMoveEnd.RemoveListener(OnMoveEnd);
//            }

//            if (touchToggle)
//            {
//                target.value.onTouchStart.RemoveListener(OnTouchStart);
//                target.value.onTouchUp.RemoveListener(OnTouchUp);
//            }
//        }
//        [SerializeField]
//        private bool downToggle;
//        private FlowOutput _OnDownDown;
//        private FlowOutput _OnDownUp;
//        private FlowOutput _OnDownLeft;
//        private FlowOutput _OnDownRight;


//        void OnDownDown()
//        {
//            _OnDownDown.Call(new Flow());
//        }
//        void OnDownUp()
//        {
//            _OnDownUp.Call(new Flow());
//        }
//        void OnDownLeft()
//        {
//            _OnDownLeft.Call(new Flow());
//        }
//        void OnDownRight()
//        {
//            _OnDownRight.Call(new Flow());
//        }

//        [SerializeField]
//        private bool pressToggle;
//        private FlowOutput _OnPressDown;
//        private FlowOutput _OnPressUp;
//        private FlowOutput _OnPressLeft;
//        private FlowOutput _OnPressRight;


//        void OnPressDown()
//        {
//            _OnPressDown.Call(new Flow());
//        }
//        void OnPressUp()
//        {
//            _OnPressUp.Call(new Flow());
//        }
//        void OnPressLeft()
//        {
//            _OnPressLeft.Call(new Flow());
//        }
//        void OnPressRight()
//        {
//            _OnPressRight.Call(new Flow());
//        }

//        [SerializeField]
//        private bool moveToggle;

//        private Vector2 moveSpeed;
//        private Vector2 moveVector2;
//        private FlowOutput _OnMove;
//        private FlowOutput _OnMoveStart;
//        private FlowOutput _OnMoveEnd;
//        private void OnMoveEnd()
//        {
//            _OnMoveEnd.Call(new Flow());
//        }

//        private void OnMoveSpeed(Vector2 arg0)
//        {
//            moveSpeed = arg0;
//        }

//        private void OnMoveStart()
//        {
//            _OnMoveStart.Call(new Flow());
//        }

//        private void OnMove(Vector2 arg0)
//        {
//            moveVector2 = arg0;
//            _OnMove.Call(new Flow());
//        }


//        [SerializeField]
//        private bool touchToggle;

//        private FlowOutput _OnTouchStart;
//        private FlowOutput _OnTouchUp;

//        private void OnTouchUp()
//        {
//            _OnTouchUp.Call(new Flow());
//        }

//        private void OnTouchStart()
//        {
//            _OnTouchStart.Call(new Flow());
//        }

//        protected override void RegisterPorts()
//        {
//            //base.RegisterPorts();

//            if (downToggle)
//            {
//                _OnDownDown = AddFlowOutput("OnDownDown");
//                _OnDownUp = AddFlowOutput("OnDownUp");
//                _OnDownLeft = AddFlowOutput("OnDownLeft");
//                _OnDownRight = AddFlowOutput("OnDownRight");
//            }

//            if (pressToggle)
//            {
//                _OnPressDown = AddFlowOutput("OnPressDown");
//                _OnPressUp = AddFlowOutput("OnPressUp");
//                _OnPressLeft = AddFlowOutput("OnPressLeft");
//                _OnPressRight = AddFlowOutput("OnPressRight");
//            }

//            if (moveToggle)
//            {
//                _OnMove = AddFlowOutput("OnMove");
//                _OnMoveStart = AddFlowOutput("OnMoveStart");
//                _OnMoveEnd = AddFlowOutput("OnMoveEnd");

//                AddValueOutput("moveSpeed", () => { return moveSpeed; });
//                AddValueOutput("moveVector2", () => { return moveVector2; });
//            }

//            if (touchToggle)
//            {
//                _OnTouchStart = AddFlowOutput("OnTouchStart");
//                _OnTouchUp = AddFlowOutput("OnTouchUp");
//            }
//        }
//#if UNITY_EDITOR
//        protected override void OnNodeInspectorGUI()
//        {
//            base.OnNodeInspectorGUI();
//            downToggle = GUILayout.Toggle(downToggle, "downEvent");
//            pressToggle = GUILayout.Toggle(pressToggle, "pressEvent");
//            moveToggle = GUILayout.Toggle(moveToggle, "moveEvent");
//            touchToggle = GUILayout.Toggle(touchToggle, "touchToggle");

//            if (GUILayout.Button("Refresh"))
//            {
//                GatherPorts();
//                Register();
//            }
//        }
//#endif
//    }
//    #endregion

//    #region ETCDPad
//    [ViewName("DPad")]
//    [Category("Events/EasyTouch")]
//    [Description(".")]
//    public class EasyDPad : EventNode<ETCDPad>
//    {
//        public override void OnGraphStarted()
//        {
//            base.OnGraphStarted();
//            Register();
//        }
//        public override void OnGraphStoped()
//        {
//            base.OnGraphStarted();
//            UnRegister();
//        }

//        void Register()
//        {
//            if (target.value == null)
//                return;

//            if (downToggle)
//            {
//                target.value.OnDownDown.AddListener(OnDownDown);
//                target.value.OnDownUp.AddListener(OnDownUp);
//                target.value.OnDownLeft.AddListener(OnDownLeft);
//                target.value.OnDownRight.AddListener(OnDownRight);
//            }
//            if (pressToggle)
//            {
//                target.value.OnPressDown.AddListener(OnPressDown);
//                target.value.OnPressUp.AddListener(OnPressUp);
//                target.value.OnPressLeft.AddListener(OnPressLeft);
//                target.value.OnPressRight.AddListener(OnPressRight);
//            }
//            if (moveToggle)
//            {
//                target.value.onMove.AddListener(OnMove);
//                target.value.onMoveStart.AddListener(OnMoveStart);
//                target.value.onMoveSpeed.AddListener(OnMoveSpeed);
//                target.value.onMoveEnd.AddListener(OnMoveEnd);
//            }

//            if (touchToggle)
//            {
//                target.value.onTouchStart.AddListener(OnTouchStart);
//                target.value.onTouchUp.AddListener(OnTouchUp);
//            }
//        }


//        void UnRegister()
//        {
//            if (downToggle)
//            {
//                target.value.OnDownDown.RemoveListener(OnDownDown);
//                target.value.OnDownUp.RemoveListener(OnDownUp);
//                target.value.OnDownLeft.RemoveListener(OnDownLeft);
//                target.value.OnDownRight.RemoveListener(OnDownRight);
//            }
//            if (pressToggle)
//            {
//                target.value.OnPressDown.RemoveListener(OnPressDown);
//                target.value.OnPressUp.RemoveListener(OnPressUp);
//                target.value.OnPressLeft.RemoveListener(OnPressLeft);
//                target.value.OnPressRight.RemoveListener(OnPressRight);
//            }
//            if (moveToggle)
//            {
//                target.value.onMove.RemoveListener(OnMove);
//                target.value.onMoveStart.RemoveListener(OnMoveStart);
//                target.value.onMoveSpeed.RemoveListener(OnMoveSpeed);
//                target.value.onMoveEnd.RemoveListener(OnMoveEnd);
//            }

//            if (touchToggle)
//            {
//                target.value.onTouchStart.RemoveListener(OnTouchStart);
//                target.value.onTouchUp.RemoveListener(OnTouchUp);
//            }
//        }
//        [SerializeField]
//        private bool downToggle;
//        private FlowOutput _OnDownDown;
//        private FlowOutput _OnDownUp;
//        private FlowOutput _OnDownLeft;
//        private FlowOutput _OnDownRight;


//        void OnDownDown()
//        {
//            _OnDownDown.Call(new Flow());
//        }
//        void OnDownUp()
//        {
//            _OnDownUp.Call(new Flow());
//        }
//        void OnDownLeft()
//        {
//            _OnDownLeft.Call(new Flow());
//        }
//        void OnDownRight()
//        {
//            _OnDownRight.Call(new Flow());
//        }

//        [SerializeField]
//        private bool pressToggle;
//        private FlowOutput _OnPressDown;
//        private FlowOutput _OnPressUp;
//        private FlowOutput _OnPressLeft;
//        private FlowOutput _OnPressRight;


//        void OnPressDown()
//        {
//            _OnPressDown.Call(new Flow());
//        }
//        void OnPressUp()
//        {
//            _OnPressUp.Call(new Flow());
//        }
//        void OnPressLeft()
//        {
//            _OnPressLeft.Call(new Flow());
//        }
//        void OnPressRight()
//        {
//            _OnPressRight.Call(new Flow());
//        }

//        [SerializeField]
//        private bool moveToggle;

//        private Vector2 moveSpeed;
//        private Vector2 moveVector2;
//        private FlowOutput _OnMove;
//        private FlowOutput _OnMoveStart;
//        private FlowOutput _OnMoveEnd;
//        private void OnMoveEnd()
//        {
//            _OnMoveEnd.Call(new Flow());
//        }

//        private void OnMoveSpeed(Vector2 arg0)
//        {
//            moveSpeed = arg0;
//        }

//        private void OnMoveStart()
//        {
//            _OnMoveStart.Call(new Flow());
//        }

//        private void OnMove(Vector2 arg0)
//        {
//            moveVector2 = arg0;
//            _OnMove.Call(new Flow());
//        }


//        [SerializeField]
//        private bool touchToggle;

//        private FlowOutput _OnTouchStart;
//        private FlowOutput _OnTouchUp;

//        private void OnTouchUp()
//        {
//            _OnTouchUp.Call(new Flow());
//        }

//        private void OnTouchStart()
//        {
//            _OnTouchStart.Call(new Flow());
//        }

//        protected override void RegisterPorts()
//        {
//            //base.RegisterPorts();

//            if (downToggle)
//            {
//                _OnDownDown = AddFlowOutput("OnDownDown");
//                _OnDownUp = AddFlowOutput("OnDownUp");
//                _OnDownLeft = AddFlowOutput("OnDownLeft");
//                _OnDownRight = AddFlowOutput("OnDownRight");
//            }

//            if (pressToggle)
//            {
//                _OnPressDown = AddFlowOutput("OnPressDown");
//                _OnPressUp = AddFlowOutput("OnPressUp");
//                _OnPressLeft = AddFlowOutput("OnPressLeft");
//                _OnPressRight = AddFlowOutput("OnPressRight");
//            }

//            if (moveToggle)
//            {
//                _OnMove = AddFlowOutput("OnMove");
//                _OnMoveStart = AddFlowOutput("OnMoveStart");
//                _OnMoveEnd = AddFlowOutput("OnMoveEnd");

//                AddValueOutput("moveSpeed", () => { return moveSpeed; });
//                AddValueOutput("moveVector2", () => { return moveVector2; });
//            }

//            if (touchToggle)
//            {
//                _OnTouchStart = AddFlowOutput("OnTouchStart");
//                _OnTouchUp = AddFlowOutput("OnTouchUp");
//            }
//        }
//#if UNITY_EDITOR
//        protected override void OnNodeInspectorGUI()
//        {
//            base.OnNodeInspectorGUI();
//            downToggle = GUILayout.Toggle(downToggle, "downEvent");
//            pressToggle = GUILayout.Toggle(pressToggle, "pressEvent");
//            moveToggle = GUILayout.Toggle(moveToggle, "moveEvent");
//            touchToggle = GUILayout.Toggle(touchToggle, "touchToggle");

//            if (GUILayout.Button("Refresh"))
//            {
//                GatherPorts();
//                Register();
//            }
//        }
//#endif
//    }
//    #endregion

//    #region EasyButton
//    [ViewName("EasyButton")]
//    [Category("Events/EasyTouch")]
//    [Description(".")]
//    public class EasyButton : EventNode<ETCButton>
//    {
//        public override void OnGraphStarted()
//        {
//            base.OnGraphStarted();
//            Register();
//        }
//        public override void OnGraphStoped()
//        {
//            base.OnGraphStarted();
//            UnRegister();
//        }

//        void Register()
//        {

//            target.value.onDown.AddListener(OnDown);
//            target.value.onPressed.AddListener(OnPressed);
//            target.value.onUp.AddListener(OnUp);

//        }


//        void UnRegister()
//        {
//            target.value.onDown.RemoveListener(OnDown);
//            target.value.onPressed.RemoveListener(OnPressed);
//            target.value.onUp.RemoveListener(OnUp);
//        }
//        [SerializeField]
//        private bool downToggle;
//        private FlowOutput _OnDown;
//        private FlowOutput _OnPressed;
//        private FlowOutput _OnPressedValue;
//        private FlowOutput _OnUp;


//        void OnDown()
//        {
//            _OnDown.Call(new Flow());
//        }
//        void OnPressed()
//        {
//            _OnPressed.Call(new Flow());
//        }

//        void OnUp()
//        {
//            _OnUp.Call(new Flow());
//        }

//        protected override void RegisterPorts()
//        {
//            //base.RegisterPorts();
//            _OnDown = AddFlowOutput("OnDown");
//            _OnPressed = AddFlowOutput("OnPressed");
//            _OnUp = AddFlowOutput("OnUp");
//        }

//    }
//    #endregion
//}