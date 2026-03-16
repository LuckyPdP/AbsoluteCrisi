using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UnrealTonemapperFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private Material tonemapMaterial;
        private RTHandle sourceHandle;
        private RTHandle tempHandle;

        public CustomRenderPass(Material mat)
        {
            this.tonemapMaterial = mat;
            this.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public void SetTarget(RTHandle colorHandle)
        {
            this.sourceHandle = colorHandle;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            RenderingUtils.ReAllocateIfNeeded(ref tempHandle, desc, name: "_TempTonemap");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (tonemapMaterial == null || sourceHandle == null) return;
            
            CommandBuffer cmd = CommandBufferPool.Get("Unreal PostProcess");
            
            Blitter.BlitCameraTexture(cmd, sourceHandle, tempHandle, tonemapMaterial, 0);
            Blitter.BlitCameraTexture(cmd, tempHandle, sourceHandle);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Dispose()
        {
            tempHandle?.Release();
        }
    }

    CustomRenderPass m_ScriptablePass;
    public Material customTonemapMaterial; 

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(customTonemapMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (customTonemapMaterial != null)
        {
            m_ScriptablePass.SetTarget(renderer.cameraColorTargetHandle);
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }

    protected override void Dispose(bool disposing)
    {
        m_ScriptablePass?.Dispose();
    }
}