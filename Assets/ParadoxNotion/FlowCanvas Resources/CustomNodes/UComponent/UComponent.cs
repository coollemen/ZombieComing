using System.Xml.Schema;
using ParadoxNotion.Design;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Object=UnityEngine.Object;


namespace FlowCanvas.Nodes
{

    #region Screen
    [Name("get_ScreenSize")]
    [Category("UnityEngine/Screen")]
    [Description("获取屏幕横向像素尺寸")]
    public class G_ScreenSize : PureFunctionNode<Vector2>
    {
        public override Vector2 Invoke()
        {
            return new Vector2(Screen.width,Screen.height);
        }
    }
    #endregion

    #region Time
    [Name("realtimeSinceStartup")]
    [Category("UnityEngine/Time")]
    [Description("游戏进行后的时间长度")]
    public class G_realtimeSinceStartup : PureFunctionNode<float>
    {
        public override float Invoke()
        {
            return Time.realtimeSinceStartup;
        }
    }

    [Name("time")]
    [Category("UnityEngine/Time")]
    [Description("游戏进行后的时间秒数")]
    public class G_time : PureFunctionNode<float>
    {
        public override float Invoke()
        {
            return Time.time;
        }
    }

    [Name("getTimeScale")]
    [Category("UnityEngine/Time")]
    [Description("游戏的时间缩放值")]
    public class G_timeScale : PureFunctionNode<float>
    {
        public override float Invoke()
        {
            return Time.timeScale;
        }
    }

    [Name("setTimeScale")]
    [Category("UnityEngine/Time")]
    [Description("设置游戏的时间缩放值")]
    public class S_timeScale : CallableActionNode<float>
    {
        public override void Invoke(float scale)
        {
            Time.timeScale= scale;
        }
    }

    [Name("timeSinceLevelLoad")]
    [Category("UnityEngine/Time")]
    [Description("关卡载入后的计时")]
    public class G_timeSinceLevelLoad : PureFunctionNode<float>
    {
        public override float Invoke()
        {
            return Time.timeSinceLevelLoad;
        }
    }

    [Name("unscaledTime")]
    [Category("UnityEngine/Time")]
    [Description("未缩放的游戏开始累计时间")]
    public class G_unscaledTime : PureFunctionNode<float>
    {
        public override float Invoke()
        {
            return Time.unscaledTime;
        }
    }

    [Name("unscaledDeltaTime")]
    [Category("UnityEngine/Time")]
    [Description("未缩放的时间增量")]
    public class G_unscaledDeltaTime : PureFunctionNode<float>
    {
        public override float Invoke()
        {
            return Time.unscaledDeltaTime;
        }
    }
    #endregion

