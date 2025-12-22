using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace QFramework.Game
{
    // 架构名称改为 GameApp
    public class GameApp : Architecture<GameApp>
    {
        protected override void Init()
        {
            // 注册相机数据模型
            this.RegisterModel<ICameraModel>(new CameraModel());
        }
    }
}