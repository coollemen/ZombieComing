
using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes
{

    #region Rigidbody

    [Name("getVelocity")]
    [Category("UnityEngine/Rigidbody")]
    [Description("获得刚体的速度")]
    public class G_RigidbodyVelocity : PureFunctionNode<Vector3, Rigidbody>
    {
        public override Vector3 Invoke(Rigidbody rigidbody)
        {
            return rigidbody.velocity;
        }
    }

    [Name("setVelocity")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体的速度")]
    public class S_RigidbodyVelocity : CallableFunctionNode<Rigidbody,Rigidbody, Vector3>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, Vector3 velocity)
        {
            rigidbody.velocity = velocity;
            return rigidbody;
        }
    }

    [Name("getAngularVelocity")]
    [Category("UnityEngine/Rigidbody")]
    [Description("获得刚体的角速度")]
    public class G_RigidbodyangularVelocity : PureFunctionNode<Vector3, Rigidbody>
    {
        public override Vector3 Invoke(Rigidbody rigidbody)
        {
            return rigidbody.angularVelocity;
        }
    }

    [Name("setAngularVelocity")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体的角速度")]
    public class S_RigidbodyangularVelocity : CallableFunctionNode<Rigidbody,Rigidbody, Vector3>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, Vector3 angularVelocity)
        {
            rigidbody.angularVelocity = angularVelocity;
            return rigidbody;
        }
    }

    [Name("getPosition")]
    [Category("UnityEngine/Rigidbody")]
    [Description("获得刚体的位置")]
    public class G_RigidbodyPosition : PureFunctionNode<Vector3, Rigidbody>
    {
        public override Vector3 Invoke(Rigidbody rigidbody)
        {
            return rigidbody.position;
        }
    }

    [Name("setPosition")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体的位置")]
    public class S_RigidbodyPosition : CallableFunctionNode<Rigidbody,Rigidbody, Vector3>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, Vector3 position)
        {
            rigidbody.position = position;
            return rigidbody;
        }
    }

    [Name("getAngularDrag")]
    [Category("UnityEngine/Rigidbody")]
    [Description("获得刚体旋转阻力")]
    public class G_RigidbodyangularDrag : PureFunctionNode<float, Rigidbody>
    {
        public override float Invoke(Rigidbody rigidbody)
        {
            return rigidbody.angularDrag;
        }
    }

    [Name("setAngularDrag")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体旋转阻力")]
    public class S_RigidbodyangularDrag : CallableFunctionNode<Rigidbody,Rigidbody, float>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, float angleDrag)
        {
            rigidbody.angularDrag = angleDrag;
            return rigidbody;
        }
    }

    [Name("getDrag")]
    [Category("UnityEngine/Rigidbody")]
    [Description("获得刚体阻力")]
    public class G_RigidbodyDrag : PureFunctionNode<float, Rigidbody>
    {
        public override float Invoke(Rigidbody rigidbody)
        {
            return rigidbody.drag;
        }
    }

    [Name("setDrag")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体阻力")]
    public class S_RigidbodyDrag : CallableFunctionNode<Rigidbody,Rigidbody, float>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, float drag)
        {
            rigidbody.angularDrag = drag;
            return rigidbody;
        }
    }

    [Name("getIsKinematic")]
    [Category("UnityEngine/Rigidbody")]
    [Description("获得刚体是否静态")]
    public class G_RigidbodyIsKinematic : PureFunctionNode<bool, Rigidbody>
    {
        public override bool Invoke(Rigidbody rigidbody)
        {
            return rigidbody.isKinematic;
        }
    }

    [Name("setIsKinematic")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体静态")]
    public class S_RigidbodyIsKinematic : CallableFunctionNode<Rigidbody,Rigidbody, bool>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, bool isKinematic)
        {
            rigidbody.isKinematic = isKinematic;
            return rigidbody;
            ;
        }
    }

    [Name("getMass")]
    [Category("UnityEngine/Rigidbody")]
    [Description("获得刚体的重量")]
    public class G_RigidbodyMass : PureFunctionNode<float, Rigidbody>
    {
        public override float Invoke(Rigidbody rigidbody)
        {
            return rigidbody.mass;
        }
    }

    [Name("setMass")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体的重量")]
    public class S_RigidbodyMass : CallableFunctionNode<Rigidbody,Rigidbody, float>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, float mass)
        {
            rigidbody.mass = mass;
            return rigidbody;
        }
    }

    [Name("getRotation")]
    [Category("UnityEngine/Rigidbody")]
    [Description("获得刚体的角度")]
    public class G_RigidbodyRotation : PureFunctionNode<Quaternion, Rigidbody>
    {
        public override Quaternion Invoke(Rigidbody rigidbody)
        {
            return rigidbody.rotation;
        }
    }

    [Name("setRotation")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体的角度")]
    public class S_RigidbodyRotation : CallableFunctionNode<Rigidbody,Rigidbody, Quaternion>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, Quaternion rotation)
        {
            rigidbody.rotation = rotation;
            return rigidbody;
        }
    }

    [Name("getUseGravity")]
    [Category("UnityEngine/Rigidbody")]
    [Description("获得刚体是否受重力影响")]
    public class G_RigidbodyUseGravity : PureFunctionNode<bool, Rigidbody>
    {
        public override bool Invoke(Rigidbody rigidbody)
        {
            return rigidbody.useGravity;
        }
    }

    [Name("setUseGravity")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体是否受重力影响")]
    public class S_RigidbodyUseGravity : CallableFunctionNode<Rigidbody,Rigidbody, bool>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, bool useGravity)
        {
            rigidbody.useGravity = useGravity;
            return rigidbody;
        }
    }

    [Name("getFreezeRotation")]
    [Category("UnityEngine/Rigidbody")]
    [Description("获得刚体是否被限制旋转")]
    public class G_RigidbodyFreezeRotation : PureFunctionNode<bool, Rigidbody>
    {
        public override bool Invoke(Rigidbody rigidbody)
        {
            return rigidbody.freezeRotation;
        }
    }

    [Name("setFreezeRotation")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体是否被限制旋转")]
    public class S_RigidbodyFreezeRotationy : CallableFunctionNode<Rigidbody,Rigidbody, bool>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, bool freezeRotation)
        {
            rigidbody.freezeRotation = freezeRotation;
            return rigidbody;
        }
    }

    [Name("movePosition")]
    [Category("UnityEngine/Rigidbody")]
    [Description("移动刚体位置,不同transform,它的移动会带来物理上的碰撞效果")]
    public class S_RigidbodyMovePosition : CallableFunctionNode<Rigidbody, Rigidbody, Vector3>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, Vector3 position)
        {
            rigidbody.MovePosition(position);
            return rigidbody;
        }
    }

    [Name("moveRotation")]
    [Category("UnityEngine/Rigidbody")]
    [Description("设置刚体角度,不同transform,它的旋转会带来物理上的碰撞效果")]
    public class S_RigidbodyMoveRotation : CallableFunctionNode<Rigidbody,Rigidbody, Quaternion>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody, Quaternion rotation)
        {
            rigidbody.MoveRotation(rotation);
            return rigidbody;
        }
    }

    [Name("sleep")]
    [Category("UnityEngine/Rigidbody")]
    [Description("取消刚体的解算,使其处于休眠状态")]
    public class S_RigidbodySleep : CallableFunctionNode<Rigidbody,Rigidbody>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody)
        {
            rigidbody.Sleep();
            return rigidbody;
        }
    }

    [Name("wakeUp")]
    [Category("UnityEngine/Rigidbody")]
    [Description("激活刚体的解算")]
    public class S_RigidbodyWakeUp : CallableFunctionNode<Rigidbody, Rigidbody>
    {
        public override Rigidbody Invoke(Rigidbody rigidbody)
        {
            rigidbody.WakeUp();
            return rigidbody;
        }
    }

    [Name("addForce")]
    [Category("UnityEngine/Rigidbody")]
    [Description("给刚体作用力")]
    [ContextDefinedInputs(typeof(Flow), typeof(Rigidbody), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow))]
    public class S_RigidbodyAddForce : FlowNode
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

        protected override void RegisterPorts()
        {
            var rigidbody = AddValueInput<Rigidbody>("rigidbody");
            var force = AddValueInput<Vector3>("ForceVector3");
            var output = AddFlowOutput("out");

            AddFlowInput("In", (f) =>
            {
                ForceMode mode = ForceMode.Acceleration;
                switch (Index)
                {
                    case 0:
                        mode = ForceMode.Acceleration;
                        break;
                    case 1:
                        mode = ForceMode.Force;
                        break;
                    case 2:
                        mode = ForceMode.Impulse;
                        break;
                    case 3:
                        mode = ForceMode.VelocityChange;
                        break;
                }
                rigidbody.value.AddForce(force.value, mode);
                output.Call(f);
            });
        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Index = UnityEditor.EditorGUILayout.Popup("ForceMode", Index,new string[4] { "Acceleration", "Force", "Impulse", "VelocityChange" });
        }
