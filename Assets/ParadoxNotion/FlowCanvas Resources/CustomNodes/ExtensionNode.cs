using ParadoxNotion.Design;
using ParadoxNotion.Services;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
//using RootMotion.FinalIK;

namespace FlowCanvas.Nodes
{
    #region  Animation 
    [Name("[NormalizeAnimationPostionAndSpeed]")]
    [Category("UnityEngine/Animation")]
    [Description("赋值或获取Animation组件指定名称的动画的进度[0-1]和动画速度")]
    [ContextDefinedInputs(typeof(Flow), typeof(Animation))]
    public class T_NormalizeAnimationPostion : FlowNode
    {
        [SerializeField]
        private bool mode = false; //false: set; true: get

        public bool Mode
        {
            get { return mode; }
            set
            {
                if (mode != value)
                {
                    mode = value;
                    GatherPorts();
                }
            }
        }

        protected override void RegisterPorts()
        {
            var animation = AddValueInput<Animation>("Animation");
            var animStateName = AddValueInput<string>("animStateName");
            if (!mode)
            {
                var output = AddFlowOutput("Out");
                var normalizedPosition = AddValueInput<float>("normalizedPosition");
                var normalizedSpeed = AddValueInput<float>("normalizedSpeed");

                AddFlowInput("In", (f) =>
                {
                    animation.value[animStateName.value].normalizedTime = normalizedPosition.value;
                    animation.value[animStateName.value].normalizedSpeed = normalizedSpeed.value;
                    output.Call(f);
                });
            }
            else
            {
            }
            AddValueOutput("normalized Position", () => { return animation.value[animStateName.value].normalizedTime; });
            AddValueOutput("normalized Speed", () => { return animation.value[animStateName.value].normalizedSpeed; });
        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Mode = EditorGUILayout.Toggle("GetOrSet", Mode);
        }
#endif
    }


    #endregion

    #region SceneManage
    [Name("[SceneManage]")]
    [Category("UnityEngine/SceneManager")]
    [Description("载入场景和信息")]
    [ContextDefinedInputs(typeof(Flow))]
    public class T_SceneLevel : FlowNode
    {
        [SerializeField]
        private bool mode = false; //false: get; true: set

        public bool Mode
        {
            get { return mode; }
            set
            {
                if (mode != value)
                {
                    mode = value;
                    GatherPorts();
                }
            }
        }
        [SerializeField]
        private bool loadSyncMode = false;

        public bool LoadSyncMode
        {
            get { return loadSyncMode; }
            set
            {
                if (loadSyncMode != value)
                {
                    loadSyncMode = value;
                    GatherPorts();
                }
            }
        }

        private AsyncOperation sync;
        protected override void RegisterPorts()
        {

            if (mode)
            {
                var output = AddFlowOutput("Out");

                var loadLevelInt = AddValueInput<int>("LoadLevelIndex");

                AddFlowInput("In", (f) =>
                {
                    if (loadSyncMode)
                    {
                        sync = SceneManager.LoadSceneAsync(loadLevelInt.value);
                    }
                    else
                    {
                        SceneManager.LoadScene(loadLevelInt.value);
                    }


                    output.Call(f);
                });

                if (loadSyncMode)
                {
                    AddValueOutput<float>("loadProcess", () => { return sync != null ? sync.progress : 0; });
                    AddValueOutput<bool>("loadDone", () => { return sync != null ? sync.isDone : false; });
                }
            }
            else
            {
                AddValueOutput<int>("CurrentLevelIndex", () => { return SceneManager.GetActiveScene().buildIndex; });
                AddValueOutput("CurrentLevelName", () => { return SceneManager.GetActiveScene().name; });
            }
        }
#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Mode = EditorGUILayout.Toggle("GetOrSet", Mode);

