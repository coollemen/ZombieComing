using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    #region Transform

    [Name("getParent")]
    [Category("UnityEngine/Transform")]
    [Description("获取父物体")]
    public class G_Parent : PureFunctionNode<Transform, Transform>
    {
        public override Transform Invoke(Transform transform)
        {
            return transform.parent;
        }
    }

    [Name("getForward")]
    [Category("UnityEngine/Transform")]
    [Description("获取局部坐标系向前向量")]
    public class G_Forward : PureFunctionNode<Vector3, Transform>
    {
        public override Vector3 Invoke(Transform transform)
        {
            return transform.forward;
        }
    }

    [Name("getRight")]
    [Category("UnityEngine/Transform")]
    [Description("获取局部坐标系向右向量")]
    public class G_Right : PureFunctionNode<Vector3, Transform>
    {
        public override Vector3 Invoke(Transform transform)
        {
            return transform.right;
        }
    }


    [Name("getUp")]
    [Category("UnityEngine/Transform")]
    [Description("获取局部坐标系向上向量")]
    public class G_Up : PureFunctionNode<Vector3, Transform>
    {
        public override Vector3 Invoke(Transform transform)
        {
            return transform.up;
        }
    }

    [Name("hasChanged")]
    [Category("UnityEngine/Transform")]
    [Description("位置信息是否发生改变")]
    public class G_HasChanged : PureFunctionNode<bool, Transform>
    {
        public override bool Invoke(Transform transform)
        {
            return transform.hasChanged;
        }
    }

    [Name("getRotation")]
    [Category("UnityEngine/Transform")]
    [Description("获得旋转数值quaternion")]
    public class G_Rotation : PureFunctionNode<Quaternion, Transform>
    {
        public override Quaternion Invoke(Transform transform)
        {
            return transform.rotation;
        }
    }

    [Name("getLocalRotation")]
    [Category("UnityEngine/Transform")]
    [Description("获得局部旋转数值quaternion")]
    public class G_LocalRotation : PureFunctionNode<Quaternion, Transform>
    {
        public override Quaternion Invoke(Transform transform)
        {
            return transform.localRotation;
        }
    }


    [Name("getPosition")]
    [Category("UnityEngine/Transform")]
    [Description("获取世界位置坐标")]
    public class G_Position : PureFunctionNode<Vector3, Transform>
    {
        public override Vector3 Invoke(Transform transform)
        {
            return transform.position;
        }
    }

    [Name("setPosition")]
    [Category("UnityEngine/Transform")]
    [Description("设置世界坐标位置")]
    public class S_Position : CallableFunctionNode<Transform,Transform, Vector3>
    {
        public override Transform Invoke(Transform transform, Vector3 position)
        {
            transform.position = position;
            return transform;
        }
    }
    [Name("setRotation")]
    [Category("UnityEngine/Transform")]
    [Description("设置旋转角度")]
    public class S_Rotation : CallableFunctionNode<Transform, Transform, Quaternion>
    {
        public override Transform Invoke(Transform transform, Quaternion rotation)
        {
            transform.rotation = rotation;
            return transform;
        }
    }

    [Name("getLocalPosition")]
    [Category("UnityEngine/Transform")]
    [Description("获取局部坐标位置")]
    public class G_LocalPosition : PureFunctionNode<Vector3, Transform>
    {
        public override Vector3 Invoke(Transform transform)
        {
            return transform.localPosition;
        }
    }

    [Name("setlocalEulerAngles")]
    [Category("UnityEngine/Transform")]
    [Description("设置局部坐标位置")]
    public class S_LocalPosition : CallableFunctionNode<Transform, Transform, Vector3>
    {
        public override Transform Invoke(Transform transform, Vector3 localEulerAngles)
        {
            transform.localEulerAngles = localEulerAngles;
            return transform;
        }
    }


    [Name("getEulerAngles")]
    [Category("UnityEngine/Transform")]
    [Description("获取世界坐标位置")]
    public class G_EulerAngles : PureFunctionNode<Vector3, Transform>
    {
        public override Vector3 Invoke(Transform transform)
        {
            return transform.eulerAngles;
        }
    }

    [Name("setEulerAngles")]
    [Category("UnityEngine/Transform")]
    [Description("设置世界坐标系角度")]
    public class S_EulerAngles : CallableFunctionNode<Transform,Transform, Vector3>
    {
        public override Transform Invoke(Transform transform, Vector3 eulerAngles)
        {
            transform.eulerAngles = eulerAngles;
            return transform;
        }
    }

    [Name("getLocalEulerAngles")]
    [Category("UnityEngine/Transform")]
    [Description("获取局部坐标系角度")]
    public class G_LocalEulerAngles : PureFunctionNode<Vector3, Transform>
    {
        public override Vector3 Invoke(Transform transform)
        {
            return transform.localEulerAngles;
        }
    }

    [Name("setLocalEulerAngles")]
    [Category("UnityEngine/Transform")]
    [Description("设置局部坐标系角度")]
    public class S_LocalEulerAngles : CallableFunctionNode<Transform,Transform, Vector3>
    {
        public override Transform Invoke(Transform transform, Vector3 localEulerAngles)
        {
            transform.localEulerAngles = localEulerAngles;
            return transform;
        }
    }

    [Name("getlocalScale")]
    [Category("UnityEngine/Transform")]
    [Description("获取局部坐标系缩放")]
    public class G_localScale : PureFunctionNode<Vector3, Transform>
    {
        public override Vector3 Invoke(Transform transform)
        {
            return transform.localScale;
        }
    }

    [Name("setlocalScale")]
    [Category("UnityEngine/Transform")]
    [Description("设置局部坐标系缩放")]
    public class S_localScale : CallableFunctionNode<Transform,Transform, Vector3>
    {
        public override Transform Invoke(Transform transform, Vector3 localScale)
        {
            transform.localScale = localScale;
            return transform;
        }
    }

    [Name("getlossyScale")]
    [Category("UnityEngine/Transform")]
    [Description("获取世界坐标系缩放")]
    public class G_lossyScale : PureFunctionNode<Vector3, Transform>
    {
        public override Vector3 Invoke(Transform transform)
        {
            return transform.lossyScale;
        }
    }

    [Name("getChildCount")]
    [Category("UnityEngine/Transform")]
    [Description("获取子物体数量")]
    public class G_ChildCount : PureFunctionNode<int, Transform>
    {
        public override int Invoke(Transform transform)
        {
            return transform.childCount;
        }
    }

    [Name("setParent")]
    [Category("UnityEngine/Transform")]
    [Description("设置父物体")]
    public class S_Parent : CallableFunctionNode<Transform,Transform, Transform>
    {
        public override Transform Invoke(Transform transform, Transform target)
        {
            transform.parent = target;
            return transform;
        }
    }

    [Name("getRoot")]
    [Category("UnityEngine/Transform")]
    [Description("获取目录根物体")]
    public class G_Root : PureFunctionNode<Transform, Transform>
    {
        public override Transform Invoke(Transform transform)
        {
            return transform.root;
        }
    }

    [Name("Find")]
    [Category("UnityEngine/Transform")]
    [Description("寻找子物体")]
    public class G_FindChild : PureFunctionNode<Transform, Transform, string>
    {
        public override Transform Invoke(Transform transform, string childName)
        {
            return transform.Find(childName);
        }
    }

    [Name("getChild")]
    [Category("UnityEngine/Transform")]
    [Description("根据指数获得子物体")]
    public class G_Child : PureFunctionNode<Transform, Transform, int>
    {
        public override Transform Invoke(Transform transform, int childIndex)
        {
            return transform.GetChild(childIndex);
        }
    }

    [Name("detachChildren")]
    [Category("UnityEngine/Transform")]
    [Description("分离所有子物体")]
    public class G_DetachChild : CallableFunctionNode<Transform,Transform>
    {
        public override Transform Invoke(Transform transform)
        {
            transform.DetachChildren();
            return transform;
        }
    }

    [Name("getSiblingIndex")]
    [Category("UnityEngine/Transform")]
    [Description("获得此物体在父物体中的排序位置")]
    public class G_SiblingIndex : PureFunctionNode<int, Transform>
    {
        public override int Invoke(Transform transform)
        {
            return transform.GetSiblingIndex();
        }
    }

    [Name("isChildOf")]
    [Category("UnityEngine/Transform")]
    [Description("该物体是否是目标的子物体")]
    public class G_IsChildOf : PureFunctionNode<bool, Transform, Transform>
    {
        public override bool Invoke(Transform transform, Transform target)
        {
            return transform.IsChildOf(target);
        }
    }

    [Name("inverseTransformDirection")]
    [Category("UnityEngine/Transform")]
    [Description("将世界坐标系方向转换成局部坐标系方向")]
    public class G_InverseTransformDirection : PureFunctionNode<Vector3, Transform, Vector3>
    {
        public override Vector3 Invoke(Transform transform, Vector3 direction)
        {
            return transform.InverseTransformDirection(direction);
        }
    }

    [Name("transformDirection")]
    [Category("UnityEngine/Transform")]
    [Description("将局部坐标系方向转换成世界坐标系方向")]
    public class G_TransformDirection : PureFunctionNode<Vector3, Transform, Vector3>
    {
        public override Vector3 Invoke(Transform transform, Vector3 direction)
        {
            return transform.TransformDirection(direction);
        }
    }

    [Name("inverseTransformPoint")]
    [Category("UnityEngine/Transform")]
    [Description("将世界坐标系位置转换成局部坐标系位置")]
    public class G_InverseTransformPoint : PureFunctionNode<Vector3, Transform, Vector3>
    {
        public override Vector3 Invoke(Transform transform, Vector3 position)
        {
            return transform.InverseTransformPoint(position);
        }
    }

    [Name("transformPoint")]
    [Category("UnityEngine/Transform")]
    [Description("将局部坐标系位置转换成世界坐标系位置")]
    public class G_TransformPoint : PureFunctionNode<Vector3, Transform, Vector3>
    {
        public override Vector3 Invoke(Transform transform, Vector3 position)
        {
            return transform.TransformPoint(position);
        }
    }

    [Name("inverseTransformVector")]
    [Category("UnityEngine/Transform")]
    [Description("将世界坐标系向量转换成局部坐标系向量")]
    public class G_InverseTransformVector : PureFunctionNode<Vector3, Transform, Vector3>
    {
        public override Vector3 Invoke(Transform transform, Vector3 vector)
        {
            return transform.InverseTransformVector(vector);
        }
    }

    [Name("transformVector")]
    [Category("UnityEngine/Transform")]
    [Description("将局部坐标系向量转换成世界坐标系向量")]
    public class G_TransformVector : PureFunctionNode<Vector3, Transform, Vector3>
    {
        public override Vector3 Invoke(Transform transform, Vector3 vector)
        {
            return transform.TransformVector(vector);
        }
    }

    [Name("lookAt")]
    [Category("UnityEngine/Transform")]
    [Description("注视目标")]
    [ContextDefinedInputs(typeof(Flow), typeof(Transform), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow))]
    public class G_LookAt : FlowNode
    {
        [SerializeField] private int index = 0;

        public int Index
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    index = value;
                    GatherPorts();
                }
            }
        }

        [SerializeField] private bool upVector3 = false;

        public bool UpVector3
        {
            get { return upVector3; }
            set
            {
                if (upVector3 != value)
                {
                    upVector3 = value;
                    GatherPorts();
                }
            }
        }


        protected override void RegisterPorts()
        {
            var transform = AddValueInput<Transform>("Transform");
            AddValueOutput("Transform", () => transform.value);
            var outPut = AddFlowOutput("Out");

            if (upVector3)
            {
                var upVector = AddValueInput<Vector3>("upVector3");

                if (index == 0)
                {
                    var target = AddValueInput<Transform>("Target Transform");
                    AddFlowInput("In", (f) =>
                    {
                        transform.value.LookAt(target.value, upVector.value);
                        outPut.Call(f);
                    });
                }
                else
                {
                    var target = AddValueInput<Vector3>("Vector3");
                    AddFlowInput("In", (f) =>
                    {
                        transform.value.LookAt(target.value, upVector.value);
                        outPut.Call(f);
                    });
                }
            }
            else
            {
                if (index == 0)
                {
                    var target = AddValueInput<Transform>("Target Transform");
                    AddFlowInput("In", (f) =>
                    {
                        transform.value.LookAt(target.value);
                        outPut.Call(f);
                    });
                }
                else
                {
                    var target = AddValueInput<Vector3>("Vector3");
                    AddFlowInput("In", (f) =>
                    {
                        transform.value.LookAt(target.value);
                        outPut.Call(f);
                    });
                }
            }


        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Index= UnityEditor.EditorGUILayout.Popup("target Type:", index, new string[2] {"Transform","Vector3"});
            UpVector3 = UnityEditor.EditorGUILayout.Toggle("Toggle UpVector:", upVector3);           
        }
