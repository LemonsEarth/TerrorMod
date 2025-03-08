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

float4 LaserShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0, float4 position : SV_Position) : COLOR0
{
    float4 baseColor = tex2D(uImage0, coords);
    float2 movedCoords = float2(coords.x, coords.y - uTime * 0.9);
    movedCoords.y -= (uTime * 0.9); // moving twice for twice the speed because speed cant be more than 1
    movedCoords.y = frac(movedCoords.y);
    float4 noiseColor = tex2D(uImage1, movedCoords);
    float2 centeredCoords = coords * 2.0 - 1.0;
    float width = sin(uTime * 4) * 0.1 + 0.1;
    float distanceToCenter = distance(abs(centeredCoords.x), width);
    float4 finalColor = noiseColor * (1 - distanceToCenter);
    if (finalColor.r < 0.1)
        finalColor = 0;
    return finalColor * 3;
}

technique Tech1
{
    pass LaserShader
    {
        PixelShader = compile ps_2_0 LaserShader();
    }
}