sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float uTime;
float uIntensity;
float uProgress;

float4 WavyShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0, float4 position : SV_Position) : COLOR0
{
    float baseAmplitude = 0.02;
    float baseFreqMod = 4;
    float dist = 0.5 - abs(coords.y - 0.5);
    
    float freqMod = baseFreqMod;
    float amplitude = baseAmplitude * uIntensity * dist;
    float2 shiftedCoords = coords + float2(0, sin((coords.x + uTime) * freqMod) * amplitude);

    float4 baseColor = tex2D(uImage0, shiftedCoords);
   
    return baseColor;
}

technique Tech1
{
    pass WavyShader
    {
        PixelShader = compile ps_2_0 WavyShader();
    }
}