#endif
    }


    [Name("rotate")]
    [Category("UnityEngine/Transform")]
    [Description("旋转")]
    [ContextDefinedInputs(typeof(Flow), typeof(Transform), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow))]
    public class G_Rotate : FlowNode
    {
        [SerializeField] private int index = 0;

        public int Index
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    index = value;
                    if (index == 0)
                    {
                        name = "rotate(Self)";
                    }
                    else
                    {
                        name = "rotate(World)";
                    }
                    GatherPorts();
                }
            }
        }

        [SerializeField] private int mode = 0;

        public int Mode
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
            var transform = AddValueInput<Transform>("Transform");
            AddValueOutput("Transform", () => transform.value);
            var outPut = AddFlowOutput("Out");

            switch (mode)
            {
                case 0:
                {
                    var axis = AddValueInput<Vector3>("Axis");
                    var angle = AddValueInput<float>("Angle");

                    AddFlowInput("In", (f) =>
                    {
                        transform.value.Rotate(axis.value, angle.value, index == 0 ? Space.Self : Space.World);
                        outPut.Call(f);
                    });

                    break;
                }
                case 1:
                {
                    var vector3 = AddValueInput<Vector3>("Vector3");
                    AddFlowInput("In", (f) =>
                    {
                        transform.value.Rotate(vector3.value, index == 0 ? Space.Self : Space.World);
                        outPut.Call(f);
                    });

                    break;
                }
                case 2:
                {
                    var floatX = AddValueInput<float>("X");
                    var floatY = AddValueInput<float>("Y");
                    var floatZ = AddValueInput<float>("Z");
                    AddFlowInput("In", (f) =>
                    {
                        transform.value.Rotate(floatX.value, floatY.value, floatZ.value,
                            index == 0 ? Space.Self : Space.World);
                        outPut.Call(f);
                    });

                    break;
                }
            }
        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Index = UnityEditor.EditorGUILayout.Popup("Space Type:", Index, new string[2] { "Self", "World" });
            Mode = UnityEditor.EditorGUILayout.Popup("Rotate Mode:",Mode, new string[3] { "Rotate Along Axis", "Rotate by Vector3", "Rotate by 3 Float"});
        }
