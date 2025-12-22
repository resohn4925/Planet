using QFramework;
using UnityEngine;


    public interface ICameraModel : IModel
    {
        BindableProperty<float> Distance { get; } // 距离
        BindableProperty<float> XAngle { get; }   // 水平角度
        BindableProperty<float> YAngle { get; }   // 垂直角度
        Transform Target { get; set; }            // 观察目标
        
        // 配置参数
        float MinDistance { get; }
        float MaxDistance { get; }
        float ZoomSpeed { get; }
        float RotateSpeed { get; }
    }

    public class CameraModel : AbstractModel, ICameraModel
    {
        public BindableProperty<float> Distance { get; } = new BindableProperty<float>(20f);
        public BindableProperty<float> XAngle { get; } = new BindableProperty<float>(0f);
        public BindableProperty<float> YAngle { get; } = new BindableProperty<float>(20f);
        public Transform Target { get; set; }

        public float MinDistance => 5f;
        public float MaxDistance => 400f;
        public float ZoomSpeed => 5f;
        public float RotateSpeed => 2f;

        protected override void OnInit()
        {
            // 数据初始化逻辑
        }
    }
