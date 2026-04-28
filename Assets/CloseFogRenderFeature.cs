using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CloseFogRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Material material;
        public RenderPassEvent passEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    class CloseFogPass : ScriptableRenderPass
    {
        private readonly Material material;
        private RTHandle tempColor;
        private RTHandle cameraColorTarget;

        public CloseFogPass(Material material, RenderPassEvent passEvent)
        {
            this.material = material;
            renderPassEvent = passEvent;
        }

        public void SetTarget(RTHandle colorTarget)
        {
            cameraColorTarget = colorTarget;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            ConfigureInput(ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth);
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            if (material == null)
                return;

            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;

            RenderingUtils.ReAllocateIfNeeded(
                ref tempColor,
                desc,
                FilterMode.Bilinear,
                TextureWrapMode.Clamp,
                name: "_CloseFogTemp"
            );
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null || cameraColorTarget == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Close Fog Pass");

            Blitter.BlitCameraTexture(cmd, cameraColorTarget, tempColor, material, 0);
            Blitter.BlitCameraTexture(cmd, tempColor, cameraColorTarget);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }

        public void Dispose()
        {
            tempColor?.Release();
        }
    }

    public Settings settings = new Settings();

    private CloseFogPass pass;

    public override void Create()
    {
        pass = new CloseFogPass(settings.material, settings.passEvent);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (settings.material == null)
            return;

        pass.SetTarget(renderer.cameraColorTargetHandle);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material == null)
            return;

        renderer.EnqueuePass(pass);
    }

    protected override void Dispose(bool disposing)
    {
        pass?.Dispose();
    }
}