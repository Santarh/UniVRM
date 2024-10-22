#ifndef URP_GLTF_PBR_INPUT_INCLUDED
#define URP_GLTF_PBR_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
#include "./GltfPbrInputSampler.hlsl"

CBUFFER_START(UnityPerMaterial)
TEXTURE2D(_BaseMap);
SAMPLER(sampler_BaseMap);
TEXTURE2D(_MetallicRoughnessMap);
SAMPLER(sampler_MetallicRoughnessMap);
TEXTURE2D(_BumpMap);
SAMPLER(sampler_BumpMap);
TEXTURE2D(_OcclusionMap);
SAMPLER(sampler_OcclusionMap);
TEXTURE2D(_EmissionMap);
SAMPLER(sampler_EmissionMap);

float4 _BaseMap_ST;
float4 _MetallicRoughnessMap_ST;
float4 _BumpMap_ST;
float4 _OcclusionMap_ST;
float4 _EmissionMap_ST;

half4 _BaseColor;
half4 _EmissionColor;

half _Metallic;
half _Roughness;
half _BumpScale;
half _OcclusionStrength;
half _Cutoff;
half _Surface;
CBUFFER_END

inline void InitializeGltfPbrSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    half4 albedoAlpha = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    half4 metallicRoughness = SampleMetallicRoughness(uv, _Metallic, _Roughness, TEXTURE2D_ARGS(_MetallicRoughnessMap, sampler_MetallicRoughnessMap));

    outSurfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);
    outSurfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;
    outSurfaceData.metallic = metallicRoughness.b;
    outSurfaceData.specular = half3(0.0, 0.0, 0.0);
    outSurfaceData.smoothness = 1.0 - metallicRoughness.g;
    outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
    outSurfaceData.occlusion = SampleOcclusion(uv, _OcclusionStrength, TEXTURE2D_ARGS(_OcclusionMap, sampler_OcclusionMap));
    outSurfaceData.emission = SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
    outSurfaceData.clearCoatMask = half(0.0);
    outSurfaceData.clearCoatSmoothness = half(0.0);
}

inline void InitializeStandardLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    // Dummy definition to avoid compile error
}

#endif