            if (Mode)
            {
                LoadSyncMode = GUILayout.Toggle(LoadSyncMode, "SyncLoad?");
            }
        }
#endif
    }
    [Name("OnLevelLoad")]
    [Category("Events/Application")]
    [Description("载入场景时")]
    public class OnLevelLoad : EventNode
    {
        private FlowOutput levelLoaded;
        private Scene loadedScene;
        public override void OnGraphStarted()
        {
            base.OnGraphStarted();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            loadedScene = arg0;
            levelLoaded.Call(new Flow());
        }

        protected override void RegisterPorts()
        {
            //base.RegisterPorts();
            levelLoaded = AddFlowOutput("LevelLoaded");
            AddValueOutput("loadedScene", () => { return loadedScene; });
            AddValueOutput("loadedSceneIndex", () => { return loadedScene.buildIndex; });
            AddValueOutput("loadedSceneName", () => { return loadedScene.name; });
        }
    }

    [Name("BuildSceneCount")]
    [Category("UnityEngine/SceneManager")]
    [Description("BuildScene相关信息")]
    public class BuildSceneCount : PureFunctionNode<int>
    {
        public override int Invoke()
        {
            return SceneManager.sceneCountInBuildSettings;
        }
    }

    [Name("BuildSceneName")]
    [Category("UnityEngine/SceneManager")]
    [Description("BuildScene相关信息")]
    public class BuildSceneName : PureFunctionNode<string, int>
    {
        public override string Invoke(int sceneIndex)
        {
            return SceneManager.GetSceneByBuildIndex(sceneIndex).name;
        }
    }
    #endregion

    #region Physics
    [Name("[RaycastFromCamera]")]
    [Category("UnityEngine/Physics")]
    [Description("从摄像机平面某点位置发射激光和场景中的物体交互,简化版本,使用camera.main,发射坐标是屏幕中央,射线检测距离是3000")]
    [ContextDefinedInputs(typeof(Flow))]
    public class T_RaycastFromCamera : FlowNode
    {
        [SerializeField]
        private bool mode = false;

        public bool Mode
        {
            get { return mode; }
            set
            {
                if (mode != value)
                {
                    mode = value;
                    GatherPorts();
                }
            }
        }

        RaycastHit hit;

        private bool hitTrue;

        protected override void RegisterPorts()
        {
            var output = AddFlowOutput("Out");

            var layerMask = AddValueInput<LayerMask>("LayerMask");
            var showRay = AddValueInput<bool>("ShowRay");

            AddValueOutput<RaycastHit>("RayCastHit", () =>
             {
                 return hit;
             }
            );

            AddValueOutput<bool>("isHited", () =>
            {
                return hitTrue;
            }
            );

            if (!mode)
            {
                var Camera = AddValueInput<Camera>("Camera");
                var castDistance = AddValueInput<float>("CastDistance");

                var screenX_Pos = AddValueInput<float>("screenX_Pos");
                var screenY_Pos = AddValueInput<float>("screenY_Pos");

                AddFlowInput("In", (f) =>
                {
                    Ray ray = Camera.value.ScreenPointToRay(new Vector3(screenX_Pos.value, screenY_Pos.value, 0));

                    if (true == showRay.value)
                    {
                        Debug.DrawLine(ray.origin, ray.origin + (ray.direction * castDistance.value), Color.red);
                    }
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, castDistance.value, layerMask.value))
                    {
                        hitTrue = true;
                    }
                    output.Call(f);
                });
            }
            else
            {
                AddFlowInput("In", (f) =>
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));

                    if (true == showRay.value)
                    {
                        Debug.DrawRay(ray.origin, ray.direction, Color.red);
                    }
                    if (Physics.Raycast(ray, out hit, 3000f, layerMask.value))
                    {
                        hitTrue = true;
                    }
                    output.Call(f);
                });
            }


        }
#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Mode = EditorGUILayout.Toggle("Simpled Mode", Mode);
        }
#endif
    }

    [Name("[RaycastFromCursor]")]
    [Category("UnityEngine/Physics")]
    [Description("从鼠标位置发射激光和场景中的物体交互,简化版本,使用camera.main")]
    [ContextDefinedInputs(typeof(Flow))]
    public class T_RaycastFromCursor : FlowNode
    {
        [SerializeField]
        private bool mode = false;

        public bool Mode
        {
            get { return mode; }
            set
            {
                if (mode != value)
                {
                    mode = value;
                    GatherPorts();
                }
            }
        }

        RaycastHit hit;

        private bool hitTrue;

        protected override void RegisterPorts()
        {
            var output = AddFlowOutput("Out");

            var layerMask = AddValueInput<LayerMask>("LayerMask");
            var showRay = AddValueInput<bool>("ShowRay");

            AddValueOutput<RaycastHit>("RayCastHit", () =>
            {
                return hit;
            }
            );

            AddValueOutput<bool>("isHited", () =>
            {
                return hitTrue;
            }
            );

            if (!mode)
            {
                var Camera = AddValueInput<Camera>("Camera");
                var castDistance = AddValueInput<float>("CastDistance");

                AddFlowInput("In", (f) =>
                {
                    Ray ray = Camera.value.ScreenPointToRay(Input.mousePosition);

                    if (true == showRay.value)
                    {
                        Debug.DrawLine(ray.origin, ray.origin + (ray.direction * castDistance.value), Color.blue);
                    }
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, castDistance.value, layerMask.value))
                    {
                        hitTrue = true;
                    }
                    output.Call(f);
                });
            }
            else
            {
                AddFlowInput("In", (f) =>
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (true == showRay.value)
                    {
                        Debug.DrawRay(ray.origin, ray.direction, Color.blue);
                    }
                    if (Physics.Raycast(ray, out hit, 3000f, layerMask.value))
                    {
                        hitTrue = true;
                    }
                    output.Call(f);
                });
            }


        }
