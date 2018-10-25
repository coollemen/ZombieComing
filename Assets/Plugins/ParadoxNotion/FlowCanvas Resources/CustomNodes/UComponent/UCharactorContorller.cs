using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    #region CharacterController

    [Name("isGrounded")]
    [Category("UnityEngine/CharacterController")]
    [Description("是否着地")]
    public class G_IsGround : PureFunctionNode<bool, CharacterController>
    {
        public override bool Invoke(CharacterController characterController)
        {
            return characterController.isGrounded;
        }
    }

    [Name("getVelocity")]
    [Category("UnityEngine/CharacterController")]
    [Description("角色位移速度")]
    public class G_Velocity : PureFunctionNode<Vector3, CharacterController>
    {
        public override Vector3 Invoke(CharacterController characterController)
        {
            return characterController.velocity;
        }
    }

    [Name("move")]
    [Category("UnityEngine/CharacterController")]
    [Description("移动角色控制器,可控制高度方向的移动,输入参数是每帧位移差量")]
    public class CCMove : CallableFunctionNode<CharacterController,CharacterController, Vector3>
    {
        public override CharacterController Invoke(CharacterController characterController, Vector3 moveDeltaVector3)
        {
            characterController.Move(moveDeltaVector3);
            return characterController;
        }
    }

    [Name("simpleMove")]
    [Category("UnityEngine/CharacterController")]
    [Description("移动角色控制器,不用考虑重力,输入参数是移动的速度")]
    public class CCSimpleMove : CallableFunctionNode<CharacterController,CharacterController, Vector3>
    {
        public override CharacterController Invoke(CharacterController characterController, Vector3 moveSpeed)
        {
            characterController.SimpleMove(moveSpeed);
            return characterController;
        }
    }

    #endregion
}
