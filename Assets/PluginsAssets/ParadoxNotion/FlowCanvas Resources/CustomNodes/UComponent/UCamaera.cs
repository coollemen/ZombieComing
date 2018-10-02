using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region Camera

    [Name("getMainCamera")]
    [Category("UnityEngine/Camera")]
    [Description("获取当前场景中tag标记为mainCamera的摄像机")]
    public class G_MainCamera : PureFunctionNode<Camera>
    {
        public override Camera Invoke()
        {
            return Camera.main;
        }
    }

    [Name("getfieldOfView")]
    [Category("UnityEngine/Camera")]
    [Description("获取当前Camera的视野")]
    public class G_CameraFov : PureFunctionNode<float, Camera>
    {
        public override float Invoke(Camera camera)
        {
            return camera.fieldOfView;
        }
    }

    [Name("setfieldOfView")]
    [Category("UnityEngine/Camera")]
    [Description("设置当前Camera的视野")]
    public class S_CameraFov : CallableFunctionNode<Camera,Camera, float>
    {
        public override Camera Invoke(Camera camera, float value)
        {
            camera.fieldOfView = value;
            return camera;
        }
    }

    [Name("viewportPointToRay")]
    [Category("UnityEngine/Camera")]
    [Description("制作从摄像机到视野中某一位置的射线Ray")]
    public class G_ViewportPointToRay : PureFunctionNode<Ray, Camera, Vector3>
    {
        public override Ray Invoke(Camera camera, Vector3 position)
        {
            return camera.ViewportPointToRay(position);
        }
    }

    [Name("screenPointToRay")]
    [Category("UnityEngine/Camera")]
    [Description("制作从摄像机平面空间某一位置发射的射线Ray")]
    public class G_ScreenPointToRay : PureFunctionNode<Ray, Camera, Vector3>
    {
        public override Ray Invoke(Camera camera, Vector3 position)
        {
            return camera.ScreenPointToRay(position);
        }
    }

    [Name("screenToViewportPoint")]
    [Category("UnityEngine/Camera")]
    [Description("将摄像机平面空间的某点位置转换成视野中的某点位置")]
    public class G_ScreenToViewportPoint : PureFunctionNode<Vector3, Camera, Vector3>
    {
        public override Vector3 Invoke(Camera camera, Vector3 position)
        {
            return camera.ScreenToViewportPoint(position);
        }
    }

    [Name("screenToWorldPoint")]
    [Category("UnityEngine/Camera")]
    [Description("将摄像机平面空间的某点位置转换成世界空间中的某点位置")]
    public class G_ScreenToWorldPoint : PureFunctionNode<Vector3, Camera, Vector3>
    {
        public override Vector3 Invoke(Camera camera, Vector3 position)
        {
            return camera.ScreenToWorldPoint(position);
        }
    }

    [Name("viewportToScreenPoint")]
    [Category("UnityEngine/Camera")]
    [Description("将视野中的某点位置转换成摄像机平面空间的某点位置")]
    public class G_ViewportToScreenPoint : PureFunctionNode<Vector3, Camera, Vector3>
    {
        public override Vector3 Invoke(Camera camera, Vector3 position)
        {
            return camera.ViewportToScreenPoint(position);
        }
    }

    [Name("viewportToWorldPoint")]
    [Category("UnityEngine/Camera")]
    [Description("将视野中的某点位置转换成世界空间中的某点位置")]
    public class G_ViewportToWorldPoint : PureFunctionNode<Vector3, Camera, Vector3>
    {
        public override Vector3 Invoke(Camera camera, Vector3 position)
        {
            return camera.ViewportToWorldPoint(position);
        }
    }

    [Name("worldToScreenPoint")]
    [Category("UnityEngine/Camera")]
    [Description("将世界空间中的某点位置转换成摄像机平面空间的某点位置")]
    public class G_WorldToScreenPoint : PureFunctionNode<Vector3, Camera, Vector3>
    {
        public override Vector3 Invoke(Camera camera, Vector3 position)
        {
            return camera.WorldToScreenPoint(position);
        }
    }

    #endregion
}