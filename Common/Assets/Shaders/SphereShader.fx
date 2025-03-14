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

float4 SphereShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0, float4 position : SV_Position) : COLOR0
{
    float4 baseColor = tex2D(uImage0, coords);
    float moveValue = moveSpeed * uTime;
    float2 movedCoords = float2(coords.x, coords.y + moveValue);
    movedCoords = frac(movedCoords);
    float4 noiseColor = tex2D(uImage1, movedCoords);
    float2 centeredCoords = coords * 2.0 - 1.0;
    float width = sin(uTime * 4) * 0.05 + 0.95;
    float distanceToCenter = distance(abs(centeredCoords), float2(0, 0));
    float4 finalColor = noiseColor * (1.5 - distanceToCenter);
    float len = length(centeredCoords);
    /*if (distanceToCenter < width || len == 0)
    {     
        return finalColor * 4;
    }*/
    float sub = width / (len * 0.8)-width;
    if (len > width)
    {
        sub -= width;
    }
    if (finalColor.r < 0.1)
        finalColor = 0;
    return finalColor * sub;
}

technique Tech1
{
    pass SphereShader
    {
        PixelShader = compile ps_2_0 SphereShader();
    }
}