    #region Vector3
    [Name("getforward(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向前向量 等同(0,0,1)")]
    public class G_Vector3forward : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return Vector3.forward;
        }
    }
    [Name("getback(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向后向量 等同(0,0,-1)")]
    public class G_Vector3back : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return Vector3.back;
        }
    }
    [Name("getright(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向右向量 等同(1,0,0)")]
    public class G_Vector3right : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return Vector3.right;
        }
    }
    [Name("getleft(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向左向量 等同(-1,0,0)")]
    public class G_Vector3left : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return Vector3.left;
        }
    }

    [Name("getUp(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向上向量 等同(0,1,0)")]
    public class G_Vector3up : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return Vector3.up;
        }
    }
    [Name("getDown(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向下向量 等同(0,-1,0)")]
    public class G_Vector3down : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return Vector3.down;
        }
    }

    [Name("getOne(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("单位向量 等同(1,1,1)")]
    public class G_Vector3one : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return Vector3.one;
        }
    }

    [Name("getZero(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("零向量 等同(0,0,0)")]
    public class G_Vector3Zero : PureFunctionNode<Vector3>
    {
        public override Vector3 Invoke()
        {
            return Vector3.zero;
        }
    }

    [Name("lerp(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("两向量间线性过渡")]
    public class G_Vector3Lerp : PureFunctionNode<Vector3,Vector3,Vector3,float>
    {
        public override Vector3 Invoke(Vector3 from, Vector3 to,float percent)
        {
            return Vector3.Lerp(from, from, percent);
        }
    }

    [Name("slerp(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("两向量间弧形过渡")]
    public class G_Vector3SLerp : PureFunctionNode<Vector3, Vector3, Vector3, float>
    {
        public override Vector3 Invoke(Vector3 from, Vector3 to, float percent)
        {
            return Vector3.Slerp(from, from, percent);
        }
    }

    [Name("clampMagnitude(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向量的方向不变,但将长度限定范围内")]
    public class G_Vector3ClampMagnitude : PureFunctionNode<Vector3, Vector3, float>
    {
        public override Vector3 Invoke(Vector3 vector ,float maxLength)
        {
            return Vector3.ClampMagnitude(vector, maxLength);
        }
    }

    [Name("cross(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向量叉乘,返回两个向量组成平面的垂直向量,垂直向量的朝向按左手坐标系判定")]
    public class G_Vector3Cross : PureFunctionNode<Vector3, Vector3, Vector3>
    {
        public override Vector3 Invoke(Vector3 vector1,Vector3 vector2 )
        {
            return Vector3.Cross(vector1, vector2);
        }
    }

    [Name("dot(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向量点乘,返回两个向量的方向相似度,0:表示垂直,1:平行")]
    public class G_Vector3Dot : PureFunctionNode<float, Vector3, Vector3>
    {
        public override float Invoke(Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Dot(vector1, vector2);
        }
    }

    [Name("angle(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("两向量间夹角")]
    public class G_Vector3Angle : PureFunctionNode<float, Vector3, Vector3>
    {
        public override float Invoke(Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Angle(vector1, vector2);
        }
    }

    [Name("distance(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("两位置间的距离,计算消耗大,避免每帧调用")]
    public class G_Vector3Distance : PureFunctionNode<float, Vector3, Vector3>
    {
        public override float Invoke(Vector3 position1, Vector3 position2)
        {
            return Vector3.Distance(position1, position2);
        }
    }

    [Name("magnitude(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向量的长度,计算消耗大,避免每帧调用,尽可能用SqrMagnitude代替")]
    public class G_Vector3Magnitude : PureFunctionNode<float, Vector3 >
    {
        public override float Invoke(Vector3 vector3)
        {
            return Vector3.Magnitude(vector3);
        }
    }
    [Name("sqrMagnitude(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("向量的长度的平方")]
    public class G_Vector3SqrMagnitude : PureFunctionNode<float, Vector3>
    {
        public override float Invoke(Vector3 vector3)
        {
            return Vector3.SqrMagnitude(vector3);
        }
    }

    [Name("max(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("返回两个向量中长度较长的向量")]
    public class G_Vector3Max : PureFunctionNode<Vector3, Vector3, Vector3>
    {
        public override Vector3 Invoke(Vector3 min,Vector3 max)
        {
            return Vector3.Max(min,max);
        }
    }

    [Name("min(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("返回两个向量中长度较长的向量")]
    public class G_Vector3Min : PureFunctionNode<Vector3, Vector3, Vector3>
    {
        public override Vector3 Invoke(Vector3 min, Vector3 max)
        {
            return Vector3.Min(min, max);
        }
    }

    [Name("moveTowards(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("以一定增值从一个位置移动到另一个位置")]
    public class G_Vector3MoveTowards : PureFunctionNode<Vector3, Vector3, Vector3,float>
    {
        public override Vector3 Invoke(Vector3 current, Vector3 target,float moveDelta)
        {
            return Vector3.MoveTowards(current, target,moveDelta);
        }
    }

    [Name("normalize(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("将向量长度归一化")]
    public class G_Vector3Normalize : PureFunctionNode<Vector3, Vector3>
    {
        public override Vector3 Invoke(Vector3 vector3)
        {
            return Vector3.Normalize(vector3);
        }
    }

    [Name("project(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("将向量投影到另一个向量上")]
    public class G_Vector3Project : PureFunctionNode<Vector3, Vector3, Vector3>
    {
        public override Vector3 Invoke(Vector3 vector3, Vector3 target)
        {
            return Vector3.Project(vector3, target);
        }
    }

    [Name("projectOnPlane(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("将向量投影到法线为onNormal的平面上")]
    public class G_Vector3ProjectOnPlane : PureFunctionNode<Vector3, Vector3, Vector3>
    {
        public override Vector3 Invoke(Vector3 vector3, Vector3 planeNormal)
        {
            return Vector3.ProjectOnPlane(vector3, planeNormal);
        }
    }

    [Name("reflect(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("将向量在onNormal的平面的反射向量")]
    public class G_Vector3Reflect : PureFunctionNode<Vector3, Vector3, Vector3>
    {
        public override Vector3 Invoke(Vector3 vector3, Vector3 planeNormal)
        {
            return Vector3.Reflect(vector3, planeNormal);
        }
    }
    [Name("scale(Vector3)")]
    [Category("UnityEngine/Vector3")]
    [Description("两向量每个元素相乘")]
    public class G_Vector3Scale : PureFunctionNode<Vector3, Vector3, Vector3>
    {
        public override Vector3 Invoke(Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Scale(vector1, vector2);
        }
    }
    #endregion

    #region Vector2

    [Name("getright(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("向右向量 等同(1,0,0)")]
    public class G_Vector2right : PureFunctionNode<Vector2>
    {
        public override Vector2 Invoke()
        {
            return Vector2.right;
        }
    }
    [Name("getleft(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("向左向量 等同(-1,0,0)")]
    public class G_Vector2left : PureFunctionNode<Vector2>
    {
        public override Vector2 Invoke()
        {
            return Vector2.left;
        }
    }

    [Name("getUp(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("向上向量 等同(0,1,0)")]
    public class G_Vector2up : PureFunctionNode<Vector2>
    {
        public override Vector2 Invoke()
        {
            return Vector2.up;
        }
    }
    [Name("getDown(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("向下向量 等同(0,-1,0)")]
    public class G_Vector2down : PureFunctionNode<Vector2>
    {
        public override Vector2 Invoke()
        {
            return Vector2.down;
        }
    }

    [Name("getOne(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("单位向量 等同(1,1,1)")]
    public class G_Vector2one : PureFunctionNode<Vector2>
    {
        public override Vector2 Invoke()
        {
            return Vector2.one;
        }
    }

    [Name("getZero(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("零向量 等同(0,0,0)")]
    public class G_Vector2Zero : PureFunctionNode<Vector2>
    {
        public override Vector2 Invoke()
        {
            return Vector2.zero;
        }
    }

    [Name("lerp(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("两向量间线性过渡")]
    public class G_Vector2Lerp : PureFunctionNode<Vector2, Vector2, Vector2, float>
    {
        public override Vector2 Invoke(Vector2 from, Vector2 to, float percent)
        {
            return Vector2.Lerp(from, from, percent);
        }
    }


    [Name("clampMagnitude(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("向量的方向不变,但将长度限定范围内")]
    public class G_Vector2ClampMagnitude : PureFunctionNode<Vector2, Vector2, float>
    {
        public override Vector2 Invoke(Vector2 vector, float maxLength)
        {
            return Vector2.ClampMagnitude(vector, maxLength);
        }
    }

    [Name("dot(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("向量点乘,返回两个向量的方向相似度,0:表示垂直,1:平行")]
    public class G_Vector2Dot : PureFunctionNode<float, Vector2, Vector2>
    {
        public override float Invoke(Vector2 vector1, Vector2 vector2)
        {
            return Vector2.Dot(vector1, vector2);
        }
    }

    [Name("angle(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("两向量间夹角")]
    public class G_Vector2Angle : PureFunctionNode<float, Vector2, Vector2>
    {
        public override float Invoke(Vector2 vector1, Vector2 vector2)
        {
            return Vector2.Angle(vector1, vector2);
        }
    }

    [Name("distance(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("两位置间的距离,计算消耗大,避免每帧调用")]
    public class G_Vector2Distance : PureFunctionNode<float, Vector2, Vector2>
    {
        public override float Invoke(Vector2 position1, Vector2 position2)
        {
            return Vector2.Distance(position1, position2);
        }
    }

    [Name("sqrMagnitude(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("向量的长度的平方")]
    public class G_Vector2SqrMagnitude : PureFunctionNode<float, Vector2>
    {
        public override float Invoke(Vector2 vector2)
        {
            return Vector2.SqrMagnitude(vector2);
        }
    }

    [Name("max(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("返回两个向量中长度较长的向量")]
    public class G_Vector2Max : PureFunctionNode<Vector2, Vector2, Vector2>
    {
        public override Vector2 Invoke(Vector2 min, Vector2 max)
        {
            return Vector2.Max(min, max);
        }
    }

    [Name("min(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("返回两个向量中长度较长的向量")]
    public class G_Vector2Min : PureFunctionNode<Vector2, Vector2, Vector2>
    {
        public override Vector2 Invoke(Vector2 min, Vector2 max)
        {
            return Vector2.Min(min, max);
        }
    }

    [Name("moveTowards(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("以一定增值从一个位置移动到另一个位置")]
    public class G_Vector2MoveTowards : PureFunctionNode<Vector2, Vector2, Vector2, float>
    {
        public override Vector2 Invoke(Vector2 current, Vector2 target, float moveDelta)
        {
            return Vector2.MoveTowards(current, target, moveDelta);
        }
    }


    [Name("reflect(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("将向量在onNormal的反射向量")]
    public class G_Vector2Reflect : PureFunctionNode<Vector2, Vector2, Vector2>
    {
        public override Vector2 Invoke(Vector2 vector2, Vector2 planeNormal)
        {
            return Vector2.Reflect(vector2, planeNormal);
        }
    }
    [Name("scale(Vector2)")]
    [Category("UnityEngine/Vector2")]
    [Description("两向量每个元素相乘")]
    public class G_Vector2Scale : PureFunctionNode<Vector2, Vector2, Vector2>
    {
        public override Vector2 Invoke(Vector2 vector1, Vector2 vector2)
        {
            return Vector2.Scale(vector1, vector2);
        }
    }
    #endregion

    #region NavMesh

    [Name("rayCast(navMesh)")]
    [Category("UnityEngine/NavMesh")]
    [Description("在navmesh上发射射线,返回碰撞点")]
    [ContextDefinedInputs(typeof(Flow), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow), typeof(NavMeshHit), typeof(bool))]
    public class G_NavmeshRayCast : FlowNode
    {
        private NavMeshHit hit;
        private bool isHited = false;

        private int mode = 0;
        private int Mode {
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
            var sourcePosition = AddValueInput<Vector3>("sourcePosition");
            var targetPosition = AddValueInput<Vector3>("targetPosition");
            
            var output = AddFlowOutput("Output");

            if (Mode == 0)
            {
                int areaIndex = 0;
                var areaMask = AddValueInput<int>("areaMask(-1=AllMask)");
                if (areaMask.value < 0)
                {
                    areaIndex = NavMesh.AllAreas;
                }
                else
                {
                    areaIndex = areaMask.value;
                }
                AddFlowInput("In",(f)=>
                {
                    isHited = NavMesh.Raycast(sourcePosition.value, targetPosition.value, out hit, areaIndex);
                    output.Call(f);
                });
            }
            else
            {
                var areaMask = AddValueInput<NavMeshQueryFilter>("NavMeshQueryFilter");
                AddFlowInput("In", (f) =>
                {   
                    isHited = NavMesh.Raycast(sourcePosition.value, targetPosition.value, out hit, areaMask.value);
                    output.Call(f);
                });
            }

            AddValueOutput<NavMeshHit>("NavMeshHit", () => { return hit; });
            AddValueOutput<bool>("isHited", () => { return isHited; });
        }
#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Mode = UnityEditor.EditorGUILayout.Popup("FilterMode", Mode, new string[] {"areaMask", "NavMeshQueryFilter" });
        }
#endif
    }

    [Name("getAreaFromName")]
    [Category("UnityEngine/NavMesh")]
    [Description("根据navmesh层名来获得其int值")]
    public class G_NavmeshGetLayIntFromName : PureFunctionNode<int, string>
    {
        public override int Invoke(string layerName)
        {
            return NavMesh.GetAreaFromName(layerName);
        }
    }

    [Name("allAreas")]
    [Category("UnityEngine/NavMesh")]
    [Description("所有navmesh层名")]
    public class G_NavmeshAllAreas : PureFunctionNode<int >
    {
        public override int Invoke()
        {
            return NavMesh.AllAreas;
        }
    }

    [Name("getAreaCost")]
    [Category("UnityEngine/NavMesh")]
    [Description("获得区域移动代价")]
    public class G_NavmeshGetAreaCost : PureFunctionNode<float,int>
    {
        public override float Invoke(int areaIndex)
        {
            return NavMesh.GetAreaCost(areaIndex);
        }
    }

    [Name("setAreaCost")]
    [Category("UnityEngine/NavMesh")]
    [Description("设置区域移动代价,输入值必须大于1")]
    public class G_NavmeshSetAreaCost : CallableActionNode<int, float>
    {
        public override void Invoke(int areaIndex, float cost)
        {
            NavMesh.SetAreaCost(areaIndex, cost);
        }
    }

    [Name("samplePathPosition")]
    [Category("UnityEngine/NavMesh")]
    [Description("计算NavMesh附加可用navmesh点")]
    [ContextDefinedInputs(typeof(Flow), typeof(Vector3), typeof(int), typeof(float))]
    [ContextDefinedOutputs(typeof(Flow), typeof(NavMeshHit), typeof(bool))]
    public class S_NavMeshSamplePathPosition : FlowNode
    {

        private bool ishited = false;
        private NavMeshHit hit;

        protected override void RegisterPorts()
        {
            var sourcePosition  = AddValueInput<Vector3>("sourcePosition");
            var areaMask = AddValueInput<int>("AreaMask,-1=All");
            var radius = AddValueInput<float>("Radius");

            int area = 0;
            if (areaMask.value < 0)
            {
                area = NavMesh.AllAreas;
            }
            else
            {
                area = areaMask.value;
            }

            var output = AddFlowOutput("OutPut");
            AddFlowInput("In", (f) =>
            {
                ishited = NavMesh.SamplePosition(sourcePosition.value, out hit, radius.value, area);
                output.Call(f);
            });

            AddValueOutput<bool>("ishited", () => { return ishited; });
            AddValueOutput<NavMeshHit>("NavMeshPath", () => { return hit; });
        }
    }

    [Name("calculatePath")]
    [Category("UnityEngine/NavMesh")]
    [Description("计算路径")]
    [ContextDefinedInputs(typeof(Flow), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow), typeof(NavMeshPath), typeof(bool))]
    public class G_NavmeshCalculatePath : FlowNode
    {
        private NavMeshPath path=new NavMeshPath();
        private bool isHited = false;

        private int mode = 0;
        private int Mode
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
            var sourcePosition = AddValueInput<Vector3>("sourcePosition");
            var targetPosition = AddValueInput<Vector3>("targetPosition");

            var output = AddFlowOutput("Output");

            if (Mode == 0)
            {
                int areaIndex = 0;
                var areaMask = AddValueInput<int>("areaMask(-1=AllMask)");
                if (areaMask.value <0)
                {
                    areaIndex = NavMesh.AllAreas;
                }
                else
                {
                    areaIndex = areaMask.value;
                }
                AddFlowInput("In", (f) =>
                {
                    isHited = NavMesh.CalculatePath(sourcePosition.value, targetPosition.value,areaIndex, path);
                    output.Call(f);
                });
            }
            else
            {
                var areaMask = AddValueInput<NavMeshQueryFilter>("NavMeshQueryFilter");
                AddFlowInput("In", (f) =>
                {
                    isHited = NavMesh.CalculatePath(sourcePosition.value, targetPosition.value, areaMask.value, path);
                    output.Call(f);
                });
            }
            AddValueOutput<NavMeshPath>("NavMeshPath", () => { return path; });
            AddValueOutput<bool>("isHited", () => { return isHited; });
        }

#if UNITY_EDITOR
        protected override void OnNodeInspectorGUI()
        {
            base.OnNodeInspectorGUI();
            Mode = UnityEditor.EditorGUILayout.Popup("FilterMode", Mode, new string[] { "areaMask", "NavMeshQueryFilter" });
        }
#endif
    }
    #endregion

    #region NavMeshAgent
    [Name("getDesiredVelocity")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("预期移动方向")]
    public class G_NavMeshAgentdesiredVelocity : PureFunctionNode<Vector3,NavMeshAgent>
    {
        public override Vector3 Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.desiredVelocity;
        }
    }

    [Name("getVelocity")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("移动速度")]
    public class G_NavMeshAgentVelocity : PureFunctionNode<Vector3, NavMeshAgent>
    {
        public override Vector3 Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.velocity;
        }
    }

    [Name("getDestination")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得移动目的坐标")]
    public class G_NavMeshAgentdestinationy : PureFunctionNode<Vector3, NavMeshAgent>
    {
        public override Vector3 Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.destination;
        }
    }

    [Name("setDestination")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置移动目的地")]
    public class S_NavMeshAgentdestinationy : CallableActionNode<NavMeshAgent,Vector3>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, Vector3 destination)
        {
             navMeshAgent.destination= destination;
        }
    }

    [Name("setAcceleration")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置移动加速度")]
    public class S_NavMeshAgentacceleration : CallableActionNode<NavMeshAgent, float>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, float acceleration)
        {
            navMeshAgent.acceleration = acceleration;
        }
    }

    [Name("getAcceleration")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得移动加速度")]
    public class G_NavMeshAgentacceleration : PureFunctionNode<float,NavMeshAgent>
    {
        public override float Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.acceleration;
        }
    }

    [Name("getAngularSpeed")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得最大转向速度")]
    public class G_NavMeshAgentangularSpeed : PureFunctionNode<float, NavMeshAgent>
    {
        public override float Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.angularSpeed;
        }
    }

    [Name("setAngularSpeed")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置最大转向速度")]
    public class S_NavMeshAgentangularSpeed : CallableActionNode<NavMeshAgent,float>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, float angleSpeed)
        {
            navMeshAgent.angularSpeed= angleSpeed;
        }
    }

    [Name("getAreaMask")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得可移动区域")]
    public class G_NavMeshAgentareaMask : PureFunctionNode<int, NavMeshAgent>
    {
        public override int Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.areaMask;
        }
    }

    [Name("setAreaMask")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置可移动区域")]
    public class S_NavMeshAgentareaMask : CallableActionNode<NavMeshAgent, int>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, int areaMask)
        {
            navMeshAgent.areaMask = areaMask;
        }
    }

    [Name("getAvoidancePriority")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得回避能力数值,值越高,越容易让步回避,0-100")]
    public class G_NavMeshAgentavoidancePriority : PureFunctionNode<int, NavMeshAgent>
    {
        public override int Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.avoidancePriority;
        }
    }

    [Name("setAvoidancePriority")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置回避能力数值,值越高,越容易让步回避,0-100")]
    public class S_NavMeshAgentavoidancePriority : CallableActionNode<NavMeshAgent, int>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, int avoidancePriority)
        {
            navMeshAgent.avoidancePriority= avoidancePriority;
        }
    }

    [Name("getAutoBraking")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得到达目的地是是否立即刹车状态")]
    public class G_NavMeshAgentautoBraking : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.autoBraking;
        }
    }

    [Name("setAutoBraking")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置到达目的地是是否立即刹车")]
    public class S_NavMeshAgentAutoBraking : CallableActionNode<NavMeshAgent, bool>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, bool autoBraking)
        {
            navMeshAgent.autoBraking = autoBraking;
        }
    }

    [Name("getAutoRepath")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当路径不可用是是否自动重新计算路径")]
    public class G_NavMeshAgentautoRepath : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.autoRepath;
        }
    }

    [Name("setAutoRepath")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置当路径不可用是是否自动重新计算路径")]
    public class S_NavMeshAgentautoRepath : CallableActionNode<NavMeshAgent, bool>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, bool autoRepath)
        {
            navMeshAgent.autoRepath = autoRepath;
        }
    }

    [Name("getAutoTraverseOffMeshLink")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当在跳转线上是是否自动滑行过去")]
    public class G_NavMeshAgentautoTraverseOffMeshLink : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.autoTraverseOffMeshLink;
        }
    }

    [Name("setAutoTraverseOffMeshLink")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置当在跳转线上是是否自动滑行过去")]
    public class S_NavMeshAutoTraverseOffMeshLink : CallableActionNode<NavMeshAgent, bool>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, bool autoTraverseOffMeshLink)
        {
            navMeshAgent.autoTraverseOffMeshLink = autoTraverseOffMeshLink;
        }
    }

    [Name("getHasPath")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前是否有移动路径")]
    public class G_NavMeshAgentHasPath : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.hasPath;
        }
    }

    [Name("getIsOnNavMesh")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前是否在navmesh上")]
    public class G_NavMeshAgentisOnNavMesh : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.isOnNavMesh;
        }
    }
    [Name("getIsOnOffMeshLink")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前是否在navmeshLink上")]
    public class G_NavMeshAgentIsOnOffMeshLink : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.isOnOffMeshLink;
        }
    }

    [Name("getIsPathStale")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前路径是否过期")]
    public class G_NavMeshAgentisPathStale : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.isPathStale;
        }
    }

    [Name("getisStopped")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前代理是否被停止")]
    public class G_NavMeshAgentisStopped : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.isStopped;
        }
    }
    [Name("setIsStopped")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置当前代理停止状态")]
    public class S_NavMeshisStopped : CallableActionNode<NavMeshAgent, bool>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, bool isStopped)
        {
            navMeshAgent.isStopped = isStopped;
        }
    }

    [Name("getNavMeshOwner")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMesh所依附的物体")]
    public class G_NavMeshAgentnavMeshOwner : PureFunctionNode<Object, NavMeshAgent>
    {
        public override Object Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.navMeshOwner;
        }
    }

    [Name("getBaseOffset")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMeshAgent和游戏物体位置的偏移值")]
    public class G_NavMeshAgentbaseOffset : PureFunctionNode<float, NavMeshAgent>
    {
        public override float Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.baseOffset;
        }
    }

    [Name("setBaseOffset")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置NavMeshAgent和游戏物体位置的偏移值")]
    public class S_NavMeshbaseOffset : CallableActionNode<NavMeshAgent, float>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, float baseOffset)
        {
            navMeshAgent.baseOffset = baseOffset;
        }
    }

    [Name("getHeight")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMeshAgent的高度")]
    public class G_NavMeshAgentheight : PureFunctionNode<float, NavMeshAgent>
    {
        public override float Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.height;
        }
    }

    [Name("setHeight")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置当前NavMeshAgent的高度")]
    public class S_NavMeshHeight : CallableActionNode<NavMeshAgent, float>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, float height)
        {
            navMeshAgent.height = height;
        }
    }

    [Name("getRadius")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMeshAgent的半径")]
    public class G_NavMeshAgentradius : PureFunctionNode<float, NavMeshAgent>
    {
        public override float Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.radius;
        }
    }

    [Name("setRadius")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置当前NavMeshAgent的半径")]
    public class S_NavMeshRadius : CallableActionNode<NavMeshAgent, float>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, float radius)
        {
            navMeshAgent.radius = radius;
        }
    }

    [Name("getSpeed")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMeshAgent的速度")]
    public class G_NavMeshAgentrspeed : PureFunctionNode<float, NavMeshAgent>
    {
        public override float Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.speed;
        }
    }

    [Name("setSpeed")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置当前NavMeshAgent的速度")]
    public class S_NavMeshSpeed : CallableActionNode<NavMeshAgent, float>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, float speed)
        {
            navMeshAgent.speed = speed;
        }
    }

    [Name("getRemainingDistance")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMeshAgent离目的的距离")]
    public class G_NavMeshAgentremainingDistance : PureFunctionNode<float, NavMeshAgent>
    {
        public override float Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.remainingDistance;
        }
    }

    [Name("getStoppingDistance")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMeshAgent到目的地时保留的距离")]
    public class G_NavMeshAgentstoppingDistance : PureFunctionNode<float, NavMeshAgent>
    {
        public override float Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.stoppingDistance;
        }
    }

    [Name("setStoppingDistance")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置NavMeshAgent到目的地时保留的距离")]
    public class S_NavMeshAgentstoppingDistance : CallableActionNode<NavMeshAgent, float>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, float stoppingDistance)
        {
            navMeshAgent.stoppingDistance = stoppingDistance;
        }
    }

    [Name("getCurrentOffMeshLinkData")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得当前NavMeshAgent所在跳转连接数据")]
    public class G_NavMeshAgentCurrentOffMeshLinkData : PureFunctionNode<OffMeshLinkData, NavMeshAgent>
    {
        public override OffMeshLinkData Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.currentOffMeshLinkData;
        }
    }

    [Name("getNextOffMeshLinkData")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得下一个NavMeshAgent所在跳转连接数据")]
    public class G_NavMeshAgentNextOffMeshLinkDat : PureFunctionNode<OffMeshLinkData, NavMeshAgent>
    {
        public override OffMeshLinkData Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.nextOffMeshLinkData;
        }
    }

    [Name("getNextPosition")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得下一个NavMeshAgent到的位置")]
    public class G_NavMeshAgentnextPosition : PureFunctionNode<Vector3, NavMeshAgent>
    {
        public override Vector3 Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.nextPosition;
        }
    }

    [Name("setNextPosition")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置下一个NavMeshAgent到的位置")]
    public class S_NavMeshAgentNextPosition : CallableActionNode<NavMeshAgent, Vector3>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, Vector3 nextPosition)
        {
            navMeshAgent.nextPosition = nextPosition;
        }
    }

    [Name("getObstacleAvoidanceType")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得NavMeshAgent回避障碍类型")]
    public class G_NavMeshAgentObstacleAvoidanceType : PureFunctionNode<ObstacleAvoidanceType, NavMeshAgent>
    {
        public override ObstacleAvoidanceType Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.obstacleAvoidanceType;
        }
    }

    [Name("setObstacleAvoidanceType")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置NavMeshAgent回避障碍类型")]
    public class S_NavMeshAgentobstacleAvoidanceType : CallableActionNode<NavMeshAgent, ObstacleAvoidanceType>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, ObstacleAvoidanceType obstacleAvoidanceType)
        {
            navMeshAgent.obstacleAvoidanceType = obstacleAvoidanceType;
        }
    }

    [Name("getPathEndPosition")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得NavMeshAgent当前路径的终点位置")]
    public class G_NavMeshAgentPathEndPosition : PureFunctionNode<Vector3, NavMeshAgent>
    {
        public override Vector3 Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.pathEndPosition;
        }
    }

    [Name("getPath")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得NavMeshAgent当前路径")]
    public class G_NavMeshAgentPath : PureFunctionNode<NavMeshPath, NavMeshAgent>
    {
        public override NavMeshPath Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.path;
        }
    }

    [Name("getUpdatePosition")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("获得NavMeshAgent移动时是否更新transform位置，默认同步")]
    public class G_NavMeshAgentupdatePosition : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.updatePosition;
        }
    }
    [Name("getPathPending")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMeshAgent是否在计算移动路径")]
    public class G_NavMeshAgentPathPending : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.pathPending;
        }
    }

    [Name("getUpdateUpAxis")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMeshAgent是否和NavMesh的Up向量平行")]
    public class G_NavMeshAgentUpdateUpAxis : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.updateUpAxis;
        }
    }

    [Name("setUpdateUpAxis")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置NavMeshAgent是否和NavMesh的Up向量平行")]
    public class S_NavMeshAgentupdateUpAxis : CallableActionNode<NavMeshAgent, bool>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, bool updateUpAxis)
        {
            navMeshAgent.updateUpAxis = updateUpAxis;
        }
    }

    [Name("getUpdateRotation")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMeshAgent是否同步tranform组件角度")]
    public class G_NavMeshAgentUpdateRotation : PureFunctionNode<bool, NavMeshAgent>
    {
        public override bool Invoke(NavMeshAgent navMeshAgent)
        {
            return navMeshAgent.updateRotation;
        }
    }
    [Name("setUpdateRotation")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置NavMeshAgent是否同步tranform组件角度")]
    public class S_NavMeshAgentupdateRotation : CallableActionNode<NavMeshAgent, bool>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, bool updateRotation)
        {
            navMeshAgent.updateRotation = updateRotation;
        }
    }


    [Name("SetDestination")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置NavMeshAgent的目的地")]
    public class S_NavMeshAgentDestination : CallableActionNode<NavMeshAgent, Vector3>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
        }
    }

    [Name("warp")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("NavMeshAgent换行到新地点")]
    public class S_NavMeshAgentWarp : CallableActionNode<NavMeshAgent, Vector3>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, Vector3 newPosition)
        {
            navMeshAgent.Warp(newPosition);
        }
    }

    [Name("activateCurrentOffMeshLink")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("NavMeshAgent是否激活当前MeshLink")]
    public class S_NavMeshAgentActivateCurrentOffMeshLink : CallableActionNode<NavMeshAgent, bool>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, bool activeCurrentLink)
        {
            navMeshAgent.ActivateCurrentOffMeshLink(activeCurrentLink);
        }
    }

    [Name("move")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置NavMeshAgent和当前位置的偏移位置")]
    public class S_NavMeshAgentMove : CallableActionNode<NavMeshAgent, Vector3>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, Vector3 offset)
        {
            navMeshAgent.Move(offset);
        }
    }

    [Name("calculatePath")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("计算NavMeshAgent路径")]
    [ContextDefinedInputs(typeof(Flow), typeof(NavMeshAgent), typeof(Vector3))]
    [ContextDefinedOutputs(typeof(Flow), typeof(bool), typeof(NavMeshHit))]
    public class S_NavMeshAgentCalculatePath : FlowNode
    {
        private bool calculated = false;
        private NavMeshPath path=new NavMeshPath();

        protected override void RegisterPorts()
        {
            var navMeshAgent = AddValueInput<NavMeshAgent>("NavMeshAgent");
            var targetPosition = AddValueInput<Vector3>("targetPosition");

            var output = AddFlowOutput("OutPut");
            AddFlowInput("In", (f) =>
            {
                calculated= navMeshAgent.value.CalculatePath(targetPosition.value, path);
                output.Call(f);});

            AddValueOutput<bool>("calculated", () => { return calculated; });
            AddValueOutput<NavMeshPath>("NavMeshPath", () => { return path; });
        }
    }

    [Name("samplePathPosition")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("计算NavMeshAgent附近可用navmesh点")]
    [ContextDefinedInputs(typeof(Flow), typeof(NavMeshAgent), typeof(int), typeof(float))]
    [ContextDefinedOutputs(typeof(Flow), typeof(bool), typeof(NavMeshHit))]
    public class S_NavMeshAgentSamplePathPosition : FlowNode
    {   
        
        private bool ishited = false;
        private NavMeshHit hit;

        protected override void RegisterPorts()
        {
            var navMeshAgent = AddValueInput<NavMeshAgent>("NavMeshAgent");
            var areaMask = AddValueInput<int>("AreaMask,-1=All");
            var radius = AddValueInput<float>("Radius");

            int area = 0;
            if (areaMask.value < 0)
            {
                area = NavMesh.AllAreas;
            }
            else
            {
                area = areaMask.value;
            }

            var output = AddFlowOutput("OutPut");
            AddFlowInput("In", (f) =>
            {
                ishited = navMeshAgent.value.SamplePathPosition(area, radius.value,out hit);
                output.Call(f);
            });

            AddValueOutput<bool>("ishited", () => { return ishited; });
            AddValueOutput<NavMeshHit>("NavMeshPath", () => { return hit; });
        }
    }

    [Name("setPath")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置NavMeshAgent移动路径")]
    public class S_NavMeshAgentSetPath : CallableActionNode<NavMeshAgent, NavMeshPath>
    {
        public override void Invoke(NavMeshAgent navMeshAgent, NavMeshPath path)
        {
            navMeshAgent.SetPath(path);
        }
    }

    [Name("resetPath")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("清空NavMeshAgent移动路径")]
    public class S_NavMeshAgentResetPath : CallableActionNode<NavMeshAgent>
    {
        public override void Invoke(NavMeshAgent navMeshAgent)
        {
            navMeshAgent.ResetPath();
        }
    }

    [Name("setAreaCost")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("设置NavMeshAgent在此区域的移动代价")]
    public class S_NavMeshAgentSetAreaCost : CallableActionNode<NavMeshAgent, int,float>
    {
        public override void Invoke(NavMeshAgent navMeshAgent,int area, float cost)
        {
            navMeshAgent.SetAreaCost(area, cost);
        }
    }

    [Name("getUAreaCost")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("当前NavMeshAgent在此区域的移动代价")]
    public class G_NavMeshAgentGetAreaCost : PureFunctionNode<float, NavMeshAgent,int>
    {
        public override float Invoke(NavMeshAgent navMeshAgent, int area)
        {
            return navMeshAgent.GetAreaCost(area);
        }
    }

    [Name("completeOffMeshLink")]
    [Category("UnityEngine/NavMeshAgent")]
    [Description("立刻标记完成NavMeshAgent在link的移动")]
    public class S_NavMeshAgentCompleteOffMeshLink : CallableActionNode<NavMeshAgent>
    {
        public override void Invoke(NavMeshAgent navMeshAgent)
        {
            navMeshAgent.CompleteOffMeshLink();
        }
    }
    #endregion

    #region Slider
    [Name("getValue")]
    [Category("UnityEngine/Slider")]
    [Description("当前Slider值")]
    public class G_SliderGetValue : PureFunctionNode<float, Slider>
    {
        public override float Invoke(Slider slider)
        {
            return slider.value;
        }
    }

    [Name("setValue")]
    [Category("UnityEngine/Slider")]
    [Description("设置当前Slider值")]
    public class S_SliderGetValue : CallableFunctionNode<Slider,Slider, float>
    {
        public override Slider Invoke(Slider slider, float value)
        {
            slider.value = value;
            return slider;
        }
    }

    [Name("getMaxValue")]
    [Category("UnityEngine/Slider")]
    [Description("当前Slider的最大值")]
    public class G_SliderGetMaxValue : PureFunctionNode<float, Slider>
    {
        public override float Invoke(Slider slider)
        {
            return slider.maxValue;
        }
    }

    [Name("setMaxValue")]
    [Category("UnityEngine/Slider")]
    [Description("设置当前Slider的最大值")]
    public class S_SliderGetMaxValue : CallableFunctionNode<Slider,Slider, float>
    {
        public override Slider Invoke(Slider slider, float maxValue)
        {
            slider.maxValue = maxValue;
            return slider;
        }
    }

    [Name("getMinValue")]
    [Category("UnityEngine/Slider")]
    [Description("当前Slider的最小值")]
    public class G_SliderGetMinValue : PureFunctionNode<float, Slider>
    {
        public override float Invoke(Slider slider)
        {
            return slider.minValue;
        }
    }

    [Name("setMinValue")]
    [Category("UnityEngine/Slider")]
    [Description("设置当前Slider的最小值")]
    public class S_SliderGetMinValue : CallableFunctionNode<Slider,Slider, float>
    {
        public override Slider Invoke(Slider slider, float minValue)
        {
            slider.minValue = minValue;
            return slider;
        }
    }

    [Name("getNormalizedValue")]
    [Category("UnityEngine/Slider")]
    [Description("当前Slider值,按百分比,0-1间")]
    public class G_SliderGetnormalizedValue : PureFunctionNode<float, Slider>
    {
        public override float Invoke(Slider slider)
        {
            return slider.normalizedValue;
        }
    }

    [Name("setNormalizedValue")]
    [Category("UnityEngine/Slider")]
    [Description("设置当前Slider值,按百分比,0-1间")]
    public class S_SliderGetnormalizedValue : CallableFunctionNode<Slider, Slider, float>
    {
        public override Slider Invoke(Slider slider, float normalizedValue)
        {
            slider.normalizedValue = normalizedValue;
            return slider;
        }
    }
    #endregion

    #region Resource
    [Name("load(Resource)")]
    [Category("UnityEngine/Resource")]
    [Description("ResourceLoad")]
    public class G_ResourceLoad : PureFunctionNode<Object, string>
    {
        public override Object Invoke(string path)
        {
            return Resources.Load(path);
        }
    }

[Name("unloadUnusedAssets")]
[Category("UnityEngine/Resource")]
[Description("卸载无用的资源")]
public class S_UnloadUnusedAssets : CallableActionNode
{
    public override void Invoke()
    {
        Resources.UnloadUnusedAssets();
    }
}
#endregion
}