#endif
    }

    [Name("addExplosionForce")]
    [Category("UnityEngine/Rigidbody")]
    [Description("爆炸物给刚体的作用力,根据爆炸物的位置,爆炸半径和爆炸力强度决定对刚体的受力情况,还可以控制影响向上运动的程度")]
    [ContextDefinedInputs(typeof(Flow), typeof(Rigidbody), typeof(Vector3), typeof(float))]
    [ContextDefinedOutputs(typeof(Flow))]
    public class S_RigidbodyAddExplosionForce : FlowNode
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

        protected override void RegisterPorts()
        {
            var rigidbody = AddValueInput<Rigidbody>("rigidbody");
            var explorsionforce = AddValueInput<float>("explorsionforce");
            var explorsionRadius = AddValueInput<float>("explorsionRadius");
            var upModerfier = AddValueInput<float>("upModerfier");
            var forcePosition = AddValueInput<Vector3>("explorsionPosition");
            var output = AddFlowOutput("out");

            AddFlowInput("In", (f) =>
            {
                ForceMode mode = ForceMode.Acceleration;
                switch (Index)
                {
                    case 0:
                        mode = ForceMode.Acceleration;
                        break;
                    case 1:
                        mode = ForceMode.Force;
                        break;
                    case 2:
                        mode = ForceMode.Impulse;
                        break;
                    case 3:
                        mode = ForceMode.VelocityChange;
                        break;
                }
                rigidbody.value.AddExplosionForce(explorsionforce.value, forcePosition.value, explorsionRadius.value,
                    upModerfier.value, mode);
                output.Call(f);
            });
        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Index = UnityEditor.EditorGUILayout.Popup("ForceMode", Index, new string[4] { "Acceleration", "Force", "Impulse", "VelocityChange" });
        }

