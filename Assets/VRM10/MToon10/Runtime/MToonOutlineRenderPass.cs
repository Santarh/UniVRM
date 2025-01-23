#if MTOON_URP
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace VRM10.MToon10
{
    public sealed class MToonOutlineRenderPass : ScriptableRenderPass
    {
        private const string ProfilerTag = nameof(MToonOutlineRenderPass);
        private readonly ProfilingSampler _profilingSampler = new ProfilingSampler(ProfilerTag);

        private readonly RenderQueueRange _renderQueueRange;
        
        public MToonOutlineRenderPass(RenderPassEvent renderPassEvent, RenderQueueRange renderQueueRange)
        {
            _renderQueueRange = renderQueueRange;
            this.renderPassEvent = renderPassEvent;
        }

#if UNITY_6000_0_OR_NEWER
        [System.Obsolete]
#endif
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, _profilingSampler))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                var camera = renderingData.cameraData.camera;
                var shaderTagId = new ShaderTagId("MToonOutline");
                var sortingSettings = new SortingSettings(camera);
                var drawingSettings = new DrawingSettings(shaderTagId, sortingSettings)
                {
                    perObjectData = PerObjectData.ReflectionProbes | PerObjectData.Lightmaps |
                                    PerObjectData.LightProbe | PerObjectData.LightData | PerObjectData.OcclusionProbe |
                                    PerObjectData.ShadowMask
                };
                var filteringSettings = FilteringSettings.defaultValue;
                filteringSettings.renderQueueRange = _renderQueueRange;
                var rendererListParams = new RendererListParams
                {
                    cullingResults = renderingData.cullResults,
                    drawSettings = drawingSettings,
                    filteringSettings = filteringSettings,
                };
                var rendererList = context.CreateRendererList(ref rendererListParams);
                cmd.DrawRendererList(rendererList);
            }
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }
    }
}
#endif
