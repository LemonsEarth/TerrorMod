sampler2D Texture : register(s0);

float fillThreshold = 0.5;
float strokeThreshold = 0.3;
float4 fillColor = float4(0, 0, 0, 1);
float4 strokeColor = float4(1, 1, 1, 1);

float4 MetaballShader(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(Texture, coords);
    float intensity = color.r;
    float fillAlpha = step(fillThreshold, intensity);
    float strokeAlpha = step(strokeThreshold, intensity) * (1 - fillAlpha);
    
    return (fillColor * fillAlpha) + (strokeColor * strokeAlpha);
}

technique Tech1
{
    pass MetaballShader
    {
        PixelShader = compile ps_2_0 MetaballShader();
    }
}