#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Mode = EditorGUILayout.Toggle("Simpled Mode", Mode);
        }
#endif
    }
    #endregion

    #region Application

    [Name("[FPS]")]
    [Category("UnityEngine/Application")]
    [Description("获取帧数string")]
    public class G_FPS : PureFunctionNode<string>
    {
        public override void OnGraphStarted()
        {
            base.OnGraphStarted();

            f_LastInterval = Time.realtimeSinceStartup;

            i_Frames = 0;
            MonoManager.current.onUpdate += Update;
        }

        public float f_UpdateInterval = 0.2F;

        private float f_LastInterval;

        private int i_Frames = 0;

        private float f_Fps;

        void Update()
        {
            ++i_Frames;

            if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
            {
                f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

                i_Frames = 0;

                f_LastInterval = Time.realtimeSinceStartup;
            }
        }

        public override string Invoke()
        {
            return f_Fps.ToString("f0");
        }
    }
    #endregion

//    #region  FBBIK 
//    [Name("setIKWeight")]
//    [Category("UnityEngine/FBBIK")]
//    [Description("控制FBBIK位置和旋转以及blendGoal的权重")]
//    [ContextDefinedInputs(typeof(Flow), typeof(FullBodyBipedIK))]
//    public class T_SetFBBIKWeight : FlowNode
//    {
//        [SerializeField]
//        private int mode = 0;

//        private string nodeName = "";

//        public int Mode
//        {
//            get { return mode; }
//            set
//            {
//                if (mode != value)
//                {
//                    mode = value;
//                    switch (mode)
//                    {
//                        case 0:
//                            nodeName = "LeftArm";
//                            break;
//                        case 1:
//                            nodeName = "RightArm";
//                            break;
//                        case 2:
//                            nodeName = "LeftLeg";
//                            break;
//                        case 3:
//                            nodeName = "RightLeg";
//                            break;
//                        case 4:
//                            nodeName = "Body";
//                            break;
//                    }
//                    GatherPorts();
//                }
//            }
//        }

//        public override string name
//        {
//            get { return base.name + " " + nodeName; }
//        }


//        IKEffector effector;
//        IKConstraintBend blend;

//        protected override void RegisterPorts()
//        {
//            var fullBodyBipedIK = AddValueInput<FullBodyBipedIK>("fullBodyBipedIK");
//            var positionWeight = AddValueInput<float>("positionWeight");
//            var rotationWeight = AddValueInput<float>("rotationWeight");
//            var blendGoal = AddValueInput<float>("blendGoalWeight");


//            var output = AddFlowOutput("Out");

//            AddFlowInput("In", (f) =>
//            {
//                switch (mode)
//                {
//                    case 0:
//                        effector = fullBodyBipedIK.value.solver.leftHandEffector;
//                        blend = fullBodyBipedIK.value.solver.leftArmChain.bendConstraint;
//                        break;
//                    case 1:
//                        effector = fullBodyBipedIK.value.solver.rightHandEffector;
//                        blend = fullBodyBipedIK.value.solver.rightArmChain.bendConstraint;
//                        break;
//                    case 2:
//                        effector = fullBodyBipedIK.value.solver.leftFootEffector;
//                        blend = fullBodyBipedIK.value.solver.leftLegChain.bendConstraint;
//                        break;
//                    case 3:
//                        effector = fullBodyBipedIK.value.solver.rightFootEffector;
//                        blend = fullBodyBipedIK.value.solver.rightLegChain.bendConstraint;
//                        break;
//                    case 4:
//                        effector = fullBodyBipedIK.value.solver.bodyEffector;
//                        blend = null;
//                        break;
//                }

//                effector.positionWeight = positionWeight.value;
//                effector.rotationWeight = rotationWeight.value;

//                if (Mode != 4)
//                    blend.weight = blendGoal.value;
//                output.Call(f);
//            });

//        }

//#if UNITY_EDITOR
//        protected override void OnNodeInspectorGUI()
//        {
//            base.OnNodeInspectorGUI();
//            Mode = EditorGUILayout.Popup(Mode, new string[] { "LeftArm", "RightArm", "LeftLeg", "RightLeg", "Body" });
//        }
//#endif
//    }


//    #endregion
}
