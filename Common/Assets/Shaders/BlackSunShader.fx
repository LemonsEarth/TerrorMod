sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float2 uImageSize0;
float2 uImageSize1;
float4 uSourceRect;
float2 desiredPos;
float2 uTargetPosition;
float2 uScreenPosition;
float2 uScreenResolution;
float uTime;
float moveSpeed;
float opacity;

float4 BlackSunShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0, float4 position : SV_Position) : COLOR0
{
    float4 baseColor = tex2D(uImage0, coords);
    float2 centeredCoords = (coords * 2.0 - 1.0);
    centeredCoords.y /= (uScreenResolution.x / uScreenResolution.y);
    float distanceToCenter = length(centeredCoords);
    float centerDir = length(centeredCoords);
    float angle = atan2(centeredCoords.y, centeredCoords.x);
    float2 movedCoords = float2((angle + uTime * (pow(distanceToCenter, 0.5) / 100)) / 6.28, centerDir + uTime / 15);
    //float angle = atan2(centeredCoords.y, centeredCoords.x);
    //float2 movedCoords = float2(angle / 6.28, moveSpeed + uTime / 5);
    //movedCoords = frac(movedCoords);
    float4 noiseColor = tex2D(uImage1, movedCoords);
    float4 finalColor = noiseColor * distanceToCenter * 0.5;
    
    return finalColor * opacity;
}

technique Tech1
{
    pass BlackSunShader
    {
        PixelShader = compile ps_2_0 BlackSunShader();
    }
}