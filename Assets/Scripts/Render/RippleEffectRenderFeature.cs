using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RippleEffectRenderFeature : ScriptableRendererFeature
{
    private RippleEffectRenderPass ripplePass;

    public override void Create()
    {
        ripplePass = new RippleEffectRenderPass();
        ripplePass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // 直接添加渲染通道，材质会在RippleEffectURP中设置
        renderer.EnqueuePass(ripplePass);
    }
}