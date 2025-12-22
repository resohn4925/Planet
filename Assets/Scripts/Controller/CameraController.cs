using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Game
{
    public partial class CameraController : ViewController,IController
    {

        private ICameraModel mModel;

        // 平滑阻尼变量
        private float currentX = 0f;
        private float currentY = 0f;
        private float currentDistance = 0f;
        private float xVelocity = 0f;
        private float yVelocity = 0f;
        private float zoomVelocity = 0f;

        [Header("Settings")]
        [SerializeField] private Transform initialTarget; // 拖入星球物体
        [SerializeField] private float smoothTime = 0.1f; // 平滑时间

        private void Start()
        {
            // 1. 获取模型
            mModel = this.GetModel<ICameraModel>();

            // 2. 初始化目标
            if (initialTarget != null)
            {
                mModel.Target = initialTarget;
            }

            // 同步初始数据，防止相机瞬移
            currentX = mModel.XAngle.Value;
            currentY = mModel.YAngle.Value;
            currentDistance = mModel.Distance.Value;
        }

        private void Update()
        {
            if (mModel.Target == null) return;

            HandleInput();
        }

        private void LateUpdate()
        {
            if (mModel.Target == null) return;

            UpdateCameraTransform();
        }

        // --- Controller: 处理输入 ---
        private void HandleInput()
        {
            // 右键或左键旋转
            if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
            {
                float mouseX = Input.GetAxis("Mouse X") * mModel.RotateSpeed;
                float mouseY = Input.GetAxis("Mouse Y") * mModel.RotateSpeed;

                mModel.XAngle.Value += mouseX;
                mModel.YAngle.Value -= mouseY;

                // 限制垂直角度 (-85 ~ 85) 防止万向节死锁或翻转
                mModel.YAngle.Value = Mathf.Clamp(mModel.YAngle.Value, -85f, 85f);
            }

            // 滚轮缩放
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.001f)
            {
                mModel.Distance.Value -= scroll * mModel.ZoomSpeed * 10f;
                mModel.Distance.Value = Mathf.Clamp(mModel.Distance.Value, mModel.MinDistance, mModel.MaxDistance);
            }
        }

        // --- View: 更新表现 ---
        private void UpdateCameraTransform()
        {
            // 平滑插值
            currentX = Mathf.SmoothDamp(currentX, mModel.XAngle.Value, ref xVelocity, smoothTime);
            currentY = Mathf.SmoothDamp(currentY, mModel.YAngle.Value, ref yVelocity, smoothTime);
            currentDistance = Mathf.SmoothDamp(currentDistance, mModel.Distance.Value, ref zoomVelocity, smoothTime);

            // 计算旋转和位置
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            Vector3 position = mModel.Target.position + rotation * new Vector3(0, 0, -currentDistance);

            transform.rotation = rotation;
            transform.position = position;
        }

        // 指定所属架构为 GameApp
        public IArchitecture GetArchitecture()
        {
            return GameApp.Interface;
        }
    }

}