#endif
    }

    [Name("addForceAtPosition")]
    [Category("UnityEngine/Rigidbody")]
    [Description("给刚体作用力,力作用在世界坐标系的某点,可能会引起物体做旋转运动")]
    [ContextDefinedInputs(typeof(Flow), typeof(Rigidbody), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow))]
    public class S_RigidbodyAddForceAtPosition : FlowNode
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

        protected override void RegisterPorts()
        {
            var rigidbody = AddValueInput<Rigidbody>("rigidbody");
            var force = AddValueInput<Vector3>("ForceVector3");
            var forcePosition = AddValueInput<Vector3>("forcePosition");
            var output = AddFlowOutput("out");

            AddFlowInput("In", (f) =>
            {
                ForceMode mode = ForceMode.Acceleration;
                switch (Index)
                {
                    case 0:
                        mode = ForceMode.Acceleration;
                        break;
                    case 1:
                        mode = ForceMode.Force;
                        break;
                    case 2:
                        mode = ForceMode.Impulse;
                        break;
                    case 3:
                        mode = ForceMode.VelocityChange;
                        break;
                }
                rigidbody.value.AddForceAtPosition(force.value, forcePosition.value, mode);
                output.Call(f);
            });
        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Index = UnityEditor.EditorGUILayout.Popup("ForceMode", Index, new string[4] { "Acceleration", "Force", "Impulse", "VelocityChange" });
            }
