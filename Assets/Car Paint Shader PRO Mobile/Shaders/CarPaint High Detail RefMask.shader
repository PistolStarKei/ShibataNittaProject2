// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "RedDotGames/Mobile/Car Paint High Detail Ref Mask" {
   Properties {
   
	  _Color ("Diffuse Material Color (RGB)", Color) = (1,1,1,1) 
	  _SpecColor ("Specular Material Color (RGB)", Color) = (1,1,1,1) 
	  _Shininess ("Shininess", Range (0.01, 10)) = 1
	  _Gloss ("Gloss", Range (0.0, 10)) = 1
	  _MainTex ("Texture(RGB) Mask (A)", 2D) = "white" {} 
	  _Cube("Reflection Map", Cube) = "" {}
	  _Reflection("Reflection Power", Range (0.00, 1)) = 0
	  _FrezPow("Fresnel Power",Range(0,2)) = .25
	  _FrezFalloff("Fresnal Falloff",Range(0,10)) = 4	  
	  
	  _SparkleTex ("Sparkle Texture", 2D) = "white" {} 

	  _FlakeScale ("Flake Scale", float) = 1
	  _FlakePower ("Flake Alpha",Range(0,1)) = 0

	  _OuterFlakePower ("Flake Outer Power",Range(1,16)) = 2

	  _paintColor2 ("Outer Flake Color (RGB)", Color) = (1,1,1,1) 

	  
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
		 #pragma target 3.0	
		 #pragma exclude_renderers d3d11 xbox360 ps3 d3d11_9x flash
		 //#include "AutoLight.cginc"
 		 #include "UnityCG.cginc"
 
         // User-specified properties
		 uniform sampler2D _MainTex; 
         //uniform sampler2D _BumpMap; 
         uniform sampler2D _SparkleTex; 		 
	 
		 
         //uniform fixed4 _BumpMap_ST;		 
         uniform fixed4 _Color; 
		 uniform fixed _Reflection;
         uniform fixed4 _SpecColor; 
         uniform half _Shininess;
		 uniform half _Gloss;
		 uniform half _FlakeScale;
		 uniform half _FlakePower;
		 uniform half _OuterFlakePower;
		 
		 //uniform fixed normalPerturbation;
		 
		 //uniform fixed4 _paintColor0; 
		 //uniform fixed4 _paintColorMid; 
		 uniform fixed4 _paintColor2;
		 
		 uniform samplerCUBE _Cube; 
		 fixed _FrezPow;
		 half _FrezFalloff;
		 
 
         // The following built-in uniforms (except _LightColor0) 
         // are also defined in "UnityCG.cginc", 
         // i.e. one could #include "UnityCG.cginc" 
         uniform fixed4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         struct vertexInput {
            float4 vertex : POSITION;
            fixed3 normal : NORMAL;
			half4 texcoord : TEXCOORD0;
			float4 tangent : TANGENT;
			
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 posWorld : TEXCOORD0;
            fixed3 normalDir : TEXCOORD1;
            fixed3 vertexLighting : TEXCOORD2;
			half4 tex : TEXCOORD3;
			fixed3 viewDir : TEXCOORD4;
			fixed3 worldNormal  : TEXCOORD5;
			fixed3 tangentWorld : TEXCOORD6;
			fixed3 binormalDirection : TEXCOORD7;
			//LIGHTING_COORDS(7,8)
			
         };
 
         vertexOutput vert(vertexInput input)
         {          
			   			   
            vertexOutput o;
 
            o.posWorld = mul(unity_ObjectToWorld, input.vertex);
            o.normalDir = normalize(fixed3(mul(fixed4(input.normal, 0.0), unity_WorldToObject).xyz));
			o.worldNormal = mul(unity_ObjectToWorld, fixed4(input.normal, 0.0f)).xyz;
						   
			o.tex = input.texcoord;
            o.pos = UnityObjectToClipPos(input.vertex);
            o.viewDir = normalize(mul(unity_ObjectToWorld, input.vertex) - fixed4(_WorldSpaceCameraPos.xyz, 1.0)).xyz;			   			   			   
			   			   			   			   			   			   			   			   			   
            o.tangentWorld = normalize(mul(unity_ObjectToWorld, fixed4(input.tangent.xyz, 0.0) ).xyz);

			   
			o.binormalDirection = cross(o.normalDir, o.tangentWorld);	
			   
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
                  attenuation * unity_LightColor[index].xyz 
                  * _Color.xyz * max(0.0, 
                  dot(o.normalDir, lightDirection));         
 
               o.vertexLighting = 
                  o.vertexLighting + diffuseReflection;
            }
            #endif
            
            //TRANSFER_VERTEX_TO_FRAGMENT(o);			   
            return o;
         }
 
         fixed4 frag(vertexOutput input) : COLOR
         {
		 
            fixed3 normalDirection = normalize(input.normalDir); 
            fixed3 viewDirection = normalize(
               _WorldSpaceCameraPos.xyz - input.posWorld.xyz);
            fixed3 lightDirection;
            fixed attenuation;
 
			fixed4 textureColor = tex2D(_MainTex, input.tex.xy);
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               attenuation = 1.0; // no attenuation
               lightDirection = 
                  normalize(_WorldSpaceLightPos0.xyz);
            } 
            else // point or spot light
            {
               fixed3 vertexToLightSource = 
                  _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
               fixed distance = length(vertexToLightSource);
               attenuation = 1.0 / distance; // linear attenuation 
               lightDirection = normalize(vertexToLightSource);
            }
 
			//attenuation = LIGHT_ATTENUATION(input);
 
            fixed3 ambientLighting = 
                UNITY_LIGHTMODEL_AMBIENT.xyz * _Color.xyz;
 
            fixed3 diffuseReflection = 
               attenuation * _LightColor0.xyz * _Color.xyz 
               * max(0.0, dot(normalDirection, lightDirection));
 
               fixed3 halfwayDirection = 
                  normalize(lightDirection + viewDirection);
				  
            fixed3 halfwayVector = 
               normalize(lightDirection + input.viewDir);				  

            fixed3 specularReflection;
			
			if (dot(normalDirection, lightDirection) < 0.0) 
            {
               specularReflection = fixed3(0.0, 0.0, 0.0); 
            }
            else // light source on the right side
            {
               specularReflection = attenuation * (_LightColor0.xyz) 
                  * (_SpecColor.xyz) * pow(max(0.0, dot(
                  reflect(-lightDirection, normalDirection), 
                  viewDirection)), _Shininess);
            }
            
		 
			specularReflection *= _Gloss;

 
 //flakes
			// fetch from the incoming normal map:
			
			
			
			fixed3 vNormal = fixed3(0.5,0.5,1.0);//encodedNormal;// tex2D( _BumpMap, input.tex );
			vNormal = 2 * vNormal - 1.0;
			//fixed3 vNormal = 2 * fixed3(0.0,1.0,0.0) - 1.0;
			
			fixed3 vFlakesNormal = tex2D( _SparkleTex, input.tex.xy * 20.0 * _FlakeScale );
			//fixed3 vFlakesNormal = tex2D( _SparkleTex, input.posWorld.yz * 1.0 );
			
			vFlakesNormal = 2 * vFlakesNormal - 1.0;
			fixed3 vNp1 = vFlakesNormal + 4 * vNormal ; 
			fixed3 vView =  normalize( input.viewDir );
			
			fixed3x3 mTangentToWorld = transpose( fixed3x3( input.tangentWorld,input.binormalDirection,input.worldNormal ) );
			
            fixed3 vNormalWorld = 
               normalize(mul(mTangentToWorld, vNormal));			

   fixed3 vNp1World = normalize( mul( mTangentToWorld, -vNp1) );
   fixed  fFresnel1 = saturate( dot( vNp1World, vView ));
   fixed  fFresnel1Sq = fFresnel1 * fFresnel1;

   fixed4 paintColor = pow(fFresnel1Sq,_OuterFlakePower) * _paintColor2;;

      
			fixed3 reflectedDir = reflect(input.viewDir, normalize(input.normalDir) );
            fixed4 reflTex = texCUBE(_Cube, reflectedDir);
			
			//Calculate Reflection vector
			fixed SurfAngle= clamp(abs(dot (reflectedDir,input.normalDir)),0,1);
			fixed frez = pow(1-SurfAngle,_FrezFalloff) ;
			frez*=_FrezPow;
			
			_Reflection += frez;			
			
			reflTex.rgb *= saturate(_Reflection);
			 
			fixed4 color = fixed4(textureColor.rgb * saturate(input.vertexLighting + ambientLighting + diffuseReflection) + specularReflection, 1.0);
			
	
			color += paintColor * _FlakePower * textureColor.a;
			color += reflTex * textureColor.a;
			color += (frez * reflTex) * textureColor.a;
 
            return color;
			
			
			
         }
         ENDCG
      }
 }
   // The definition of a fallback shader should be commented out 
   // during development:
   Fallback "Mobile/Diffuse"
}