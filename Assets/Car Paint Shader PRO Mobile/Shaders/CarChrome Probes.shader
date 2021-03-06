// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "RedDotGames/Mobile/Light Probes Support/Car Chrome" {
   Properties {
   
	  _Color ("Diffuse Material Color (RGB)", Color) = (0,0,0,1) 
	  _SpecColor ("Specular Material Color (RGB)", Color) = (1,1,1,1) 
	  _Shininess ("Shininess", Range (0.01, 10)) = 1
	  _Gloss ("Gloss", Range (0.0, 10)) = 0
	  _MainTex ("Diffuse Texture", 2D) = "white" {} 
	  _Cube("Reflection Map", Cube) = "" {}
	  _Reflection("Reflection Power", Range (0.00, 1)) = 1
	  _SHLightingScale("LightProbe influence scale",Range(0,1)) = 1
	  
   }
SubShader {
   Tags { "QUEUE"="Geometry" "RenderType"="Opaque" " IgnoreProjector"="True"}	  
      Pass {  
      
         Tags { "LightMode" = "ForwardBase" } // pass for 
            // 4 vertex lights, ambient light & first pixel light
 
         CGPROGRAM
         #pragma fragmentoption ARB_precision_hint_fastest
         #pragma multi_compile_fwdbase 
         #pragma vertex vert
         #pragma fragment frag
		 #pragma target 2.0	
		 #pragma exclude_renderers d3d11 xbox360 ps3 d3d11_9x flash
		 
		 //#include "AutoLight.cginc"
 		 #include "UnityCG.cginc"
 
         // User-specified properties
		 uniform sampler2D _MainTex; 
         //uniform sampler2D _BumpMap; 
		 
		 fixed _SHLightingScale;
		 
         //uniform fixed4 _BumpMap_ST;		 
         uniform fixed4 _Color; 
		 uniform fixed _Reflection;
         uniform fixed4 _SpecColor; 
         uniform fixed _Shininess;
		 uniform half _Gloss;
		 
		 //uniform fixed normalPerturbation;
		 
		 //uniform fixed4 _paintColor0; 
		 //uniform fixed4 _paintColorMid; 
		 
		 uniform samplerCUBE _Cube; 
		 half4 _CubeTex_TexelSize;
		  
         // The following built-in uniforms (except _LightColor0) 
         // are also defined in "UnityCG.cginc", 
         // i.e. one could #include "UnityCG.cginc" 
         uniform fixed4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         struct vertexInput {
            fixed4 vertex : POSITION;
            fixed3 normal : NORMAL;
			fixed4 texcoord : TEXCOORD0;
         };
         struct vertexOutput {
            fixed4 pos : SV_POSITION;
            fixed4 posWorld : TEXCOORD0;
            fixed3 normalDir : TEXCOORD1;
            fixed3 vertexLighting : TEXCOORD2;
			fixed4 tex : TEXCOORD3;
			fixed3 viewDir : TEXCOORD4;
			//fixed3 worldNormal  : TEXCOORD5;
			//LIGHTING_COORDS(7,8)
			
         };
         
         vertexOutput vert(vertexInput input)
         {          
            vertexOutput o;
 
            o.posWorld = mul(unity_ObjectToWorld, input.vertex);
            o.normalDir = normalize(fixed3(mul(fixed4(input.normal, 0.0), unity_WorldToObject).xyz));			   
			   
			o.tex = input.texcoord;
            o.pos = UnityObjectToClipPos(input.vertex);
            o.viewDir = normalize(mul(unity_ObjectToWorld, input.vertex) - fixed4(_WorldSpaceCameraPos.xyz, 1.0)).xyz;
            //output.viewDir = normalize(_WorldSpaceCameraPos - fixed3(mul(modelMatrix, input.vertex)));			   
			   
            //output.binormalWorld = normalize(
            //  cross(output.worldNormal, output.tangentWorld) 
            //  * input.tangent.w); // tangent.w is specific to Unity			   
			   
			
			   
            // Diffuse reflection by four "vertex lights"            
            o.vertexLighting = fixed3(0.0, 0.0, 0.0);
            #ifdef VERTEXLIGHT_ON
            for (int index = 0; index < 4; index++)
            {    
               fixed4 lightPosition = fixed4(unity_4LightPosX0[index], 
                  unity_4LightPosY0[index], 
                  unity_4LightPosZ0[index], 1.0);
                  
               fixed3 vertexToLightSource = 
                  fixed3(lightPosition.xyz - o.posWorld.xyz);        
               fixed3 lightDirection = normalize(vertexToLightSource);
               
               fixed squaredDistance = 
                  dot(vertexToLightSource, vertexToLightSource);
               fixed attenuation = 1.0 / (1.0 + 
                  unity_4LightAtten0[index] * squaredDistance);
                  
               fixed3 diffuseReflection =  
                  attenuation * unity_LightColor[index].rgb 
                  * _Color.rgb * max(0.0, 
                  dot(o.normalDir, lightDirection));         
 
               o.vertexLighting = 
                  o.vertexLighting + diffuseReflection;
            }
            #endif
            
            float3 shl = ShadeSH9(float4(o.normalDir,1.0f));
 			//o.SHLighting = shl * _SHLightingScale;
            o.vertexLighting += shl * _SHLightingScale;            
            
            //TRANSFER_VERTEX_TO_FRAGMENT(o);			   
            return o;
         }
 
         fixed4 frag(vertexOutput input) : COLOR
         {
		 
		 
		    //fixed4 encodedNormal = tex2D(_BumpMap, _BumpMap_ST.xy * input.tex.xy + _BumpMap_ST.zw);
		 
            fixed3 normalDirection = normalize(input.normalDir); 
            fixed3 viewDirection = normalize(
               _WorldSpaceCameraPos.xyz- input.posWorld.xyz);
            fixed3 lightDirection;
            fixed attenuation;
 
			fixed4 textureColor = tex2D(_MainTex, input.tex.xy);
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = 
                  normalize(fixed3(_WorldSpaceLightPos0.xyz));
            } 
            else // point or spot light
            {
               fixed3 vertexToLightSource = 
                  fixed3(_WorldSpaceLightPos0.xyz - input.posWorld.xyz);
               fixed distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
			//attenuation = LIGHT_ATTENUATION(input);
 
            fixed3 ambientLighting = 
                UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
 
            fixed3 diffuseReflection = 
               attenuation * _LightColor0.rgb * _Color.rgb 
               * max(0.0, dot(normalDirection, lightDirection));
 
               fixed3 halfwayDirection = 
                  normalize(lightDirection + viewDirection);
				  
            fixed3 halfwayVector = 
               normalize(lightDirection + input.viewDir);				  
				  
			fixed dotLN = dot(lightDirection, input.normalDir); 				  
				  
				  
               //fixed w = pow(1.0 - max(0.0, 
                 // dot(halfwayDirection, viewDirection)), 5.0);
			

            fixed3 specularReflection;
			//fixed3 metalicReflection;
			
			if (dot(normalDirection, lightDirection) < 0.0) 
            //if (dotLN) < 0.0) 
               // light source on the wrong side?
            {
               specularReflection = fixed3(0.0, 0.0, 0.0); 
                  // no specular reflection
			   //metalicReflection = fixed3(0.0, 0.0, 0.0); 
            }
            else // light source on the right side
            {
               specularReflection = attenuation * _LightColor0.rgb 
                  * _SpecColor.rgb * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
            
		 
			specularReflection *= _Gloss;

			fixed3 reflectedDir = reflect(input.viewDir, normalize(input.normalDir) );
            fixed4 reflTex = texCUBE(_Cube, reflectedDir);
			
			reflTex.rgb *= _Reflection;
			
			
			
   fixed4 finalColor;
   finalColor.a = 1.0;
 
			fixed4 color = fixed4(textureColor.rgb * saturate(input.vertexLighting + ambientLighting + diffuseReflection) + specularReflection, 1.0);
			color += reflTex;
            return color;
			
			
			
         }
         ENDCG
      }
 }
   // The definition of a fallback shader should be commented out 
   // during development:
   Fallback "Mobile/Diffuse"
}