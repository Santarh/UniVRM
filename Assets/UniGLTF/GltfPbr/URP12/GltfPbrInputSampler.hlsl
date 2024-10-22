#ifndef URP_GLTF_PBR_INPUT_SAMPLER_INCLUDED
#define URP_GLTF_PBR_INPUT_SAMPLER_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

half Alpha(half albedoAlpha, half4 color, half cutoff)
{
    half alpha = albedoAlpha * color.a;

#if defined(_ALPHATEST_ON)
    clip(alpha - cutoff);
#endif

    return alpha;
}

half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
{
    return half4(SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv));
}

half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
{
#ifdef _NORMALMAP
    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
    return UnpackNormalScale(n, scale);
#else
    return half3(0.0h, 0.0h, 1.0h);
#endif
}

half3 SampleEmission(float2 uv, half3 emissionColor, TEXTURE2D_PARAM(emissionMap, sampler_emissionMap))
{
#ifndef _EMISSION
    return 0;
#else
    return SAMPLE_TEXTURE2D(emissionMap, sampler_emissionMap, uv).rgb * emissionColor;
#endif
}

// NOTE: G: roughness, B: metallic
half4 SampleMetallicRoughness(float2 uv, half metallic, half roughness, TEXTURE2D_PARAM(metallicRoughnessMap, sampler_metallicRoughnessMap))
{
    half4 metallicRoughness;

#ifdef _METALLICROUGHNESSMAP
    metallicRoughness = SAMPLE_TEXTURE2D(metallicRoughnessMap, sampler_metallicRoughnessMap, uv);
    metallicRoughness.g *= roughness;
    metallicRoughness.b *= metallic;
#else
    metallicRoughness.g = roughness;
    metallicRoughness.b = metallic;
#endif

    return metallicRoughness;
}

half SampleOcclusion(float2 uv, half occlusionStrength, TEXTURE2D_PARAM(occlusionMap, sampler_occlusionMap))
{
#ifdef _OCCLUSIONMAP
    half occ = SAMPLE_TEXTURE2D(occlusionMap, sampler_occlusionMap, uv).g;
    return LerpWhiteTo(occ, occlusionStrength);
#else
    return half(1.0);
#endif
}

#endif
