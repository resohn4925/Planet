using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RippleEffectRenderPass : ScriptableRenderPass
{
    private static Material rippleMaterial;
    private ProfilingSampler profilingSampler;
    private int tempTextureID = Shader.PropertyToID("_TempRippleTexture");

    public RippleEffectRenderPass()
    {
        profilingSampler = new ProfilingSampler("RippleEffect");
    }

    public static void SetMaterial(Material material)
    {
        rippleMaterial = material;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (rippleMaterial == null)
        {
            //Debug.LogWarning("Ripple material is null.");
            return;
        }

        var camera = renderingData.cameraData.camera;
        if (camera.cameraType != CameraType.Game && camera.cameraType != CameraType.SceneView)
            return;

        CommandBuffer cmd = CommandBufferPool.Get("Ripple Effect");

        using (new ProfilingScope(cmd, profilingSampler))
        {
            var source = renderingData.cameraData.renderer.cameraColorTarget;
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;

            // 获取相机目标描述符
            cmd.GetTemporaryRT(tempTextureID, descriptor.width, descriptor.height,
                0, FilterMode.Bilinear, descriptor.graphicsFormat);

            // 应用波纹效果到临时纹理
            cmd.Blit(source, tempTextureID, rippleMaterial);

            // 复制回原始目标
            cmd.Blit(tempTextureID, source);

            // 释放临时纹理
            cmd.ReleaseTemporaryRT(tempTextureID);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}