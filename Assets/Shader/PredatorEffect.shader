// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate.matrix.texture[0]' with 'UNITY_MATRIX_TEXTURE0'
// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'
// Upgrade NOTE: replaced 'texRECTproj' with 'tex2Dproj'

Shader "Custom/PredatorEffect"
{
    Properties
    {
        _Distortion( "Distortion", range( 0, 0.05 ) ) = 0.025
    }
 
    SubShader
    {
   
        Tags
        {
            "Queue" = "Transparent"
        }
 
        GrabPass
        {
        }
           
        Pass
        {
            Name "DISTORT"
               
            CGPROGRAM
                #pragma fragment PS
                #pragma vertex VS
               
                // It is important to bind the sampler to a specific register, explained further down..
                sampler2D _GrabTexture : register(s0);
 
                struct VS_IN
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };
 
                struct VS_OUT
                {
                    float4 pos : POSITION;
                    float4 uvgrab : TEXCOORD0;
                    float3 normal : TEXCOORD1;
                };
               
                #define PS_IN VS_OUT
               
                uniform float _Distortion;
               
                VS_OUT VS( VS_IN v )
                {
                    VS_OUT o = (VS_OUT)0;
                    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
                   
                    // Here uvgrab is calculated by getting the vertex position into texture space
                    // glstate.matrix.texture is an array of matrices, GrabPass automatically sets us up with a handy matrix
                    // The index of glstate.matrix.texture depends on the register it was bound to earlier, so for s0, it is index 0, for s1 it is index 1
                    o.uvgrab = mul( UNITY_MATRIX_TEXTURE0, v.vertex );
                    o.normal = mul( (float3x3)UNITY_MATRIX_MVP, v.normal );
   
                    return o;
                }
               
                half4 PS( PS_IN i ) : COLOR
                {
                    // Normalise the interpolated normal
                    i.normal = normalize( i.normal );
                   
                    // To simulate refraction, we want to shift the texture coordinates towards the normal
                    // Remember, the normal is transformed by the mvp matrix, so this value is essentially relative to the viewer
                    // Here the refraction vector is taken from the direction of the normal, with its magnitude squared (hence * abs(i.normal))
                    half3 refracted = i.normal * abs(i.normal);
                   
                    // The reason for the usage of uvgrab.w is something I am uncertain of.
                    // My current hypothesis is that the texture matrix used in the vertex shader
                    // usually does this step, and therefore texRectproj (used below) does not take w into account,
                    // so this only needs to be applied to the refraction vector, and not the original uvgrab.
                    i.uvgrab.xy = refracted.xy * (i.uvgrab.w * _Distortion) + i.uvgrab.xy;
                   
                    half4 col = tex2Dproj( _GrabTexture, i.uvgrab );
                    return col;
                }
 
            ENDCG
           
            // Call set texture in register order, grabTexture is bound to s0 so that comes first,
            // if we had another texture bound to s1 that would come second, and so on
            SetTexture [_GrabTexture] {}
        }
       
    }
 
}
