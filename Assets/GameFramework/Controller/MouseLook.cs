using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace GameFramework
{
    //序列化，显示在Inspector里
    [Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;


        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }


        public void LookRotation(Transform character, Transform camera)
        {
            //float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
            //float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            //返回围绕着Y轴旋转的一个旋转
            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            //返回围绕着X轴旋转的一个旋转
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            //限制X轴的旋转范围
            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            if (smooth)
            {
                //给相机和人物旋转做一个平滑处理
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }

            UpdateCursorLock();
        }

        /// <summary>
        /// 保证在绕X轴选择的角度值在限制范围内
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            //根据四元数的公式，这个运算得出来一个tan(angle)值
            //q.x=n* sin(angle/2) 在这里sin里面是一个弧度值
            //q.w=cos(angle/2)
            //q.x=sin(angle/2)/cos(angle/2)=tan(angle/2)
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            //q.x=tan(angle/2);
            //angle= 2*Atan(q.x)(弧度，要转角度)=2 * Mathf.Rad2Deg * Mathf.Atan (q.x);
            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
            //如果angle的值没有超过min，max，那转换前后的值是一样的。
            //如果angle的值小于min或者大于max了，那么angle的取值就发送了变化，这时候就又要重新转换下
            //这个就是上面那个angle公式的逆运算了
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if (!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        /// <summary>
        /// 鼠标的状态
        /// </summary>
        private void InternalLockUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}