using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region Physics

    [Name("rayCast")]
    [Category("UnityEngine/Physics")]
    [Description("发射激光,返回碰撞参数")]
    [ContextDefinedInputs(typeof(Flow), typeof(Ray), typeof(LayerMask), typeof(float))]
    [ContextDefinedOutputs(typeof(Flow), typeof(bool), typeof(RaycastHit))]
    public class G_RayCast : FlowNode
    {
        private bool isHited = false;
        private RaycastHit hit;

        protected override void RegisterPorts()
        {
            var ray = AddValueInput<Ray>("Ray");
            var layermask = AddValueInput<LayerMask>("LayerMask");
            var maxDistance = AddValueInput<float>("MaxDistance");
            var output = AddFlowOutput("out");

            AddValueOutput<bool>("isHitSomething", () => { return isHited; });
            AddValueOutput<RaycastHit>("RayCastHit", () => { return hit; });

            AddFlowInput("In", (f) =>
                {
                    float rayCastDistance = maxDistance.value;
                    if (maxDistance.value < 0.01f)
                    {
                        rayCastDistance = Mathf.Infinity;
                    }
                    if (Physics.Raycast(ray.value, out hit, rayCastDistance, layermask.value))
                    {
                        isHited = true;
                    }
                    output.Call(f);
                }
            );
        }
    }

    [Name("lineCast")]
    [Category("UnityEngine/Physics")]
    [Description("发射激光,返回碰撞参数")]
    [ContextDefinedInputs(typeof(Flow), typeof(Vector3), typeof(LayerMask))]
    [ContextDefinedOutputs(typeof(Flow), typeof(bool), typeof(RaycastHit))]
    public class G_LineCast : FlowNode
    {
        private bool isHited = false;
        private RaycastHit hit;

        protected override void RegisterPorts()
        {
            var startPosition = AddValueInput<Vector3>("startPosition");
            var endPosition = AddValueInput<Vector3>("endPosition");
            var layermask = AddValueInput<LayerMask>("LayerMask");

            var output = AddFlowOutput("out");

            AddValueOutput<bool>("isHitSomething", () => { return isHited; });
            AddValueOutput<RaycastHit>("RayCastHit", () => { return hit; });

            AddFlowInput("In", (f) =>
                {
                    if (Physics.Linecast(startPosition.value, endPosition.value, out hit, layermask.value))
                    {
                        isHited = true;
                    }
                    output.Call(f);
                }
            );
        }
    }

    [Name("getGravity")]
    [Category("UnityEngine/Physics")]
    [Description("重力值")]
    public class G_gravity : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return Physics.gravity;
        }
    }

    [Name("setGravity")]
    [Category("UnityEngine/Physics")]
    [Description("设置重力值")]
    public class S_gravity : CallableActionNode<Vector3>
    {
        public override void Invoke(Vector3 gravity)
        {
            Physics.gravity = gravity;
        }
    }

    [Name("ignoreLayerCollision")]
    [Category("UnityEngine/Physics")]
    [Description("忽视层之间的碰撞关系")]
    public class S_IgnoreLayerCollision : CallableActionNode<LayerMask, LayerMask, bool>
    {
        public override void Invoke(LayerMask layer1, LayerMask layer2, bool Ignore)
        {
            Physics.IgnoreLayerCollision(layer1, layer2, Ignore);
        }
    }

    [Name("ignoreCollision")]
    [Category("UnityEngine/Physics")]
    [Description("忽视collider之间的碰撞关系")]
    public class S_IgnoreCollision : CallableActionNode<Collider, Collider, bool>
    {
        public override void Invoke(Collider collider1, Collider collider2, bool Ignore)
        {
            Physics.IgnoreCollision(collider1, collider2, Ignore);
        }
    }

    #endregion
}