#endif

    }

    [Name("addRelativeForce")]
    [Category("UnityEngine/Rigidbody")]
    [Description("给刚体局部坐标系的一个力")]
    [ContextDefinedInputs(typeof(Flow), typeof(Rigidbody), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow))]
    public class S_RigidbodyAddRelativeForce : FlowNode
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

        protected override void RegisterPorts()
        {
            var rigidbody = AddValueInput<Rigidbody>("rigidbody");
            var force = AddValueInput<Vector3>("ForceVector3");
            var output = AddFlowOutput("out");

            AddFlowInput("In", (f) =>
            {
                ForceMode mode = ForceMode.Acceleration;
                switch (Index)
                {
                    case 0:
                        mode = ForceMode.Acceleration;
                        break;
                    case 1:
                        mode = ForceMode.Force;
                        break;
                    case 2:
                        mode = ForceMode.Impulse;
                        break;
                    case 3:
                        mode = ForceMode.VelocityChange;
                        break;
                }
                rigidbody.value.AddRelativeForce(force.value, mode);
                output.Call(f);
            });
        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Index = UnityEditor.EditorGUILayout.Popup("ForceMode", Index, new string[4] { "Acceleration", "Force", "Impulse", "VelocityChange" });
        }

#endif
    }

    [Name("addRelativeTorque")]
    [Category("UnityEngine/Rigidbody")]
    [Description("给刚体局部坐标系一个转矩力,可以使其旋转")]
    [ContextDefinedInputs(typeof(Flow), typeof(Rigidbody), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow))]
    public class S_RigidbodyAddRelativeTorque : FlowNode
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

        protected override void RegisterPorts()
        {
            var rigidbody = AddValueInput<Rigidbody>("rigidbody");
            var force = AddValueInput<Vector3>("torqueForce");
            var output = AddFlowOutput("out");

            AddFlowInput("In", (f) =>
            {
                ForceMode mode = ForceMode.Acceleration;
                switch (Index)
                {
                    case 0:
                        mode = ForceMode.Acceleration;
                        break;
                    case 1:
                        mode = ForceMode.Force;
                        break;
                    case 2:
                        mode = ForceMode.Impulse;
                        break;
                    case 3:
                        mode = ForceMode.VelocityChange;
                        break;
                }
                rigidbody.value.AddRelativeTorque(force.value, mode);
                output.Call(f);
            });
        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Index = UnityEditor.EditorGUILayout.Popup("ForceMode", Index, new string[4] { "Acceleration", "Force", "Impulse", "VelocityChange" });
        }
#endif
    }

    [Name("addTorque")]
    [Category("UnityEngine/Rigidbody")]
    [Description("给刚体世界坐标系一个转矩力,可以使其旋转")]
    [ContextDefinedInputs(typeof(Flow), typeof(Rigidbody), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow))]
    public class S_RigidbodyAddTorque : FlowNode
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

        protected override void RegisterPorts()
        {
            var rigidbody = AddValueInput<Rigidbody>("Rigidbody");
            AddValueOutput("Rigidbody", () => rigidbody.value);
            var force = AddValueInput<Vector3>("torqueForce");
            var output = AddFlowOutput("out");

            AddFlowInput("In", (f) =>
            {
                ForceMode mode = ForceMode.Acceleration;
                switch (Index)
                {
                    case 0:
                        mode = ForceMode.Acceleration;
                        break;
                    case 1:
                        mode = ForceMode.Force;
                        break;
                    case 2:
                        mode = ForceMode.Impulse;
                        break;
                    case 3:
                        mode = ForceMode.VelocityChange;
                        break;
                }
                rigidbody.value.AddTorque(force.value, mode);
                output.Call(f);
            });
        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Index = UnityEditor.EditorGUILayout.Popup("ForceMode", Index, new string[4] { "Acceleration", "Force", "Impulse", "VelocityChange" });
        }

#endif
    }

    #endregion
}