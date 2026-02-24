sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float uTime;
float uIntensity;
float uProgress;

float4 VignetteShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0, float4 position : SV_Position) : COLOR0
{   
    float4 baseColor = tex2D(uImage0, coords);
    float2 centeredCoords = coords * 2.0 - 1.0;
    float vignetteDist = 0.4;
    float distanceX = abs(centeredCoords.x) - vignetteDist;
    float distanceY = abs(centeredCoords.y) - vignetteDist;
    float2 fullDist = max(float2(distanceX, distanceY), 0.0);
    float colorIntensity = length(fullDist);
    colorIntensity = saturate(colorIntensity * uIntensity);
    baseColor += float4(uColor * colorIntensity, 1) * uProgress * (sin(uTime) + 4) * 0.25;
   
    return baseColor;
}

technique Tech1
{
    pass VignetteShader
    {
        PixelShader = compile ps_2_0 VignetteShader();
    }
}