#endif
    }


    [Name("rotateAround")]
    [Category("UnityEngine/Transform")]
    [Description("围绕某点沿世界坐标轴向轴向旋转角度")]
    public class G_RotateAround : CallableFunctionNode<Transform,Transform, Vector3, Vector3, float>
    {
        public override Transform Invoke(Transform transform, Vector3 point, Vector3 worldAxis, float angle)
        {
            transform.RotateAround(point, worldAxis, angle);
            return transform;
        }
    }

    [Name("setAsFirstSibling")]
    [Category("UnityEngine/Transform")]
    [Description("在父物体的层级下排行第一")]
    public class G_SetAsFirstSibling : CallableFunctionNode<Transform,Transform>
    {
        public override Transform Invoke(Transform transform)
        {
            transform.SetAsFirstSibling();
            return transform;
        }
    }

    [Name("setAsLastSibling")]
    [Category("UnityEngine/Transform")]
    [Description("在父物体的层级下排行最后")]
    public class G_SetAsLastSibling : CallableFunctionNode<Transform, Transform>
    {
        public override Transform Invoke(Transform transform)
        {
            transform.SetAsLastSibling();
            return transform;
        }
    }

    [Name("setLocalPosition")]
    [Category("UnityEngine/Transform")]
    [Description("设置局部位置")]
    public class S_LocalPos : CallableFunctionNode<Transform,Transform, Vector3>
    {
        public override Transform Invoke(Transform transform, Vector3 localPosition)
        {
            transform.localPosition = localPosition;
            return transform;
        }
    }

    [Name("setPositionAndRotation")]
    [Category("UnityEngine/Transform")]
    [Description("设置物体的位置和旋转角度")]
    public class G_SetPositionAndRotation : CallableFunctionNode<Transform,Transform, Vector3, Quaternion>
    {
        public override Transform Invoke(Transform transform, Vector3 worldPosition, Quaternion rotation)
        {
            transform.SetPositionAndRotation(worldPosition, rotation);
            return transform;
        }
    }

    [Name("setParent")]
    [Category("UnityEngine/Transform")]
    [Description("设置父物体,并且选择是否保持世界坐标位置不跟随父物体移动")]
    public class G_SetParent : CallableFunctionNode<Transform,Transform, Transform, bool>
    {
        public override Transform Invoke(Transform transform, Transform target, bool stayWorldPostion = false)
        {
            transform.SetParent(target, stayWorldPostion);
            return transform;
        }
    }

    [Name("translate")]
    [Category("UnityEngine/Transform")]
    [Description("移动")]
    [ContextDefinedInputs(typeof(Flow), typeof(Transform), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow))]
    public class G_Translate : FlowNode
    {
        [SerializeField] private int index = 0;

        public int Index
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    index = value;
                    if (index == 0)
                    {
                        name = "translate(Self)";
                    }
                    else
                    {
                        name = "translate(World)";
                    }
                    GatherPorts();
                }
            }
        }

        [SerializeField] private int mode = 0;

        public int Mode
        {
            get { return mode; }
            set
            {
                if (mode != value)
                {
                    mode = value;

                    if (mode == 1)
                    {
                        name = "translate";
                    }
                    GatherPorts();
                }
            }
        }


        protected override void RegisterPorts()
        {
            var transform = AddValueInput<Transform>("Transform");
            AddValueOutput("Transform",()=>transform.value);
            var outPut = AddFlowOutput("Out");

            switch (mode)
            {
                case 0:
                {
                    var Trans = AddValueInput<Vector3>("Translation");

                    AddFlowInput("In", (f) =>
                    {
                        transform.value.Translate(Trans.value, index == 0 ? Space.Self : Space.World);
                        outPut.Call(f);
                    });

                    break;
                }
                case 1:
                {
                    var vector3 = AddValueInput<Vector3>("Vector3");
                    var relative = AddValueInput<Transform>("Relative");

                    AddFlowInput("In", (f) =>
                    {
                        transform.value.Translate(vector3.value, relative.value);
                        outPut.Call(f);
                    });

                    break;
                }
                case 2:
                {
                    var floatX = AddValueInput<float>("X");
                    var floatY = AddValueInput<float>("Y");
                    var floatZ = AddValueInput<float>("Z");
                    AddFlowInput("In", (f) =>
                    {
                        transform.value.Translate(floatX.value, floatY.value, floatZ.value,
                            index == 0 ? Space.Self : Space.World);
                        outPut.Call(f);
                    });

                    break;
                }
            }
        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            if(Mode!=1)Index = UnityEditor.EditorGUILayout.Popup("Space Type:", Index, new string[2] {"Self", "World"});
            Mode = UnityEditor.EditorGUILayout.Popup("Rotate Mode:", Mode,
                new string[3] {"Translate Vector3", "Translate Relative", "Translate by 3 Float" });
        }
#endif
    }

    #endregion
}